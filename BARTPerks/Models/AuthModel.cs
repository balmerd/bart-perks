using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace BARTPerks.Models
{
    //
    // Application token
    //

    public class Auth0AppTokenRequest
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string audience { get; set; }
    }

    public class Auth0AppTokenResponse : BaseAPIResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
    }

    //
    // User token
    //

    public class Auth0UserTokenRequest
    {
        public string client_id { get; set; }
        public string email { get; set; }
        public string connection { get; set; }
        public string password { get; set; }
        public Auth0UserTokenRequestUserMeta user_metadata;
        public string email_verified { get; set; }
        public Auth0UserTokenRequestAppMeta app_metadata { get; set; }
    }

    public class Auth0UserTokenRequestUserMeta
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobile { get; set; }
        public string preferences { get; set; }
    }

    public class Auth0UserTokenRequestAppMeta
    {
        public string source_ref { get; set; }
    }

    public class Auth0UserTokenResponse : BaseAPIResponse
    {
        public string _id { get; set; }
        public string email_verified { get; set; }
    }
}