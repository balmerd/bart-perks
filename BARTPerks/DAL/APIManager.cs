using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using BARTPerks.Models;
using System.Collections.Specialized;

namespace BARTPerks.DAL
{
    public class APIManager
    {
        private string baseUrl = ConfigurationManager.AppSettings["APIBaseUrl"];

        public CouponCodeValidationResponse ValidateCouponCode(string couponCode)
        {
            var request = GetRequestObject(String.Format("{0}/invitations/{1}", baseUrl, couponCode));
            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<CouponCodeValidationResponse>(apiResponse.JSONResponseString);
            response.URI = apiResponse.URI;
            response.StatusCode = apiResponse.StatusCode;
            return response;
        }

        public JoinWaitListResponse JoinWaitList(string emailAddress)
        {
            var request = GetRequestObject(String.Format("{0}/user", baseUrl), method: "POST");
            var jsonData = JsonConvert.SerializeObject(new JoinWaitListRequest { email = emailAddress });

            AddJSONDataToRequest(request, jsonData);

            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<JoinWaitListResponse>(apiResponse.JSONResponseString);
            response.StatusCode = apiResponse.StatusCode;
            return response;
        }

        public JoinWaitListResponse GiftCardSignup(PerksModel model)
        {
            var request = GetRequestObject(String.Format("{0}/waitlist", baseUrl), method: "POST", token: RequestAuth0IdToken(model));

            var jsonData = JsonConvert.SerializeObject(new UserSignupRequest {
                first_name = model.FirstName,
                last_name = model.LastName,
                cid = model.ClipperCardNumber,
                email = model.EmailAddress,
                invitiation_code = model.CouponCode
            });

            AddJSONDataToRequest(request, jsonData);

            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<JoinWaitListResponse>(apiResponse.JSONResponseString);
            response.StatusCode = apiResponse.StatusCode;
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
            string jsonResponseString = string.Empty;
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
                jsonResponseString = reader.ReadToEnd();

                apiResponse = new APIResponse { URI = request.RequestUri.AbsoluteUri, StatusCode = response.StatusCode, JSONResponseString = jsonResponseString };
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

        private string RequestAuth0IdToken(PerksModel model)
        {
            /* 
             * FROM https://mail.google.com/mail/u/0/#search/auth0/16523694c6d5377d
             * 
                Method - POST
                URL - https://bart-sso-dev.auth0.com/dbconnections/signup
                Headers
                    Content-Type - application/json
                BODY
                {
                    "client_id": "eMANjbzwCtxlK6tNACaGXDkSukQoQHAE", 
                    "email": "bimesh@gmail.com", 
                    "connection": "bartdb-dev", 
                    "password": "123456qwerty!", 
                    "user_metadata": { 
                        "first_name": "bimesh", 
                        "last_name": "giri", 
                        "mobile": "818-3578904", 
                        "preferences": ""
                    }, 
                    "email_verified": true, 
                    "app_metadata": { 
                        "source_ref": "app"
                    }
                }
             */
            string id_token = "";
            string token_request_url = ConfigurationManager.AppSettings["auth0:token_request_url"];
            string client_id = ConfigurationManager.AppSettings["auth0:client_id"];
            string username = model.EmailAddress;
            string password = model.Password;
            string connection = ConfigurationManager.AppSettings["auth0:connection"];
            string grant_type = ConfigurationManager.AppSettings["auth0:grant_type"];
            string scope = ConfigurationManager.AppSettings["auth0:scope"];

            if (string.IsNullOrEmpty(token_request_url) ||
                string.IsNullOrEmpty(client_id) ||
                string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(connection) ||
                string.IsNullOrEmpty(grant_type) ||
                string.IsNullOrEmpty(scope))
            {
                throw new Exception("Application settings for Auth0 token request are missing.");
            }

            using (var client = new WebClient())
            {
                var reqParams = new NameValueCollection();

                reqParams.Add("client_id", client_id);
                reqParams.Add("username", username);
                reqParams.Add("password", password);
                reqParams.Add("connection", connection);
                reqParams.Add("grant_type", grant_type);
                reqParams.Add("scope", scope);
                reqParams.Add("id_token", "");
                reqParams.Add("device", "");

                client.Headers.Add("ContentType", "x-www-form-urlencoded");

                client.UploadValues(token_request_url, "POST", reqParams);

                var responseBytes = client.UploadValues(token_request_url, "POST", reqParams);
                var responseBody = Encoding.UTF8.GetString(responseBytes);
                var tokenResponse = JsonConvert.DeserializeObject<Auth0TokenResponse>(responseBody);

                id_token = tokenResponse.id_token;
            }

            return id_token;
        }
    }
}