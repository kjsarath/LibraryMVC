using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryMVC.Models;
using LibraryMVC.DAL;

namespace LibraryMVC.Controllers
{
    public class ReturnController : Controller
    {
        OrisonSystemSCHOOLEntities db = new OrisonSystemSCHOOLEntities();

        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                if (Session["LoginInfo"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            var rL = db.LibraryBookReturnListSP("%","%",DateTime.Today  ).ToList();
            return View(rL);

        }

        [MultipleButton(Name ="action",Argument ="Add")]
        public ActionResult Add()
        {
            return View();
        }

        public PartialViewResult SearchGrid(String searchText)
        {
            var rL = db.LibraryBookReturnListSP("%", "%", DateTime.Today).ToList();
            if (Request.IsAjaxRequest())
            {
                if (searchText != null && searchText != "" && rL.Count > 0)
                {
                    var nrL = (from LibraryBookReturnListSP_Result ir in rL where ((ir.AccessionNo == null ? "" : ir.AccessionNo).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.ReturnNo == null ? "" : ir.ReturnNo).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.Title == null ? "" : ir.Title).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.IssueNo  == null ? "" : ir.IssueNo).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.MemberCode == null ? "" : ir.MemberCode).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.MemberName == null ? "" : ir.MemberName).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) select ir).ToList();
                    return PartialView("ReturnList", nrL);
                }
                else
                {
                    return PartialView("ReturnList", rL);
                }
            }
            else
            {
                return PartialView("ReturnList", rL );
            }
        }
    }
}