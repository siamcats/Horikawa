using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Runtime.Serialization;
using Windows.UI.ViewManagement;

namespace iBuki
{
    public class DesignConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Hands

        private Hands _handsType;
        public Hands HandsType
        {
            get { return _handsType; }
            set
            {
                if (value == _handsType) return;
                _handsType = value;
                OnPropertyChanged();
            }
        }
        
        private Color _handsColor = Color.FromArgb(255, 21, 53, 85);
        public Color HandsColor
        {
            get { return _handsColor; }
            set
            {
                if (value == _handsColor) return;
                _handsColor = value;
                OnPropertyChanged();
            }
        }

        private Color _secondHandColor = Color.FromArgb(255, 21, 53, 85);
        public Color SecondHandColor
        {
            get { return _secondHandColor; }
            set
            {
                if (value == _secondHandColor) return;
                _secondHandColor = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Clock Dial

        private Scale _scaleType;
        public Scale ScaleType
        {
            get { return _scaleType; }
            set
            {
                if (value == _scaleType) return;
                _scaleType = value;
                OnPropertyChanged();
            }
        }

        private Brush _scaleColor;
        public Brush ScaleColor
        {
            get { return _scaleColor; }
            set
            {
                if (value == _scaleColor) return;
                _scaleColor = value;
                OnPropertyChanged();
            }
        }

        private Index _indexType;
        public Index IndexType
        {
            get { return _indexType; }
            set
            {
                if (value == _indexType) return;
                _indexType = value;
                OnPropertyChanged();
            }
        }

        private Brush _indexColor;
        public Brush IndexColor
        {
            get { return _indexColor; }
            set
            {
                if (value == _indexColor) return;
                _indexColor = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _dialImage;
        public ImageSource DialImage
        {
            get { return _dialImage; }
            set
            {
                if (value == _dialImage) return;
                _dialImage = value;
                OnPropertyChanged();
            }
        }

        private Color _backgroundColor = Color.FromArgb(255,20,20,20);
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (value == _backgroundColor) return;
                _backgroundColor = value;

                //×ボタンとかの文字色も合わせる
                var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;
                appTitleBar.ButtonForegroundColor = value;
                appTitleBar.ButtonInactiveForegroundColor = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Date Display

        private bool _isDateDisplay = true;
        public bool IsDateDisplay
        {
            get { return _isDateDisplay; }
            set
            {
                if (value == _isDateDisplay) return;
                _isDateDisplay = value;
                OnPropertyChanged();
            }
        }

        private double _dateFontSize = 50;
        public double DateFontSize
        {
            get { return _dateFontSize; }
            set
            {
                if (value == _dateFontSize) return;
                _dateFontSize = value;
                OnPropertyChanged();
            }
        }

        private string _dateFontFamily = "Verdana";
        public string DateFontFamily
        {
            get { return _dateFontFamily; }
            set
            {
                if (value == _dateFontFamily) return;
                _dateFontFamily = value;
                OnPropertyChanged();
            }
        }

        private string _dateDisplayFormat = "d ddd";
        public string DateDisplayFormat
        {
            get { return _dateDisplayFormat; }
            set
            {
                if (value == _dateDisplayFormat) return;
                _dateDisplayFormat = value;
                OnPropertyChanged();
            }
        }

        private Brush _dateColor = new SolidColorBrush(Color.FromArgb(255, 21, 53, 85));
        public Brush DateColor
        {
            get { return _dateColor; }
            set
            {
                if (value == _dateColor) return;
                _dateColor = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
