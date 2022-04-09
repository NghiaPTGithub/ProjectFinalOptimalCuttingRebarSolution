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
    public class MessFactorMutateLocal : IValueConverter
    {
        //kich thuoc quan the
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string textGot = value.ToString();

            if (Utils.IsInteger(textGot, true) && Int32.Parse(textGot) > 0)
            {
                return "";
            }
            else
            {
                return textGot.Replace(" ", "") == "" ? "This value is not null" : "Factor mutate local is a positive interger !";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}
