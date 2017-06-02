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
    public class BooksController : Controller
    {
        OrisonSystemSCHOOLEntities db = new OrisonSystemSCHOOLEntities();

        // GET: Books
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
            OrisonSystemSCHOOLEntities e = new OrisonSystemSCHOOLEntities();
            var books = e.LibraryBookMasterListSP();

            return View(books.ToList());
        }

        public ActionResult Detail(int ID)
        {
            OrisonSystemSCHOOLEntities e = new OrisonSystemSCHOOLEntities();
            School_LibraryBookMaster bookD = (e.School_LibraryBookMaster.Where(u => u.BookMastID == ID)).Single();
            return View(bookD);
        }

        [HttpGet]
        [MultipleButton(Name = "action", Argument = "Edit")]
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School_LibraryBookMaster bookToUpdate = db.School_LibraryBookMaster.Where(i => i.BookMastID == ID).Single();
            if (bookToUpdate == null)
            {
                return HttpNotFound();
            }
            return View(bookToUpdate);
        }

        public bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
        
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        public ActionResult Save(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bookToUpdate = db.School_LibraryBookMaster.Where(i => i.BookMastID == ID).Single();
            if (TryUpdateModel(bookToUpdate))
            {
                try
                {
                    HttpPostedFileBase upload = Request.Files[0];
                    if (HasFile(upload))
                    {
                        string mimeType = upload.ContentType;
                        System.IO.Stream fileStream = upload.InputStream;
                        string fileName = System.IO.Path.GetFileName(upload.FileName);
                        int fileLength = upload.ContentLength;
                        byte[] fileData = new byte[fileLength];
                        fileStream.Read(fileData, 0, fileLength);
                        bookToUpdate.img = fileData;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Detail", new { ID = ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }
            return View(bookToUpdate);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Back")]
        public ActionResult Back(int? ID)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Cancel")]
        public ActionResult Cancel(int? ID)
        {
            return RedirectToAction("Detail", new { ID = (int)ID });
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Search")]
        public ActionResult Search(string txtISBN)
        {

            return RedirectToAction("Index", "ISBN", new { isbn = txtISBN });
        }

        [HttpGet]
        [MultipleButton(Name = "action", Argument = "AddCopy")]
        public ActionResult AddCopy(int? ID)
        {
            School_LibraryBookDetails bd = new Models.School_LibraryBookDetails();
            bd.BookMastID = (int)ID;
            string sqlQuery = "SELECT [dbo].[getNextAccessionNo] ()";
            string newcode = db.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
            bd.AccessionNo = newcode;
            return View(bd);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SaveCopy")]
        public ActionResult SaveCopy(int? ID)
        {
            var copyToUpdate = new School_LibraryBookDetails();
            if (TryUpdateModel(copyToUpdate))
            {
                try
                {
                    OrisonSystemSCHOOLEntities db = new OrisonSystemSCHOOLEntities();
                    if (copyToUpdate.AccessionNo == null || copyToUpdate.AccessionNo == "")
                    {
                        string sqlQuery = "SELECT [dbo].[getNextAccessionNo] ()";
                        string newcode = db.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
                        copyToUpdate.AccessionNo = newcode;
                    }
                    db.School_LibraryBookDetails.Add(copyToUpdate);
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

        //[HttpGet]
        //[MultipleButton(Name = "action", Argument = "CopyDetail")]
        public ActionResult CopyDetail(int ID)
        {
            return RedirectToAction("Index","BookCopy",new { ID=ID});
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "BackToBook")]
        public ActionResult BackToBook(int? BookMastID)
        {
            return RedirectToAction("Detail", new { ID = (int)BookMastID });
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "AddBook")]
        public ActionResult AddBook(int? ID)
        {
            School_LibraryBookMaster bd = new Models.School_LibraryBookMaster();
            string sqlQuery = "SELECT [dbo].[getNextBookCode] ()";
            string newcode = db.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
            bd.BookCode  = newcode;
            return View(bd);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SaveBook")]
        public ActionResult SaveBook(int? ID)
        {
            var bookToUpdate = new School_LibraryBookMaster();
            if (TryUpdateModel(bookToUpdate))
            {
                try
                {
                    HttpPostedFileBase upload = Request.Files[0];
                    if (HasFile(upload))
                    {
                        string mimeType = upload.ContentType;
                        System.IO.Stream fileStream = upload.InputStream;
                        string fileName = System.IO.Path.GetFileName(upload.FileName);
                        int fileLength = upload.ContentLength;
                        byte[] fileData = new byte[fileLength];
                        fileStream.Read(fileData, 0, fileLength);
                        bookToUpdate.img = fileData;
                    }
                    db.School_LibraryBookMaster.Add(bookToUpdate);
                    db.SaveChanges();
                    return RedirectToAction("Detail", new { ID = bookToUpdate.BookMastID });
                }
                catch (RetryLimitExceededException )
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }
            return View(bookToUpdate);
        }

        [HttpPost]
        public PartialViewResult SearchGrid(string searchString)
        {
            List< LibraryBookMasterListSP_Result> bL = db.LibraryBookMasterListSP().ToList();
            if (Request.IsAjaxRequest())
            {
                if (!string.IsNullOrEmpty(searchString) && bL != null && bL.Count() > 0)
                {
                    var searchedlist = (from LibraryBookMasterListSP_Result list in bL where ((list.BOOK_CODE==null?"":list.BOOK_CODE).IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 || (list.ISBN == null ? "" : list.ISBN).IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 || (list.TITLE == null ? "" : list.TITLE).IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) select list).ToList();
                    return PartialView("BookList", searchedlist);
                }
                else
                {
                    return PartialView("BookList", bL);
                }
            }
            else
            {
                return PartialView("BookList", bL);
            }
        }

        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "EditCopy")]
        //public ActionResult EditCopy(int? ID)
        //{
        //    School_LibraryBookDetails bd = db.School_LibraryBookDetails.Where(b => b.BookID == (int)ID).Single();
        //    return View(bd);
        //}

        //public ActionResult EditCopyAlt(int? ID)
        //{
        //    return  EditCopy((int)ID);
        //}

        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "SaveAsCopy")]
        //public ActionResult SaveAsCopy(int? BookID)
        //{
        //    var copyToUpdate = db.School_LibraryBookDetails.Where(b=>b.BookID ==(int)BookID).Single();
        //    if (TryUpdateModel(copyToUpdate))
        //    {
        //        try
        //        {
        //            if (copyToUpdate.AccessionNo == null || copyToUpdate.AccessionNo == "")
        //            {
        //                string sqlQuery = "SELECT [dbo].[getNextAccessionNo] ()";
        //                string newcode = db.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        //                copyToUpdate.AccessionNo = newcode;
        //            }
        //            db.SaveChanges();
        //            return RedirectToAction("Detail", "Books", new { ID = copyToUpdate.BookMastID });
        //        }
        //        catch (RetryLimitExceededException /* dex */)
        //        {
        //            //Log the error (uncomment dex variable name and add a line here to write a log.
        //            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
        //        }
        //        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //        {
        //            var obj = ex.EntityValidationErrors;
        //        }
        //    }
        //    return View(copyToUpdate);
        //}

        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "CancelEditCopy")]
        //public ActionResult CancelEditCopy(int? BookMastID)
        //{
        //    return RedirectToAction("Detail", new { ID = (int)BookMastID });
        //}
    }
}