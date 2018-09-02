using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace BARTPerks.Models
{
    public class JoinWaitListRequest
    {
        public string email { get; set; }
    }

    public class JoinWaitListResponse
    {
        public string status { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string JSONResponseString { get; set; }
    }

    public class CouponCodeValidationResponse
    {
        // request status.
        public string status { get; set; }

        // currently returned when sending invalid code (should return StatusCode 404, but it doesn't)
        public string error { get; set; }

        public CouponCodeValidationResponseData data { get; set; }

        // not part of the JSON response, added by me to capture the HTTP status
        public HttpStatusCode StatusCode { get; set; }
    }

    public class CouponCodeValidationResponseData
    {
        // whether the code is still valid. It is not valid if the code has previously been used to signup an account.
        public bool valid { get; set; }

        // signup points associated with the invitation code.
        public int points { get; set; }
    }
}