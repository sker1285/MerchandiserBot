using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MerchandiserBot
{
    public class TimeCheck
    {
        [Required]
        public DateTime? Checkin { get; set; }

        


       

        public static TimeCheck Birth(dynamic o)
        {
            try
            {
                return new TimeCheck
                {
                    Checkin = DateTime.Parse(o.Checkin.ToString()),                
                };
            }
            catch
            {
                //throw new InvalidCastException("Time could not be read");
                throw new InvalidCastException("時間格式不對");
            }
        }
    }
}