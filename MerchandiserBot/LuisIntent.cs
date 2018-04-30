using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchandiserBot
{
    public class LuisIntent
    {
        public static string intent(string strIntent)
        {
            if (strIntent == "搜尋險種")
            {
                //string strRestr = objLUISRes.entities.Find((x => x.type == "greeting")).entity;
                return "搜尋險種";

            }
            else if (strIntent == "搜尋小額終老保險")
            {
                return "搜尋小額終老保險";
            }
            else if (strIntent == "搜尋實物給付型保險")
            {
                return "搜尋實物給付型保險";
            }
            else if (strIntent == "搜尋大男子保險")
            {
                return "搜尋大男子保險";
            }
            else if (strIntent == "搜尋醫療險")
            {
                return "搜尋醫療險";
            }
            else if (strIntent == "搜尋壽險")
            {
                return "搜尋壽險";
            }
            else if (strIntent == "搜尋活力系列保險")
            {
                return "搜尋活力系列保險";
            }
            else if (strIntent == "搜尋意外傷害險")
            {
                return "搜尋意外傷害險";
            }
            else if (strIntent == "搜尋HER大女子保險")
            {
                return "搜尋HER大女子保險";
            }
            else if (strIntent == "搜尋OIU保險")
            {
                return "搜尋OIU保險";
            }
            else if (strIntent == "搜尋利變壽")
            {
                return "搜尋利變壽";
            }
            else if (strIntent == "搜尋年金保險")
            {
                return "搜尋年金保險";
            }
            else if (strIntent == "搜尋投資型保險")
            {
                return "搜尋投資型保險";
            }
            else if (strIntent == "搜尋生死合險")
            {
                return "搜尋生死合險";
            }
            else if (strIntent == "搜尋長照")
            {
                return "搜尋長照";
            }
            else if (strIntent == "搜尋展新人生保險")
            {
                return "搜尋展新人生保險";
            }
            else
            {
                return "Unable to read...";
            }

        }
    }
}