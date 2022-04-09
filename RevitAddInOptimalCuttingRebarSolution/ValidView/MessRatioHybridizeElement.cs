using RevitAddInOptimalCuttingRebarSolution.zLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RevitAddInOptimalCuttingRebarSolution.ValidView
{
    [ValueConversion(typeof(string), typeof(string))]
    public class MessRatioHybridizeElement : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //ti le lai ghep cac element
            string textGot = value.ToString();
            if (Utils.IsDouble(textGot, true)
                && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) >= 0
                && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) <= 1)
            {
                return "";
            }
            else
            {
                return textGot.Replace(" ", "") == "" ? "This value is not null" : "Max generation quantity is a positive interger !";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}
