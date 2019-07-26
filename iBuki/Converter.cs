using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace iBuki
{
    public class BoolToEnumConverter : IValueConverter
    {
        /// <param name="value">enum</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns>bool</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var parameterString = parameter as string;
            if (parameterString == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return DependencyProperty.UnsetValue;
            }

            object paramvalue = Enum.Parse(value.GetType(), parameterString);

            return (int)paramvalue == (int)value;
        }

        /// <param name="value">bool</param>
        /// <param name="targetType">enum</param>
        /// <param name="parameter">string</param>
        /// <param name="language"></param>
        /// <returns>enum</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var ParameterString = parameter as string;
            return ParameterString == null
                ? DependencyProperty.UnsetValue
                : Enum.Parse(targetType, ParameterString);
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            (bool)value ^ (parameter as string ?? string.Empty).Equals("Reverse") ?
                Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            (Visibility)value == Visibility.Visible ^ (parameter as string ?? string.Empty).Equals("Reverse");
    }
}
