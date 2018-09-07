using System;

namespace BARTPerks.Models
{
    //
    // Application token
    //

    public class AppTokenRequest
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string audience { get; set; }
    }

    public class AppTokenResponse : APIResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
    }

    //
    // User token
    //

    public class UserTokenRequest
    {
        public string client_id { get; set; }
        public string email { get; set; }
        public string connection { get; set; }
        public string password { get; set; }
        public UserTokenRequestUserMeta user_metadata;
        public string email_verified { get; set; }
        public UserTokenRequestAppMeta app_metadata { get; set; }
    }

    public class UserTokenRequestUserMeta
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobile { get; set; }
        public string preferences { get; set; }
    }

    public class UserTokenRequestAppMeta
    {
        public string source_ref { get; set; }
    }

    public class UserTokenResponse : APIResponse
    {
        public string _id { get; set; }
        public string email_verified { get; set; }
    }
}