using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace iBuki
{
    public class BoolToEnumConverter : IValueConverter
    {

        /// <summary>
        /// Enum->Bool
        /// </summary>
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
        /// <summary>
        /// Bool->Visibility
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language) =>
            (bool)value ^ (parameter as string ?? string.Empty).Equals("Reverse") ?
                Visibility.Visible :
                Visibility.Collapsed ;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            (Visibility)value == Visibility.Visible ^ (parameter as string ?? string.Empty).Equals("Reverse");
    }


    public class ColorToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Color->Brush
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = value is Color ?
                (Color)value :
                Colors.Black;
            return new SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var brush = value is SolidColorBrush ?
                (SolidColorBrush)value :
                new SolidColorBrush(Colors.Black);
            return brush.Color;
        }
    }

    /// <summary>
    /// EnumとIntの相互変換
    /// </summary>
    public class EnumToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is null) return DependencyProperty.UnsetValue;

            //Int->Enum
            if (targetType.IsEnum)
            {
                return Enum.ToObject(targetType, value);
            }

            //Enum->Int
            if (value.GetType().IsEnum)
            {
                return (int)value;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }
    }
}
