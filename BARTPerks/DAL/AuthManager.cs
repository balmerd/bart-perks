using System;
using System.Configuration;
using Newtonsoft.Json;
using BARTPerks.Models;

namespace BARTPerks.DAL
{
    public partial class APIManager
    {
        private Auth0AppTokenResponse RequestAuthAppToken(PerksModel model)
        {
            var request = GetRequestObject(ConfigurationManager.AppSettings["auth0:token_request_url"], method: "POST");

            var jsonData = JsonConvert.SerializeObject(new Auth0AppTokenRequest
            {
                grant_type = "client_credentials",
                client_id = ConfigurationManager.AppSettings["auth0:client_id"],
                client_secret = ConfigurationManager.AppSettings["auth0:client_secret"],
                audience = ConfigurationManager.AppSettings["auth0:audience"]
            });

            AddJSONDataToRequest(request, jsonData);

            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<Auth0AppTokenResponse>(apiResponse.JSONResponseString);
            response.URI = apiResponse.URI;
            response.StatusCode = apiResponse.StatusCode;
            return response;
        }

        private Auth0UserTokenResponse RequestAuthUserToken(PerksModel model)
        {
            var request = GetRequestObject(ConfigurationManager.AppSettings["auth0:signup_request_url"], method: "POST");

            var jsonData = JsonConvert.SerializeObject(new Auth0UserTokenRequest
            {
                client_id = ConfigurationManager.AppSettings["auth0:client_id"],
                email = model.EmailAddress,
                connection = ConfigurationManager.AppSettings["auth0:connection"],
                password = model.Password,
                user_metadata = new Auth0UserTokenRequestUserMeta
                {
                    first_name = model.FirstName,
                    last_name = model.LastName,
                    mobile = "",
                    preferences = ""
                },
                email_verified = "true",
                app_metadata = new Auth0UserTokenRequestAppMeta
                {
                    source_ref = ""
                }
            });

            AddJSONDataToRequest(request, jsonData);

            var apiResponse = GetAPIResponse(request);
            var response = JsonConvert.DeserializeObject<Auth0UserTokenResponse>(apiResponse.JSONResponseString);
            response.URI = apiResponse.URI;
            response.StatusCode = apiResponse.StatusCode;
            return response;
        }
    }
}