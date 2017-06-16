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

        [HttpPost]
        [MultipleButton(Name ="action",Argument ="Add")]
        public ActionResult Add()
        {
            var rD = new LibraryBookReturnDetailsSP_Result();
            String rNo = db.Database.SqlQuery<String>("SELECT dbo.getNextReturnNo()").ToList().FirstOrDefault();
            rD.ReturnNo = rNo;
            rD.ReturnDate = DateTime.Today;
            return View(rD);
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

        public ActionResult Details(int? ID)
        {
            var rD = db.LibraryBookReturnDetailsSP((int)ID, "%").FirstOrDefault();
            return View(rD);
        }
            
        [HttpPost]
        [MultipleButton(Name ="action",Argument ="Back")]
        public ActionResult Back(int? ID)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultipleButton(Name ="action",Argument ="Save")]
        public ActionResult Save(int? ID)
        {
            if(ID==null)
            {
                var rD = new LibraryBookReturnDetailsSP_Result();
                if (TryUpdateModel(rD))
                {
                    String rNo = db.Database.SqlQuery<String>("SELECT dbo.getNextReturnNo()").ToList().FirstOrDefault();
                    rD.ReturnNo = rNo;
                    var r = new School_LibraryReturn();
                    r.RetunNo = rNo;
                    r.IssueId = rD.IssueID;
                    r.Modifiedtime = DateTime.Now.ToShortDateString();
                    r.ModifiedUser = "0";
                    r.Remarks = rD.Remarks;
                    r.ReturnDate = rD.ReturnDate;
                    r.RType = "Returned";
                    db.School_LibraryReturn.Add(r);
                    db.SaveChanges();
                    var isD = db.School_LibraryIssue.Where(i => i.ID == rD.IssueID).FirstOrDefault();
                    isD.IssueStatus = "Returned";
                    isD.Fine = rD.Fine;
                    isD.RID = r.ID;
                    var bd = db.School_LibraryBookDetails.Where(d => d.BookID == rD .BookID).Single();
                    if (isD.IssueStatus == "Returned") bd.IssueStatus = "In";
                    else bd.IssueStatus = "Out";
                    db.SaveChanges();
                }
            }
            else
            {
                var rD = db.LibraryBookReturnDetailsSP((int)ID, "%").FirstOrDefault();
                if (TryUpdateModel(rD))
                {
                    var r = db.School_LibraryReturn.Where(d => d.ID == (int)ID).FirstOrDefault();
                    r.IssueId = rD.IssueID;
                    r.Modifiedtime = DateTime.Now.ToShortDateString();
                    r.ModifiedUser = "0";
                    r.Remarks = rD.Remarks;
                    r.ReturnDate = rD.ReturnDate;
                    r.RType = "Returned";
                    var isD = db.School_LibraryIssue.Where(i => i.ID == rD.IssueID).FirstOrDefault();
                    isD.IssueStatus = "Returned";
                    isD.Fine = rD.Fine;
                    var bd = db.School_LibraryBookDetails.Where(d => d.BookID == rD.BookID).Single();
                    if (isD.IssueStatus == "Returned") bd.IssueStatus = "In";
                    else bd.IssueStatus = "Out";
                    db.SaveChanges();
                }
            }
            return View();
        }

        public ActionResult ViewIssueSelection()
        {
            var isL = db.LibraryBookIssueListSP("%", "%").Where(ir=>ir.IssueStatus!="Returned").ToList();
            return View("IssueSelection", isL);
        }

        [HttpPost]
        public PartialViewResult SearchIssue(string searchText)
        {
            if (Request.IsAjaxRequest())
            {
                if (searchText != null && searchText != "")
                {
                    var biL = db.LibraryBookIssueListSP("%","%").Where(ir => ir.IssueStatus != "Returned" && ((ir.AccountCode  == null ? "" : ir.AccountCode ).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.AccessionNo  == null ? "" : ir.AccessionNo ).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.Title == null ? "" : ir.Title).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.BookCode  == null ? "" : ir.BookCode ).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.IssueNo  == null ? "" : ir.IssueNo).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.MemberName  == null ? "" : ir.MemberName).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
                    return PartialView("IssueSelectionList", biL);
                }
                else
                {
                    return PartialView("IssueSelectionList", db.LibraryBookIssueListSP("%", "%").Where(ir => ir.IssueStatus != "Returned").ToList());
                }
            }
            else
            {
                return PartialView("IssueSelectionList", db.LibraryBookIssueListSP("%", "%").Where(ir => ir.IssueStatus != "Returned").ToList());
            }
        }

        [HttpGet]
        public JsonResult FillIssueData(int? ID)
        {
            if (ID == null)
            {
                return Json(null);
            }
            var mem = db.LibraryGetIssueDetails("%","%", (int)ID).Single();
            return Json(mem, JsonRequestBehavior.AllowGet);
        }
    }
}