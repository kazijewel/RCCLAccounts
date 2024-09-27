
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
using ProvidentFund.WebUi.Reports;
using ProvidentFund.Data;
using FastReport;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Services;
using ProvidentFund.Data.Entities;
using static ProvidentFund.WebUi.Controllers.LoanDisbursmentController;
using System.Drawing;
using Fizzler;
using Microsoft.CodeAnalysis.Operations;

namespace ProvidentFund.WebUi.Controllers
{

    public class ReportController : Controller
    {
        #region ClassDeclare

        public class LoanInterestDate
        {
            public int TransactionID { get; set; }
            public string InterestDate { get; set; }
        }

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


        private readonly IEmployeeCPFOpeningService _employeeCPFOpeningService;
        private readonly IBankAccountInfoService _bankAccountInfoService;
        public IEmpService empService { get; set; }
        public IBankDepositService bankservice { get; set; }
        public ReportController(IWebHostEnvironment webHostEnvironment,
            ILogger<ReportController> logger,     
            IHttpContextAccessor accessor,           
            AppDbContext db,
            IEmployeeCPFOpeningService employeeCPFOpeningService,
            IEmpService _empService,
            IBankAccountInfoService bankAccountInfoService,
            IBankDepositService _bankservice)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;       
            _accessor = accessor;

            _db = db;
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            this.empService = _empService;
            hostUrl = _accessor.HttpContext.Request.Scheme + "://" + _accessor.HttpContext.Request.Host;
            _employeeCPFOpeningService = employeeCPFOpeningService;
            _bankAccountInfoService = bankAccountInfoService;
            this.bankservice = _bankservice;
        }
       public IActionResult Index()
       {
           
         return View();

            //reportName = "EmployeeInfo.frx";
            //_title = "Employye Information";

            //var sql = "SELECT * FROM EmployeeInfos";


            //_logger.LogInformation(sql);
            //sqls.Clear();
            //sqls.Add(sql);
            //return ShowReport(0);

        }
        public async Task<IActionResult> PertialView(string viewName)
        {

            ViewBag.Employee = new SelectList(await _employeeCPFOpeningService.GetAllEmpNoAndName(), "EmpolyeeId", "EmployeeName");
            ViewBag.Branches = new SelectList(await empService.GetBranches(), "BranchId", "BranchName");
            ViewBag.Bank = new SelectList(await _bankAccountInfoService.GetAllBankAsync(), "BankId", "BankNames");
            ViewBag.LoanAccount = new SelectList(GetLoanLoad(), "LoanId", "LoanNo");
            ViewBag.MonthlyLoanInterest = new SelectList(GetMonthlyLoanInterestDate(), "TransactionID", "InterestDate");

            ViewBag.Ledger = new SelectList(GetLedgerLoad(), "LedgerId", "LedgerName");
            ViewBag.AccountInfo = new SelectList(await bankservice.GetBankAccount(), "BankAcInfoId", "AccountNo");

            return PartialView(viewName);

        }
        //
        public IActionResult GetEmployeeInfo(String BranchID)
        {
            reportName = "EmployeeInfo.frx";
            _title = "Employee Information";

            var sql = " Select e.*,b.BranchName from [dbo].[EmployeeInfos] e left outer join[dbo].[Departments] d on e.DepartmentId = d.Id left outer join[dbo].[BranchInformation] b on b.BranchId = e.BranchId left outer join[dbo].[Designations] de on de.Id = e.DesignationId where e.BranchId like '" + BranchID + "'  order by BranchId,Cast(e.EmployeeNo as int)  ";
     
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult GetEmployeeDetail(String EmpolyeeId)
        {
            reportName = "EmployeeDetails.frx";
            _title = "Employee Details ";

            var sql = " Select e.EmpolyeeId,e.EmployeeNo,e.EmployeeName,e.FatherName,e.MotherName,e.MobileNo,e.Email, " +
                " e.Gender,CASE WHEN e.PicturePath is NULL THEN '' WHEN e.PicturePath =''  THEN '' Else '" + hostUrl + "'+'/' + replace(e.PicturePath, '\\','/') END AS PicturePath ," +
                " CASE WHEN e.SigniturePath is NULL THEN '' WHEN e.SigniturePath =''  THEN '' Else '" + hostUrl + "'+'/' + replace(e.SigniturePath, '\\','/') END AS SigniturePath," +
                " e.DateOfBirth,e.JoiningDate,e.RetiredMentDate,e.PresentAddress,e.PermanentAddress, " +
                " e.BasicSalary,e.OwnContPer,e.CompanyContPer,e.CpfStartDate,b.BranchName,de.Name as Designation,d.Name as DepartmentName,e.NID " +
                " from  [dbo].[EmployeeInfos] e " +
                " left outer join[dbo].[Departments] d on e.DepartmentId = d.Id" +
                " left outer join[dbo].[BranchInformation] b on b.BranchId = e.BranchId " +
                " left outer join[dbo].[Designations] de on de.Id = e.DesignationId " +
                " where e.EmpolyeeId like '" + EmpolyeeId +"'  ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);

            string sql1 = "Select e.EmpolyeeTransferId as Serial,e.BasicSalary,b.BranchName,e.PostingDate,e.LeaveDate,de.Name as Designation,d.Name as DepartmentName ,1 OrderType," +
                "'Previous Branch' History from[dbo].[EmployeeTransferHistory] e left outer join[dbo].[Departments] d on e.DepartmentId = d.Id left outer join[dbo].[BranchInformation] " +
                "b on b.BranchId = e.BranchId left outer join[dbo].[Designations] de on de.Id = e.DesignationId where e.TranserEmpolyeeId like '" + EmpolyeeId +"' Union Select e.EmpolyeeId as Serial," +
                "e.BasicSalary,b.BranchName,'' PostingDate,'' LeaveDate,de.Name as Designation,d.Name as DepartmentName ,1 OrderType,'Present Branch' History from[dbo].[EmployeeInfos] e " +
                "left outer join[dbo].[Departments] d on e.DepartmentId = d.Id left outer join[dbo].[BranchInformation] b on b.BranchId = e.BranchId left outer join[dbo].[Designations] de " +
                "on de.Id = e.DesignationId where e.EmpolyeeId like '" + EmpolyeeId +"' order by OrderType,Serial";

            sqls.Add(sql1);
            return ShowReport(1);
        }
        // Chart Of Account
        public IActionResult ChartOfAccount()
        {
            reportName = "rptChartOfAccount.frx";
            _title = "Chart of Account";
            var sql = " Select L.LedgerId,L.LedgerCode,L.LedgerName,P.PrimaryGroupId,P.PrimaryGroupName, Isnull(M.MainGroupId,'')MainGroupId,Isnull(M.MainGroupName,'')MainGroupName,Isnull(S.SubGroupId,'')SubGroupId, Isnull(S.SubGroupName,'')SubGroupName,P.ItemOf, Case When P.ItemOf ='Asset' Then 1 When P.ItemOf ='Liabilities' Then 2 When P.ItemOf ='Expense' Then 3 When P.ItemOf ='Income' Then 4 else 0 End As Type from [dbo].[Ledgers] L inner join   [dbo].[PrimaryGroups] P on L.PrimaryId=p.PrimaryId Left outer join [dbo].[MainGroups] M On L.MainId=M.MainId Left outer join [dbo].[SubGroups] S On L.SubId=S.SubId order by Type,P.PrimaryGroupId,M.MainGroupId, Cast (SUBSTRING(L.LedgerId, 3, 50) as int) ";
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


        public IActionResult GetMonthlyCPFStatement(String Year, String Month)
        {
            reportName = "rptMonthlyCPFStatement.frx";
            _title = "Monthly CPF Statement ";


            var sql = "Select * from [dbo].[func_MonthlyCPFStatement] ('" + Year + "','" + Month + "') order by BranchId,year,MonthId,EmpolyeeId";


            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
        public IActionResult GetCPFLedgerBalance(String AsOnDate)
        {
            reportName = "rptCPFLedgerBalance.frx";
            _title = "Branch Wise CPF Ledger Balance";


            var sql = "SELECT * FROM [dbo].[func_CPFLedgerBalance] ('" + AsOnDate + "') ";


            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult GetIndividualCPFLedger(String FromDate, String Todate, String EmpolyeeId)
        {
            reportName = "rptIndividualCPLedger.frx";
            _title = "Individual CPF Ledger Report ";

            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " Select * from [dbo].[func_IndividualCPFLedger] (CONVERT(date,'" + FromDate + "') , CONVERT(date,'" + Todate + "') ,'" + EmpolyeeId +"' ) ";
           
           
            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult GetBankWiseAccountStatement(String FromDate, String Todate, String BankId)
        {
            reportName = "rptBankWiseAccountStatement.frx";
            _title = "Bank Wise Account Statement ";

            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = "SELECT * FROM [dbo].[func_BankWiseAccountBalance](CONVERT(date,'" + FromDate + "') , CONVERT(date,'" + Todate + "') ,'" + BankId + "') ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult GetCPFLoanLedgerBalance(String AsOnDate)
        {
            reportName = "rptCPFLoanLedgerBalance.frx";
            _title = "CPF Loan Ledger Balance";

            caption = "As On Date " + dateToString(stringToDate(AsOnDate), "dd-MM-yy");

            var sql = "SELECT * FROM [dbo].[func_CPFLoanLedgerBalance](CONVERT(date,'" + AsOnDate + "')) ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        public IActionResult GetIndividualCPFLoanLedger(String FromDate, String Todate, String LoanID)
        {
            reportName = "rptIndividualCPFLoanLedger.frx";
            _title = "Individual CPF Loan Ledger Report ";

            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " Select * from [dbo].[func_IndividualCPFLoanLedger] (CONVERT(date,'" + FromDate + "') , CONVERT(date,'" + Todate + "') ,'" + LoanID + "' ) ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        // Officer Wise Monthly CPF Statement
        public IActionResult GetOfficerWiseMonthlyCPFStatement(String Year) 
        { 
            reportName = "rptYearWiseCPFLBalance.frx";
            _title = "Officer Wise Monthly CPF Statement ";

            caption = "Year : " +  Year ;

            // var sql = "Select * from [dbo].[func_MonthlyCPFStatement] ('" + Year + "','" + Month + "') order by BranchId,year,MonthId,EmpolyeeId";

            var sql = "Exec [dbo].[sp_RptOfficerWiseYearlyCPFStatement] '" + Year + "' ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
        //

        public IActionResult GetMonthlyLoanInterestPosting(String TrID)
        {
            reportName = "rptMonthlyLoanInterestPosting.frx";
            _title = "Monthly Loan Interest Posting Statement ";

            caption = "Date : " + TrID;

            var sql = "Exec [dbo].[sp_RptMonthlyLoanInteretPosting] '" + TrID + "' ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
        // Officer Wise Individual CPF Ledger
        public IActionResult GetOfficerWiseIndividualCPFLedger(String FromDate, String Todate, String EmpolyeeId)
        {
            reportName = "rptOfficerWiseIndividualCPLedger.frx";
            _title = "Officer Wise Individual CPF Ledger Report ";

            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " Select * from [dbo].[func_OfficerWiseIndividualCPFStatement]  (CONVERT(date,'" + FromDate + "') , CONVERT(date,'" + Todate + "') ,'" + EmpolyeeId + "' ) ";


            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }
        //
        public IActionResult GetOffAndBrWiseCPFStatement(String Year, String Month)
        {
            reportName = "rptBranchAndOfficerWiseMonthlyCPFDepositStatement.frx";
            _title = "Officer And Branch Wise Monthly CPF Statement ";


            var sql = "Select * from [dbo].[func_BranchAndOfficerWiseMonthlyCPFDepositStatement] ('" + Year + "','" + Month + "') Order by BranchId,YearMonth,EmpolyeeId";


            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        // Officer & Br  Wise  CPF Profit Statement
        public IActionResult GetOfficerAndBranchWiseCPFProfitStatement(String Year)
        {
            reportName = "rptBranchAndOfficerWiseMonthlyCPFProfitStatement.frx";
            _title = "Officer & Branch Wise Profit Distribution Statement";

            caption = "Year : " + Year;

             var sql = "Select * from [dbo].[func_BranchAndOfficerWiseMonthlyCPFProfitStatement] ('" + Year + "') order by BranchId,Year,EmpolyeeId";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }



        // Trail Balance Between Date
        public IActionResult GetTrailBalanceBetweenDate(String FromDate, String Todate)
        {
            reportName = "rptTrailBalanceBetweenDate.frx";
            _title = "Trail Balance Between Date ";

            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " Exec [dbo].[sp_RptTrailBalanceBetweenDate]  '" + FromDate + "' , '" + Todate + "' ,'B-1','FY-1'  ";


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

        // Individual Bank Ledger

        public IActionResult GetIndividualBankLedger(String FromDate, String Todate, String BankAcInfoId)
        {
            reportName = "rptIndividualBankLedger.frx";
            _title = "Individual Bank Ledger Report ";

            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " Select * from [dbo].[func_IndividualBankLedger] (CONVERT(date,'" + FromDate + "') , CONVERT(date,'" + Todate + "') ,'" + BankAcInfoId + "' ) ";

            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
        }

        // Loan Transaction Date Between 
        public IActionResult GetLoanTransactionBetweenDate(String FromDate, String Todate)
        {
            reportName = "rptLoanTransactionDateBetween.frx";
            _title = "CPF Loan Transaction Date Between ";

            caption = "From " + dateToString(stringToDate(FromDate), "dd-MM-yy") + " To : " + dateToString(stringToDate(Todate), "dd-MM-yy");

            var sql = " Select * from [dbo].[func_CPFLoanTransactionDateBetween] (CONVERT(date,'" + FromDate + "') , CONVERT(date,'" + Todate + "')) ";


            _logger.LogInformation(sql);
            sqls.Clear();
            sqls.Add(sql);
            return ShowReport(0);
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
        public List<LoanList> GetLoanLoad()
        {
            List<LoanList> loans = new List<LoanList>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = " Select L.LoanInfoId,(L.LoanNo+' - '+E.EmployeeName)LoanNo from [dbo].[LoanInformation] L inner join[dbo].[EmployeeInfos] E on L.EmpolyeeId = E.EmpolyeeId order by LoanInfoId ";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    loans.Add(new LoanList
                    {
                        LoanId = Convert.ToInt32(sqlData["LoanInfoId"]),
                        LoanNo = sqlData["LoanNo"].ToString(),
                    });
                }
            }
            finally
            {
                con.Close();
            }

            return loans;

        }
      
        public List<LoanInterestDate> GetMonthlyLoanInterestDate()
        {
            List<LoanInterestDate> loans = new List<LoanInterestDate>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = " Select Distinct TransactionId,convert(date,TransactionDate,105)TransactionDate from [dbo].[InterestPosting] order by TransactionDate asc ";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    loans.Add(new LoanInterestDate
                    {
                        TransactionID = Convert.ToInt32(sqlData["TransactionId"]),
                        InterestDate = sqlData["TransactionDate"].ToString()
                    });
                }
            }
            finally
            {
                con.Close();
            }

            return loans;

        }


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


        #endregion

    }

}
