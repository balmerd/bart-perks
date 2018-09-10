using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using BARTPerks.DAL;
using BARTPerks.Models;

namespace BARTPerks.Test
{
    [TestClass]
    public class AuthTests
    {
        APIManager apiManager = null;

        [TestInitialize]
        public void Initialize()
        {
            apiManager = new APIManager();
        }

        [TestMethod]
        public void AppAuthTokenSuccess()
        {
            var apiResponse = apiManager.RequestAppAuthToken();

            Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            Assert.IsNotNull(apiResponse.access_token);
            Assert.AreEqual("Bearer", apiResponse.token_type);
        }

        [TestMethod]
        public void UserAuthTokenFailure()
        {
            var apiResponse = apiManager.RequestUserAuthToken(new PerksModel
            {
                CouponCode = "BART-TEST-1",
                FirstName = "David",
                LastName = "Balmer",
                Password = "zaq12wsx",
                ClipperCardNumber = "123456789",
                EmailAddress = "david.balmer@transsight.com"
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, apiResponse.StatusCode);
            Assert.AreEqual("user_exists", apiResponse.code);
            Assert.AreEqual("The user already exists.", apiResponse.description);
        }
    }
}
