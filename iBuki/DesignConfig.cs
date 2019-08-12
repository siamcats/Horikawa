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
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml;

namespace iBuki
{
    public class DesignConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Background

        private Color _backgroundColor = Color.FromArgb(255, 20, 20, 20);
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

        private bool _isBackgroundImageDisplay = false;
        public bool IsBackgroundImageDisplay
        {
            get { return _isBackgroundImageDisplay; }
            set
            {
                if (value == _isBackgroundImageDisplay) return;
                _isBackgroundImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _backgroundImage;
        public ImageSource BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                if (value == _backgroundImage) return;
                _backgroundImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Dial

        private bool _isScaleDisplay = true;
        public bool IsScaleDisplay
        {
            get { return _isScaleDisplay; }
            set
            {
                if (value == _isScaleDisplay) return;
                _isScaleDisplay = value;
                OnPropertyChanged();
            }
        }

        private int _scaleCount = 60;
        public int ScaleCount
        {
            get { return _scaleCount; }
            set
            {
                if (value == _scaleCount) return;
                _scaleCount = value;
                OnPropertyChanged();
            }
        }

        private double _scaleLength = 3;
        public double ScaleLength
        {
            get { return _scaleLength; }
            set
            {
                if (value == _scaleLength) return;
                _scaleLength = value;
                OnPropertyChanged();
            }
        }

        private double _scaleThickness = 0.7;
        public double ScaleThickness
        {
            get { return _scaleThickness; }
            set
            {
                if (value == _scaleThickness) return;
                _scaleThickness = value;
                OnPropertyChanged();
            }
        }

        private Color _scaleColor = Color.FromArgb(255, 21, 53, 85);
        public Color ScaleColor
        {
            get { return _scaleColor; }
            set
            {
                if (value == _scaleColor) return;
                _scaleColor = value;
                OnPropertyChanged();
            }
        }

        private bool _isIndexDisplay = true;
        public bool IsIndexDisplay
        {
            get { return _isIndexDisplay; }
            set
            {
                if (value == _isIndexDisplay) return;
                _isIndexDisplay = value;
                OnPropertyChanged();
            }
        }

        private IndexType _indexType;
        public IndexType IndexType
        {
            get { return _indexType; }
            set
            {
                if (value == _indexType) return;
                _indexType = value;
                OnPropertyChanged();
            }
        }

        private Color _indexColor = Color.FromArgb(255, 21, 53, 85);
        public Color IndexColor
        {
            get { return _indexColor; }
            set
            {
                if (value == _indexColor) return;
                _indexColor = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Hands

        private HandsType _handsType;
        public HandsType HandsType
        {
            get { return _handsType; }
            set
            {
                if (value == _handsType) return;
                _handsType = value;
                OnPropertyChanged();
            }
        }

        private bool _isHandsDisplay = true;
        public bool IsHandsDisplay
        {
            get { return _isHandsDisplay; }
            set
            {
                if (value == _isHandsDisplay) return;
                _isHandsDisplay = value;
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

        private bool _isSecondHandDisplay = true;
        public bool IsSecondHandDisplay
        {
            get { return _isSecondHandDisplay; }
            set
            {
                if (value == _isSecondHandDisplay) return;
                _isSecondHandDisplay = value;
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
