using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryMVC.Models;
using System.Data.Entity.Infrastructure;
using System.Net;
using LibraryMVC.DAL;

namespace LibraryMVC.Controllers
{
    public class MembersController : Controller
    {
        OrisonSystemSCHOOLEntities db = new OrisonSystemSCHOOLEntities();

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
            var members = db.LibraryMembersListSP();
            return View(members.ToList());
        }

        [HttpPost ]
        public PartialViewResult SearchGrid(string searchString)
        {
            List<LibraryMembersListSP_Result> mL = db.LibraryMembersListSP().ToList();
            if(Request.IsAjaxRequest())
            {
                if(!string.IsNullOrEmpty(searchString) && mL!=null && mL.Count>0)
                {
                    var searchedlist = (from LibraryMembersListSP_Result list in mL where ((list.Member_Code  == null ? "" : list.Member_Code).IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 || (list.Member_Name  == null ? "" : list.Member_Name ).IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) select list).ToList();
                    return PartialView("MemberList", searchedlist);
                }
                else
                {
                    return PartialView("MemberList", mL);
                }
            }
            else
            {
                return PartialView("MemberList", mL);
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Add")]
        public ActionResult Add(int? ID)
        {
            LibraryMemberDetailsSP_Result  md = new Models.LibraryMemberDetailsSP_Result();
            string sqlQuery = "SELECT [dbo].[getNextMemberNo] ()";
            string newcode = db.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
            md.Code  = newcode;
            return View(md);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Back")]
        public ActionResult Back(int? ID)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        public ActionResult Save(int? ID)
        {
            var memberToAdd = new LibraryMemberDetailsSP_Result ();
            if (TryUpdateModel(memberToAdd))
            {
                try
                {
                    Accounts ma = new Accounts();
                    ma.AccountCode = memberToAdd.Code;
                    ma.AccountName = memberToAdd.Name ;
                    string sqlQuery = "Select ID from Accounts where AccountName like 'Members'";
                    Int32 parent = db.Database.SqlQuery<Int32>(sqlQuery).FirstOrDefault();
                    ma.Parent =parent;
                    ma.ParentLevel = 4;
                    ma.VoucherEntry = true;
                    ma.FinalAccount = false;
                    ma.AccountGroup =1;
                    ma.SubGroup = 19;
                    sqlQuery = "Select ID from AccountCategoryMast where Description like 'Member'";
                    int accCat = db.Database.SqlQuery<int>(sqlQuery).FirstOrDefault();
                    ma.AccCategory = accCat;
                    ma.InActive = false;
                    ma.BranchID = 31;
                    ma.CreatedDate = DateTime.Now ;
                    ma.ModifiedDate = DateTime.Now;
                    db.Accounts.Add(ma);
                    db.SaveChanges();

                    Library_Members lm = new Library_Members();
                    lm.Code = memberToAdd.Code;
                    lm.Name = memberToAdd.Name;
                    lm.Category = memberToAdd.Category;
                    lm.ContactNum = memberToAdd.ContactNum;
                    lm.ContactNumAlt = memberToAdd.ContactNumAlt;
                    lm.ContactEmail = memberToAdd.ContactEmail;
                    lm.JoiningDate = memberToAdd.JoiningDate;
                    lm.Address1 = memberToAdd.Address1;
                    lm.Address2 = memberToAdd.Address2;
                    lm.Area = memberToAdd.Area;
                    lm.Emirates = memberToAdd.Emirates;
                    lm.Nationality = memberToAdd.Nationality;
                    lm.Status = memberToAdd.Status;
                    lm.Remarks = memberToAdd.Remarks;
                    lm.AccountID = ma.ID;
                    db.Library_Members.Add(lm);
                    db.SaveChanges();
                    //return RedirectToAction("Detail", new { ID = bookToUpdate.BookMastID });
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int ID)
        {
            OrisonSystemSCHOOLEntities e = new OrisonSystemSCHOOLEntities();
            LibraryMemberDetailsSP_Result  memD = (e.LibraryMemberDetailsSP(ID)).Single();
            return View(memD);
        }

        [HttpGet]
        [MultipleButton(Name = "action", Argument = "Edit")]
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryMemberDetailsSP_Result memToUpdate = db.LibraryMemberDetailsSP(ID).Single();
            if (memToUpdate == null)
            {
                return HttpNotFound();
            }
            return View(memToUpdate);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SaveEdit")]
        public ActionResult SaveEdit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var memToUpdate = db.LibraryMemberDetailsSP(ID).Single();
            if (TryUpdateModel(memToUpdate))
            {
                try
                {
                    Accounts ma = (db.Accounts.Where(a=>a.ID==memToUpdate.AccountID)).Single();
                    ma.AccountCode = memToUpdate.Code;
                    ma.AccountName = memToUpdate.Name;
                    ma.ModifiedDate = DateTime.Now;

                    Library_Members lm = (db.Library_Members.Where(a => a.ID == ID)).Single();
                    lm.Code = memToUpdate.Code;
                    lm.Name = memToUpdate.Name;
                    lm.Category = memToUpdate.Category;
                    lm.ContactNum = memToUpdate.ContactNum;
                    lm.ContactNumAlt = memToUpdate.ContactNumAlt;
                    lm.ContactEmail = memToUpdate.ContactEmail;
                    lm.JoiningDate = memToUpdate.JoiningDate;
                    lm.Address1 = memToUpdate.Address1;
                    lm.Address2 = memToUpdate.Address2;
                    lm.Area = memToUpdate.Area;
                    lm.Emirates = memToUpdate.Emirates;
                    lm.Nationality = memToUpdate.Nationality;
                    lm.Status = memToUpdate.Status;
                    lm.Remarks = memToUpdate.Remarks;
                 
                    db.SaveChanges();
                    return RedirectToAction("Detail", new { ID = ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }
            return View(memToUpdate);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Cancel")]
        public ActionResult Cancel(int? ID)
        {
            return RedirectToAction("Detail",new { ID = ID });
        }
    }
}