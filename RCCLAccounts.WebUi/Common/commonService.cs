
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RCCLAccounts.WebUi.Common
{
    public class commonService
    {

        private IHttpContextAccessor _accessor;
        private AppDbContext _db;

        private string sqlCon;


        public commonService( IHttpContextAccessor accessor,
            AppDbContext db)
        {
           
            _accessor = accessor;
            _db = db;
            sqlCon= _db.Database.GetDbConnection().ConnectionString;
        }


        public async Task<string> getLedgerId(int id)
        {
            //var obj = _unitAccounts.Ledger.GetFirstOrDefault(x => x.Id == id);
            var obj = await _db.Ledgers.FirstOrDefaultAsync(x => x.AutoId == id );
            return obj.LedgerId;
        }

        //public string getLedgerName(string id) 
        //{
        //    string ledgerName = "";
        //    try
        //    {
        //        int lId = Convert.ToInt32(id);
        //        var obj = _unitAccounts.Ledger.GetFirstOrDefault(x => x.Id==lId);
        //        ledgerName = obj.LedgerName;
        //    }
        //    catch(Exception exp)
        //    {
        //        var obj = _unitAccounts.Ledger.GetFirstOrDefault(x => x.LedgerId==id);
        //        ledgerName = obj.LedgerName;
        //    }
        //    return ledgerName;
        //}

        //public string getFiscalYearByDate(string fromDate, string toDate)
        //{
        //    var companyId = "B-1";
        //    string fiscalYear = "0";
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    try
        //    {
        //        con.Open();
        //        string sql = "Select  [dbo].[DateSelect](@fromDate,@toDate,@companyId) dateFSL";
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        cmd.Parameters.AddWithValue("@fromDate", fromDate);
        //        cmd.Parameters.AddWithValue("@toDate", toDate);
        //        cmd.Parameters.AddWithValue("@companyId", companyId);
        //        SqlDataReader sqlData = cmd.ExecuteReader();
        //        if (sqlData.Read())
        //        {
        //            if (sqlData.HasRows)
        //            {
        //                fiscalYear = sqlData["dateFSL"].ToString();
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return fiscalYear;
        //}

        // Check Date interval in the same Fiscal Year
        public string getSameFiscalYearDateCheck(string fromDate,string toDate, string branchID)
        {
            string fiscalYear = "0";
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "Select  [dbo].[DateSelect](@i,@j,@branchID) dateFSL";
                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.Parameters.AddWithValue("@i",fromDate);
                cmd.Parameters.AddWithValue("@j",toDate);
                cmd.Parameters.AddWithValue("@branchID", branchID);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if(sqlData.Read())
                {
                    if(sqlData.HasRows)
                    {
                        fiscalYear = sqlData["dateFSL"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return fiscalYear;
        }

        // Get Fiscal year ID
        public string getFiscalYear(string date, string branchID)
        {
            string ret = "";
            DateTime dDate = DateTime.Parse(date);
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "Select  [dbo].[VoucherSelect](@date,@branchID) fsl";
                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.Parameters.AddWithValue("@date", dDate);
                cmd.Parameters.AddWithValue("@branchID", branchID);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if(sqlData.Read())
                {
                    if(sqlData.HasRows)
                    {
                        ret = sqlData["fsl"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        public string getVoucherNo(string type,string date)
        {
            string branchID = "B-1";
            string ret = "";
            string fsl = getFiscalYear(date, branchID);
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "";
                string prefix = "";

                if (type.Equals("Cash Payment"))
                {
                     sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+1, 1)as varchar) " +
                    " VoucherNo from Vouchers where VoucherType = 'dba'  and FiscalYearId like @fiscalYearId";

                    prefix = "DR-CH-";
                }
               else  if(type.Equals("Cash Receipt"))
                {
                    sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+1, 1)as varchar) " +
                    " VoucherNo from Vouchers where VoucherType in ('cca','cci')   and FiscalYearId like @fiscalYearId";
                    prefix = "CR-CH-";
                }
                else if (type.Equals("Bank Payment"))
                {
                    sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+1, 1)as varchar) " +
                     " VoucherNo from Vouchers where VoucherType  = 'dba'  and FiscalYearId like @fiscalYearId";
                    prefix = "DR-BK-";
                }

                else if (type.Equals("Bank Receipt"))
                {
                    sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+1, 1)as varchar) " +
                    " VoucherNo from Vouchers where VoucherType in ('cba','cbi')   and FiscalYearId like @fiscalYearId";
                    prefix = "CR-BK-";
                }
                else  if (type.Equals("Journal"))
                {
                    sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+1, 1)as varchar) " +
                     " VoucherNo from Vouchers where VoucherType = 'jau'  and FiscalYearId like @fiscalYearId";
                    prefix = "JV-NO-";
                }
                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.Parameters.AddWithValue("@fiscalYearId",fsl);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if(sqlData.Read())
                {
                    if(sqlData.HasRows)
                    {
                        ret = prefix  + sqlData["VoucherNo"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public string getVoucherNoIncreseTwo(string type, string date)
        {
            string branchID = "B-1";
            string ret = "";
            string fsl = getFiscalYear(date, branchID);
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "";
                string prefix = "";

                if (type.Equals("Cash Payment"))
                {
                    sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+2, 1)as varchar) " +
                   " VoucherNo from Vouchers where VoucherType = 'dba'  and FiscalYearId like @fiscalYearId";

                    prefix = "DR-CH-";
                }
                else if (type.Equals("Cash Receipt"))
                {
                    sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+2, 1)as varchar) " +
                    " VoucherNo from Vouchers where VoucherType in ('cca','cci')   and FiscalYearId like @fiscalYearId";
                    prefix = "CR-CH-";
                }
                else if (type.Equals("Bank Payment"))
                {
                    sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+2, 1)as varchar) " +
                     " VoucherNo from Vouchers where VoucherType  = 'dba'  and FiscalYearId like @fiscalYearId";
                    prefix = "DR-BK-";
                }

                else if (type.Equals("Bank Receipt"))
                {
                    sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+2, 1)as varchar) " +
                    " VoucherNo from Vouchers where VoucherType in ('cba','cbi')   and FiscalYearId like @fiscalYearId";
                    prefix = "CR-BK-";
                }
                else if (type.Equals("Journal"))
                {
                    sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+2, 1)as varchar) " +
                     " VoucherNo from Vouchers where VoucherType = 'jau'  and FiscalYearId like @fiscalYearId";
                    prefix = "JV-NO-";
                }
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@fiscalYearId", fsl);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        ret = prefix + sqlData["VoucherNo"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        private decimal getBudget(string ledgerId, string companyId, string fiscalYearId)
        {
            decimal returnData = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "SELECT isnull(BudgetAmount,0) budgetAmt FROM tbLedgerBudget where " +
                    " LedgerId like @ledgerId and FiscalYearId like @fiscalYearId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ledgerId", ledgerId);
                cmd.Parameters.AddWithValue("@companyId", companyId);
                cmd.Parameters.AddWithValue("@fiscalYearId", fiscalYearId);

                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        returnData = Convert.ToDecimal(sqlData["budgetAmt"].ToString());
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        private decimal getVoucherBalance(string ledgerId, string companyId, string fiscalYearId)
        {
            decimal returnData = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "SELECT ISNULL(SUM(DrAmount)- SUM(CrAmount),0) balance FROM tbVoucher where " +
                    " LedgerId like @ledgerId and FiscalYearId like @fiscalYearId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ledgerId", ledgerId);
                cmd.Parameters.AddWithValue("@companyId", companyId);
                cmd.Parameters.AddWithValue("@fiscalYearId", fiscalYearId);

                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        returnData = Convert.ToDecimal(sqlData["balance"].ToString());
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        private decimal getOpeningBalance(string ledgerId, string companyId, string fiscalYearId)
        {
            decimal returnData = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "SELECT ISNULL(SUM(DrAmount)- SUM(CrAmount),0) openingBalance FROM tbLedgerOpeningBalance where " +
                    " LedgerId like @ledgerId and FiscalYearId like @fiscalYearId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ledgerId", ledgerId);
                cmd.Parameters.AddWithValue("@companyId", companyId);
                cmd.Parameters.AddWithValue("@fiscalYearId", fiscalYearId);

                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        returnData = Convert.ToDecimal(sqlData["openingBalance"].ToString());
                    }
                }
            }
            finally
            {
                con.Close();
            }

            return returnData;
        }
  
        public async Task<object> LedgerBudgetBalance(int id, string voucherDate)
        {
            var returnData = new Object();
            string companyId = "B-1";
            string ledgerId = await getLedgerId(id);
            string fiscalYear = getFiscalYear(voucherDate, companyId);

            decimal budgetAmount = getBudget(ledgerId, companyId, fiscalYear);
            decimal voucherBalance = getVoucherBalance(ledgerId, companyId, fiscalYear);
            decimal openingBalance = getOpeningBalance(ledgerId, companyId, fiscalYear);

            returnData = new
            {
                budget = budgetAmount,
                balance = voucherBalance + openingBalance
            };

            return returnData;
        }

        public IEnumerable<string> getNarrations()
        {
            List<string> arr = new List<string>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct Narration from Vouchers where Narration is not null and Narration !='' order by Narration";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    arr.Add(sqlData["Narration"].ToString());
                }
            }
            finally
            {
                con.Close();
            }
            return arr;
        }

        public List<object> auditInfo(long id = 0)
        {
            List<object> list = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select Top(1) AuditApprove,AuditBy, AuditIp, AuditTime, ApproveBy, ApproveIp, ApproveTime,CompanyId " +
                "from Vouchers where AutoId =@id ";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
               
                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        list.Add(sqlData["AuditApprove"].ToString());
                        list.Add(sqlData["AuditBy"].ToString());
                        list.Add(sqlData["AuditIp"].ToString());
                        list.Add(sqlData["AuditTime"]);
                        list.Add(sqlData["ApproveBy"].ToString());
                        list.Add(sqlData["ApproveIp"].ToString());
                        list.Add(sqlData["ApproveTime"]);
                        list.Add(sqlData["CompanyId"]);
                    }
                }
            }
            finally
            {
                con.Close();
            }

            return list;
        }

        public string fiscalYearIdTrans(string voucherDate)
        {
            SqlConnection con = new SqlConnection(sqlCon);


            string sql = "Select vFiscalYearIdTrans from [dbo].[funFiscalYearInfo]('" + voucherDate + "',"
                     + " '" + voucherDate + "', '" + _accessor.HttpContext.Session.GetString("companyName") + "')";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["vFiscalYearIdTrans"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "0";
        }

        public object getFiscalYearDate()
        {

            var obj = new Object();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select Top(1) convert(date,OpeningDate)OpeningDate,convert(date,ClosingDate)ClosingDate from FiscalYears Where RunningFlag=1 and IsClosed=0 order by AutoId desc";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        obj = new
                        {
                            Opening = ((DateTime)sqlData["OpeningDate"]).ToString("dd-MM-yyyy"),
                            Closing = ((DateTime)sqlData["ClosingDate"]).ToString("dd-MM-yyyy")

                    };


                    }
                }
            }
            finally
            {
                con.Close();
            }
            return obj;
        }


        public string getTransactionId()
        {
        
            var companyId = "B-1";
            //var branchId = session.GetString("branchId");
            string ret = "";
            //string fsl = getFiscalYear(date);
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select isNull(MAX(cast(TransactionId as bigint)),0)+1 id from Vouchers where CompanyId like '" + companyId + "' ";
                // "and FiscalYearId like '" + fsl + "' and CompanyId = '" + companyId + "' and BranchId = '" + branchId + "' ";
                SqlCommand sqlCmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = sqlCmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        ret = sqlData["id"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

    }
}
