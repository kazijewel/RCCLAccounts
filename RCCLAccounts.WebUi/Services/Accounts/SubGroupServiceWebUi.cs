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

namespace ESL.Services
{
    public class SubGroupServiceWebUi
    {
   
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        private commonService commonService;
        string sqlCon;
        public SubGroupServiceWebUi( IHttpContextAccessor accessor, AppDbContext db)
        {
           
            _accessor = accessor;
            _db = db;
            sqlCon= _db.Database.GetDbConnection().ConnectionString;
            commonService = new commonService( _accessor, _db);
        }

        public string maxId()
        {
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = " Select isnull(max(cast(SUBSTRING(SubGroupId,PATINDEX('%[0-9]%',SubGroupId),LEN(SubGroupId)) as int)),0)+1 maxId from SubGroups ";
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
        public string maxCode(string group,string code,string type)
        {
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "Select isnull(max(cast(s.SubGroupCode as int)),'" + code + "')+1 maxId from SubGroups s " +
                         " inner join PrimaryGroups as p " +
                         " on s.PrimaryGroupId = p.PrimaryGroupId " +
                         " where  s.SubGroupId like '" + group + "%' and  p.ItemOf like '" + type + "'";
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

        public IEnumerable<object> getAllData(string type)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select s.SubId Id, p.ItemOf Type,p.PrimaryGroupCode Code,p.PrimaryGroupName Name,m.MainGroupCode,m.MainGroupName,s.SubGroupCode,s.SubGroupName " +
                    "from SubGroups as s " +
                    "inner join PrimaryGroups as p " +
                    "on s.PrimaryGroupId = p.PrimaryGroupId " +
                    "inner join MainGroups as m " +
                    "on s.MainGroupId = m.MainGroupId " +
                    "where p.ItemOf like '" + type + "' order by p.ItemOf";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                Console.WriteLine(sql);
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["Id"].ToString(),
                        Type = sqlData["Type"].ToString(),
                        PrimaryGroupCode = sqlData["Code"].ToString(),
                        PrimaryGroupName = sqlData["Name"].ToString(),
                        MainGroupCode = sqlData["MainGroupCode"].ToString(),
                        MainGroupName = sqlData["MainGroupName"].ToString(),
                        SubGroupCode = sqlData["SubGroupCode"].ToString(),
                        SubGroupName = sqlData["SubGroupName"].ToString()
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
                string sql = "select s.SubId Id,p.ItemOf Type,p.PrimaryGroupId,p.PrimaryGroupCode Code,p.PrimaryGroupName Name,m.MainGroupId,m.MainGroupCode," +
                    "m.MainGroupName,s.SubGroupId,s.SubGroupCode,s.SubGroupName,s.Active " +
                    "from SubGroups as s " +
                    "inner join PrimaryGroups as p " +
                    "on s.PrimaryGroupId = p.PrimaryGroupId " +
                    "inner join MainGroups as m " +
                    "on s.MainGroupId = m.MainGroupId " +
                    "where s.SubId = " + id;

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                Console.WriteLine(sql);
                while (sqlData.Read())
                {
                    obj = new
                    {
                        Id = sqlData["Id"].ToString(),
                        Type = sqlData["Type"].ToString(),
                        PrimaryGroupId = sqlData["PrimaryGroupId"].ToString(),
                        PrimaryGroupCode = sqlData["Code"].ToString(),
                        PrimaryGroupName = sqlData["Name"].ToString(),
                        MainGroupId = sqlData["MainGroupId"].ToString(),
                        MainGroupCode = sqlData["MainGroupCode"].ToString(),
                        MainGroupName = sqlData["MainGroupName"].ToString(),
                        SubGroupId = sqlData["SubGroupId"].ToString(),
                        SubGroupCode = sqlData["SubGroupCode"].ToString(),
                        SubGroupName = sqlData["SubGroupName"].ToString(),
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
