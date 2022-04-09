using RevitAddInOptimalCuttingRebarSolution.zLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RevitAddInOptimalCuttingRebarSolution.ValidView
{
    [ValueConversion(typeof(string), typeof(string))]
    public class MessRatioMutateElement : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //ty le dot bien muc element
            string textGot = value.ToString();
            if (Utils.IsDouble(textGot, true)
                && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) >= 0
                && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) <= 1)
            {
                return "";
            }
            else
            {
                return textGot.Replace(" ", "") == "" ? "This value is not null" : "Ratio mutate element is a real number >= 0 and <=1 !";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}
