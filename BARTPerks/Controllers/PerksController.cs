using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BARTPerks.Models;

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
            ViewBag.Message = "submitted";

            return View(model);
        }
    }
}