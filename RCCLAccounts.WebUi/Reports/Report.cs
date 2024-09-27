using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using FastReport.Data;
using FastReport.Utils;
using FastReport.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ProvidentFund.WebUi.Reports
{
    public class Report
    {

        public Report()
        {

        }

        public  static WebReport GetReport(IHttpContextAccessor _accessor,string reportpath,List<string> sqls)
        {

            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

           // ISession _session = _accessor.HttpContext.Session;
            ReportDbConnection connection = null;
            try
            {
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"{"appsettings.json"}");
                var str = System.IO.File.ReadAllText(folderDetails);
                connection = JsonConvert.DeserializeObject<ReportDbConnection>(str);
            }
            catch (Exception) { }


            if (connection != null)
            {
                WebReport webReport = new WebReport();
                webReport.ShowPreparedReport = false;
                webReport.SinglePage = true;
                webReport.Report.Load(reportpath);

                var addlist = Dns.GetHostEntry(Dns.GetHostName());
                string GetHostName = addlist.HostName.ToString();
                string GetIPV6 = addlist.AddressList[0].ToString();
                string GetIPV4 = addlist.AddressList[1].ToString();

        
                webReport.Report.Dictionary.Connections[0].ConnectionString = connection.reportDbConnection;//"Data Source=ESL12;AttachDbFilename=;Initial Catalog=ESLInventory;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=123";
                
                webReport.Report.SetParameterValue("userName", "admin");
                webReport.Report.SetParameterValue("userIp", GetIPV4);
                webReport.Report.SetParameterValue("developer", "JR Software");
                webReport.Report.SetParameterValue("phone", "Email: rupaliho@gmail.com");
                webReport.Report.SetParameterValue("companyName", "Rupali Credit Co-operative Ltd.");
                webReport.Report.SetParameterValue("companyAddress", "BM Heights,2nd Floor,318 Shekh Mujib Road,Badamtooli Moor,Agrabad,Chittagong, Bangladesh, 4100");
                int i = 0;
                foreach (string sql in sqls)
                {
                    webReport.Report.SetParameterValue("sql" + i, sql);
                    i++;
                }
                return webReport;
            }
            return null;

        }
    }
}
