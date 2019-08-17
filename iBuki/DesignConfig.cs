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

        private double _scaleRadius = 45.3;
        public double ScaleRadius
        {
            get { return _scaleRadius; }
            set
            {
                if (value == _scaleRadius) return;
                _scaleRadius = value;
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

        private bool _isAlterScale = false;
        public bool IsAlterScale
        {
            get { return _isAlterScale; }
            set
            {
                if (value == _isAlterScale) return;
                _isAlterScale = value;
                OnPropertyChanged();
            }
        }

        private int _alterScaleInterval = 60;
        public int AlterScaleInterval
        {
            get { return _alterScaleInterval; }
            set
            {
                if (value == _alterScaleInterval) return;
                _alterScaleInterval = value;
                OnPropertyChanged();
            }
        }

        private double _alterScaleRadius = 45.3;
        public double AlterScaleRadius
        {
            get { return _alterScaleRadius; }
            set
            {
                if (value == _alterScaleRadius) return;
                _alterScaleRadius = value;
                OnPropertyChanged();
            }
        }

        private double _alterScaleLength = 3;
        public double AlterScaleLength
        {
            get { return _alterScaleLength; }
            set
            {
                if (value == _alterScaleLength) return;
                _alterScaleLength = value;
                OnPropertyChanged();
            }
        }

        private double _alterScaleThickness = 0.7;
        public double AlterScaleThickness
        {
            get { return _alterScaleThickness; }
            set
            {
                if (value == _alterScaleThickness) return;
                _alterScaleThickness = value;
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

        private Color _indexColor = Color.FromArgb(255, 255, 255, 255);
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

        private double _indexRadius = 43;
        public double IndexRadius
        {
            get { return _indexRadius; }
            set
            {
                if (value == _indexRadius) return;
                _indexRadius = value;
                OnPropertyChanged();
            }
        }

        private double _indexLength = 10;
        public double IndexLength
        {
            get { return _indexLength; }
            set
            {
                if (value == _indexLength) return;
                _indexLength = value;
                OnPropertyChanged();
            }
        }

        private double _indexThickness = 2;
        public double IndexThickness
        {
            get { return _indexThickness; }
            set
            {
                if (value == _indexThickness) return;
                _indexThickness = value;
                OnPropertyChanged();
            }
        }

        private string _indexFontFamily = "Verdana";
        public string IndexFontFamily
        {
            get { return _indexFontFamily; }
            set
            {
                if (value == _indexFontFamily) return;
                _indexFontFamily = value;
                OnPropertyChanged();
            }
        }

        private double _indexFontSize = 5;
        public double IndexFontSize
        {
            get { return _indexFontSize; }
            set
            {
                if (value == _indexFontSize) return;
                _indexFontSize = value;
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

        private double _dateX = 50;
        public double DateX
        {
            get { return _dateX; }
            set
            {
                if (value == _dateX) return;
                _dateX = value;
                OnPropertyChanged();
            }
        }

        private double _dateY = 50;
        public double DateY
        {
            get { return _dateY; }
            set
            {
                if (value == _dateY) return;
                _dateY = value;
                OnPropertyChanged();
            }
        }

        private string _dateDisplayFormat = "dd";
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

        private Color _dateColor = Color.FromArgb(255, 21, 53, 85);
        public Color DateColor
        {
            get { return _dateColor; }
            set
            {
                if (value == _dateColor) return;
                _dateColor = value;
                OnPropertyChanged();
            }
        }

        private double _dateBorderWidth = 100;
        public double DateBorderWith
        {
            get { return _dateBorderWidth; }
            set
            {
                if (value == _dateBorderWidth) return;
                _dateBorderWidth = value;
                OnPropertyChanged();
            }
        }

        private double _dateBorderHeight = 50;
        public double DateBorderHeight
        {
            get { return _dateBorderHeight; }
            set
            {
                if (value == _dateBorderHeight) return;
                _dateBorderHeight = value;
                OnPropertyChanged();
            }
        }

        private double _dateBorderThickness = 2;
        public double DateBorderThickness
        {
            get { return _dateBorderThickness; }
            set
            {
                if (value == _dateBorderThickness) return;
                _dateBorderThickness = value;
                OnPropertyChanged();
            }
        }

        private Color _dateBorderColor = Color.FromArgb(255, 20, 20, 20);
        public Color DateBorderColor
        {
            get { return _dateBorderColor; }
            set
            {
                if (value == _dateBorderColor) return;
                _dateBorderColor = value;
                OnPropertyChanged();
            }
        }

        private Color _dateBackgroundColor = Color.FromArgb(255, 255, 255, 255);
        public Color DateBackgroundColor
        {
            get { return _dateBackgroundColor; }
            set
            {
                if (value == _dateBackgroundColor) return;
                _dateBackgroundColor = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
