using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using BARTPerks.DAL;
using BARTPerks.Models;

namespace BARTPerks.Test
{
    [TestClass]
    public class CouponCodeTests
    {
        APIManager apiManager = null;

        [TestInitialize]
        public void Initialize()
        {
            apiManager = new APIManager();
        }

        [TestMethod]
        public void ValidCouponCode()
        {
            var apiResponse = apiManager.ValidateCouponCode("BART-TEST-1");

            Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            Assert.AreEqual("success", apiResponse.status);
            Assert.AreEqual(true, apiResponse.data.valid);
        }

        [TestMethod]
        public void InValidCouponCode()
        {
            var apiResponse = apiManager.ValidateCouponCode("XXX");

            Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            Assert.AreEqual("error", apiResponse.status);
            Assert.AreEqual("Bad Invitation Code", apiResponse.error);
        }
    }
}
