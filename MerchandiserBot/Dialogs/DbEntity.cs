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
                using (var cmd = dbConn.CreateCommand())
                {
                    cmd.CommandText = "select * from ADPwd";
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
                    return dt;
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
    }

}