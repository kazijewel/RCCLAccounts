
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

namespace RCCLAccounts.WebUi.Services
{
    public class AccountReportServiceWebUi
    {
       
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        private commonService commonService;
        string sqlCon;
        public AccountReportServiceWebUi( IHttpContextAccessor accessor, AppDbContext db)
        {
            
            _accessor = accessor;
            _db = db;
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            commonService = new commonService( _accessor, _db);
        }
       
        //public string getFiscalYear(string fromDate,string toDate)
        //{
        //    return commonService.getFiscalYearByDate(fromDate,toDate);
        //}
        //public IEnumerable<object> primaryGroup()
        //{
        //    ISession session = commonService.getSession();
        //    var companyId = session.GetString("companyId");
        //    var obj = from x in _unitAccounts.PrimaryGroup.GetAll(x=>x.CompanyId.ToString() == companyId)
        //              select new { Id = x.Id, Name = x.Code + "-" + x.Name };
        //    return obj;
        //}
        //public IEnumerable<object> getFiscalYearList()
        //{
        //    ISession session = commonService.getSession();
        //    var companyId = session.GetString("companyId");
        //    List<object> objList = new List<object>();
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    string sql = "select FiscalYearId,CONVERT(varchar,OpeningDate,105)OpeningDate," +
        //        "CONVERT(varchar,ClosingDate,105)ClosingDate from tbFiscalYear Where CompanyId like '"+companyId+"' order by OpeningDate";
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objList.Add(new
        //                {
        //                    Id = reader["FiscalYearId"].ToString(),
        //                    Name = reader["OpeningDate"].ToString() + " - " + reader["ClosingDate"].ToString()
        //                }); ;
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return objList;
        //}
        //public int chequeCancellationSaveWork(ChequeCancel obj)
        //{
        //    int ret = 0;
        //    string sql = "update tbChequeCancel set CancelDate=@CancelDate," +
        //                    "Reason=@Reason," +
        //                    "UdFlag='Cancel'," +
        //                    "UserName=@userName," +
        //                    "UserIp=@userIp," +
        //                    "EntryTime=@entryTime " +
        //                    "where BankLedgerId=@ledgerName " +
        //                    "and FolioNo=@folioNo";
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        cmd.Parameters.AddWithValue("@ledgerName", obj.BankLedgerId);
        //        cmd.Parameters.AddWithValue("@folioNo", obj.FolioNo);
        //        cmd.Parameters.AddWithValue("@CancelDate", obj.CancelDate);
        //        cmd.Parameters.AddWithValue("@Reason", obj.Reason);
        //        cmd.Parameters.AddWithValue("@userName", obj.UserName);
        //        cmd.Parameters.AddWithValue("@userIp", obj.UserIp);
        //        cmd.Parameters.AddWithValue("@entryTime", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"));
               
        //        ret = cmd.ExecuteNonQuery();
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return ret;
        //}
        public IEnumerable<object> getBankAC()
        {
            List<object> objList = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select Distinct BankLedgerId,BankName from tbChequeCancel WHERE  UdFlag = 'Not Used' ";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objList.Add(new
                        {
                            Id = reader["BankLedgerId"].ToString(),
                            Name = reader["BankName"].ToString()
                        });
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return objList;
        }

        //public IEnumerable<object> getVoucherType()
        //{
        //    ISession session = commonService.getSession();
        //    var companyId = session.GetString("companyId");
        //    var branchId = session.GetString("branchId");
        //    List<object> objList = new List<object>();
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    string sql = "select distinct LEFT(EntryFrom,1)+SUBSTRING(EntryFrom,CHARINDEX(' ',EntryFrom)+1,1)Id,EntryFrom from tbVoucher Where CompanyId like '" + companyId + "' and BranchId like '" + branchId + "' ";
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objList.Add(new
        //                {
        //                    Id = reader["Id"].ToString(),
        //                    Name = reader["EntryFrom"].ToString()
        //                });
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return objList;
        //}

        //public IEnumerable<object> getMainCategory(string type)
        //{
        //    ISession session = commonService.getSession();
        //    var companyId = session.GetString("companyId");
        //    List<object> objList = new List<object>();
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    string sql = "SELECT Id,Name from tbPrimaryGroup WHERE Type Like '"+type+ "' and CompanyId like '" + companyId + "' ";
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objList.Add(new
        //                {
        //                    Id = reader["Id"].ToString(),
        //                    Name = reader["Name"].ToString()
        //                });
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return objList;
        //}

        //public IEnumerable<object> getMainGroup(string id)
        //{

        //    ISession session = commonService.getSession();
        //    var companyId = session.GetString("companyId");
        //    List<object> objList = new List<object>();
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    string sql = "select Id,MainGroupName from tbMainGroup Where PrimaryGroupId Like '" + id + "' and CompanyId like '" + companyId + "' ";
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objList.Add(new
        //                {
        //                    Id = reader["Id"].ToString(),
        //                    Name = reader["MainGroupName"].ToString()
        //                });
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return objList;
        //}

        //public IEnumerable<object> getSubGroup(string id)
        //{
        //    ISession session = commonService.getSession();
        //    var companyId = session.GetString("companyId");
        //    List<object> objList = new List<object>();
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    string sql = "select Id,SubGroupName from tbSubGroup Where MainGroupId Like '" + id + "' and CompanyId like '"+companyId+"' ";
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objList.Add(new
        //                {
        //                    Id = reader["Id"].ToString(),
        //                    Name = reader["SubGroupName"].ToString()
        //                });
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return objList;
        //}


        //public IEnumerable<object> getVoucher(string voucherType, string findType, string fromDate, string toDate)
        //{
        //    ISession session = commonService.getSession();
        //    var companyId = session.GetString("companyId");
        //    var branchId = session.GetString("branchId");
        //    string fiscal = commonService.getFiscalYearByDate(fromDate, toDate);
           
        //    string voucherData = "";
        //    List<object> objList = new List<object>();
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    if(findType == "Date")
        //    {
        //        voucherData = " AND VoucherDate BETWEEN '" + fromDate + "' AND '" + toDate + "' ";
        //    }
           
        //    string sql = "select distinct VoucherNo,VoucherNo Caption, CAST(SUBSTRING(VoucherNo, " +
        //    " PATINDEX('%[0-9]%', VoucherNo), LEN(VoucherNo))as int) " +
        //    " from tbVoucher where EntryFrom like '"+voucherType+"' " +
        //    " and FiscalYearId like '"+fiscal+"' and CompanyId like '"+companyId+ "' and BranchId like '" + branchId + "' " + voucherData + " " +
        //    " order by CAST(SUBSTRING(VoucherNo, PATINDEX('%[0-9]%', VoucherNo), LEN(VoucherNo)) as int)";
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objList.Add(new
        //                {
        //                    Id = reader["VoucherNo"].ToString(),
        //                    Name = reader["Caption"].ToString()
        //                });
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //     return objList;
        //}

        //public IEnumerable<object> getVoucherData(string formDate, string toDate, string vType)
        //{
        //    ISession session = commonService.getSession();
        //    var companyId = session.GetString("companyId");
        //    var branchId = session.GetString("branchId");
        //    List<object> objList = new List<object>();
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    string sql = "SELECT distinct(VoucherNo) VoucherNo,CAST(substring(VoucherNo,7,50) AS int) FROM tbVoucher " +
        //        " WHERE VoucherDate BETWEEN CONVERT(DATE,'"+ formDate + "',105) AND CONVERT(DATE,'"+ toDate + "',105) " +
        //        " AND vouchertype in ('"+ vType + "') and CompanyId like '"+companyId+ "' and BranchId like '" + branchId + "' " +
        //        " AND ORDER BY CAST(substring(VoucherNo,7,50) AS int)";
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objList.Add(new
        //                {
        //                    Id = reader["VoucherNo"].ToString(),
        //                    Name = reader["VoucherNo"].ToString()
        //                });
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return objList;
        //}
        public IEnumerable<object> userName(string formDate, string toDate)
        {
            List<object> objList = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "SELECT DISTINCT UserName FROM tbUdVoucher WHERE EntryTime BETWEEN " +
                " CONVERT(DATE, '"+ formDate + "',105) AND CONVERT(DATE, '"+ toDate + "',105)";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objList.Add(new
                        {
                            Id = reader["UserName"].ToString(),
                            Name = reader["UserName"].ToString()
                        });
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return objList;
        }
        public IEnumerable<object> getChequeNo(string id)
        {
            List<object> objList = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select Distinct Id,FolioNo from tbChequeCancel WHERE  UdFlag = 'Not Used' and BankLedgerId LIKE '"+id+"'";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objList.Add(new
                        {
                            Id = reader["Id"].ToString(),
                            Name = reader["FolioNo"].ToString()
                        });
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return objList;
        }
        //public IEnumerable<object> getLedgerList(string ledgerType)
        //{
        //    List<object> objList = new List<object>();
        //    //string fsl = commonService.getFiscalYear(DateTime.Now.ToString("yyyy/MM/dd"));
        //    string companyId = commonService.getSession().GetString("companyId");
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    string sql = "select distinct LedgerId,LedgerCode,LedgerName from tbLedger " +
        //        " where CompanyId like '"+companyId+"' " +
        //        " order by LedgerName";
        //    /*switch(ledgerType)
        //    {
        //        case "G":
        //            sql = "SELECT distinct LedgerId,isnull(LedgerCode,'')LedgerCode,L as LedgerName FROM vwLedgerList " +
        //                " where H <> 'Fixed Asset' order by LedgerName";
        //            break;
        //    }*/
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objList.Add(new
        //                {
        //                    Id = reader["LedgerId"].ToString(),
        //                    Name = reader["LedgerName"].ToString()
        //                });
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return objList;
        //}
        //public string ledgerName(string ledgerId)
        //{
        //    string companyId = commonService.getSession().GetString("companyId");
        //    var obj = _unitAccounts.Ledger.GetFirstOrDefault(x=>x.LedgerId==ledgerId && x.CompanyId.ToString()==companyId);
        //    if(obj!=null)
        //    {
        //        return obj.LedgerName;
        //    }
        //    return "";
        //}
        //public string ledgerCode(string ledgerId)
        //{
        //    string companyId = commonService.getSession().GetString("companyId");
        //    var obj = _unitAccounts.Ledger.GetFirstOrDefault(x=>x.LedgerId==ledgerId && x.CompanyId.ToString() == companyId);
        //    if(obj!=null)
        //    {
        //        return obj.LedgerCode;
        //    }
        //    return "";
        //}
        //public string ledgerPath(string ledgerId)
        //{
        //    string companyId = commonService.getSession().GetString("companyId");
        //    string ret = "";
        //    SqlConnection con = new SqlConnection(sqlCon);
        //    string sql = "select a.Type,ISNULL(b.Name,'')Name,ISNULL(c.MainGroupName,'')MainGroupName," +
        //        " ISNULL(d.SubGroupName,'')SubGroupName,a.LedgerName from tbLedger a " +
        //    " inner join tbPrimaryGroup b on a.PrimaryGroupId = b.PrimaryGroupId " +
        //    " left join tbMainGroup c on a.MainGroupId = c.MainGroupId " +
        //    " left join tbSubGroup d on a.SubGroupId = d.SubGroupId " +
        //    " where a.LedgerId like @ledgerId And a.CompanyId like '"+companyId+"' ";
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        cmd.Parameters.AddWithValue("@ledgerId",ledgerId);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            if(reader.Read())
        //            {
        //                ret = reader["Type"].ToString();
        //                if (reader["Name"] != "")
        //                {
        //                    ret = ret + "/" + reader["Name"];
        //                }
        //                if (reader["MainGroupName"] != "")
        //                {
        //                    ret = ret + "/" + reader["MainGroupName"];
        //                }
        //                if (reader["SubGroupName"] != "")
        //                {
        //                    ret = ret + "/" + reader["SubGroupName"];
        //                }
        //                ret = ret + "/" + reader["LedgerName"];
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return ret;
        //}
        public IEnumerable<object> getUserName(string fromDate, string toDate)
        {
            List<object> objList = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select Distinct UserName Id, UserName from tbChequeCancel WHERE  UdFlag = 'Cancel' AND Convert(Date,CancelDate,105) BETWEEN Convert(Date,'" + fromDate + "',105) AND Convert(Date,'" + toDate + "',105)  ";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objList.Add(new
                        {
                            Id = reader["Id"].ToString(),
                            Name = reader["UserName"].ToString()
                        });
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return objList;
        }
        public string getVoucherName(string a)
        {
            string ret = "";
            switch(a)
            {
                case "CP":ret = "Cash Payment Voucher";break;
                case "CR":ret = "Cash Receipt Voucher";break;
                case "BP":ret = "Bank Payment Voucher";break;
                case "BR":ret = "Bank Receipt Voucher";break;
                case "JV":ret = "Journal Voucher";break;
                case "CV":ret = "Contra Voucher"; break;
                case "FA":ret = "Fixed Asset Voucher";break;
                case "DV":ret = "Depriciation Voucher";break;
                default:break;
            }
            return ret;
        }

        public IEnumerable<object> getPrimaryGroup(string type)
        {
            List<object> objList = new List<object>();
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    string sql = "select distinct l.PrimaryGroupId,Name from tbLedger as l " +
                                 "left join tbPrimaryGroup as pg on l.PrimaryGroupId=pg.PrimaryGroupId " +
                                 "where l.Type like '" + type + "'";
                    command.CommandText = sql;
                    Console.WriteLine(sql);
                    _db.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        // do something with result
                        while (reader.Read())
                        {
                            objList.Add(new
                            {
                                Id = reader["PrimaryGroupId"].ToString(),
                                Name = reader["Name"].ToString()

                            });
                        }
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                _db.Database.CloseConnection();
            }
            return objList;

        }

        public IEnumerable<object> getMainGroupList(string type,string primaryGroupId)
        {
            List<object> objList = new List<object>();
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    string sql = "select distinct MainGroupId,MainGroupName from tbMainGroup " +
                                 "where Type like '"+ type + "' and PrimaryGroupId like '"+ primaryGroupId + "' ";
                    command.CommandText = sql;
                    Console.WriteLine(sql);
                    _db.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        // do something with result
                        while (reader.Read())
                        {
                            objList.Add(new
                            {
                                Id = reader["MainGroupId"].ToString(),
                                Name = reader["MainGroupName"].ToString()

                            });
                        }
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                _db.Database.CloseConnection();
            }
            return objList;

        }

        public IEnumerable<object> getSubGroupList(string type, string primaryGroupId, string groupId)
        {
            List<object> objList = new List<object>();
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    string sql = "select distinct SubGroupId,SubGroupName from tbSubGroup " +
                                 "where Type like '"+ type +"' and PrimaryGroupId like '"+ primaryGroupId +"' and " +
                                 "MainGroupId like '"+ groupId +"' ";
                    command.CommandText = sql;
                    Console.WriteLine(sql);
                    _db.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        // do something with result
                        while (reader.Read())
                        {
                            objList.Add(new
                            {
                                Id = reader["SubGroupId"].ToString(),
                                Name = reader["SubGroupName"].ToString()

                            });
                        }
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                _db.Database.CloseConnection();
            }
            return objList;

        }
    }   
}

