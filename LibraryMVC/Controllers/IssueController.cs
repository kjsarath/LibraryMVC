using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryMVC.Models;
using LibraryMVC.DAL;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace LibraryMVC.Controllers
{
    public class IssueController : Controller
    {
        OrisonSystemSCHOOLEntities db = new OrisonSystemSCHOOLEntities();

        public ActionResult Index()
        {
            if(ModelState.IsValid )
            {
                if (Session["LoginInfo"] == null)
                    return RedirectToAction("Login", "Login");
            }
            var biL = db.LibraryBookIssueListSP("%", "%");
            return View(biL.ToList());
        }

        [HttpPost]
        public PartialViewResult SearchGrid(string searchText)
        {
            var biL = db.LibraryBookIssueListSP("%", "%").ToList();
            if (Request.IsAjaxRequest())
            {
                if (searchText != null && searchText != "" && biL.Count > 0)
                {
                    var newL = (from LibraryBookIssueListSP_Result ir in biL where ((ir.Title == null ? "" : ir.Title).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.BookCode == null ? "" : ir.BookCode).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.AccountCode == null ? "" : ir.AccountCode).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.MemberName == null ? "" : ir.MemberName).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) select ir).ToList();
                    return PartialView("IssueList", newL);
                }
                else
                {
                    return PartialView("IssueList", biL);
                }
            }
            else
            {
                return PartialView("IssueList", biL);
            }
        }

        public ActionResult Details(int? ID)
        {
            var isD = db.LibraryGetIssueDetails("%", "%", (int)ID).ToList().Single();
            return View(isD);
        }

        [HttpPost ]
        [MultipleButton (Name ="action",Argument ="Add")]
        public ActionResult Add(int? ID)
        {
            var biD = new LibraryGetIssueDetails_Result();
            string niNum = db.Database.SqlQuery<string>("Select dbo.getNextIssueNo()").FirstOrDefault();
            biD.IssueNo = niNum;
            biD.IssueDate = DateTime.Today;
            DataTable dtAcc = clsDBOperations.GetTable("select distinct lbd.AccessionNo,lbd.BookID,lbm.Title,lbm.BookCode,lbm.BookMastID from School_LibraryBookMaster lbm Inner Join School_LibraryBookDetails lbd on lbm.BookMastID=lbd.BookMastID", db);
            ViewBag.AccessionList = (from DataRow  dr in dtAcc.Rows select dr).Select(c=> new Tuple<string, int>(c["AccessionNo"].ToString(), int.Parse(c["BookID"].ToString()))).ToList();
            //ViewBag.BookCodeList = (from DataRow dr in dtAcc.Rows select dr).Select(c => new Tuple<string, int>(c["BookCode"].ToString(), int.Parse(c["BookMastID"].ToString()))).Distinct().ToList();
            return View(biD);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        public ActionResult Save(int? ID)
        {
            var biD = new LibraryGetIssueDetails_Result();
            if (TryUpdateModel(biD))
            {
                try
                {
                    var x = new School_LibraryIssue();
                    x.AccessionNo = biD.AccessionNo;
                    x.IssueNo = biD.IssueNo;
                    x.IssueDate = biD.IssueDate;
                    x.FromDate = biD.IssueDate;
                    x.ToDate = biD.ToDate;
                    x.MaxDays = biD.NoOfDays;
                    x.MemberID = biD.MemberID;
                    x.MemberType = biD.Description;
                    x.BookID = biD.BookID;
                    x.Title = biD.Title;
                    x.IssueStatus = "Out";

                    db.School_LibraryIssue.Add(x);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { ID = x.ID });
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Back")]
        public ActionResult Back(int? ID)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult FillWithAccessionNo(int? ID)
        {
            if (ID == null)
            {
                return Json(null);
            }
            int bmID = db.Database.SqlQuery<int>("Select BookMastID from School_LibraryBookDetails where BookID=" + ID.ToString() + "").FirstOrDefault();
            var books = db.LibraryBookMasterListSP().Where(b=>b.BookMastID==bmID ).Single();
            return Json(books, JsonRequestBehavior.AllowGet);
        }
    }
}