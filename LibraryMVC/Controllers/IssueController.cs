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
            if ((int)ID == 0)
            {
                var biD = new LibraryGetIssueDetails_Result();
                if (TryUpdateModel(biD))
                {
                    try
                    {
                        var x = new School_LibraryIssue();
                        x.AccessionNo = biD.AccessionNo;
                        x.IssueNo = biD.IssueNo;
                        x.IssueDate = biD.FromDate;
                        x.FromDate = biD.FromDate;
                        x.ToDate = biD.ToDate;
                        x.MaxDays = biD.NoOfDays;
                        x.MemberID = biD.MemberID;
                        x.MemberType = biD.Description;
                        x.BookID = biD.BookID;
                        x.Title = biD.Title;
                        x.IssueStatus = "Issued";

                        db.School_LibraryIssue.Add(x);

                        var bd = db.School_LibraryBookDetails.Where(d => d.BookID == biD.BookID).Single();
                        if(biD.issuedstatus=="Returned")bd.IssueStatus = "In";
                        else bd.IssueStatus = "Out";

                        db.SaveChanges();
                        return RedirectToAction("Details", new { ID = x.ID });
                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                    }
                }
            }
            else
            {
                var biD = db.LibraryGetIssueDetails("%", "%", (int)ID).Single();
                if (TryUpdateModel(biD))
                {
                    try
                    {
                        var x =db.School_LibraryIssue.Where(s=>s.ID==(int)ID).Single();
                        x.AccessionNo = biD.AccessionNo;
                        x.IssueNo = biD.IssueNo;
                        x.IssueDate = biD.FromDate;
                        x.FromDate = biD.FromDate;
                        x.ToDate = biD.ToDate;
                        x.MaxDays = biD.NoOfDays;
                        x.MemberID = biD.MemberID;
                        x.MemberType = biD.Description;
                        x.BookID = biD.BookID;
                        x.Title = biD.Title;
                        x.IssueStatus = "Issued";

                        var bd = db.School_LibraryBookDetails.Where(d => d.BookID == biD.BookID).Single();
                        if (biD.issuedstatus == "Returned") bd.IssueStatus = "In";
                        else bd.IssueStatus = "Out";

                        db.SaveChanges();
                        return RedirectToAction("Details", new { ID = x.ID });
                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [MultipleButton(Name = "action", Argument = "Edit")]
        public ActionResult Edit(int? ID)
        {
            var biD = db.LibraryGetIssueDetails("%","%",(int)ID).Single();
            DataTable dtAcc = clsDBOperations.GetTable("select distinct lbd.AccessionNo,lbd.BookID,lbm.Title,lbm.BookCode,lbm.BookMastID from School_LibraryBookMaster lbm Inner Join School_LibraryBookDetails lbd on lbm.BookMastID=lbd.BookMastID", db);
            ViewBag.AccessionList = (from DataRow dr in dtAcc.Rows select dr).Select(c => new Tuple<string, int>(c["AccessionNo"].ToString(), int.Parse(c["BookID"].ToString()))).ToList();
            return View("Add",biD);
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
            var bk = db.LibraryBookCopyDetailsSP((int)ID).Single();
            return Json(bk, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FillBookData(int? ID)
        {
            if (ID == null)
            {
                return Json(null);
            }
            var bk = db.LibraryBookCopyDetailsSP((int)ID).Single();
            return Json(bk, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewBookSelection()
        {
            var bks = db.LibraryBookCopyDetailsSP(0).ToList();
            return PartialView("BookSelection",bks);
        }

        [HttpPost]
        public PartialViewResult SearchBooks(string searchText)
        {
            if (Request.IsAjaxRequest())
            {
                if (searchText != null && searchText != "")
                {
                   var biL = db.LibraryBookCopyDetailsSP(0).Where(ir => ((ir.Title == null ? "" : ir.Title).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.BookCode == null ? "" : ir.BookCode).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.AccessionNo == null ? "" : ir.AccessionNo).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
                    return PartialView("BookSelectionList", biL);
                }
                else
                {
                    return PartialView("BookSelectionList", db.LibraryBookCopyDetailsSP(0).ToList());
                }
                //if (Request.IsAjaxRequest())
                //{
                //    if (searchText != null && searchText != "" && biL.Count > 0)
                //    {
                //        var newL = (from LibraryBookCopyDetailsSP_Result ir in biL where ((ir.Title == null ? "" : ir.Title).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.BookCode == null ? "" : ir.BookCode).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.AccessionNo == null ? "" : ir.AccessionNo).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ) select ir).ToList();
                //        return PartialView("BookSelectionList", newL);
                //    }
                //    else
                //    {
                //        return PartialView("BookSelectionList", biL);
                //    }
                //}
            }
            else
            {
                return PartialView("BookSelectionList", db.LibraryBookCopyDetailsSP(0).ToList());
            }
        }

        public ActionResult ViewMemberSelection()
        {
            var mems = db.LibraryMembersListSP().ToList();
            return PartialView("MemberSelection", mems);
        }

        [HttpPost]
        public PartialViewResult SearchMembers(string searchText)
        {
            if (Request.IsAjaxRequest())
            {
                if (searchText != null && searchText != "")
                {
                    var biL = db.LibraryMembersListSP().Where(ir => ((ir.Member_Code  == null ? "" : ir.Member_Code).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (ir.Member_Name == null ? "" : ir.Member_Name).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 )).ToList();
                    return PartialView("MemberSelectionList", biL);
                }
                else
                {
                    return PartialView("MemberSelectionList", db.LibraryMembersListSP().ToList());
                }
            }
            else
            {
                return PartialView("MemberSelectionList", db.LibraryMembersListSP().ToList());
            }
        }

        [HttpGet]
        public JsonResult FillMemberData(int? ID)
        {
            if (ID == null)
            {
                return Json(null);
            }
            var mem = db.LibraryMemberDetailsSP (0,(int)ID).Single();
            return Json(mem, JsonRequestBehavior.AllowGet);
        }
    }
}