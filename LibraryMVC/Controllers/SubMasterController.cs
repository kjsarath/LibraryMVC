using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryMVC.Models;
using LibraryMVC.DAL;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace LibraryMVC.Controllers
{
    public class SubMasterController : Controller
    {
        OrisonSystemSCHOOLEntities db = new OrisonSystemSCHOOLEntities();
        
        [HttpGet ]
        [OutputCache(NoStore =false,Duration =0,VaryByParam ="*")]
        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                if (Session["LoginInfo"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            var subL = db.LibrarySubMasterListSP("%").ToList();
            return View(subL);
        }

        [HttpPost]
        public PartialViewResult SearchGrid(string searchString)
        {
            var subL = db.LibrarySubMasterListSP("%").ToList();
            if (Request.IsAjaxRequest())
            {
                if (searchString != null && searchString != "" && subL.Count > 0)
                {
                    var newL = (from LibrarySubMasterListSP_Result list in subL where ((list.Category  == null ? "" : list.Category).IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 || (list.Description  == null ? "" : list.Description).IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) select list).ToList();
                    return PartialView("SubMasterList", newL);
                }
                else
                {
                    return PartialView("SubMasterList", subL);
                }
            }
            else
            {
                return PartialView("SubMasterList", subL);
            }
        }

        public ActionResult Detail(int? ID)
        {
            //var smD = db.LibrarySubMasterDetailsSP(ID).ToList().Single();
            //return View(smD);
            return RedirectToAction("Edit", new { ID = (int)ID });
        }

        [HttpPost]
        [MultipleButton(Name = "action",Argument ="Add")]
        public ActionResult Add(int? ID)
        {
            LibrarySubMasterDetailsSP_Result smD = new Models.LibrarySubMasterDetailsSP_Result();
            return View(smD);
        }

        [HttpPost]
        [MultipleButton(Name="action",Argument ="Save")]
        public ActionResult Save(int? ID)
        {
            var smD = new LibrarySubMasterDetailsSP_Result();
            if(TryUpdateModel(smD ))
            {
                try
                {
                    var lCM = new School_LibraryCategoryMaster();
                    lCM.Category = smD.Category;
                    lCM.Description = smD.Description;
                    db.School_LibraryCategoryMaster.Add(lCM);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet ]
        //[MultipleButton(Name ="action",Argument ="Edit")]
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var smD = db.LibrarySubMasterDetailsSP((int)ID).ToList().Single();
            if (smD == null)
            {
                return HttpNotFound();
            }
            return View(smD);
        }

        [HttpPost]
        [MultipleButton(Name ="action",Argument ="Cancel")]
        public ActionResult Cancel(int? ID)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Back")]
        public ActionResult Back(int? ID)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SaveEdit")]
        public ActionResult SaveEdit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var smD = db.LibrarySubMasterDetailsSP((int)ID).ToList().Single();
            if(TryUpdateModel(smD))
            {
                try
                {
                    School_LibraryCategoryMaster lCM = db.School_LibraryCategoryMaster.Where(l => l.ID == (int)ID).Single();
                    lCM.Category = smD.Category;
                    lCM.Description = smD.Description;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }
            return View(smD);
        }
    }
}