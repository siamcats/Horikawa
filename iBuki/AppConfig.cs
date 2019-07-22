using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using System.Runtime.CompilerServices;

namespace iBuki
{
    public class AppConfig : INotifyPropertyChanged
    {
        private Movement _movement;
        public Movement Movement
        {
            get { return _movement; }
            set
            {
                if (value == _movement) return;
                _movement = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            Debug.WriteLine("PropertyChange");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public enum Movement
    {
        Quartz,
        Mechanical
    }

    public class BoolToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ParameterString = parameter as string;
            if (ParameterString == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return DependencyProperty.UnsetValue;
            }

            object paramvalue = Enum.Parse(value.GetType(), ParameterString);

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
}
