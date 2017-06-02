using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LibraryMVC.DAL
{
    public class clsDBOperations
    {
        public static DataTable  GetTable(string query,System.Data.Entity.DbContext context)
        {
            DataTable retVal = new DataTable();
            //EntityConnection entityConn = (EntityConnection)context.Database.Connection;
            SqlConnection sqlConn = (SqlConnection)context.Database.Connection;
            SqlCommand cmd = new SqlCommand(query , sqlConn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(retVal);
            return retVal;
        }
    }
}