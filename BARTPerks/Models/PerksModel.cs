using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BARTPerks.Models
{
    public class PerksModel
    {
        public string CouponCode { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClipperCardNumber { get; set; }
    }
}