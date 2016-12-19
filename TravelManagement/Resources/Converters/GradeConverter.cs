using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TravelManagement.Converters
{
    public class GradeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int format = int.Parse(value.ToString());
                if (format == 1)
                    return "危急";
                else
                    return "异常";
            }
            return "异常";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
