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
            response.StatusCode = apiResponse.StatusCode;
            return response;
        }

        public JoinWaitListResponse JoinWaitList(string emailAddress)
        {
            var request = GetRequestObject(String.Format("{0}/waitlist", baseUrl), method: "POST");
            var jsonData = JsonConvert.SerializeObject(new JoinWaitListRequest { email = emailAddress });

            AddJSONDataToRequest(request, jsonData);

            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<JoinWaitListResponse>(apiResponse.JSONResponseString);
            response.StatusCode = apiResponse.StatusCode;
            return response;
        }

        private HttpWebRequest GetRequestObject(string url, string method = "GET")
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            //request.CookieContainer = new CookieContainer();
            //request.CookieContainer.Add(new Cookie("Auth_Meridian", authTicket) { Domain = request.Host });
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

                apiResponse = new APIResponse { StatusCode = response.StatusCode, JSONResponseString = jsonResponseString };
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