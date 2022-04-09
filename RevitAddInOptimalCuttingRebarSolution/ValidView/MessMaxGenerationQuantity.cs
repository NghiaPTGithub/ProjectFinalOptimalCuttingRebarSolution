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
    public class MessMaxGenerationQuantity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //ti le lai ghep cac element
            string textGot = value.ToString () ;
            if (Utils.IsInteger(textGot, true) && Int32.Parse(textGot) > 0)
            {
                return "";
            }
            else
            {
                return textGot.Replace(" ", "") == "" ? "This value is not null" : "Ratio hybriddize element is a real number >= 0 and <=1 !";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}
