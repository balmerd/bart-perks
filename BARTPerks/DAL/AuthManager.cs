using System;
using System.Configuration;
using Newtonsoft.Json;
using BARTPerks.Models;

namespace BARTPerks.DAL
{
    public partial class APIManager
    {
        public AppTokenResponse RequestAppAuthToken()
        {
            var request = GetRequestObject(ConfigurationManager.AppSettings["auth0:token_request_url"], method: "POST");

            var jsonData = JsonConvert.SerializeObject(new AppTokenRequest
            {
                grant_type = "client_credentials",
                client_id = ConfigurationManager.AppSettings["auth0:client_id"],
                client_secret = ConfigurationManager.AppSettings["auth0:client_secret"],
                audience = ConfigurationManager.AppSettings["auth0:audience"]
            });

            AddJSONDataToRequest(request, jsonData);

            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<AppTokenResponse>(apiResponse.JSONString);
            response.URI = apiResponse.URI;
            response.StatusCode = apiResponse.StatusCode;
            log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }

        public UserTokenResponse RequestUserAuthToken(PerksModel model)
        {
            var request = GetRequestObject(ConfigurationManager.AppSettings["auth0:signup_request_url"], method: "POST");

            var tokenRequest = new UserTokenRequest
            {
                client_id = ConfigurationManager.AppSettings["auth0:client_id"],
                email = model.EmailAddress,
                connection = ConfigurationManager.AppSettings["auth0:connection"],
                password = model.Password,
                user_metadata = new UserTokenRequestUserMeta
                {
                    first_name = model.FirstName,
                    last_name = model.LastName,
                    mobile = "",
                    preferences = ""
                },
                email_verified = "true",
                app_metadata = new UserTokenRequestAppMeta
                {
                    source_ref = "app"
                }
            };

            var jsonData = JsonConvert.SerializeObject(tokenRequest);

            AddJSONDataToRequest(request, jsonData);

            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<UserTokenResponse>(apiResponse.JSONString);
            response.URI = apiResponse.URI;
            response.StatusCode = apiResponse.StatusCode;
            log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }
    }
}