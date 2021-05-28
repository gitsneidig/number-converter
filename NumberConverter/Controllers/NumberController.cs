using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Humanizer;
using NumberConverter.Models;

namespace NumberConverter.Controllers
{
    public class NumberController : Controller
    {
        public ActionResult Convert()
        {
            Number num = new Number();
            return View(num);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Convert(Number num)
        {
            // Validate the number to convert has a value
            if (string.IsNullOrEmpty(num.NumberBeforeConversion))
                ModelState.AddModelError("", "Please enter a value for Number to Convert.");

            // Validate the number to convert is numeric
            bool isValidNumber = Double.TryParse(num.NumberBeforeConversion, out _);
            if (!isValidNumber && ModelState.IsValid)
                ModelState.AddModelError("", "Please ensure your Number to Convert value contains only values of type Integer or Double.");

            // Validate the number to convert is less than the Integer MaxValue
            if (isValidNumber && Double.Parse(num.NumberBeforeConversion) > int.MaxValue && ModelState.IsValid)
                ModelState.AddModelError("", $"Please ensure your Number to Convert value is less than or equal to {int.MaxValue}.");

            if (ModelState.IsValid)
            {
                try { 
                    // Split the provided number by decimal point
                    string[] splitValue = num.NumberBeforeConversion.Split('.');
                
                    // The first item of the split array has already been validated and can be be passed to int.Parse()
                    // Humanize the first item of the split array and uppercase the first letter
                    num.ConvertedDigits = UppercaseFirstLetter(int.Parse(splitValue[0]).ToWords());
                
                    // If no decimal value was included, then do not covert a decmial value
                    if (splitValue.Length > 1)
                        // Convert the decimal value to it's fraction representation, and append ' and '
                        num.ConvertedFraction = $" and {ConvertDecimalValue(splitValue[1])}";

                    // Append the word dollars at the end
                    if (string.IsNullOrEmpty(num.ConvertedFraction))
                    {
                        num.ConvertedDigits += " dollars";
                    } else {
                        num.ConvertedFraction += " dollars";
                    }

                    // Append dollar sign to original number value
                    num.NumberBeforeConversion = $"${num.NumberBeforeConversion}";
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Number", "Convert"));
                }
            } else {
                // Reset the value if it is invalid
                num.NumberBeforeConversion = "";
            }
            return View(num);
        }

        private string ConvertDecimalValue(string decimalValue)
        {
            string place = "1";
            for (var i = 0; i < decimalValue.Length; i++)
                place += "0";

            return $"{decimalValue}/{place}";
        }

        private string UppercaseFirstLetter(string stringValue) 
            => $"{char.ToUpper(stringValue[0])}{stringValue.Substring(1)}"; 
    }
}