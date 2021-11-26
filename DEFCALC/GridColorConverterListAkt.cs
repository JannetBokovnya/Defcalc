using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Telerik.Windows.Controls.GridView;

namespace DEFCALC
{
   public class GridColorConverterListAkt:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            

            string tb = (string) value;

            if ((tb == "1") || ((tb == "2")))
            {
                return new SolidColorBrush(Color.FromArgb(150, 143,188,143));
               // return new SolidColorBrush(Color.FromArgb(255, 233, 150, 122));
            }

            return new SolidColorBrush(Colors.White);

          


        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
