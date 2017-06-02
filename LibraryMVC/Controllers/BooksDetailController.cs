using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryMVC.Controllers
{
    public class BooksDetailController : Controller
    {
        // GET: BooksDetail
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
    }
}