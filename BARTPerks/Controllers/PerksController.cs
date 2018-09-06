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
            ViewBag.Message = "";
            ViewBag.AlertMessage = "";

            return View();
        }

        [HttpPost]
        public ActionResult RewardParticipation(PerksModel model, string Action)
        {
            ViewBag.Message = "";
            ViewBag.AlertMessage = "";

            try
            {
                var apiManager = new APIManager();
                var apiResponse = apiManager.ValidateCouponCode(model.CouponCode);

                if (apiResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    if (apiResponse.status.Equals("error", StringComparison.CurrentCultureIgnoreCase))
                    {
                        // currently happens when code not found
                        ViewBag.AlertMessage = JsonConvert.SerializeObject(apiResponse);
                    }
                    else
                    {
                        if (apiResponse.data.valid)
                        {
                            if (model.CouponCode.Equals("BART-TEST-2"))
                            {
                                return View("JoinWaitList", model);
                            }
                            else
                            {
                                return View("Signup", model);
                            }
                        }
                        else
                        {
                            ViewBag.AlertMessage = JsonConvert.SerializeObject(apiResponse);
                        }
                    }
                }
                else if (apiResponse.StatusCode.Equals(HttpStatusCode.NotFound))
                {
                    ViewBag.AlertMessage = JsonConvert.SerializeObject(apiResponse);
                }
                else
                {
                    ViewBag.Message = JsonConvert.SerializeObject(apiResponse);
                    return View("Error", model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("{0} {1} {2}", ex.Source, ex.Message, ex.StackTrace);
                return View("Error", model);
            }

            return View(model);
        }

        public ActionResult JoinWaitList()
        {
            ViewBag.Message = "";
            ViewBag.AlertMessage = "";

            return View();
        }

        [HttpPost]
        public ActionResult JoinWaitList(PerksModel model, string Action)
        {
            ViewBag.Message = "";
            ViewBag.AlertMessage = "";

            try
            {
                var apiManager = new APIManager();
                var apiResponse = apiManager.JoinWaitList(model.EmailAddress);

                if (apiResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    if (apiResponse.status.Equals("error", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ViewBag.AlertMessage = JsonConvert.SerializeObject(apiResponse);
                    }
                    else
                    {
                    }
                }
                else
                {
                    ViewBag.Message = JsonConvert.SerializeObject(apiResponse);
                    return View("Error", model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("{0} {1} {2}", ex.Source, ex.Message, ex.StackTrace);
                return View("Error", model);
            }

            return View(model);
        }

        public ActionResult Signup()
        {
            ViewBag.Message = "";
            ViewBag.AlertMessage = "";

            return View();
        }

        [HttpPost]
        public ActionResult Signup(PerksModel model, string Action)
        {
            ViewBag.Message = "";
            ViewBag.AlertMessage = "";

            try
            {
                var apiManager = new APIManager();
                var apiResponse = apiManager.GiftCardSignup(model);

                if (apiResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    if (apiResponse.status.Equals("error", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ViewBag.AlertMessage = JsonConvert.SerializeObject(apiResponse);
                    }
                    else
                    {
                        return View("SignupComplete", model);
                    }
                }
                else
                {
                    ViewBag.Message = JsonConvert.SerializeObject(apiResponse);
                    return View("Error", model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("{0} {1} {2}", ex.Source, ex.Message, ex.StackTrace);
                return View("Error", model);
            }

            return View(model);
        }

        public ActionResult SignupComplete()
        {
            ViewBag.Message = "";
            ViewBag.AlertMessage = "";

            return View();
        }
    }
}