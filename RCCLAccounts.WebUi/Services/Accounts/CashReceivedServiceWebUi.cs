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
using System.Security.Claims;
using System.Threading.Tasks;

namespace RCCLAccounts.WebUi.Services
{
    public class CashReceivedServiceWebUi
    {
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        private commonService commonService;
        string sqlCon;
        public CashReceivedServiceWebUi( IHttpContextAccessor accessor, AppDbContext db)
        {

            _accessor = accessor;
            _db = db;
            sqlCon= _db.Database.GetDbConnection().ConnectionString;
            commonService = new commonService( _accessor,_db);
        }
        public async Task<ActionResult<IEnumerable<object>>>  ledgerList()
        {
            string companyId = "B-1";
            //var obj = from x in _unitAccounts.Ledger.GetAll(x => x.LedgerType != "Cash" && x.CompanyId == Convert.ToInt32(companyId))
            //      select new { Id = x.Id, Name =x.LedgerName };

            var obj = await _db.Ledgers
            .Where(x => x.CompanyId == companyId && x.LedgerType != "Cash" )
            .Select(x => new { Id = x.LedgerId, Name = x.LedgerName  })
            .ToListAsync();

            return obj;
        }
        public async Task<ActionResult<IEnumerable<object>>> ledgerListCash()
        {
            string companyId = "B-1";
            //var obj = from x in _unitAccounts.Ledger.GetAll(x=>x.LedgerType=="Cash" && x.CompanyId == Convert.ToInt32(companyId))
            //      select new { Id = x.Id, Name =x.LedgerName };
            var obj = await _db.Ledgers
           .Where(x => x.CompanyId == companyId && x.LedgerType == "Cash")
           .Select(x => new { Id = x.LedgerId, Name = x.LedgerName })
           .ToListAsync();

            return obj;
        }
        
        public async Task<string> getLedgerId(string id)
        {
            var ledgerId = "";
            //var obj = _unitAccounts.Ledger.GetFirstOrDefault(x=>x.Id==Convert.ToInt32(id));

            var obj = await _db.Ledgers.FirstOrDefaultAsync(x => x.LedgerId == id);

            if (obj!=null)
            {
                ledgerId = obj.LedgerId;
            }
            return ledgerId;
        }
        public async Task<string> getLedgerUniqueId()
        {
           // ISession session = commonService.getSession();
            var companyId = "B-1";
            //var branchId = session.GetString("branchId");
            var ledgerId = "";
           // var obj = _unitAccounts.Ledger.GetFirstOrDefault(x=>x.LedgerType=="Cash" && x.CompanyId == Convert.ToInt32(companyId));
            var obj = await _db.Ledgers.FirstOrDefaultAsync(x => x.LedgerType == "Cash" && x.CompanyId == companyId);

            if (obj!=null)
            {
                ledgerId = obj.LedgerId.ToString();
            }
            return ledgerId;
        }
        
      
        public IEnumerable<object> getData(string transactionId)
        {
            //ISession session = commonService.getSession();
            var companyId = "B-1";
           // var branchId = session.GetString("branchId");
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                //convert(varchar,a.VoucherDate,105)
                string sql = "select a.AutoId Id,a.TransactionId,a.VoucherNo,a.VoucherDate,a.LedgerId,a.LedgerName," +
                    " a.TransactionWith,a.Narration,ISNULL(CONVERT(float,a.CrAmount),0)DrAmount,b.LedgerId lId,b.LedgerCode,a.AttachBill from Vouchers a " +
                    " inner join Ledgers b on a.LedgerId=b.LedgerId " +
                    " where a.TransactionId like @transactionId and " +
                    "a.CompanyId like @companyId  and " +       //and a.BranchId like @branchId
                    "a.EntryFrom like '%Cash Received Voucher%' order by a.CrAmount asc";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@transactionId", transactionId);
                cmd.Parameters.AddWithValue("@companyId", companyId);
               // cmd.Parameters.AddWithValue("@branchId", branchId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["Id"].ToString(),
                        TransactionId = sqlData["TransactionId"].ToString(),
                        VoucherNo = sqlData["VoucherNo"].ToString(),
                        VoucherDate = sqlData["VoucherDate"].ToString(),
                        LedgerUniqueId = sqlData["lId"].ToString(),
                        LedgerId = sqlData["LedgerId"].ToString(),
                        LedgerCode = sqlData["LedgerCode"].ToString(),
                        LedgerName = sqlData["LedgerName"].ToString(),
                        TransactionWith = sqlData["TransactionWith"].ToString(),
                        Narration = sqlData["Narration"].ToString(),
                        drAmt = sqlData["DrAmount"].ToString(),
                        Attachment = sqlData["AttachBill"].ToString()
                    });
                }
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getAllData(string fromDate,string toDate)
        {
            //ISession session = commonService.getSession();
            var companyId = "B-1";
            //var branchId = session.GetString("branchId");
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select a.AutoId Id,a.VoucherNo,a.VoucherDate,b.LedgerName,a.TransactionWith+' - '+a.Narration Narration, " +
                " CONVERT(float, a.CrAmount)CrAmount,a.VoucherType,a.TransactionId,a.AttachBill,a.UserName,a.AuditApprove,u.FullName ApproveBy,a.ApproveTime " +
                "  from Vouchers a inner join Ledgers b on a.LedgerId = b.LedgerId " +
                "left join AspNetUsers u on a.ApproveBy = u.FullName " +
                " where " +
                "a.VoucherType like 'cca' and a.CrAmount != 0 and convert(date, a.VoucherDate)  between convert(varchar, '" + fromDate+ "',105) and convert(varchar, '" + toDate+ "',105)  " +
                " and a.EntryFrom like '%Cash Received Voucher%' order by a.VoucherDate desc";
                Console.WriteLine(sql);
                SqlCommand cmd = new SqlCommand(sql,con);
                SqlDataReader sqlData = cmd.ExecuteReader();
               
                while(sqlData.Read())
                {
                    returnData.Add(new
                    {
                        TransactionId = sqlData["TransactionId"].ToString(),
                        VoucherNo = sqlData["VoucherNo"].ToString(),
                        VoucherDate = sqlData["VoucherDate"].ToString(),
                        LedgerName = sqlData["LedgerName"].ToString(),
                        Narration = sqlData["Narration"].ToString(),
                        DrAmt = sqlData["CrAmount"].ToString(),
                        Attachment = sqlData["AttachBill"].ToString(),
                        UserName = sqlData["UserName"].ToString(),
                        Id = sqlData["Id"].ToString(),
                        AuditApprove = sqlData["AuditApprove"].ToString(),
                        ApproveBy = sqlData["ApproveBy"].ToString(),
                        ApproveTime = sqlData["AuditApprove"].ToString() == "2" ? sqlData["ApproveTime"].ToString() : null
                    }) ;
                }
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        public object LedgerBudgetBalance(int id,string voucherDate)
        {
            return commonService.LedgerBudgetBalance(id, voucherDate);
        }
        
        public string getVoucherNo(string date)
        {
            return voucherNo(date);
        }
        private string voucherNo(string date)
        {
            //ISession session = commonService.getSession();
            var companyId = "B-1";
            //var branchId = session.GetString("branchId");
            var fsl = commonService.getFiscalYear(date, companyId);
            string ret = "";
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "SELECT ISNULL((MAX(CAST(SUBSTRING(VoucherNo,7,50) AS INT))+1),1) voucher  FROM Vouchers " +
                    " WHERE FiscalYearId = @fsl and Vouchertype in ('cca','cci') ";
                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.Parameters.AddWithValue("@companyId",companyId);
                //cmd.Parameters.AddWithValue("@branchId", branchId);
                cmd.Parameters.AddWithValue("@fsl",fsl);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if(sqlData.Read())
                {
                    if(sqlData.HasRows)
                    {
                        ret = "CR-CH-" + sqlData["voucher"];
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public int cashSave(List<Voucher> objList,string bankHeadId,string bankHead,string bankCode ,string userName,string userIp)
        {
            //ISession session = commonService.getSession();
            var companyId = "B-1";
            //var branchId = session.GetString("branchId");
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlTransaction transaction = null;
            SqlCommand cmd, cmd1,cmd2,cmd3;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("CashReceived");

                string transId = objList[0].TransactionId.ToString();
                string fiscalYear = commonService.getFiscalYear(objList[0].VoucherDate.ToString("yyyy/MM/dd"), companyId);
                string transType = "Cash";
                string voucherNo = "";
                decimal credit = 0;
               
                string AuditApprove = "1";
                string AuditBy = "";
                string AuditIp = "";
                DateTime AuditTime = DateTime.Now;
                string ApproveBy = "";
                string ApproveIp = "";
                DateTime ApproveTime = DateTime.Now;

                
                if(fiscalYear == "0")
                {
                    return 0;
                }           

                if (objList[0].AutoId!=0)
                {

                    List<object> ainfos = commonService.auditInfo(objList[0].AutoId);
                    if (ainfos.Count > 0)
                    {
                        AuditApprove = ainfos[0].ToString();
                        AuditBy = ainfos[1].ToString();
                        AuditIp = ainfos[2].ToString();
                        AuditTime = (DateTime)ainfos[3];
                        ApproveBy = ainfos[4].ToString();
                        ApproveIp = ainfos[5].ToString();
                        ApproveTime = (DateTime)ainfos[6];
                        companyId = ainfos[7].ToString();
                       // branchId = ainfos[8].ToString();
                    }

                    string sqlDel = "delete Vouchers where TransactionId like @transactionId and VoucherNo like @voucherNo " +
                        " and CompanyId = @companyId  ";


                    string sqlUd = "insert into UdVouchers " +
                    " select MasterNo,FiscalYearId,VoucherNo, VoucherDate,ChequeNo, ChequeDate,VoucherType, LedgerId,LedgerCode,LedgerName," +
                    " BalanceAmount,DrAmount, CrAmount, Narration, TransactionWith,CostCenterId,CostCenterName,ProductId,ProductName,ChequeClear,AuditApprove,AuditBy, AuditTime,AuditIp, " +
                    " ApproveBy,ApproveTime, ApproveIp,  AttachBill,AttachCheque, AttachReference,ReferenceNo,ReferenceDetails, TransactionType,BankName, BranchName,CompanyId,  " +
                    " 'Old',EntryFrom,UserName,UserIp,EntryTime,TransactionId " +                   
                    " from Vouchers where TransactionId like @transactionId and VoucherNo like @voucherNo and CompanyId = @companyId  ";

                    cmd3 = new SqlCommand(sqlUd,con,transaction);
                    cmd3.Parameters.AddWithValue("@voucherNo", objList[0].VoucherNo);
                    cmd3.Parameters.AddWithValue("@companyId", companyId);
                    //cmd3.Parameters.AddWithValue("@branchId", branchId);
                    cmd3.Parameters.AddWithValue("@transactionId", transId);
                    cmd3.ExecuteNonQuery();

                    cmd2 = new SqlCommand(sqlDel,con,transaction);
                    cmd2.Parameters.AddWithValue("@voucherNo", objList[0].VoucherNo);
                    cmd2.Parameters.AddWithValue("@companyId", companyId);
                   // cmd2.Parameters.AddWithValue("@branchId", branchId);
                    cmd2.Parameters.AddWithValue("@transactionId", transId);
                    cmd2.ExecuteNonQuery();
                }

                string sql2 = "insert into Vouchers (TransactionId,TransactionType,MasterNo,FiscalYearId,VoucherNo,VoucherDate," +
                   " VoucherType,LedgerId,LedgerCode,LedgerName,BalanceAmount,DrAmount," +
                   " CrAmount,Narration,TransactionWith,ChequeNo,ChequeDate,BankName,BranchName,ChequeClear," +
                   " AttachCheque,AuditApprove,AuditBy,AuditIp," +
                   " AuditTime,ApproveBy,ApproveIp,ApproveTime,AttachBill,AttachReference,ProductId,ProductName,CompanyId," +
                   " EntryFrom,UserName,UserIp,EntryTime,CostCenterId,CostCenterName,ReferenceNo,ReferenceDetails) " +
                   " values (@TransactionId,@TransactionType,@MasterNo,@FiscalYearId,@VoucherNo,@VoucherDate," +
                   " @VoucherType,@LedgerId,@LedgerCode,@LedgerName,@BalanceAmount, @DrAmount," +
                   " @CrAmount,@Narration,@TransactionWith,@ChequeNo,@ChequeDate,@BankName,@BranchName,@ChequeClear," +
                   " @AttachCheque,@AuditApprove,@AuditBy,@AuditIp," +
                   " @AuditTime,@ApproveBy,@ApproveIp,@ApproveTime,@AttachBill,@AttachReference,@ProductId,@ProductName,@CompanyId," +
                   " @EntryFrom,@UserName,@UserIp,@EntryTime,@CostCenterId,@CostCenterName,@ReferenceNo,@ReferenceDetails)";

                foreach (var item in objList)
                {


                    string sql = "insert into Vouchers (TransactionId,TransactionType,MasterNo,FiscalYearId,VoucherNo,VoucherDate," +
                    " VoucherType,LedgerId,LedgerCode,LedgerName,BalanceAmount,DrAmount," +
                    " CrAmount,Narration,TransactionWith,ChequeNo,ChequeDate,BankName,BranchName,ChequeClear," +
                    " AttachCheque,AuditApprove,AuditBy,AuditIp," +
                    " AuditTime,ApproveBy,ApproveIp,ApproveTime,AttachBill,AttachReference,ProductId,ProductName,CompanyId," +
                    " EntryFrom,UserName,UserIp,EntryTime,CostCenterId,CostCenterName,ReferenceNo,ReferenceDetails) " +
                    " values (@TransactionId,@TransactionType,@MasterNo,@FiscalYearId,@VoucherNo,@VoucherDate," +
                    " @VoucherType,@LedgerId,@LedgerCode,@LedgerName,@BalanceAmount,@DrAmount," +
                    " @CrAmount,@Narration,@TransactionWith,@ChequeNo,@ChequeDate,@BankName,@BranchName,@ChequeClear," +
                    " @AttachCheque,@AuditApprove,@AuditBy,@AuditIp," +
                    " @AuditTime,@ApproveBy,@ApproveIp,@ApproveTime,@AttachBill,@AttachReference,@ProductId,@ProductName,@CompanyId," +
                    " @EntryFrom,@UserName,@UserIp,@EntryTime,@CostCenterId,@CostCenterName,@ReferenceNo,@ReferenceDetails)";

                    credit += item.CrAmount;
                    
                    cmd = new SqlCommand(sql, con,transaction);
                    cmd.Parameters.AddWithValue("@TransactionId", item.TransactionId);
                    cmd.Parameters.AddWithValue("@TransactionType", transType);
                    cmd.Parameters.AddWithValue("@MasterNo", "");
                    cmd.Parameters.AddWithValue("@FiscalYearId", fiscalYear);
                    cmd.Parameters.AddWithValue("@VoucherNo", item.VoucherNo);
                    cmd.Parameters.AddWithValue("@VoucherDate", item.VoucherDate);
                    cmd.Parameters.AddWithValue("@VoucherType", "cca");
                    cmd.Parameters.AddWithValue("@LedgerId", item.LedgerId);
                    cmd.Parameters.AddWithValue("@LedgerCode", item.LedgerCode);
                    
                    cmd.Parameters.AddWithValue("@LedgerName", item.LedgerName);
                    cmd.Parameters.AddWithValue("@BalanceAmount", 0);
                    cmd.Parameters.AddWithValue("@DrAmount", 0);
                    cmd.Parameters.AddWithValue("@CrAmount", item.CrAmount);
                    cmd.Parameters.AddWithValue("@Narration", (item.Narration==null?"":item.Narration));
                    cmd.Parameters.AddWithValue("@TransactionWith", item.TransactionWith==null?"":item.TransactionWith);
                    cmd.Parameters.AddWithValue("@ChequeNo", "");
                    cmd.Parameters.AddWithValue("@ChequeDate", "1900-01-01");
                    cmd.Parameters.AddWithValue("@BankName", "");
                    cmd.Parameters.AddWithValue("@BranchName", "");
                    cmd.Parameters.AddWithValue("@ChequeClear", "1");
                    cmd.Parameters.AddWithValue("@AttachCheque", "");
                    cmd.Parameters.AddWithValue("@AuditApprove", AuditApprove);
                    cmd.Parameters.AddWithValue("@AuditBy", AuditBy);
                    cmd.Parameters.AddWithValue("@AuditIp", AuditIp);
                    cmd.Parameters.AddWithValue("@AuditTime", AuditTime);
                    cmd.Parameters.AddWithValue("@ApproveBy", ApproveBy);
                    cmd.Parameters.AddWithValue("@ApproveIp", ApproveIp);
                    cmd.Parameters.AddWithValue("@ApproveTime", ApproveTime);
                    cmd.Parameters.AddWithValue("@AttachBill", objList[0].AttachBill);
                    cmd.Parameters.AddWithValue("@AttachReference", "");
                    cmd.Parameters.AddWithValue("@ProductId", "");
                    cmd.Parameters.AddWithValue("@ProductName", "");
                    cmd.Parameters.AddWithValue("@EntryFrom", "Cash Received Voucher");

                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@UserIp", userIp);
                    cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@CostCenterId", "U-1");
                    cmd.Parameters.AddWithValue("@CostCenterName", "Rupali");
                    cmd.Parameters.AddWithValue("@ReferenceNo", "");
                    cmd.Parameters.AddWithValue("@ReferenceDetails", "");

                    // cmd.Parameters.AddWithValue("@BranchId", branchId);
                    cmd.ExecuteNonQuery();
                }

                cmd1 = new SqlCommand(sql2, con,transaction);
                cmd1.Parameters.AddWithValue("@TransactionId", objList[0].TransactionId);
                cmd1.Parameters.AddWithValue("@TransactionType", transType);
                cmd1.Parameters.AddWithValue("@MasterNo", "");
                cmd1.Parameters.AddWithValue("@FiscalYearId", fiscalYear);
                cmd1.Parameters.AddWithValue("@VoucherNo", objList[0].VoucherNo);
                cmd1.Parameters.AddWithValue("@VoucherDate", objList[0].VoucherDate);
                cmd1.Parameters.AddWithValue("@VoucherType", "cca");
                cmd1.Parameters.AddWithValue("@LedgerId", bankHeadId);
                cmd1.Parameters.AddWithValue("@LedgerCode", bankCode); //LedgerCode
                cmd1.Parameters.AddWithValue("@LedgerName", bankHead);
                cmd1.Parameters.AddWithValue("@BalanceAmount", 0);
                cmd1.Parameters.AddWithValue("@DrAmount", credit);
                cmd1.Parameters.AddWithValue("@CrAmount", 0);
                cmd1.Parameters.AddWithValue("@Narration", (objList[0].Narration==null?"":objList[0].Narration));
                cmd1.Parameters.AddWithValue("@TransactionWith", objList[0].TransactionWith == null ? "" : objList[0].TransactionWith);
                cmd1.Parameters.AddWithValue("@ChequeNo", "");
                cmd1.Parameters.AddWithValue("@ChequeDate", "1900-01-01");
                cmd1.Parameters.AddWithValue("@BankName", "");
                cmd1.Parameters.AddWithValue("@BranchName", "");
                cmd1.Parameters.AddWithValue("@ChequeClear", "1");
                cmd1.Parameters.AddWithValue("@AttachCheque", "");
                cmd1.Parameters.AddWithValue("@AuditApprove", AuditApprove);
                cmd1.Parameters.AddWithValue("@AuditBy", AuditBy);
                cmd1.Parameters.AddWithValue("@AuditIp", AuditIp);
                cmd1.Parameters.AddWithValue("@AuditTime", AuditTime);
                cmd1.Parameters.AddWithValue("@ApproveBy", ApproveBy);
                cmd1.Parameters.AddWithValue("@ApproveIp", ApproveIp);
                cmd1.Parameters.AddWithValue("@ApproveTime", ApproveTime);
                cmd1.Parameters.AddWithValue("@AttachBill", objList[0].AttachBill);
                cmd1.Parameters.AddWithValue("@AttachReference", "");
                cmd1.Parameters.AddWithValue("@ProductId", "");
                cmd1.Parameters.AddWithValue("@ProductName", "");
                cmd1.Parameters.AddWithValue("@EntryFrom", "Cash Received Voucher");

                cmd1.Parameters.AddWithValue("@UserName", userName);
                cmd1.Parameters.AddWithValue("@UserIp", userIp);
                cmd1.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                cmd1.Parameters.AddWithValue("@CompanyId", companyId);
                //cmd1.Parameters.AddWithValue("@BranchId", branchId);
                cmd1.Parameters.AddWithValue("@CostCenterId", "U-1");
                cmd1.Parameters.AddWithValue("@CostCenterName", "Rupali");
                cmd1.Parameters.AddWithValue("@ReferenceNo", "");
                cmd1.Parameters.AddWithValue("@ReferenceDetails", "");
                cmd1.ExecuteNonQuery();

                transaction.Commit();
                ret = 1;
            }
            catch(Exception exp)
            {
                if(transaction!=null)
                {
                    transaction.Rollback();
                }
                var e = exp;
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        
    }
}
