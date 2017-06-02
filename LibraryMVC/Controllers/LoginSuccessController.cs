using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryMVC.Controllers
{
    public class LoginSuccessController : Controller
    {
        // GET: LoginSuccess
        [HttpGet ]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                if(Session["LoginInfo"]==null)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            return RedirectToAction("Login", "Login");
        }
    }
}