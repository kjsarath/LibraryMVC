using LibraryMVC.DAL;
using LibraryMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LibraryMVC.Controllers
{
    public class MainController : Controller
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
            return View();
        }

        [HttpPost ]
        public JsonResult Chart1()
        {
            List<object> iData = new List<object>();
            List<object> ilbl = new List<object>();
            DataTable dtAcc = clsDBOperations.GetTable("Select * from dbo.Library_IssueReturnComparisonFn()", db);
            foreach (DataColumn dc in dtAcc.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow dr in dtAcc.Rows select dr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Chart2()
        {
            List<object > iData = new List<object>();
            List<object> ilbl = new List<object>();
            DataTable dtAcc = clsDBOperations.GetTable("Select * from dbo.[Library_InOutComparisonFn]()", db);
            foreach (DataColumn dc in dtAcc.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow dr in dtAcc.Rows select dr[dc.ColumnName]).ToList();
                iData.Add(x);
            }   
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Chart3()
        {
            List<object> iData = new List<object>();
            List<object> ilbl = new List<object>();
            DataTable dtAcc = clsDBOperations.GetTable("Select * from dbo.[Library_BookCopyCountFn]()", db);
            //foreach (DataColumn dc in dtAcc.Columns)
            //{
            //    ilbl.Add(dc.ColumnName);
            //}
            //iData.Add(ilbl);
            foreach (DataColumn dc in dtAcc.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow dr in dtAcc.Rows select dr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            return Json(iData, JsonRequestBehavior.AllowGet);
        }
    }
    
}
public class InOutComparisonData
{
    public string label { get; set; }
    public object value { get; set; }
    public string color { get; set; }
}