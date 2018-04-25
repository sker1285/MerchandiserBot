using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MerchandiserBot
{
    public class FormBirth
    {
        [Required]
        public DateTime? Checkin { get; set; }

        public static FormBirth Birth(dynamic o)
        {
            try
            {
                return new FormBirth
                {
                    Checkin = DateTime.Parse(o.Checkin.ToString())
                };
            }
            catch
            {
                throw new InvalidCastException("時間格式不對");
            }
        }
    }
}