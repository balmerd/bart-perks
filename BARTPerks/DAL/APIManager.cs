using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using log4net;
using Newtonsoft.Json;
using BARTPerks.Models;

namespace BARTPerks.DAL
{
    public partial class APIManager
    {
        private static readonly string baseUrl = ConfigurationManager.AppSettings["APIBaseUrl"];
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CouponCodeValidationResponse ValidateCouponCode(string couponCode)
        {
            var request = GetRequestObject(String.Format("{0}/invitations/{1}", baseUrl, couponCode));
            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<CouponCodeValidationResponse>(apiResponse.JSONString);
            response.URI = apiResponse.URI;
            response.StatusCode = apiResponse.StatusCode;
            log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }

        public JoinWaitListResponse JoinWaitList(string emailAddress)
        {
            var request = GetRequestObject(String.Format("{0}/waitlist", baseUrl), method: "POST");
            var jsonData = JsonConvert.SerializeObject(new JoinWaitListRequest { email = emailAddress });

            AddJSONDataToRequest(request, jsonData);

            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<JoinWaitListResponse>(apiResponse.JSONString);
            response.URI = apiResponse.URI;
            response.StatusCode = apiResponse.StatusCode;
            log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }

        public UserSignupResponse UserSignup(PerksModel model)
        {
            var app = RequestAppAuthToken();
            var user = RequestUserAuthToken(model);

            if (app.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                return new UserSignupResponse
                {
                    StatusCode = app.StatusCode,
                    code = app.code,
                    message = app.message,
                    description = app.description,
                    policy = app.policy
                };
            }

            if (user.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                return new UserSignupResponse
                {
                    StatusCode = user.StatusCode,
                    code = user.code,
                    message = user.message,
                    description = user.description,
                    policy = user.policy
                };
            }

            var request = GetRequestObject(String.Format("{0}/signup", baseUrl), method: "POST", token: app.access_token);

            var jsonData = JsonConvert.SerializeObject(new UserSignupRequest
            {
                uid = user._id,
                first_name = model.FirstName,
                last_name = model.LastName,
                cid = model.ClipperCardNumber,
                email = model.EmailAddress,
                invitiation_code = model.CouponCode
            });

            AddJSONDataToRequest(request, jsonData);

            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<UserSignupResponse>(apiResponse.JSONString);
            response.URI = apiResponse.URI;
            response.StatusCode = apiResponse.StatusCode;
            log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }

        private HttpWebRequest GetRequestObject(string url, string method = "GET", string token = "")
        {
            var request = WebRequest.Create(url) as HttpWebRequest;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add($"Authorization: Bearer {token}");
            }

            request.Method = method;
            request.Timeout = 10000;
            request.KeepAlive = false;
            return request;
        }

        private APIResponse GetAPIResponse(HttpWebRequest request)
        {
            string JSONString = string.Empty;
            HttpWebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;
            APIResponse apiResponse = null;

            try
            {
                response = request.GetResponse() as HttpWebResponse;
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                reader = new StreamReader(dataStream);
                // Read the content.
                JSONString = reader.ReadToEnd();

                apiResponse = new APIResponse { URI = request.RequestUri.AbsoluteUri, StatusCode = response.StatusCode, JSONString = JSONString };
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    {
                        using (var errorReader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            JSONString = errorReader.ReadToEnd();

                            apiResponse = new APIResponse { URI = request.RequestUri.AbsoluteUri, StatusCode = errorResponse.StatusCode, JSONString = JSONString };
                        }
                    }
                }
            }
            finally
            {
                // Clean up the streams.
                if (reader != null)
                {
                    reader.Close();
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }

            return apiResponse;
        }

        private void AddJSONDataToRequest(HttpWebRequest request, string postData)
        {
            Stream dataStream = null;

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                request.AllowAutoRedirect = false;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            finally
            {
                if (dataStream != null)
                {
                    dataStream.Close();
                }
            }
        }
    }
}