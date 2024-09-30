
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
    public class PrimaryGroupService
    {
      
        private IHttpContextAccessor _accessor;
    
        private readonly AppDbContext _dbContext;
        string sqlCon;
        private commonService commonService;
        public PrimaryGroupService(IHttpContextAccessor accessor, AppDbContext dbContext)
        {
            
            _accessor = accessor;
            _dbContext = dbContext;
            sqlCon = _dbContext.Database.GetDbConnection().ConnectionString;
        
            commonService = new commonService(_accessor, _dbContext);
        }

        public string maxPCode(string group, string code)
        {
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select isnull(max(cast(PrimaryGroupCode as int))+1,'" + code + "') MaxCode from PrimaryGroups " +
                        " where PrimaryGroupId like '" + group + "%'";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["MaxCode"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "0";
        }
        public string max(string id)
        {
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select ISNULL(MAX(CAST(SUBSTRING(PrimaryGroupId,2,LEN(PrimaryGroupId)) as int)),0)+1 maxId " +
                " from PrimaryGroups where PrimaryGroupId like '" + id + "%' ";

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

    }
}
