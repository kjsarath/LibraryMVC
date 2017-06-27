using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LibraryMVC.Controllers
{
    public class MainController : Controller
    {
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                if (Session["LoginInfo"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            return View();
        }

        [HttpPost ]
        public JsonResult Chart1()
        {
            List<object> iData = new List<object>();
            List<string> labels = new List<string>();
            labels.Add("January");
            labels.Add("February");
            labels.Add("March");
            labels.Add("April");
            List<int> lst_dataItem_1 = new List<int>();
            lst_dataItem_1.Add(1);
            lst_dataItem_1.Add(2);
            lst_dataItem_1.Add(3);
            lst_dataItem_1.Add(4);
            iData.Add(labels);
            iData.Add(lst_dataItem_1);
            return Json(iData, JsonRequestBehavior.AllowGet);
        }
    }
}