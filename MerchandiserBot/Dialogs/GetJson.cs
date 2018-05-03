using MerchandiserBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MerchandiserBot.Dialogs
{
    public class GetJson
    {
        static public List<Product> GetProdList(string filepath)
        {
            if (File.Exists(filepath))
            {
                string strLogList;
                using (FileStream fs = new FileStream(filepath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        strLogList = sr.ReadToEnd();
                    }
                }

                return JsonConvert.DeserializeObject<List<Product>>(strLogList).ToList();
            }
            else
            {
                return new List<Product>();
            }
        }
    }
}