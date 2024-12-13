
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
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
    public class JournalVoucherServiceWebUi
    {
        
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        private commonService commonService;
        string sqlCon;
        public JournalVoucherServiceWebUi( IHttpContextAccessor accessor, AppDbContext db)
        {
           
            _accessor = accessor;
            _db = db;
            sqlCon= _db.Database.GetDbConnection().ConnectionString;
            commonService = new commonService(_accessor,_db);
        }
        public async Task<ActionResult<IEnumerable<object>>>  ledgerList()
        {
            string companyId = "B-1";

            //var obj = from x in _unitAccounts.Ledger.GetAll(
            //    x => //x.PrimaryGroupId!="A1" &&
            //    x.LedgerType=="General" && 
            //    x.CompanyId == Convert.ToInt32(companyId))
            //      select new { Id = x.Id, Name = x.LedgerName }

            var obj = await _db.Ledgers
            .Where(x => x.CompanyId == companyId && x.LedgerType == "General")
            .OrderBy(x => x.LedgerName)
            .Select(x => new { Id = x.LedgerId, Name = x.LedgerName })
            .ToListAsync();

            return obj;
        }
        public async Task<ActionResult<IEnumerable<object>>> ledgerListDrCrHead()
        {
            string companyId = "B-1";

            //var obj = from x in _unitAccounts.Ledger.GetAll(
            //    x => //x.PrimaryGroupId != "A1" &&
            //    x.LedgerType == "General" &&
            //    x.CompanyId == Convert.ToInt32(companyId))
            //          select new { Id = x.Id, Name = x.LedgerName };
            var obj = await _db.Ledgers
            .Where(x => x.CompanyId == companyId && x.LedgerType == "General")
            .OrderBy(x => x.LedgerName)
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
        public string getBillSlip(string voucherNo)
        {
            var companyId = "B-1";
            // var branchId = session.GetString("branchId");
            string ret = "";
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct AttachBill from Vouchers where VoucherNo like @voucherNo and CompanyId = @companyId ";  // and BranchId = @branchId


                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@voucherNo", voucherNo);
                cmd.Parameters.AddWithValue("@companyId", companyId);
                //cmd.Parameters.AddWithValue("@branchId", branchId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        ret = sqlData["AttachBill"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }

        public string getVoucherNo(string date)
        {
            string ret = "";

            //ISession session = commonService.getSession();
            var companyId = "B-1";
            //var branchId = session.GetString("branchId");

            string fsl = commonService.getFiscalYear(date, companyId);

            
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "Select cast(isnull(max(cast(replace(substring(VoucherNo,7,len(VoucherNo)), '', '')as int))+1, 1)as varchar) " +
                    " voucherNo from Vouchers where VoucherNo like 'JV-NO-%' and FiscalYearId like @fsl " +
                    " and CompanyId = @companyId ";    //and BranchId = @branchId

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@fsl", fsl);
                cmd.Parameters.AddWithValue("@companyId", companyId);
               // cmd.Parameters.AddWithValue("@branchId", branchId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        ret = sqlData["voucherNo"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        public IEnumerable<object> getData(string transactionId, string date)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);

            //ISession session = commonService.getSession();
            var companyId = "B-1";
            // var branchId = session.GetString("branchId");
            try
            {
                con.Open();
                string sql = "select a.AutoId Id,a.TransactionId,a.VoucherNo,a.VoucherDate," +
                " b.LedgerId  lId, b.LedgerId,b.LedgerCode,b.LedgerName,a.AttachBill,Narration, " +
                " CONVERT(float, a.CrAmount)CrAmount,CONVERT(float, a.DrAmount)DrAmount,a.ReferenceDetails JVType " +
                " from Vouchers a inner join Ledgers b on a.LedgerId = b.LedgerId " +
                " where a.VoucherType = 'jau' and a.transactionId like @transactionId " +
                " and a.EntryFrom like '%Journal Voucher%'";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@transactionId", transactionId);
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
                        Narration = sqlData["Narration"].ToString(),
                        Attachment = sqlData["AttachBill"].ToString(),
                        drAmount = sqlData["DrAmount"].ToString(),
                        crAmount = sqlData["CrAmount"].ToString(),
                        jvType = sqlData["JVType"].ToString()
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
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
          //  ISession session = commonService.getSession();
            var companyId = "B-1";
            //  var branchId = session.GetString("branchId");
            try
            {
                con.Open();
                string sql = "select distinct Max(a.AutoId)Id,TransactionId,VoucherNo, convert(varchar,VoucherDate)VoucherDate," +
                " Narration,CONVERT(float,SUM(CrAmount))CrAmount,CONVERT(float,SUM(DrAmount))DrAmount,AttachBill,a.AuditApprove,u.FullName ApproveBy,Max(ApproveTime) ApproveTime " +
                " from Vouchers a " +
                 "left join AspNetUsers u on a.ApproveBy = u.FullName " +
                "where VoucherType = 'jau'   and convert(date, a.VoucherDate) between convert(varchar, '" + fromDate + "',105) and convert(varchar, '" + toDate + "',105)  " +
                " and EntryFrom like '%Journal Voucher%'  "+
                " group by TransactionId,VoucherNo,VoucherDate,Narration,AttachBill,AuditApprove,u.FullName order by CONVERT(varchar,VoucherDate) desc";

                SqlCommand cmd = new SqlCommand(sql,con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while(sqlData.Read())
                {
                    returnData.Add(new
                    {
                        TransactionId = sqlData["TransactionId"].ToString(),
                        VoucherNo = sqlData["VoucherNo"].ToString(),
                        VoucherDate = sqlData["VoucherDate"].ToString(),
                        Narration = sqlData["Narration"].ToString(),
                        CrAmount = sqlData["CrAmount"].ToString(),
                        DrAmount = sqlData["DrAmount"].ToString(),
                        Attachment = sqlData["AttachBill"].ToString(),
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
            var returnData = commonService.LedgerBudgetBalance(id,voucherDate);
            return returnData;
        }
        
        public string getAttachBill(string voucherNo)
        {
           // ISession session = commonService.getSession();
            var companyId = "B-1";
           // var branchId = session.GetString("branchId");
            string ret = "";
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select AttachBill from Vouchers where VoucherNo like @voucherNo  and CompanyId = @companyId "; //and BranchId = @branchId
                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.Parameters.AddWithValue("@voucherNo",voucherNo);
                cmd.Parameters.AddWithValue("@companyId", companyId);
                //cmd.Parameters.AddWithValue("@branchId", branchId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if(sqlData.Read())
                {
                    if(sqlData.HasRows)
                    {
                        ret = sqlData["AttachBill"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        
        public int journalVoucherSave(List<Voucher> objList,string typeVoucher,string bankHeadId,string bankHeadName,
            string bankCode, string userName,string userIp)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlTransaction transaction = null;
            SqlCommand cmd, cmd1,cmd2,cmd3,cmd4,cmd5;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("JournalVoucher");
                //ISession session = commonService.getSession();
                var companyId = "B-1";
                //var branchId = session.GetString("branchId");
                string fiscalYear = commonService.getFiscalYear(objList[0].VoucherDate.ToString("yyyy/MM/dd"), companyId);
                string transType = "Transfer";
                decimal debit = 0;

                string AuditApprove = "1";
                string AuditBy = "";
                string AuditIp = "";
                DateTime AuditTime = DateTime.Now;
                string ApproveBy = "";
                string ApproveIp = "";
                DateTime ApproveTime = DateTime.Now;
                string transId = objList[0].TransactionId.ToString();

                if (fiscalYear == "0")
                {
                    return 0;
                }

                if (objList[0].AutoId != 0)
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
                        //branchId = ainfos[8].ToString();
                    }
                    
                    string sqlDel = "delete Vouchers where TransactionId like @transactionId and VoucherNo like @voucherNo " +
                       " and CompanyId = @companyId "; //and BranchId = @branchId


                    //string sqlUd = "insert into tbUdVoucher " +
                    //" select TransactionId, TransactionType, MRNo, FiscalYearId, VoucherNo, VoucherDate, VoucherType, LedgerId, LedgerName, DrAmount," +
                    //" CrAmount, Narration, TransactionWith, ChequeNo, ChequeDate, BankName, BranchName, ChequeClear, AttachCheque, AuditApprove, " +
                    //" AuditBy, AuditIp, AuditTime, ApproveBy, ApproveIp, ApproveTime, AttachBill, AttachReference, POSId, POSName, CompanyId, " +
                    //" EntryFrom,'Old',UserId,UserName,UserIp,EntryTime,JVType,BranchId " +
                    //" from tbVoucher where VoucherNo like @voucherNo and TransactionId like @transactionId and CompanyId = @companyId and BranchId = @branchId ";

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

                string sqlDebit2 = "insert into Vouchers (TransactionId,TransactionType,MasterNo,FiscalYearId,VoucherNo,VoucherDate," +
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

                string sqlCredit2 = "insert into Vouchers (TransactionId,TransactionType,MasterNo,FiscalYearId,VoucherNo,VoucherDate," +
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

                foreach (var item in objList)
                {
                    if(typeVoucher.Equals("Debit"))
                    {
                        string sqlDebit = "insert into Vouchers (TransactionId,TransactionType,MasterNo,FiscalYearId,VoucherNo,VoucherDate," +
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

                        debit += item.DrAmount;

                        cmd = new SqlCommand(sqlDebit, con, transaction);
                        cmd.Parameters.AddWithValue("@TransactionId", objList[0].TransactionId);
                        cmd.Parameters.AddWithValue("@TransactionType", transType);
                        cmd.Parameters.AddWithValue("@MasterNo", "");
                        cmd.Parameters.AddWithValue("@FiscalYearId", fiscalYear);
                        cmd.Parameters.AddWithValue("@VoucherNo", objList[0].VoucherNo);
                        cmd.Parameters.AddWithValue("@VoucherDate", item.VoucherDate);
                        cmd.Parameters.AddWithValue("@VoucherType", "jau");
                        cmd.Parameters.AddWithValue("@LedgerId", item.LedgerId);
                        cmd.Parameters.AddWithValue("@LedgerCode", item.LedgerCode);
                        cmd.Parameters.AddWithValue("@LedgerName", item.LedgerName);
                        cmd.Parameters.AddWithValue("@BalanceAmount", 0);
                        cmd.Parameters.AddWithValue("@DrAmount", 0);
                        cmd.Parameters.AddWithValue("@CrAmount", item.DrAmount);
                        cmd.Parameters.AddWithValue("@Narration", (item.Narration == null ? "" : item.Narration));
                        cmd.Parameters.AddWithValue("@TransactionWith", (item.TransactionWith == null ? "" : item.TransactionWith));
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
                        cmd.Parameters.AddWithValue("@EntryFrom", "Journal Voucher");
                       // cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@UserIp", userIp);
                        cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                        //cmd.Parameters.AddWithValue("@JVType", typeVoucher);
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);

                        cmd.Parameters.AddWithValue("@CostCenterId", "U-1");
                        cmd.Parameters.AddWithValue("@CostCenterName", "Rupali");
                        cmd.Parameters.AddWithValue("@ReferenceNo", "");
                        cmd.Parameters.AddWithValue("@ReferenceDetails", typeVoucher);

                        //cmd.Parameters.AddWithValue("@BranchId", branchId);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string sqlCredit = "insert into Vouchers (TransactionId,TransactionType,MasterNo,FiscalYearId,VoucherNo,VoucherDate," +
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

                        debit += item.DrAmount;

                        cmd1 = new SqlCommand(sqlCredit, con, transaction);
                        cmd1.Parameters.AddWithValue("@TransactionId", objList[0].TransactionId);
                        cmd1.Parameters.AddWithValue("@TransactionType", transType);
                        cmd1.Parameters.AddWithValue("@MasterNo", "");
                        cmd1.Parameters.AddWithValue("@FiscalYearId", fiscalYear);
                        cmd1.Parameters.AddWithValue("@VoucherNo", objList[0].VoucherNo);
                        cmd1.Parameters.AddWithValue("@VoucherDate", item.VoucherDate);
                        cmd1.Parameters.AddWithValue("@VoucherType", "jau");
                        cmd1.Parameters.AddWithValue("@LedgerId", item.LedgerId);
                        cmd1.Parameters.AddWithValue("@LedgerCode", item.LedgerCode);
                        cmd1.Parameters.AddWithValue("@LedgerName", item.LedgerName);
                        cmd1.Parameters.AddWithValue("@BalanceAmount", 0);
                        cmd1.Parameters.AddWithValue("@DrAmount", item.DrAmount);
                        cmd1.Parameters.AddWithValue("@CrAmount", 0);
                        cmd1.Parameters.AddWithValue("@Narration", (item.Narration == null ? "" : item.Narration));
                        cmd1.Parameters.AddWithValue("@TransactionWith", (item.TransactionWith == null ? "" : item.TransactionWith));
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
                        cmd1.Parameters.AddWithValue("@EntryFrom", "Journal Voucher");
                       // cmd1.Parameters.AddWithValue("@UserId", userId);
                        cmd1.Parameters.AddWithValue("@UserName", userName);
                        cmd1.Parameters.AddWithValue("@UserIp", userIp);
                        cmd1.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                        //cmd1.Parameters.AddWithValue("@JVType", typeVoucher);
                        cmd1.Parameters.AddWithValue("@CompanyId", companyId);
                        // cmd1.Parameters.AddWithValue("@BranchId", branchId);


                        cmd1.Parameters.AddWithValue("@CostCenterId", "U-1");
                        cmd1.Parameters.AddWithValue("@CostCenterName", "Rupali");
                        cmd1.Parameters.AddWithValue("@ReferenceNo", "");
                        cmd1.Parameters.AddWithValue("@ReferenceDetails", typeVoucher);

                        cmd1.ExecuteNonQuery();
                    }
                }
                if(typeVoucher.Equals("Debit"))
                {
                    cmd4 = new SqlCommand(sqlDebit2, con, transaction);
                    cmd4.Parameters.AddWithValue("@TransactionId", objList[0].TransactionId);
                    cmd4.Parameters.AddWithValue("@TransactionType", transType);
                    cmd4.Parameters.AddWithValue("@MasterNo", "");
                    cmd4.Parameters.AddWithValue("@FiscalYearId", fiscalYear);
                    cmd4.Parameters.AddWithValue("@VoucherNo", objList[0].VoucherNo);
                    cmd4.Parameters.AddWithValue("@VoucherDate", objList[0].VoucherDate);
                    cmd4.Parameters.AddWithValue("@VoucherType", "jau");
                    cmd4.Parameters.AddWithValue("@LedgerId", bankHeadId);
                    cmd4.Parameters.AddWithValue("@LedgerCode", bankCode);
                    cmd4.Parameters.AddWithValue("@LedgerName", bankHeadName);
                    cmd4.Parameters.AddWithValue("@BalanceAmount", 0);
                    cmd4.Parameters.AddWithValue("@DrAmount", debit);
                    cmd4.Parameters.AddWithValue("@CrAmount", 0);
                    cmd4.Parameters.AddWithValue("@Narration", (objList[0].Narration == null ? "" : objList[0].Narration));
                    cmd4.Parameters.AddWithValue("@TransactionWith", (objList[0].TransactionWith == null ? "" : objList[0].TransactionWith));
                    cmd4.Parameters.AddWithValue("@ChequeNo", "");
                    cmd4.Parameters.AddWithValue("@ChequeDate", "1900-01-01");
                    cmd4.Parameters.AddWithValue("@BankName", "");
                    cmd4.Parameters.AddWithValue("@BranchName", "");
                    cmd4.Parameters.AddWithValue("@ChequeClear", "1");
                    cmd4.Parameters.AddWithValue("@AttachCheque", "");
                    cmd4.Parameters.AddWithValue("@AuditApprove", AuditApprove);
                    cmd4.Parameters.AddWithValue("@AuditBy", AuditBy);
                    cmd4.Parameters.AddWithValue("@AuditIp", AuditIp);
                    cmd4.Parameters.AddWithValue("@AuditTime", AuditTime);
                    cmd4.Parameters.AddWithValue("@ApproveBy", ApproveBy);
                    cmd4.Parameters.AddWithValue("@ApproveIp", ApproveIp);
                    cmd4.Parameters.AddWithValue("@ApproveTime", ApproveTime);
                    cmd4.Parameters.AddWithValue("@AttachBill", objList[0].AttachBill);
                    cmd4.Parameters.AddWithValue("@AttachReference", "");
                    cmd4.Parameters.AddWithValue("@ProductId", "");
                    cmd4.Parameters.AddWithValue("@ProductName", "");
                    cmd4.Parameters.AddWithValue("@EntryFrom", "Journal Voucher");
                    //cmd4.Parameters.AddWithValue("@UserId", userId);
                    cmd4.Parameters.AddWithValue("@UserName", userName);
                    cmd4.Parameters.AddWithValue("@UserIp", userIp);
                    cmd4.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                   // cmd4.Parameters.AddWithValue("@JVType", typeVoucher);
                    cmd4.Parameters.AddWithValue("@CompanyId", companyId);
                    // cmd4.Parameters.AddWithValue("@BranchId", branchId);

                    cmd4.Parameters.AddWithValue("@CostCenterId", "U-1");
                    cmd4.Parameters.AddWithValue("@CostCenterName", "Rupali");
                    cmd4.Parameters.AddWithValue("@ReferenceNo", "");
                    cmd4.Parameters.AddWithValue("@ReferenceDetails", typeVoucher);


                    cmd4.ExecuteNonQuery();
                    transaction.Commit();
                }
                else
                {
                    cmd5 = new SqlCommand(sqlCredit2, con, transaction);
                    cmd5.Parameters.AddWithValue("@TransactionId", objList[0].TransactionId);
                    cmd5.Parameters.AddWithValue("@TransactionType", transType);
                    cmd5.Parameters.AddWithValue("@MasterNo", "");
                    cmd5.Parameters.AddWithValue("@FiscalYearId", fiscalYear);
                    cmd5.Parameters.AddWithValue("@VoucherNo", objList[0].VoucherNo);
                    cmd5.Parameters.AddWithValue("@VoucherDate", objList[0].VoucherDate);
                    cmd5.Parameters.AddWithValue("@VoucherType", "jau");
                    cmd5.Parameters.AddWithValue("@LedgerId", bankHeadId);
                    cmd5.Parameters.AddWithValue("@LedgerCode", bankCode);
                    cmd5.Parameters.AddWithValue("@LedgerName", bankHeadName);
                    cmd5.Parameters.AddWithValue("@BalanceAmount", 0);
                    cmd5.Parameters.AddWithValue("@DrAmount", 0);
                    cmd5.Parameters.AddWithValue("@CrAmount", debit);
                    cmd5.Parameters.AddWithValue("@Narration", (objList[0].Narration == null ? "" : objList[0].Narration));
                    cmd5.Parameters.AddWithValue("@TransactionWith", (objList[0].TransactionWith == null ? "" : objList[0].TransactionWith));
                    cmd5.Parameters.AddWithValue("@ChequeNo", "");
                    cmd5.Parameters.AddWithValue("@ChequeDate", "1900-01-01");
                    cmd5.Parameters.AddWithValue("@BankName", "");
                    cmd5.Parameters.AddWithValue("@BranchName", "");
                    cmd5.Parameters.AddWithValue("@ChequeClear", "1");
                    cmd5.Parameters.AddWithValue("@AttachCheque", "");
                    cmd5.Parameters.AddWithValue("@AuditApprove", AuditApprove);
                    cmd5.Parameters.AddWithValue("@AuditBy", AuditBy);
                    cmd5.Parameters.AddWithValue("@AuditIp", AuditIp);
                    cmd5.Parameters.AddWithValue("@AuditTime", AuditTime);
                    cmd5.Parameters.AddWithValue("@ApproveBy", ApproveBy);
                    cmd5.Parameters.AddWithValue("@ApproveIp", ApproveIp);
                    cmd5.Parameters.AddWithValue("@ApproveTime", ApproveTime);
                    cmd5.Parameters.AddWithValue("@AttachBill", objList[0].AttachBill);
                    cmd5.Parameters.AddWithValue("@AttachReference", "");
                    cmd5.Parameters.AddWithValue("@ProductId", "");
                    cmd5.Parameters.AddWithValue("@ProductName", "");
                    cmd5.Parameters.AddWithValue("@EntryFrom", "Journal Voucher");
                    //cmd5.Parameters.AddWithValue("@UserId", userId);
                    cmd5.Parameters.AddWithValue("@UserName", userName);
                    cmd5.Parameters.AddWithValue("@UserIp", userIp);
                    cmd5.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                    //cmd5.Parameters.AddWithValue("@JVType", typeVoucher);
                    cmd5.Parameters.AddWithValue("@CompanyId", companyId);
                    // cmd5.Parameters.AddWithValue("@BranchId", branchId);

                    cmd5.Parameters.AddWithValue("@CostCenterId", "U-1");
                    cmd5.Parameters.AddWithValue("@CostCenterName", "Rupali");
                    cmd5.Parameters.AddWithValue("@ReferenceNo", "");
                    cmd5.Parameters.AddWithValue("@ReferenceDetails", typeVoucher);

                    cmd5.ExecuteNonQuery();
                    transaction.Commit();
                }
                ret = 1;
            }
            catch (Exception exp)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                var exception = exp;
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        
    }
}
