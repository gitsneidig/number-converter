using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NumberConverter.Models
{
    public class Number
    {
        [Display(Name = "Number to Convert:")]
        public string NumberBeforeConversion { get; set; }
        public string ConvertedDigits { get; set; }
        public string ConvertedFraction { get; set; }
    }
}