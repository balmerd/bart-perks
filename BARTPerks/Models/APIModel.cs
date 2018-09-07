using System;
using System.Net;

namespace BARTPerks.Models
{
    public class APIResponse
    {
        public string URI { get; set; }
        public string JSONString { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        // returned on 400 error
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string policy { get; set; } // returned when code = "invalid_password"
    }

    public class JoinWaitListRequest
    {
        public string email { get; set; }
    }

    public class UserSignupRequest
    {
        public string uid { get; set; } // from Auth0
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string cid { get; set; } // Clipper Card ID
        public string email { get; set; }
        public string invitiation_code { get; set; }
    }

    public class UserSignupResponse : APIResponse
    {
        public string status { get; set; }
        public UserSignupResponseData data { get; set; }
    }

    public class UserSignupResponseData : UserSignupRequest
    {
        public string uid { get; set; }
    }

    public class JoinWaitListResponse : APIResponse
    {
        public string status { get; set; }
        public string error { get; set; }
    }

    public class CouponCodeValidationResponse : APIResponse
    {
        // request status.
        public string status { get; set; }

        // currently returned when sending invalid code (should return StatusCode 404, but it doesn't)
        public string error { get; set; }

        public CouponCodeValidationResponseData data { get; set; }
    }

    public class CouponCodeValidationResponseData
    {
        // whether the code is still valid. It is not valid if the code has previously been used to signup an account.
        public bool valid { get; set; }

        // signup points associated with the invitation code.
        public int points { get; set; }
    }
}