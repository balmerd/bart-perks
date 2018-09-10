using System;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using BARTPerks.DAL;
using BARTPerks.Models;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace BARTPerks.Controllers
{
    public class PerksController : Controller
    {
        public ActionResult RewardParticipation()
        {
            ViewBag.ErrorMessage = "";
            ViewBag.ErrorDetails = "";

            return View();
        }

        [HttpPost]
        public ActionResult RewardParticipation(PerksModel model, string Action)
        {
            ViewBag.Message = "";
            ViewBag.ErrorMessage = "";
            ViewBag.ErrorDetails = "";

            try
            {
                var apiManager = new APIManager();
                var apiResponse = apiManager.ValidateCouponCode(model.CouponCode);

                if (apiResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    if (apiResponse.status.Equals("error", StringComparison.CurrentCultureIgnoreCase))
                    {
                        // currently happens when code not found
                        ViewBag.ErrorMessage = apiResponse.error;
                        ViewBag.ErrorDetails = JsonConvert.SerializeObject(apiResponse);
                    }
                    else
                    {
                        if (apiResponse.data.valid)
                        {
                            if (model.CouponCode.Equals("waitlist"))
                            {
                                return View("JoinWaitList", model);
                            }
                            else
                            {
                                return View("UserSignup", model);
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Invalid coupon code.";
                            ViewBag.ErrorDetails = JsonConvert.SerializeObject(apiResponse);
                        }
                    }
                }
                else if (apiResponse.StatusCode.Equals(HttpStatusCode.NotFound))
                {
                    ViewBag.ErrorMessage = "Invalid coupon code.";
                    ViewBag.ErrorDetails = JsonConvert.SerializeObject(apiResponse);
                }
                else
                {
                    ViewBag.ErrorMessage = string.Format("Unexpected HttpStatusCode: {0}.", apiResponse.StatusCode);
                    ViewBag.ErrorDetails = JsonConvert.SerializeObject(apiResponse);
                    return View("Error", model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Exception calling the API.";
                ViewBag.ErrorDetails = string.Format("{0} {1} {2}", ex.Source, ex.Message, ex.StackTrace);
                return View("Error", model);
            }

            return View(model);
        }

        public ActionResult JoinWaitList(PerksModel model)
        {
            if (string.IsNullOrEmpty(model.CouponCode))
            {
                return RedirectToAction("RewardParticipation");
            }

            ViewBag.Message = "";
            ViewBag.ErrorMessage = "";
            ViewBag.ErrorDetails = "";

            return View();
        }

        [HttpPost]
        public ActionResult JoinWaitList(PerksModel model, string Action)
        {
            ViewBag.Message = "";
            ViewBag.ErrorMessage = "";
            ViewBag.ErrorDetails = "";

            try
            {
#if DEBUG
                if (model.EmailAddress == "me@home.com")
                {
                    ViewBag.Message = " you have been added to the waitlist.";
                    return View("ProcessComplete", model);
                }
#endif
                var apiManager = new APIManager();
                var apiResponse = apiManager.JoinWaitList(model.EmailAddress);

                if (apiResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    if (apiResponse.status.Equals("error", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ViewBag.ErrorMessage = apiResponse.error;
                        ViewBag.ErrorDetails = JsonConvert.SerializeObject(apiResponse);
                    }
                    else
                    {
                        ViewBag.Message = " you have been added to the waitlist.";
                        return View("ProcessComplete", model);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = string.Format("Unexpected HttpStatusCode: {0}.", apiResponse.StatusCode);
                    ViewBag.ErrorDetails = JsonConvert.SerializeObject(apiResponse);
                    return View("Error", model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Exception calling the API.";
                ViewBag.ErrorDetails = string.Format("{0} {1} {2}", ex.Source, ex.Message, ex.StackTrace);
                return View("Error", model);
            }

            return View(model);
        }

        public ActionResult UserSignup()
        {
            ViewBag.Message = "";
            ViewBag.ErrorMessage = "";
            ViewBag.ErrorDetails = "";

            return View();
        }

        [HttpPost]
        public ActionResult UserSignup(PerksModel model, string Action)
        {
            ViewBag.Message = "";
            ViewBag.ErrorMessage = "";
            ViewBag.ErrorDetails = "";

            try
            {
                var apiManager = new APIManager();
                var apiResponse = apiManager.UserSignup(model);

                if (apiResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    if (apiResponse.status.Equals("error", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ViewBag.ErrorMessage = apiResponse.description;
                        ViewBag.ErrorDetails = JsonConvert.SerializeObject(apiResponse);
                    }
                    else
                    {
                        ViewBag.Message = " your signup is complete!";
                        return View("ProcessComplete", model);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = apiResponse.description;
                    ViewBag.ErrorDetails = JsonConvert.SerializeObject(apiResponse);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Exception calling the API.";
                ViewBag.ErrorDetails = string.Format("{0} {1} {2}", ex.Source, ex.Message, ex.StackTrace);
                return View("Error", model);
            }

            return View(model);
        }

        public ActionResult ProcessComplete()
        {
            ViewBag.ErrorMessage = "";
            ViewBag.ErrorDetails = "";

            return View();
        }
    }
}