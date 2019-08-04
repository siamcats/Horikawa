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
using Windows.UI.ViewManagement;

namespace iBuki
{
    public class AppConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double _windowSize = 500;
        public double WindowSize
        {
            get { return _windowSize; }
            set
            {
                if (value == _windowSize) return;
                _windowSize = value;
                OnPropertyChanged();
            }
        }

        private ElementTheme _systemTheme = ElementTheme.Light;
        public ElementTheme SystemTheme
        {
            get { return _systemTheme; }
            set
            {
                if (value == _systemTheme) return;
                _systemTheme = value;
                OnPropertyChanged();
            }
        }

        private Movement _movement = Movement.Mechanical;
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

        private bool _isTopMost = true;
        public bool IsTopMost
        {
            get { return _isTopMost; }
            set
            {
                if (value == _isTopMost) return;
                _isTopMost = value;
                OnPropertyChanged();
            }
        }
    }
}
