using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BARTPerks.Models;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using BARTPerks.DAL;

namespace BARTPerks.Controllers
{
    public class PerksController : Controller
    {
        public ActionResult RewardParticipation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RewardParticipation(PerksModel model, string Action)
        {
            var apiManager = new APIManager();

            ViewBag.Message = "";

            var response = apiManager.ValidateCouponCode(model.CouponCode);

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                if (response.status.Equals("error", StringComparison.CurrentCultureIgnoreCase))
                {
                    // currently happens when code not found
                }
                else
                {
                    if (response.data.valid)
                    {

                    }
                    else
                    {

                    }
                }
            }
            else if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {

            }
            else
            {

            }
            
            return View(model);
        }

        public ActionResult JoinWaitList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinWaitList(PerksModel model, string Action)
        {
            var apiManager = new APIManager();

            ViewBag.Message = "";

            var response = apiManager.JoinWaitList(model.EmailAddress);

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                if (response.status.Equals("error", StringComparison.CurrentCultureIgnoreCase))
                {
                }
                else
                {
                }
            }
            else
            {

            }

            return View(model);
        }

        public ActionResult GiftCardSignup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GiftCardSignup(PerksModel model, string Action)
        {
            //var apiManager = new APIManager();

            //ViewBag.Message = "";

            //var response = apiManager.GiftCardSignup(model);

            //if (response.StatusCode.Equals(HttpStatusCode.OK))
            //{
            //    if (response.status.Equals("error", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //    }
            //    else
            //    {
            //    }
            //}
            //else
            //{

            //}

            return View(model);
        }
    }
}