
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using RCCLAccounts.WebUi.Reports;
using RCCLAccounts.Data;
using FastReport;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Services;
using RCCLAccounts.Data.Entities;
using System.Drawing;
using Fizzler;
using Microsoft.CodeAnalysis.Operations;
using RCCLAccounts.WebUi.Common;
using RCCLAccounts.WebUi.Services;

namespace RCCLAccounts.WebUi.Controllers
{

    public class ReportController : Controller
    {
        #region ClassDeclare

        public class LedgerList
        {
            public string LedgerId { get; set; }
            public string LedgerName { get; set; }
        }

        #endregion

        private readonly ILogger<ReportController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
  
        private readonly IHttpContextAccessor _accessor;
   
        private string reportName = "";
        private string _title = "";
        private string caption = "";
        List<string> sqls = new List<string>();

       // private ApplicationDbContext _db { get; }
        private AppDbContext _db { get; }
        string sqlCon;
        string hostUrl;

        AccountReportServiceWebUi service;
        public ReportController(IWebHostEnvironment webHostEnvironment,
            ILogger<ReportController> logger,     
            IHttpContextAccessor accessor,           
            AppDbContext db)
            
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;       
            _accessor = accessor;
            _db = db;
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
       
            hostUrl = _accessor.HttpContext.Request.Scheme + "://" + _accessor.HttpContext.Request.Host;
            
            service = new AccountReportServiceWebUi(_accessor, _db);
        }
       public IActionResult Index()
       {
           
         return View();

        }
        public async Task<IActionResult> PertialView(string viewName)
        {

            ViewBag.Ledger = new SelectList(GetLedgerLoad(), "LedgerId", "LedgerName");
            return PartialView(viewName);

        }
        //


        public IActionResult VoucherAll(string vType, string findType, string fromDate, string toDate, string voucherNo)
        {
         
            var companyId = "B-1";          
            string voucherData;
            string voucherType = service.getVoucherName(vType);
            voucherNo = voucherNo.Equals("All") ? "%" : voucherNo;
            if (findType == "Date")
            {
                voucherData = " v.VoucherNo IN(select item from Split('" + voucherNo + "')) " +
                    " AND v.voucherDate BETWEEN '" + fromDate + "' AND '" + toDate + "' and v.EntryFrom like '%" + voucherType + "%' ";
            }
            else
            {
                voucherData = " v.VoucherNo = '" + voucherNo + "' and v.EntryFrom = '" + voucherType + "'";
            }
            string sql = "select a.VoucherNo,DAY(VoucherDate)dV,MONTH(VoucherDate)mV,YEAR(VoucherDate)yV," +
            " a.Narration,CONVERT(float,a.CrAmount)crAmount,dbo.number(a.DrAmount)drWords, " +
            " dbo.number(a.CrAmount)crWords,a.VoucherType,a.TransactionWith,a.CompanyId,a.UserName,a.LedgerId,a.TransactionType, " +
            " CONVERT(varchar, a.ChequeDate, 105)ChequeDate,a.LedgerCode debitCode,a.LedgerName debitLedger," +
            " b.LedgerCode creditCode,b.LedgerName creditLedger,a.ChequeNo " +
            " from(SELECT  VoucherNo, VoucherDate, Narration, DrAmount, CrAmount, VoucherType, TransactionWith, " +
            " v.CompanyId, v.UserName, v.LedgerId, l.LedgerName, v.TransactionType, v.ChequeDate,l.LedgerCode,v.ChequeNo FROM Vouchers v " +
            " INNER JOIN Ledgers l ON v.LedgerId = l.LedgerId WHERE " + voucherData + " and CrAmount != 0 and v.CompanyId like '" + companyId + "') as a " +
            " left join(SELECT VoucherNo, l.LedgerCode, l.LedgerName, v.CrAmount FROM Vouchers v INNER JOIN Ledgers l ON v.LedgerId = l.LedgerId " +
            " WHERE " + voucherData + " and DrAmount!= 0 and v.CompanyId like '" + companyId + "' ) b on a.VoucherNo = b.VoucherNo";
            switch (voucherType)
            {
                case "Cash Payment Voucher":
                    reportName = "rptCashPaymentVoucherSingle.frx";
                    _title = "CASH DEBIT VOUCHER";

                    break;
                case "Bank Payment Voucher":
                    reportName = "rptBankPaymentVoucherSingle.frx";
                    _title = "BANK DEBIT VOUCHER";
                    break;
                case "Journal Voucher":
                    reportName = "rptJournalVoucher.frx";
                    _title = "JOURNAL VOUCHER";
                    sql = "SELECT VoucherNo,DAY(VoucherDate)dV,MONTH(VoucherDate)mV,YEAR(VoucherDate)yV," +
                    " VoucherDate,Narration,DrAmount,CrAmount,VoucherType," +
                    " TransactionWith,v.CompanyId,v.UserName, v.LedgerId, l.LedgerName,v.TransactionType, " +
                    " CONVERT(varchar, v.ChequeDate, 105)dtCheque, " +
                    " dbo.NumberToWords(((select SUM(CrAmount) from Vouchers where " +
                    " VoucherNo = v.VoucherNo and " + voucherData + " and CompanyId like '" + companyId + "' )))drWords " +
                    " FROM Vouchers v INNER JOIN Ledgers l ON v.LedgerId = l.LedgerId WHERE " + voucherData + " and v.CompanyId like '" + companyId + "'  " +
                    " order by VoucherNo,CrAmount";
                    break;
                default:
                    reportName = "rptCashPaymentVoucher.frx";
                    _title = "DEBIT VOUCHER";
                    sql = "select VoucherNo,DAY(VoucherDate)dV,MONTH(VoucherDate)mV,YEAR(VoucherDate)yV,Narration,TransactionWith,LedgerName," +
                    " CONVERT(float, DrAmount)DrAmount,CONVERT(float, CrAmount)CrAmount,dbo.number((select SUM(DrAmount) from Vouchers where VoucherNo=a.VoucherNo and EntryFrom=a.EntryFrom and CompanyId=a.CompanyId ))drWords " +
                    " from Vouchers a where VoucherNo LIKE '" + voucherNo + "' and EntryFrom like '%" + voucherType + "%' and a.CompanyId like '" + companyId + "'  order by DrAmount";
                    break;
            }

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
       

        // Chart Of Account
        public IActionResult ChartOfAccount()
        {
            reportName = "rptChartOfAccount.frx";
            _title = "Chart of Account";
            var sql = " Select L.LedgerId,L.LedgerCode,L.LedgerName,P.PrimaryGroupId,P.PrimaryGroupName, Isnull(M.MainGroupId,'')MainGroupId,Isnull(M.MainGroupName,'')MainGroupName,Isnull(S.SubGroupId,'')SubGroupId, Isnull(S.SubGroupName,'')SubGroupName,P.ItemOf, Case When P.ItemOf ='Asset' Then 1 When P.ItemOf ='Liability' Then 2 When P.ItemOf ='Expense' Then 3 When P.ItemOf ='Income' Then 4 else 0 End As Type from [dbo].[Ledgers] L inner join   [dbo].[PrimaryGroups] P on L.PrimaryId=p.PrimaryId Left outer join [dbo].[MainGroups] M On L.MainId=M.MainId Left outer join [dbo].[SubGroups] S On L.SubId=S.SubId order by Type,P.PrimaryGroupId,M.MainGroupId, Cast (SUBSTRING(L.LedgerId, 3, 50) as int) ";
             _logger.LogInformation(sql);
             sqls.Clear();
             sqls.Add(sql);
            return ShowReport(0);
        }

        /// <summary>
        /// Opening Trail Balance
        /// </summary>
        /// <param name="fiscalYearId"></param>
        /// <param name="fiscalYear"></param>
        /// <param name="balanceType"></param>
        /// <returns></returns>
        public IActionResult OpeningTrailBalance(string fiscalYearId, string fiscalYear, string balanceType)
        {
            reportName = "rptOpeningTrailBalance.frx";
            _title = "OPENING TRIAL BALANCE (LEDGER WISE)";
            caption = fiscalYear;
          
            var companyId = "B-1";
           
            string clause = "";


            if (balanceType.Equals("With"))
            {
                clause = " where balance!=0 ";
            }
            string sql = "select * from funOpeningTrialBalance('" + fiscalYearId + "','" + companyId + "') " + clause + " " +
                " order by SL,PrimaryGroupCode,HeadName," +
                " MainGroupCode,GroupName,SubGroupCode,SubGroupName,LedgerCode,LedgerName";
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
        public IActionResult GetDebitCreditSuplementory(String FromDate, String Todate,String VoucherType)
        {
            reportName = "rptDebitCreditSuplementory.frx";
            _title = "Suplementory ";
            caption = VoucherType;
            var sql = "";

            if (VoucherType == "DEBIT")
            {
                sql = " select VoucherDate,VoucherNo,TransactionType,Narration,DrAmount as Amount,LedgerName,UserName,UserIp,EntryTime,ChequeNo,AuditApprove," +
                      " case when AuditApprove=1 then 'Unauthorised' else  'Authorised' end as Autho from Vouchers where cast (VoucherDate AS date)" +
                      " between CONVERT(date,'" + FromDate + "') and CONVERT(date,'" + Todate + "')  and LedgerId not in ('AL220','AL221','AL222') " +
                      " and DrAmount>0 AND CompanyId like 'B-1'  and FiscalYearId like 'FY-1' order by VoucherDate,LedgerName,TransactionType,VoucherNo ";
            }
            else {

                sql = " select VoucherDate,VoucherNo,TransactionType,Narration,CrAmount as Amount,LedgerName,UserName,UserIp,EntryTime,ChequeNo,AuditApprove," +
                      " case when AuditApprove=1 then 'Unauthorised' else  'Authorised' end as Autho from Vouchers where cast (VoucherDate AS date)" +
                      " between CONVERT(date,'" + FromDate + "') and CONVERT(date,'" + Todate + "') and LedgerId not in ('AL220','AL221','AL222')  " +
                      " and CrAmount>0  AND CompanyId like 'B-1'  and FiscalYearId like 'FY-1' order by VoucherDate,LedgerName,TransactionType,VoucherNo  ";
            }

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        // Trail Balance Between Date
        public IActionResult GetTrailBalanceBetweenDate(String FromDate, String Todate)
        {

            string FiscalYear = service.getFiscalYear(FromDate, "B-1");

            reportName = "rptTrailBalanceBetweenDate.frx";
            _title = "Trail Balance Between Date ";

            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " Exec [dbo].[sp_RptTrailBalanceBetweenDate]  '" + FromDate + "' , '" + Todate + "' ,'B-1','" + FiscalYear + "'   ";


            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
        // Subsidariy Ledger
        public IActionResult GetSubsidiaryLedger(String FromDate, String Todate,String LedgerId)
        {
            reportName = "rptSubsidiaryLedger.frx";
            _title = "Subsidiary Ledger ";
            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " SELECT * FROM dbo.rptCostLedger('" + FromDate + "','" + Todate + "','" + LedgerId + "','%','B-1') order by date,convert(Numeric,autoid) ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);

            string sql1 = "select * from funLedgerPath('B-1','" + LedgerId + "')";

            sqls.Add(sql1);
            return ShowReport(1);
        }
  
        // Statement Of Affairs
        public IActionResult GetStatementOfAffairs(String FromDate, String Todate)
        {
            reportName = "rptStatementOfAffairs.frx";
            _title = "Statement Of Affairs";

            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " Select * from [dbo].[funcAffairs] (CONVERT(date,'" + FromDate + "') , CONVERT(date,'" + Todate + "'),'B-1','FY-1'  ) ";


            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }


        //  Income Statement
        public IActionResult GetIncomeStatementBetweenDate(String FromDate, String Todate)
        {
 
            reportName = "rptIncomeStatement.frx";
            _title = "Income Statement ";
             
            caption = " For The Period Of " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");
    
            var sql = " select * from [funAddjustedTrialBetweenDate] ('" + FromDate + "', '" + Todate + "', 'B-1') where (closingBal != 0 or Tranbal != 0 or Drbal != 0) order by sl,HeadId,GroupName,Ledger_Name  ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }


        //  Statement Of Affairs (Sub Group)
        public IActionResult GetStatementOfAffairsSubGroup(String FromDate, String Todate)
        {
            string FiscalYear = service.getFiscalYear(FromDate, "B-1");
            reportName = "rptStatementOfAffairs(Sub Group).frx";
            _title = "Statement of Affairs (Sub Group)";

            caption = " From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " select * from [funTrialBetweenDate] ('" + FromDate + "', '" + Todate + "', 'B-1','" + FiscalYear + "') where lgroup in ('Asset','Liabilitie') and (Op_Dr!=0 or Op_Cr!=0 or Trans_Dr!=0  or Trans_Cr!=0 or Cl_Dr!=0 or Cl_Cr!=0 ) order by sl,HeadId,GroupName,Ledger_Name  ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
        public ActionResult ShowReport(int multidbCount)
        {
            WebReport webReport = ReportGenerate(multidbCount);
            if (webReport != null)
            {
                return PartialView("_ReportView", webReport);
            }
            return NotFound();
        }
        public WebReport ReportGenerate(int multidbCount)
        {
            var path = $"{this._webHostEnvironment.WebRootPath}\\FastReport\\" + reportName;
            WebReport webReport = Reports.Report.GetReport(_accessor, path, sqls);
            if (webReport != null)
            {
                for (int i = 1; i <= multidbCount; i++)
                {
                    webReport.Report.Dictionary.Connections[i].ConnectionString = webReport.Report.Dictionary.Connections[0].ConnectionString;
                }
                webReport.Report.SetParameterValue("title", _title);
                webReport.Report.SetParameterValue("caption", caption);
                webReport.Report.Prepare();
                return webReport;
            }
            return null;
        }
        public static string dateToString(DateTime date, string format)
        {
            return date.ToString(format);
        }
        public static DateTime stringToDate(string date)
        {
            return Convert.ToDateTime(date);
        }

        #region Parameter Method

      
        public List<LedgerList> GetLedgerLoad()
        {
            List<LedgerList> ledgers = new List<LedgerList>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = " Select LedgerId,LedgerName from [dbo].[Ledgers] order by ledgerId";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    ledgers.Add(new LedgerList
                    {
                        LedgerId = sqlData["LedgerId"].ToString(),
                        LedgerName = sqlData["LedgerName"].ToString(),
                    });
                }
            }
            finally
            {
                con.Close();
            }

            return ledgers;

        }


        public IActionResult fiscalYearLoad()
        {
            var obj = service.getFiscalYearList();
            if (obj != null)
                return Json(new SelectList(obj, "Id", "Name"));
            return Json(new { });
        }

        #endregion

    }

}
