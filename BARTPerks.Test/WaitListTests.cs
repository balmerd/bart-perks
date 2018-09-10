using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using BARTPerks.DAL;
using BARTPerks.Models;

namespace BARTPerks.Test
{
    [TestClass]
    public class WaitListTests
    {
        APIManager apiManager = null;

        [TestInitialize]
        public void Initialize()
        {
            apiManager = new APIManager();
        }

        [TestMethod]
        public void AlreadyRegisteredEmailAddress()
        {
            var apiResponse = apiManager.JoinWaitList("david.balmer@transsight.com");

            Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            Assert.AreEqual("error", apiResponse.status);
            Assert.AreEqual("The email address is already on the waitlist.", apiResponse.error);
        }
    }
}
