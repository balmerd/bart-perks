using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace BARTPerks.Models
{
    public class BaseAPIResponse
    {
        public string URI { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class JoinWaitListRequest
    {
        public string email { get; set; }
    }

    public class UserSignupRequest
    {
        public string _id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string cid { get; set; } // Clipper Card ID
        public string email { get; set; }
        public string invitiation_code { get; set; }
    }

    public class APIResponse : BaseAPIResponse
    {
        public string JSONResponseString { get; set; }
    }

    public class JoinWaitListResponse : BaseAPIResponse
    {
        public string status { get; set; }
    }

    public class CouponCodeValidationResponse : BaseAPIResponse
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