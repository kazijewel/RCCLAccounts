
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RCCLAccounts.WebUi.Services
{
    public class LedgerServiceWebUi
    {
       
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        private commonService commonService;
        string sqlCon;
        public LedgerServiceWebUi( IHttpContextAccessor accessor, AppDbContext db)
        {
       
            _accessor = accessor;
            _db = db;
            sqlCon= _db.Database.GetDbConnection().ConnectionString;
            commonService = new commonService( _accessor, _db);
        }

        public async Task<ActionResult<IEnumerable<object>>>  primaryGroup()
        {
            //ISession session = commonService.getSession();
            var companyId = "B-1"; //session.GetString("companyId");
            //var obj = from x in _unitAccounts.PrimaryGroup.GetAll(x => x.CompanyId.ToString() == companyId)
            //          select new { Id = x.PrimaryGroupId, Name = x.Code + "-" + x.Name };

            var obj = await _db.PrimaryGroups
            .Where(x => x.CompanyId == companyId)
            .Select(x => new { Id = x.PrimaryGroupId, Name = x.PrimaryGroupCode + "-" + x.PrimaryGroupName })
            .ToListAsync();


            return obj;
        }
        public async Task<ActionResult<IEnumerable<object>>> mainGroup(string pId)
        {
            // ISession session = commonService.getSession();
            // var companyId = session.GetString("companyId");
            var companyId = "B-1";
            if (pId == "%")
            {
                //var obj = from x in _unitAccounts.MainGroup.GetAll(x=>x.CompanyId == Convert.ToInt32(companyId))
                //          select new { Id = x.MainGroupId, Name = x.MainGroupCode + "-" + x.MainGroupName };

            var obj = await _db.MainGroups
           .Where(x => x.CompanyId == companyId)
           .Select(x => new { Id = x.MainGroupId, Name = x.MainGroupCode + "-" + x.MainGroupName })
           .ToListAsync();

                return obj;
            }
            else
            {
                //var obj = from x in _unitAccounts.MainGroup.GetAll(x => x.PrimaryGroupId == pId && x.CompanyId == Convert.ToInt32(companyId))
                //          select new { Id = x.MainGroupId, Name = x.MainGroupCode + "-" + x.MainGroupName };

            var obj = await _db.MainGroups
           .Where(x => x.PrimaryGroupId == pId && x.CompanyId == companyId)
           .Select(x => new { Id = x.MainGroupId, Name = x.MainGroupCode + "-" + x.MainGroupName })
           .ToListAsync();
                
                return obj;
            }
        }

        public int LedgerNameUpdate (string ledgerId, string ledgerName)
        {
            int ret = 0;
            string sql = "update Ledgers set LedgerName = '" + ledgerName + "' where LedgerId = '"+ ledgerId+ "'";
            string sqlLOpBal = "update LedgerOpeningBalances set LedgerName = '" + ledgerName + "' where LedgerId = '" + ledgerId + "'";
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlCommand cmd2 = new SqlCommand(sqlLOpBal, con);
                ret = cmd.ExecuteNonQuery();
                ret = cmd2.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public async Task<ActionResult<IEnumerable<object>>>  subGroup(string pId, string mId)
        {
            // ISession session = commonService.getSession();
            //var companyId = session.GetString("companyId");
            var companyId = "B-1";
            if (pId == "%" && mId == "%")
            {
                //var obj = from x in _unitAccounts.SubGroup.GetAll(x => x.CompanyId == Convert.ToInt32(companyId))
                //          select new { Id = x.SubGroupId, Name = x.SubGroupCode + "-" + x.SubGroupName };

                var obj = await _db.SubGroups
               .Where(x =>  x.CompanyId == companyId)
               .Select(x => new { Id = x.SubGroupId, Name = x.SubGroupCode + "-" + x.SubGroupName })
               .ToListAsync();

                return obj;
            }
            else
            {
                //var obj = from x in _unitAccounts.SubGroup.GetAll(x => x.PrimaryGroupId == pId && x.MainGroupId == mId && x.CompanyId == Convert.ToInt32(companyId))
                //          select new { Id = x.SubGroupId, Name = x.SubGroupCode + "-" + x.SubGroupName };

              var obj = await _db.SubGroups
             .Where(x => x.PrimaryGroupId == pId && x.MainGroupId == mId &&  x.CompanyId == companyId)
             .Select(x => new { Id = x.SubGroupId, Name = x.SubGroupCode + "-" + x.SubGroupName })
             .ToListAsync();


                return obj;
            }
        }
        public object getFirstData(int id)
        {
            //ISession session = commonService.getSession();
            //var companyId = session.GetString("companyId");
            //var branchId = session.GetString("branchId");
            var companyId = "B-1";
            var returnData = new Object();
            SqlConnection con = new SqlConnection(sqlCon);

            string sql = "select c.PrimaryGroupId pId,c.PrimaryGroupCode+'-'+c.PrimaryGroupName primaryData,d.MainGroupId mId,d.MainGroupCode+'-'+d.MainGroupName mainData," +
            " e.SubId sId, e.SubGroupCode + '-' + e.SubGroupName subData,a.AutoId autoId, b.LedgerOpId openingId, a.LedgerId,a.LedgerCode," +
            " a.LedgerName,CAST(b.DrAmount as float)DrAmount,CAST(b.CrAmount as float)CrAmount," +
            " CONVERT(varchar,a.OpeningDate,105)OpeningDate,a.Active,a.LedgerType,a.CompanyId " +  //,a.BranchId 
            " from Ledgers a " +
            " inner join LedgerOpeningBalances b on a.LedgerId = b.LedgerId " +
            " inner join PrimaryGroups c on a.PrimaryGroupId = c.PrimaryGroupId " +
            " left join MainGroups d on a.MainGroupId = d.MainGroupId " +
            " left join SubGroups e on a.SubGroupId = e.SubGroupId " +
            " where a.AutoId = " + id + " and a.CompanyId='" + companyId + "'  ";   //and b.BranchId= '"+branchId+"'
            Console.WriteLine(sql);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        returnData = (new
                        {
                            pId = reader["pId"].ToString(),
                            primaryData = reader["primaryData"].ToString(),
                            mId = reader["mId"].ToString(),
                            mainData = reader["mainData"].ToString(),
                            sId = reader["sId"].ToString(),
                            subData = reader["subData"].ToString(),
                            autoId = reader["autoId"].ToString(),
                            openingId = reader["openingId"].ToString(),
                            ledgerId = reader["LedgerId"].ToString(),
                            ledgerCode = reader["LedgerCode"].ToString(),
                            ledgerName = reader["LedgerName"].ToString(),
                            drAmount = reader["DrAmount"].ToString(),
                            crAmount = reader["CrAmount"].ToString(),
                            openingDate = reader["OpeningDate"].ToString(),
                            ledgerType = reader["LedgerType"].ToString(),
                            active = reader["Active"].ToString(),
                            companyId = reader["CompanyId"].ToString() //,
                            //branchId = reader["BranchId"].ToString()
                        });
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getAllData(string type)
        {
            // ISession session = commonService.getSession();
            //var companyId = session.GetString("companyId");
            // var branchId = session.GetString("branchId");
            var companyId = "B-1";

            List<object> list = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select distinct c.ItemOf  Type,c.PrimaryGroupName,ISNULL(d.MainGroupName,'')MainGroupName,ISNULL(e.SubGroupName,'')SubGroupName," +
            " a.LedgerCode,a.LedgerName,a.AutoId Id " +
            " from Ledgers a " +
            " inner join LedgerOpeningBalances b on a.LedgerId = b.LedgerId " +
            " inner join PrimaryGroups c on a.PrimaryGroupId = c.PrimaryGroupId " +
            " left join MainGroups d on a.MainGroupId = d.MainGroupId " +
            " left join SubGroups e on a.SubGroupId = e.SubGroupId " +
            " where  c.ItemOf  like '" + type + "' and a.CompanyId='" + companyId + "'  "; //and b.BranchId= '" + branchId + "'
            Console.WriteLine(sql);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new
                        {
                            id = reader["Id"].ToString(),
                            type = reader["Type"].ToString(),
                            primaryGroupName = reader["PrimaryGroupName"].ToString(),
                            mainGroupName = reader["MainGroupName"].ToString(),
                            subGroupName = reader["SubGroupName"].ToString(),
                            ledgerCode = reader["LedgerCode"].ToString(),
                            ledgerName = reader["LedgerName"].ToString()
                        });

                    }
                }
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public string maxId(string pId)
        {
            //ISession session = commonService.getSession();
            //var companyId = session.GetString("companyId");
            //var branchId = session.GetString("branchId");
            var companyId = "B-1";
            string type = pId.Substring(0, 1) + "L";
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = " Select isnull(max(cast(SUBSTRING(LedgerId,3,10) as int)),0)+1 maxId from" +
                    " dbo.Ledgers where LedgerId like '" + type + "%'  "; //and CompanyId like '"+ companyId + "'
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return type + reader["maxId"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "0";
        }

        public int primaryId(string primaryGroupId)
        {
            //ISession session = commonService.getSession();
            //var companyId = session.GetString("companyId");
            var companyId = "B-1";
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "Select PrimaryId id from PrimaryGroups Where PrimaryGroupId = '" + primaryGroupId + "' and CompanyId like '" + companyId + "' ";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader["id"]);
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return 0;
        }

        public string primaryCode(string id)
        {
            //ISession session = commonService.getSession();
            //var companyId = session.GetString("companyId");
            var companyId = "B-1";
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select PrimaryGroupCode Code from PrimaryGroups where PrimaryGroupId like '" + id + "' and CompanyId like '" + companyId + "' ";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["Code"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "0";
        }

        
        public string mainCode(string id)
        {
            //ISession session = commonService.getSession();
            //var companyId = session.GetString("companyId");
            var companyId = "B-1";
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select MainGroupCode from MainGroups where MainGroupId like '" + id + "' and CompanyId like '" + companyId + "' ";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["MainGroupCode"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "0";
        }
        public string subCode(string id)
        {
            // ISession session = commonService.getSession();
            // var companyId = session.GetString("companyId");
            var companyId = "B-1";
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select SubGroupCode from SubGroups where SubGroupId like '" + id + "' and CompanyId like '" + companyId + "' ";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["SubGroupCode"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "0";
        }

        public string PrimaryGroupId(int id)
        {
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select PrimaryGroupId from PrimaryGroups where PrimaryId like '" + id + "' ";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["PrimaryGroupId"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "0";
        }

        public string MainGroupId(int id)
        {
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select MainGroupId from MainGroups where MainId like '" + id + "'";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["MainGroupId"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "";
        }
        public string SubGroupId(int id)
        {
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select SubGroupId from SubGroups where SubId like '" + id + "'";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["SubGroupId"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "";
        }
        public int openingBalanceSave(Ledger obj,decimal debit,decimal credit,int id)
        {
            //ISession session = commonService.getSession();
            //var companyId = session.GetString("companyId");
            //var branchId = session.GetString("branchId");
            //,[PKLegerId]  ,[LedgerCode]
            var companyId = "B-1";
            int ret=0;
            string sql = "INSERT into dbo.LedgerOpeningBalances(FiscalYearId,LedgerId,LedgerName,DrAmount,CrAmount,OpeningDate,CompanyId," +
                        " Flag,UserName,UserIp,EntryTime) " + //,BranchId
                        " values(@fiscalYear,@ledgerId,@ledgerName,@drAmount,@crAmount,@openingDate,@companyId,@flag,@userName,@userIp,@entryTime)";//,@branchId
            if (id!=0)
            {
                sql = "update LedgerOpeningBalances set LedgerName=@ledgerName,DrAmount=@drAmount,CrAmount=@crAmount,OpeningDate=@openingDate," +
                    " UserName=@userName,UserIp=@userIp,EntryTime=@entryTime where LedgerOpId like @id";
            }
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ledgerName", obj.LedgerName);
                cmd.Parameters.AddWithValue("@drAmount", debit);
                cmd.Parameters.AddWithValue("@crAmount", credit);
                cmd.Parameters.AddWithValue("@openingDate", obj.OpeningDate);
                cmd.Parameters.AddWithValue("@userName", obj.UserName);
                cmd.Parameters.AddWithValue("@userIp", obj.UserIp);
                cmd.Parameters.AddWithValue("@entryTime", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"));
                if (id!=0)
                {
                    cmd.Parameters.AddWithValue("@id", id);
                }
                if (id==0)
                {
                    cmd.Parameters.AddWithValue("@fiscalYear", commonService.getFiscalYear(obj.OpeningDate.ToString("yyyy/MM/dd"), "B-1") );
                    cmd.Parameters.AddWithValue("@ledgerId", obj.LedgerId);
                    cmd.Parameters.AddWithValue("@companyId", companyId);
                   // cmd.Parameters.AddWithValue("@branchId", branchId);
                    cmd.Parameters.AddWithValue("@flag", "Opening");
                }
                ret = cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        private int deleteOpening(int id)
        {
            int ret = 0;
            string sql = "delete LedgerOpeningBalances where LedgerOpId=@id";
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                ret = cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        public string maxCode(string pId, string mId, string sId)
        {
            //ISession session = commonService.getSession();
            //var companyId = session.GetString("companyId");
            //var branchId = session.GetString("branchId");
            var companyId = "B-1";
            string primaryGroupCode = "";
            string mainGroupCode = "00";
            string subGroupCode = "00";
            string ledgerCode = "000001";
            string maxId = "";
            string mCode = "";

            if (pId != "")
                primaryGroupCode = primaryCode(pId);
            if (mId != "")
            {

                mainGroupCode = mainCode(mId);
            }

            if (sId != "")
            {
                subGroupCode = subCode(sId);
            }

            SqlConnection con = new SqlConnection(sqlCon);

            string sql = "select isnull(max(cast(LedgerCode as bigint)),0)+1 maxId from Ledgers " +
                " where CompanyId='" + companyId + "' and PrimaryGroupId like '" + pId + "' " +
                " and MainGroupId like '" + mId + "' and SubGroupId like '" + sId + "'";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        maxId = reader["maxId"].ToString();

                        if (maxId.Length < 2)
                        {
                            return primaryGroupCode + mainGroupCode + subGroupCode + ledgerCode;
                        }
                        else
                        {
                            return maxId;
                        }
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
