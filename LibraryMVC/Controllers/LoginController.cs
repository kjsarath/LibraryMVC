using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryMVC.Models;
using System.Data.Entity;
using System.Web.Security;

namespace LibraryMVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            Session["LoginInfo"] = null;
            return View();
        }

        [HttpPost ]
        public ActionResult Login(PortalUser puser)
        {
            if (ModelState.IsValid)
            {
                String pw = enCrypt(puser.Password);
                OrisonSystemSCHOOLEntities e = new OrisonSystemSCHOOLEntities();
                if (e.PortalUsers.Where(u=>u.UserName==puser.UserName && u.Password==pw ).Count() > 0)
                {
                    FormsAuthentication.SetAuthCookie(puser.UserName, true);
                    Session["LoginInfo"] = puser.ID;
                    return RedirectToAction("Index","LoginSuccess");
                }
                else
                {
                    ViewBag.Message = "Your credentials are wrong...";
                    return View("Login");
                }

            }
            return View("Login");
        }
        public static string enCrypt(string Pwd)
        {
            char[] PwdChar = Pwd.ToCharArray();
            string deCodedPwd = "";
            foreach (char c in PwdChar)
            {
                deCodedPwd = deCodedPwd + (char)(Convert.ToInt32(c) + 10);
            }
            return deCodedPwd;
        }

        public ActionResult LoginSuccess()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}