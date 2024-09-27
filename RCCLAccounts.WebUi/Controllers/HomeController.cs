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



	
		#endregion
	}
}