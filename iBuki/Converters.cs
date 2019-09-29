using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

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
            // ConverterParameterの指定がない
            var stringParam = (string)parameter;
            if (string.IsNullOrEmpty(stringParam))
            {
                return DependencyProperty.UnsetValue;
            }
            var intParam = (int)Enum.Parse(value.GetType(), stringParam);

            // Enumでない
            if (value is null) return DependencyProperty.UnsetValue;
            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return DependencyProperty.UnsetValue;
            }
            var intValue = (int)value;

            return intParam == intValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // ConverterParameterの指定がない
            var stringParam = (string)parameter;
            return string.IsNullOrEmpty(stringParam)
                ? DependencyProperty.UnsetValue
                : Enum.Parse(targetType, stringParam);
        }
    }

    /// <summary>
    /// BoolとVisibilityの相互変換
    /// </summary>
    public class BoolAndVisibilityInterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null) return DependencyProperty.UnsetValue;
            if (value is bool)
            {
                var visibilityValue = (bool)value ?
                    Visibility.Visible :
                    Visibility.Collapsed;
                return visibilityValue;
            }
            else if (value is Visibility)
            {
                var boolValue = (Visibility)value == Visibility.Visible ?
                    true :
                    false;
                return boolValue;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }
    }


    /// <summary>
    /// BoolとVisibilityの相互変換（逆）
    /// </summary>
    public class BoolAndVisibilityInterReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null) return DependencyProperty.UnsetValue;
            if (value is bool)
            {
                var visibilityValue = (bool)value ?
                    Visibility.Collapsed :
                    Visibility.Visible;
                return visibilityValue;
            }
            else if (value is Visibility)
            {
                var boolValue = (Visibility)value == Visibility.Visible ?
                    false :
                    true ;
                return boolValue;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }
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
            //Brush->Color
            else if (value is SolidColorBrush)
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

    /// <summary>
    /// Decimalの小数点2桁以下を切り捨てる
    /// </summary>
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

    /// <summary>
    /// Double->Thickness
    /// </summary>
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


    public class UriToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new BitmapImage(new Uri(value.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
