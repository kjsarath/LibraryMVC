using LibraryMVC.DAL;
using LibraryMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryMVC.Controllers
{
    public class BookCopyController : Controller
    {
        OrisonSystemSCHOOLEntities db = new OrisonSystemSCHOOLEntities();
        
        public ActionResult Index(int? ID)
        {
            OrisonSystemSCHOOLEntities e = new OrisonSystemSCHOOLEntities();
            School_LibraryBookDetails copyD = (e.School_LibraryBookDetails.Where(u => u.BookID == ID)).Single();
            return View(copyD);
        }

        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "EditCopy")]
        public ActionResult EditCopy(int? ID)
        {
            School_LibraryBookDetails bd = db.School_LibraryBookDetails.Where(b => b.BookID == (int)ID).Single();
            return View(bd);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SaveAsCopy")]
        public ActionResult SaveAsCopy(int? ID)
        {
            var copyToUpdate = db.School_LibraryBookDetails.Where(b => b.BookID == (int)ID).Single();
            if (TryUpdateModel(copyToUpdate))
            {
                try
                {
                    if (copyToUpdate.AccessionNo == null || copyToUpdate.AccessionNo == "")
                    {
                        string sqlQuery = "SELECT [dbo].[getNextAccessionNo] ()";
                        string newcode = db.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
                        copyToUpdate.AccessionNo = newcode;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Detail", "Books", new { ID = copyToUpdate.BookMastID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var obj = ex.EntityValidationErrors;
                }
            }
            return View(copyToUpdate);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CancelEditCopy")]
        public ActionResult CancelEditCopy(int? BookMastID)
        {
            return RedirectToAction("Detail","Books", new { ID = (int)BookMastID });
        }


    }
}