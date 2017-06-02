using LibraryMVC.DAL;
using LibraryMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace LibraryMVC.Controllers
{
    public class ISBNController : Controller
    {
        School_LibraryBookMaster bookD = new School_LibraryBookMaster();
        byte[] img ;
        // GET: ISBN
        public ActionResult Index(string isbn)
        {
            if (isbn == null)
            {
                return RedirectToAction("Index", "Books");
            }
            getFromISBN(isbn);
            return View(bookD);
        }

        [NonAction]
        private void getFromISBN(string isbn)
        {
            String ISBN = isbn;
            
            
            try
            {
                System.Net.HttpWebRequest oHTTPRequest = System.Net.HttpWebRequest.Create("https://www.googleapis.com/books/v1/volumes?q=isbn:" + ISBN) as System.Net.HttpWebRequest;
                oHTTPRequest.ContentType = "Json";
                string result = "";
                try
                {
                    Stream response = oHTTPRequest.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(response);
                    result = reader.ReadToEnd();
                }
                finally
                {
                }
                System.Xml.XmlDocument doc = JsonConvert.DeserializeXmlNode(result, "root");
                XmlReader xmlReader = new XmlNodeReader(doc);
                DataSet ds = new DataSet();
                ds.ReadXml(xmlReader);
                if (ds.Tables.Contains("industryIdentifiers"))
                {
                    DataColumnCollection columns = ds.Tables["industryIdentifiers"].Columns;
                    if (columns.Contains("identifier"))
                    {
                        if (ds.Tables["IndustryIdentifiers"].Rows[0]["type"].ToString() == "ISBN_13" || ds.Tables["IndustryIdentifiers"].Rows[0]["type"].ToString() == "OTHER")
                        {
                            bookD.ISBN  = ds.Tables["industryIdentifiers"].Rows[0]["identifier"].ToString();
                        }
                        else
                        {
                            bookD.ISBN = ds.Tables["industryIdentifiers"].Rows[1]["identifier"].ToString();
                        }
                    }
                    else
                    {
                        bookD.ISBN = "";
                    }
                }
                if (ds.Tables.Contains("imageLinks"))
                {
                    DataColumnCollection columns = ds.Tables["imageLinks"].Columns;
                    if (columns.Contains("thumbnail"))
                    {
                        string sURL = ds.Tables["imageLinks"].Rows[0]["thumbnail"].ToString().Trim();
                        System.Net.WebClient webClient = new System.Net.WebClient();
                        bookD.img = webClient.DownloadData(sURL);
                        img = webClient.DownloadData(sURL);
                        //imgBook.Value  = sURL;
                    }
                    else
                    {
                    }
                }
                if (ds.Tables.Contains("volumeInfo"))
                {
                    DataColumnCollection columns = ds.Tables["volumeInfo"].Columns;
                    string title;
                    if (columns.Contains("Title"))
                    {
                        title = ds.Tables["volumeInfo"].Rows[0]["title"].ToString();
                        bookD.Title  = title.Replace("'", "");
                    }
                    else
                    {
                        bookD.Title  = "";
                    }
                    string subtitle;
                    if (columns.Contains("Subtitle"))
                    {
                        subtitle = ds.Tables["volumeInfo"].Rows[0]["Subtitle"].ToString();
                        bookD.SubTitle  = subtitle.Replace("'", "");
                    }
                    else
                    {
                        bookD.SubTitle = "";
                    }
                    string catgory;
                    if (columns.Contains("categories"))
                    {
                        catgory = ds.Tables["volumeInfo"].Rows[0]["categories"].ToString();
                        bookD.Category  = catgory.Replace("'", "");
                    }
                    else
                    {
                        bookD.Category = "";
                    }
                    string lang;
                    if (columns.Contains("Language"))
                    {
                        lang = ds.Tables["volumeInfo"].Rows[0]["Language"].ToString();
                        bookD.language  = lang.Replace("'", "");
                    }
                    else
                    {
                        bookD.language = "English";
                    }
                    string desc;
                    if (columns.Contains("Description"))
                    {
                        desc = ds.Tables["volumeInfo"].Rows[0]["Description"].ToString();
                        bookD.description  = desc.Replace("'", "");
                    }
                    else
                    {
                        bookD.description = "";
                    }
                    string pub;
                    if (columns.Contains("publisher"))
                    {
                        pub = ds.Tables["volumeInfo"].Rows[0]["publisher"].ToString();
                        bookD.Publisher  = pub.Replace("'", "");
                    }
                    else
                    {
                        bookD.Publisher = "";
                    }
                    string aut1;
                    string aut2;
                    if (ds.Tables.Contains("authors"))
                    {
                        if (ds.Tables["authors"].Rows.Count > 0)
                        {
                            if (ds.Tables["authors"].Rows.Count <= 2)
                            {
                                aut1 = ds.Tables["authors"].Rows[0]["authors_Text"].ToString();
                                aut2 = ds.Tables["authors"].Rows[1]["authors_Text"].ToString();
                                bookD.Author1  = aut1.Replace("'", "");
                                bookD.Author2  = aut2.Replace("'", "");
                            }
                        }
                    }
                    else if (columns.Contains("authors"))
                    {
                        aut1 = ds.Tables["volumeInfo"].Rows[0]["authors"].ToString();
                        bookD.Author1 = aut1.Replace("'", "");
                    }
                    else
                    {
                        bookD.Author1 = "";
                    }

                }
                else
                {
                    System.Net.HttpWebRequest oHTTPRequest1 = System.Net.HttpWebRequest.Create("http://xisbn.worldcat.org/webservices/xid/isbn/ " + isbn + "?method=getMetadata&format=xml&fl=*&callback=mymethod") as System.Net.HttpWebRequest;
                    oHTTPRequest1.ContentType = "xml";
                    string result1 = "";
                    try
                    {
                        Stream response1 = oHTTPRequest1.GetResponse().GetResponseStream();
                        StreamReader reader1 = new StreamReader(response1);
                        result1 = reader1.ReadToEnd();


                    }
                    finally
                    {
                    }

                    System.Xml.XmlDocument doc1 = new System.Xml.XmlDocument();
                    doc1.LoadXml(result1);
                    XmlReader xmlReaderX = new XmlNodeReader(doc1);
                    DataSet ds1 = new DataSet();
                    ds.ReadXml(xmlReader);
                    if (ds1.Tables.Contains("isbn"))
                    {
                        DataColumnCollection columns = ds1.Tables["isbn"].Columns;
                        if (columns.Contains("isbn_Text"))
                        {
                            bookD.ISBN  = ds1.Tables["isbn"].Rows[0]["isbn_Text"].ToString();
                            string pub2;
                            if (columns.Contains("publisher"))
                            {
                                pub2 = ds1.Tables["isbn"].Rows[0]["publisher"].ToString();
                                bookD.Publisher  = pub2.Replace("'", "");
                            }
                            else
                            {
                                bookD.Publisher = "";
                            }
                            string auth1;
                            if (columns.Contains("author"))
                            {
                                auth1 = ds1.Tables["isbn"].Rows[0]["author"].ToString();
                                bookD.Author1  = auth1.Replace("'", "");
                            }
                            else
                            {
                                bookD.Author1 = "";
                            }

                            string title1;
                            if (columns.Contains("title"))
                            {
                                title1 = ds1.Tables["isbn"].Rows[0]["title"].ToString();
                                bookD.Title  = title1.Replace("'", "");
                            }
                            else
                            {
                                bookD.Title = "";
                            }
                            if (columns.Contains("lang"))
                            {
                                bookD.language  = ds1.Tables["isbn"].Rows[0]["lang"].ToString();
                            }
                            else
                            {
                                bookD.language = "";
                            }
                        }
                        else
                        {
                            bookD.ISBN = "";
                        }

                    }
                    else
                    {
                        //useISBNSearchAPI(isbn);
                    }
                }
                bookD.ISBN = ISBN;
            }
            catch(System.Exception)
            {

            }
        }

        public bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        public ActionResult Save()
        {
            var bookToUpdate = new School_LibraryBookMaster();
            if (TryUpdateModel(bookToUpdate))
            {
                try
                {
                    OrisonSystemSCHOOLEntities db = new OrisonSystemSCHOOLEntities();
                    string sqlQuery = "SELECT [dbo].[getNextBookCode] ()";
                    string newcode = db.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
                    bookToUpdate.BookCode = newcode;
                    db.School_LibraryBookMaster.Add(bookToUpdate);
                    db.SaveChanges();
                    return RedirectToAction("Detail", "Books", new { ID = bookToUpdate.BookMastID });
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
            return View(bookToUpdate);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Back")]
        public ActionResult Back(int? ID)
        {
            return RedirectToAction("Index","Books");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Cancel")]
        public ActionResult Cancel(int? ID)
        {
            return RedirectToAction("Index", "Books");
        }

        

    }
}