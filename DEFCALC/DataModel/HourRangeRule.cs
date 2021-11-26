using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace DEFCALC.DataModel
{
    public class HourRangeRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            double valDouble;
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
                    if ((valDouble >= 0) && (valDouble <= 12))
                    {
                        return new ValidationResult(true, null);
                    }
                    else
                    {
                        return new ValidationResult(false, "Числа должны быть от 0 до 12");
                    }
                    
                }
                else
                {
                    return new ValidationResult(false, "Допустимы только  числа ");
                }

                
            }
        }
    }
}
