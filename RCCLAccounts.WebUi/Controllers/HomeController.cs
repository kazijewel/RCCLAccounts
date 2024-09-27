using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProvidentFund.Data;
using ProvidentFund.WebUi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ProvidentFund.WebUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _dbContext;
		string sqlCon;

		public HomeController(ILogger<HomeController> logger,
			AppDbContext dbContext)
        {
            _logger = logger;
			_dbContext = dbContext;
			sqlCon = _dbContext.Database.GetDbConnection().ConnectionString;
		}

        [Authorize]
        public IActionResult Index()
        {		
			ViewBag.TotalEmp = getTotalEmployee();
            ViewBag.TotalCPFBalance = getTotalCPFBalance();
			ViewBag.TotalCPFLoanBalance = getTotalCPFLoanBalance();

			ViewBag.TotalSODBankBalance = getTotalSODBankBalance();
			ViewBag.TotalSTDBankBalance = getTotalSTDBankBalance();

			return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		#region DashboardMethod


		public int getTotalEmployee()
		{
			int TotalEmp=0;
			SqlConnection con = new SqlConnection(sqlCon);
			try
			{
				con.Open();
				string sql = "Select Count(EmpolyeeId)TotalEmployee from [dbo].[EmployeeInfos] ";

				SqlCommand cmd = new SqlCommand(sql, con);
				SqlDataReader sqlData = cmd.ExecuteReader();
				while (sqlData.Read())
				{

					TotalEmp = Convert.ToInt32(sqlData["TotalEmployee"]);

				}
			}
			catch (Exception exp) { }
			finally
			{
				con.Close();
			}
			return TotalEmp;
		}

        public decimal getTotalCPFBalance()
        {
            decimal TotalCPFBalance = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "Select [dbo].[func_TotalCPFBalance] () As Balance";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {

                    TotalCPFBalance = Convert.ToDecimal(sqlData["Balance"]);

                }
            }
            catch (Exception exp) { }
            finally
            {
                con.Close();
            }
            return TotalCPFBalance;
        }



		public decimal getTotalCPFLoanBalance()
		{
			decimal TotalCPFLoanBalance = 0;
			SqlConnection con = new SqlConnection(sqlCon);
			try
			{
				con.Open();
				string sql = "Select [dbo].[func_TotalCPFLoanBalance] () As Balance";

				SqlCommand cmd = new SqlCommand(sql, con);
				SqlDataReader sqlData = cmd.ExecuteReader();
				while (sqlData.Read())
				{

					TotalCPFLoanBalance = Convert.ToDecimal(sqlData["Balance"]);

				}
			}
			catch (Exception exp) { }
			finally
			{
				con.Close();
			}
			return TotalCPFLoanBalance;
		}



		public decimal getTotalSODBankBalance()
		{
			decimal TotalSODBankBalance = 0;
			SqlConnection con = new SqlConnection(sqlCon);
			try
			{
				con.Open();
				string sql = "Select  [dbo].[func_BankTypeWiseTotalBalance] ('SOD') As Balance";

				SqlCommand cmd = new SqlCommand(sql, con);
				SqlDataReader sqlData = cmd.ExecuteReader();
				while (sqlData.Read())
				{

					TotalSODBankBalance = Convert.ToDecimal(sqlData["Balance"]);

				}
			}
			catch (Exception exp) { }
			finally
			{
				con.Close();
			}
			return TotalSODBankBalance;
		}



		public decimal getTotalSTDBankBalance()
		{
			decimal TotalSTDBankBalance = 0;
			SqlConnection con = new SqlConnection(sqlCon);
			try
			{
				con.Open();
				string sql = "Select  [dbo].[func_BankTypeWiseTotalBalance] ('STD') As Balance";

				SqlCommand cmd = new SqlCommand(sql, con);
				SqlDataReader sqlData = cmd.ExecuteReader();
				while (sqlData.Read())
				{

					TotalSTDBankBalance = Convert.ToDecimal(sqlData["Balance"]);

				}
			}
			catch (Exception exp) { }
			finally
			{
				con.Close();
			}
			return TotalSTDBankBalance;
		}

		[HttpGet]
		public IActionResult GetDountChartData()
		{
		
			var data = GetEmployeeDountChart();

			return Json(data);
		}


        public List<object> GetEmployeeDountChart()
        {
            string sql =  "SELECT Gender, COUNT(*) AS Count FROM [dbo].[EmployeeInfos] GROUP BY Gender  ";


            SqlConnection con = new SqlConnection(sqlCon);
            SqlCommand cmd = new SqlCommand(sql, con);
            List<object> obj = new List<object>();
            try
            {
                con.Open();
                SqlDataReader idr = cmd.ExecuteReader();

                if (idr.HasRows)
                {
                    while (idr.Read())
                    {
                        obj.Add(new
                        {
                            label = Convert.ToString(idr["Gender"]),
                            value = Convert.ToInt32(idr["Count"])
                        });
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return obj;
        }

		public List<object> GetYearlyCPFDeposit(int year)
		{
			
			string sql = "EXEC sp_DashboardYearlyCPFDeposit @ParameterName";

			SqlConnection con = new SqlConnection(sqlCon);
			SqlCommand cmd = new SqlCommand(sql, con);
			cmd.Parameters.AddWithValue("@ParameterName", year);

			List<object> obj = new List<object>();

			try
			{
				con.Open();
				SqlDataReader idr = cmd.ExecuteReader();

				if (idr.HasRows)
				{
					while (idr.Read())
					{
						obj.Add(new
						{
							cpf = Convert.ToInt32(idr["CPFDeposit"]),
							rccl = Convert.ToInt32(idr["RCCLContribution"])
							//MonthName = Convert.ToString(idr["MonthName"])
						});
					}
				}
			}
			finally
			{
				con.Close();
			}

			return obj;
		}

		[HttpGet]
		public IActionResult GetYearlyCPFDepositChartData(int year)
		{

			var data = GetYearlyCPFDeposit(year);

			return Json(data);
		}

		#endregion
	}
}