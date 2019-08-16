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
    // Convert: ModelProperty -> View

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
                Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            (Visibility)value == Visibility.Visible ^ (parameter as string ?? string.Empty).Equals("Reverse");
    }


    /// <summary>
    /// ColorとSolidColorBrushの相互変換
    /// </summary>
    public class ColorAndBrushInterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null) return DependencyProperty.UnsetValue;

            //color->Brush
            if (value is Color)
            {
                var colorValue = (Color)value;
                return new SolidColorBrush(colorValue);
            }

            //brush->color
            if (value is SolidColorBrush)
            {
                var brushValue = (SolidColorBrush)value;
                return brushValue.Color;
            }

            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }
    }

    /// <summary>
    /// EnumとIntの相互変換
    /// </summary>
    public class EnumAndIntInterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null) return DependencyProperty.UnsetValue;

            //Int->Enum
            if (targetType.IsEnum) return Enum.ToObject(targetType, value);

            //Enum->Int
            if (value.GetType().IsEnum) return (int)value;

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }
    }

    public class DecimalTruncateConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null) return DependencyProperty.UnsetValue;
            var doubleValue = (double)value;
            return (Math.Floor(doubleValue * 10) / 10).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DoubleToThickness : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null) return DependencyProperty.UnsetValue;
            if (value is double)
            {
                var doubleValue = (double)value;
                return new Thickness(doubleValue);
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
