using System;
using System.Windows;
using System.Windows.Controls;

namespace DEFCALC.DataModel
{
    public class DoubleRangeRule : ValidationRule
    {

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            double valDouble;
            double parameter = 0;
            string checkedValue = (string)value;
            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            checkedValue = checkedValue.Replace(".", separator);
            checkedValue = checkedValue.Replace(",", separator);
            if (checkedValue == "")
            {
                return new ValidationResult(true, null);
            }
            else
            {
                if (double.TryParse(checkedValue, out valDouble))
                {
                   
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, "Допустимы только  числа ");
                }
            }



        }
    }
}




//try
//{
//    if (((string)value).Length > 0)
//    {
//        parameter = Double.Parse((String)value);
//    }
//}
//catch (Exception e)
//{
//    return new ValidationResult(false, "Illegal characters or "
//                                 + e.Message);
//}

//if ((parameter < this.Min) || (parameter > this.Max))
//{
//    return new ValidationResult(false,
//        "Please enter value in the range: "
//        + this.Min + " - " + this.Max + ".");
//}
