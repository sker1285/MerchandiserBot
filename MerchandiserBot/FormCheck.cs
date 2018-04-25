using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AdaptiveCards;

namespace MerchandiserBot
{
    public class FormCheck
    {
      
        [Required]
        public DateTime? BirthCheck { get; set; }
        [Required]
        public string Gender { get; set; }






        public static FormCheck GendernBirth(dynamic o)
        {
            try
            {
                return new FormCheck
                {
                    BirthCheck = o.BirthCheck,
                    Gender = o.Gender
                };
            }
            catch
            {
                throw new InvalidCastException("表單格式不對");
            }
        }
    }
}