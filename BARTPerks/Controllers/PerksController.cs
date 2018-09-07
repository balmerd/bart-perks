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
                                return View("UserSignup", model);
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

        public ActionResult UserSignup()
        {
            ViewBag.Message = "";
            ViewBag.AlertMessage = "";

            return View();
        }

        [HttpPost]
        public ActionResult UserSignup(PerksModel model, string Action)
        {
            ViewBag.Message = "";
            ViewBag.AlertMessage = "";

            try
            {
                var apiManager = new APIManager();
                var apiResponse = apiManager.UserSignup(model);

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