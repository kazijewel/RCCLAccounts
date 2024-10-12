
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Data;
using RCCLAccounts.WebUi.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RCCLAccounts.WebUi.Services
{
    public class NarrationServiceWebUi
    {
   
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        string sqlCon;
        private commonService commonService;
        public NarrationServiceWebUi(IHttpContextAccessor accessor, AppDbContext db)
        {
      
            _accessor = accessor;
            _db = db;
            sqlCon= _db.Database.GetDbConnection().ConnectionString;
            commonService = new commonService( _accessor, _db);
        }

        public string maxId()
        {
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = " Select isnull(max(cast(SUBSTRING(NarrationCode,3,10) as int)),0)+1 maxId from Narrations ";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["maxId"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "0";
        }
     

        public IEnumerable<object> getAllData()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "SELECT NarrationId,NarrationCode,NarrationName,VoucherType,Active  FROM Narrations";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                Console.WriteLine(sql);
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        NarrationId = sqlData["NarrationId"].ToString(),
                        NarrationCode = sqlData["NarrationCode"].ToString(),
                        NarrationName = sqlData["NarrationName"].ToString(),
                        VoucherType = sqlData["VoucherType"].ToString(),
                        Active = sqlData["Active"].ToString()
                        
                    });
                }
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }

        public object findData(int id)
        {
            object obj = null;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "SELECT NarrationId,NarrationCode,NarrationName,VoucherType,Active  FROM Narrations " +
                    "where NarrationId = " + id;

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                Console.WriteLine(sql);
                while (sqlData.Read())
                {
                    obj = new
                    {
                        NarrationId = sqlData["NarrationId"].ToString(),
                        NarrationCode = sqlData["NarrationCode"].ToString(),
                        NarrationName = sqlData["NarrationName"].ToString(),
                        VoucherType = sqlData["VoucherType"].ToString(),
                        Active = sqlData["Active"].ToString()
                    };
                }
            }
            finally
            {
                con.Close();
            }
            return obj;
        }



    }
}
