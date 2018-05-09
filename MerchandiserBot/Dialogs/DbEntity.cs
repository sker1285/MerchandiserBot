using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MerchandiserBot.Dialogs
{
    public class DbEntity
    {

        public DataTable AD()
        {
            var list = new List<ADPwd>();
            string conn = ConfigurationManager.AppSettings["Connstr"];
            using (var dbConn = new SqlConnection(conn))
            {
                dbConn.Open();
                try {   using (var cmd = dbConn.CreateCommand())
                {
                    cmd.CommandText = "select * from ADPwd where Id =100";
                    var dt = new DataTable();
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    var count = dt.Rows.Count;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new ADPwd()
                        {
                            Id = int.Parse(dt.Rows[i]["Id"].ToString()),
                            name = dt.Rows[i]["name"].ToString(),
                            img = dt.Rows[i]["img"].ToString(),
                        });
                    }
                        Console.WriteLine("Hello World!");
                        return dt;
                } }
                catch (Exception e)
                {
                    Console.WriteLine("資料庫問題"+e);
                    return null;
                }
             
            }
        }

        public DataTable Inweb()
        {
            var list = new List<InwebPwd>();
            string conn = ConfigurationManager.AppSettings["Connstr"];
            using (var dbConn = new SqlConnection(conn))
            {
                dbConn.Open();
                using (var cmd = dbConn.CreateCommand())
                {
                    cmd.CommandText = "select * from InwebPwd";
                    var dt = new DataTable();
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    var count = dt.Rows.Count;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new InwebPwd()
                        {
                            Id = int.Parse(dt.Rows[i]["Id"].ToString()),
                            name = dt.Rows[i]["name"].ToString(),
                            img = dt.Rows[i]["img"].ToString(),
                        });
                    }
                    return dt;
                }
            }
        }


        //取業務員資料
        public DataTable MerchandiserData(string mid)
        {
            var list = new List<Merchandiser>();
            string conn = ConfigurationManager.AppSettings["Connstr"];
            using (var dbConn = new SqlConnection(conn))
            {
                dbConn.Open();
                using (var cmd = dbConn.CreateCommand())
                {
                    cmd.CommandText = $"select * from Merchandiser where Id = '{mid}'";
                    var dt = new DataTable();
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    var count = dt.Rows.Count;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Merchandiser()
                        {
                            Id = dt.Rows[i]["Id"].ToString(),
                            Name = dt.Rows[i]["Name"].ToString(),
                            IdentityNum = dt.Rows[i]["IdentityNum"].ToString(),
                            Birth = dt.Rows[i]["Birth"].ToString(),
                            errortimes = int.Parse(dt.Rows[i]["errortimes"].ToString()),
                            auth = int.Parse(dt.Rows[i]["auth"].ToString()),
                        });
                    }
                    return dt;
                }
            }
        }

        //取密碼身分驗證錯誤次數
        public DataTable errortimes(int error)
        {
            var list = new List<Merchandiser>();
            string conn = ConfigurationManager.AppSettings["Connstr"];
            using (var dbConn = new SqlConnection(conn))
            {
                dbConn.Open();
                using (var cmd = dbConn.CreateCommand())
                {
                    cmd.CommandText = $"update Merchandiser set errortimes = {error} " +                      
                        $"where Id = '{PwdSetting.Dialogs.CertifiedDialog.getId()}'";
                    var dt = new DataTable();
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    var count = dt.Rows.Count;                  
                    return dt;
                }
            }
        }

        //身分認證
        public DataTable M_Check(string id, string idnum, string birth) 
        {
            var list = new List<Merchandiser>();
            string conn = ConfigurationManager.AppSettings["Connstr"];
            using (var dbConn = new SqlConnection(conn))
            {
                dbConn.Open();
                using (var cmd = dbConn.CreateCommand())
                {
                    cmd.CommandText = 
                        $"select * from Merchandiser where Id = '{id}' and IdentityNum = '{idnum}' and Birth = '{birth}'";
                    var dt = new DataTable();
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    var count = dt.Rows.Count;
                   

                    return dt;
                }
            }
        }


        public DataTable PwdRecord(string id,string date,string type)
        {
            var list = new List<PwdRecord>();
            string conn = ConfigurationManager.AppSettings["Connstr"];
            using (var dbConn = new SqlConnection(conn))
            {
                dbConn.Open();
                using (var cmd = dbConn.CreateCommand())
                {
                    cmd.CommandText = $"insert into PwdRecord (M_Id,Date,PwdType) values ('{id}','{date}','{type}')";
                    var dt = new DataTable();
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    var count = dt.Rows.Count;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new PwdRecord()
                        {
                            Id = int.Parse(dt.Rows[i]["Id"].ToString()),
                            M_Id = dt.Rows[i]["M_Id"].ToString(),
                            Date = dt.Rows[i]["Date"].ToString(),
                            PwdType = dt.Rows[i]["PwdType"].ToString(),
                        });
                    }
                    return dt;
                }
            }
        }
    }

}