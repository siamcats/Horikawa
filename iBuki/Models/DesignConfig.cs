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
using Windows.UI.Xaml.Media.Imaging;

namespace iBuki
{
    /// <summary>
    /// VMのうちSettingへ変換・保存するプロパティはここに持たせる
    /// </summary>
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

        private BitmapImage _backgroundImage;
        public BitmapImage BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                if (value == _backgroundImage) return;
                _backgroundImage = value;
                OnPropertyChanged();
            }
        }

        private Stretch _backgroundImageStretch = Stretch.Fill;
        public Stretch BackgroundImageStretch
        {
            get { return _backgroundImageStretch; }
            set
            {
                if (value == _backgroundImageStretch) return;
                _backgroundImageStretch = value;
                OnPropertyChanged();
            }
        }

        private double _backgroundImageCoordinateX = 0;
        public double BackgroundImageCoordinateX
        {
            get { return _backgroundImageCoordinateX; }
            set
            {
                if (value == _backgroundImageCoordinateX) return;
                var coordinate = new Thickness(value, BackgroundImageCoordinate.Top, value * -1, BackgroundImageCoordinate.Bottom);
                BackgroundImageCoordinate = coordinate;
                _backgroundImageCoordinateX = value;
                OnPropertyChanged();
            }
        }

        private double _backgroundImageCoordinateY = 0;
        public double BackgroundImageCoordinateY
        {
            get { return _backgroundImageCoordinateY; }
            set
            {
                if (value == _backgroundImageCoordinateY) return;
                var coordinate = new Thickness(BackgroundImageCoordinate.Left, value * -1, BackgroundImageCoordinate.Right, value);
                BackgroundImageCoordinate = coordinate;
                _backgroundImageCoordinateY = value;
                OnPropertyChanged();
            }
        }

        // CoordinateのX,Y値を、Thickness,Top,Right,Buttom値に変換する（Marginにバインドするため）
        private Thickness _backgroundImageCoordinate = new Thickness();
        public Thickness BackgroundImageCoordinate
        {
            get => _backgroundImageCoordinate;
            set
            {
                if (value == _backgroundImageCoordinate) return;
                _backgroundImageCoordinate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Foreground

        private bool _isForegroundImageDisplay = false;
        public bool IsForegroundImageDisplay
        {
            get { return _isForegroundImageDisplay; }
            set
            {
                if (value == _isForegroundImageDisplay) return;
                _isForegroundImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _ForegroundImage;
        public BitmapImage ForegroundImage
        {
            get { return _ForegroundImage; }
            set
            {
                if (value == _ForegroundImage) return;
                _ForegroundImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Dial Scale

        private bool _isScaleDisplay = false;
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

        private int _scaleCount = 1;
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

        private double _scaleRadius = 1;
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

        private double _scaleLength = 1;
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

        private double _scaleThickness = 1;
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

        private int _alterScaleInterval = 1;
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

        private double _alterScaleRadius = 1;
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

        private double _alterScaleLength = 1;
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

        private double _alterScaleThickness = 1;
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

        #endregion

        #region Dial Index

        private bool _isIndexDisplay = false;
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

        private IndexType _indexType = IndexType.Bar;
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

        private double _indexRadius = 1;
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

        private double _indexLength = 1;
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

        private double _indexThickness = 1;
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

        private int _indexInterval = 1;
        public int IndexInterval
        {
            get { return _indexInterval; }
            set
            {
                if (value == _indexInterval) return;
                _indexInterval = value;
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

        private bool _isHandsDisplay = false;
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
        
        private Color _handsColor = Color.FromArgb(255, 255, 255, 255);
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

        private Color _handsStrokeColor = Color.FromArgb(255, 255, 255, 255);
        public Color HandsStrokeColor
        {
            get { return _handsStrokeColor; }
            set
            {
                if (value == _handsStrokeColor) return;
                _handsStrokeColor = value;
                OnPropertyChanged();
            }
        }

        private double _handsStrokeThickness = 0;
        public double HandsStrokeThickness
        {
            get { return _handsStrokeThickness; }
            set
            {
                if (value == _handsStrokeThickness) return;
                _handsStrokeThickness = value;
                OnPropertyChanged();
            }
        }

        private bool _isSecondHandDisplay = false;
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

        private Color _secondHandColor = Color.FromArgb(255, 255, 255, 255);
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

        private bool _isDateDisplay = false;
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

        private double _dateCoordinateX = 0;
        public double DateCoordinateX
        {
            get { return _dateCoordinateX; }
            set
            {
                if (value == _dateCoordinateX) return;
                var coordinate = new Thickness(value, DateCoordinate.Top, value * -1, DateCoordinate.Bottom);
                DateCoordinate = coordinate;
                _dateCoordinateX = value;
                OnPropertyChanged();
            }
        }

        private double _dateCoordinateY = 0;
        public double DateCoordinateY
        {
            get { return _dateCoordinateY; }
            set
            {
                if (value == _dateCoordinateY) return;
                var coordinate = new Thickness(DateCoordinate.Left, value * -1, DateCoordinate.Right, value);
                DateCoordinate = coordinate;
                _dateCoordinateY = value;
                OnPropertyChanged();
            }
        }

        // CoordinateのX,Y値を、Thickness,Top,Right,Buttom値に変換する（Marginにバインドするため）
        private Thickness _dateCoordinate = new Thickness(0,0,0,0);
        public Thickness DateCoordinate
        {
            get => _dateCoordinate;
            set
            {
                if (value == _dateCoordinate) return;
                _dateCoordinate = value;
                OnPropertyChanged();
            }
        }

        private double _dateWidth = 1;
        public double DateWidth
        {
            get { return _dateWidth; }
            set
            {
                if (value == _dateWidth) return;
                _dateWidth = value;
                OnPropertyChanged();
            }
        }

        private double _dateHeight = 1;
        public double DateHeight
        {
            get { return _dateHeight; }
            set
            {
                if (value == _dateHeight) return;
                _dateHeight = value;
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

        private double _dateBorderThickness = 1;
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

        private Color _dateFontColor = Color.FromArgb(255, 21, 53, 85);
        public Color DateFontColor
        {
            get { return _dateFontColor; }
            set
            {
                if (value == _dateFontColor) return;
                _dateFontColor = value;
                OnPropertyChanged();
            }
        }

        private string _dateFormat = "dd";
        public string DateFormat
        {
            get { return _dateFormat; }
            set
            {
                if (value == _dateFormat) return;
                _dateFormat = value;
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

        private double _dateFontSize = 20;
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

        #endregion

        #region Moon Phase

        private bool _isMoonPhaseDisplay = false;
        public bool IsMoonPhaseDisplay
        {
            get { return _isMoonPhaseDisplay; }
            set
            {
                if (value == _isMoonPhaseDisplay) return;
                _isMoonPhaseDisplay = value;
                OnPropertyChanged();
            }
        }

        private double _moonPhaseSize = 100;
        public double MoonPhaseSize
        {
            get { return _moonPhaseSize; }
            set
            {
                if (value == _moonPhaseSize) return;
                _moonPhaseSize = value;
                OnPropertyChanged();
            }
        }

        private double _moonPhaseCoordinateX = 0;
        public double MoonPhaseCoordinateX
        {
            get { return _moonPhaseCoordinateX; }
            set
            {
                if (value == _moonPhaseCoordinateX) return;
                var coordinate = new Thickness(value, MoonPhaseCoordinate.Top, value * -1, MoonPhaseCoordinate.Bottom);
                MoonPhaseCoordinate = coordinate;
                _moonPhaseCoordinateX = value;
                OnPropertyChanged();
            }
        }

        private double _moonPhaseCoordinateY = 0;
        public double MoonPhaseCoordinateY
        {
            get { return _moonPhaseCoordinateY; }
            set
            {
                if (value == _moonPhaseCoordinateY) return;
                var coordinate = new Thickness(MoonPhaseCoordinate.Left, value * -1, MoonPhaseCoordinate.Right, value);
                MoonPhaseCoordinate = coordinate;
                _moonPhaseCoordinateY = value;
                OnPropertyChanged();
            }
        }

        // CoordinateのX,Y値を、Thickness,Top,Right,Buttom値に変換する（Marginにバインドするため）
        private Thickness _moonPhaseCoordinate = new Thickness(0, 0, 0, 0);
        public Thickness MoonPhaseCoordinate
        {
            get => _moonPhaseCoordinate;
            set
            {
                if (value == _moonPhaseCoordinate) return;
                _moonPhaseCoordinate = value;
                OnPropertyChanged();
            }
        }

        private bool _isMoonPhaseBackgroundImageDisplay = false;
        public bool IsMoonPhaseBackgroundImageDisplay
        {
            get { return _isMoonPhaseBackgroundImageDisplay; }
            set
            {
                if (value == _isMoonPhaseBackgroundImageDisplay) return;
                _isMoonPhaseBackgroundImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _moonPhaseBackgroundImage;
        public BitmapImage MoonPhaseBackgroundImage
        {
            get { return _moonPhaseBackgroundImage; }
            set
            {
                if (value == _moonPhaseBackgroundImage) return;
                _moonPhaseBackgroundImage = value;
                OnPropertyChanged();
            }
        }

        private bool _isMoonPhaseForegroundImageDisplay = false;
        public bool IsMoonPhaseForegroundImageDisplay
        {
            get { return _isMoonPhaseForegroundImageDisplay; }
            set
            {
                if (value == _isMoonPhaseForegroundImageDisplay) return;
                _isMoonPhaseForegroundImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _moonPhaseForegroundImage;
        public BitmapImage MoonPhaseForegroundImage
        {
            get { return _moonPhaseForegroundImage; }
            set
            {
                if (value == _moonPhaseForegroundImage) return;
                _moonPhaseForegroundImage = value;
                OnPropertyChanged();
            }
        }

        private Color _moonPhaseForegroundColor = Color.FromArgb(255, 255, 255, 255);
        public Color MoonPhaseForegroundColor
        {
            get { return _moonPhaseForegroundColor; }
            set
            {
                if (value == _moonPhaseForegroundColor) return;
                _moonPhaseForegroundColor = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Chronograph

        private bool _isChronographDisplay = false;
        public bool IsChronographDisplay
        {
            get { return _isChronographDisplay; }
            set
            {
                if (value == _isChronographDisplay) return;
                _isChronographDisplay = value;
                OnPropertyChanged();
                OnPropertyChanged("SubDialSecondVisibility");
                OnPropertyChanged("SubDialTotalizer30mVisibility");
                OnPropertyChanged("SubDialTotalizer12hVisibility");
            }
        }

        private bool _isNotification = false;
        public bool IsNotification
        {
            get { return _isNotification; }
            set
            {
                if (value == _isNotification) return;
                _isNotification = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _notificationTime = TimeSpan.Zero;
        public TimeSpan NotificationTime
        {
            get { return _notificationTime; }
            set
            {
                if (value == _notificationTime) return;
                _notificationTime = value;
                OnPropertyChanged();
            }
        }

        private NotificationAction _notificationAction = NotificationAction.None;
        public NotificationAction NotificationAction
        {
            get { return _notificationAction; }
            set
            {
                if (value == _notificationAction) return;
                _notificationAction = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubDialSecondDisplay = false;
        public bool IsSubDialSecondDisplay
        {
            get { return _isSubDialSecondDisplay; }
            set
            {
                if (value == _isSubDialSecondDisplay) return;
                _isSubDialSecondDisplay = value;
                OnPropertyChanged();
                OnPropertyChanged("SubDialSecondVisibility");
            }
        }

        //クロノグラフONかつインダイアルONなら表示する
        public Visibility SubDialSecondVisibility
        {
            get
            {
                if (IsChronographDisplay == true && IsSubDialSecondDisplay == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        private double _subDialSecondSize = 100;
        public double SubDialSecondSize
        {
            get { return _subDialSecondSize; }
            set
            {
                if (value == _subDialSecondSize) return;
                _subDialSecondSize = value;
                OnPropertyChanged();
            }
        }

        private double _subDialSecondCoordinateX = 0;
        public double SubDialSecondCoordinateX
        {
            get { return _subDialSecondCoordinateX; }
            set
            {
                if (value == _subDialSecondCoordinateX) return;
                var coordinate = new Thickness(value, SubDialSecondCoordinate.Top, value * -1, SubDialSecondCoordinate.Bottom);
                SubDialSecondCoordinate = coordinate;
                _subDialSecondCoordinateX = value;
                OnPropertyChanged();
            }
        }

        private double _subDialSecondCoordinateY = 0;
        public double SubDialSecondCoordinateY
        {
            get { return _subDialSecondCoordinateY; }
            set
            {
                if (value == _subDialSecondCoordinateY) return;
                var coordinate = new Thickness(SubDialSecondCoordinate.Left, value * -1, SubDialSecondCoordinate.Right, value);
                SubDialSecondCoordinate = coordinate;
                _subDialSecondCoordinateY = value;
                OnPropertyChanged();
            }
        }

        // CoordinateのX,Y値を、Thickness,Top,Right,Buttom値に変換する（Marginにバインドするため）
        private Thickness _subDialSecondCoordinate = new Thickness(0, 0, 0, 0);
        public Thickness SubDialSecondCoordinate
        {
            get => _subDialSecondCoordinate;
            set
            {
                if (value == _subDialSecondCoordinate) return;
                _subDialSecondCoordinate = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubDialSecondBackgroundImageDisplay = false;
        public bool IsSubDialSecondBackgroundImageDisplay
        {
            get { return _isSubDialSecondBackgroundImageDisplay; }
            set
            {
                if (value == _isSubDialSecondBackgroundImageDisplay) return;
                _isSubDialSecondBackgroundImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _subDialSecondBackgroundImage;
        public BitmapImage SubDialSecondBackgroundImage
        {
            get { return _subDialSecondBackgroundImage; }
            set
            {
                if (value == _subDialSecondBackgroundImage) return;
                _subDialSecondBackgroundImage = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubDialSecondHandImageDisplay = false;
        public bool IsSubDialSecondHandImageDisplay
        {
            get { return _isSubDialSecondHandImageDisplay; }
            set
            {
                if (value == _isSubDialSecondHandImageDisplay) return;
                _isSubDialSecondHandImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _subDialSecondHandImage;
        public BitmapImage SubDialSecondHandImage
        {
            get { return _subDialSecondHandImage; }
            set
            {
                if (value == _subDialSecondHandImage) return;
                _subDialSecondHandImage = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubDialTotalizer30mDisplay = false;
        public bool IsSubDialTotalizer30mDisplay
        {
            get { return _isSubDialTotalizer30mDisplay; }
            set
            {
                if (value == _isSubDialTotalizer30mDisplay) return;
                _isSubDialTotalizer30mDisplay = value;
                OnPropertyChanged();
                OnPropertyChanged("SubDialTotalizer30mVisibility");
            }
        }

        //クロノグラフONかつインダイアルONなら表示する
        public Visibility SubDialTotalizer30mVisibility
        {
            get
            {
                if (IsChronographDisplay == true && IsSubDialTotalizer30mDisplay == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        private double _subDialTotalizer30mSize = 100;
        public double SubDialTotalizer30mSize
        {
            get { return _subDialTotalizer30mSize; }
            set
            {
                if (value == _subDialTotalizer30mSize) return;
                _subDialTotalizer30mSize = value;
                OnPropertyChanged();
            }
        }

        private double _subDialTotalizer30mCoordinateX = 0;
        public double SubDialTotalizer30mCoordinateX
        {
            get { return _subDialTotalizer30mCoordinateX; }
            set
            {
                if (value == _subDialTotalizer30mCoordinateX) return;
                var coordinate = new Thickness(value, SubDialTotalizer30mCoordinate.Top, value * -1, SubDialTotalizer30mCoordinate.Bottom);
                SubDialTotalizer30mCoordinate = coordinate;
                _subDialTotalizer30mCoordinateX = value;
                OnPropertyChanged();
            }
        }

        private double _subDialTotalizer30mCoordinateY = 0;
        public double SubDialTotalizer30mCoordinateY
        {
            get { return _subDialTotalizer30mCoordinateY; }
            set
            {
                if (value == _subDialTotalizer30mCoordinateY) return;
                var coordinate = new Thickness(SubDialTotalizer30mCoordinate.Left, value * -1, SubDialTotalizer30mCoordinate.Right, value);
                SubDialTotalizer30mCoordinate = coordinate;
                _subDialTotalizer30mCoordinateY = value;
                OnPropertyChanged();
            }
        }

        // CoordinateのX,Y値を、Thickness,Top,Right,Buttom値に変換する（Marginにバインドするため）
        private Thickness _subDialTotalizer30mCoordinate = new Thickness(0, 0, 0, 0);
        public Thickness SubDialTotalizer30mCoordinate
        {
            get => _subDialTotalizer30mCoordinate;
            set
            {
                if (value == _subDialTotalizer30mCoordinate) return;
                _subDialTotalizer30mCoordinate = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubDialTotalizer30mBackgroundImageDisplay = false;
        public bool IsSubDialTotalizer30mBackgroundImageDisplay
        {
            get { return _isSubDialTotalizer30mBackgroundImageDisplay; }
            set
            {
                if (value == _isSubDialTotalizer30mBackgroundImageDisplay) return;
                _isSubDialTotalizer30mBackgroundImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _subDialTotalizer30mBackgroundImage;
        public BitmapImage SubDialTotalizer30mBackgroundImage
        {
            get { return _subDialTotalizer30mBackgroundImage; }
            set
            {
                if (value == _subDialTotalizer30mBackgroundImage) return;
                _subDialTotalizer30mBackgroundImage = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubDialTotalizer30mHandImageDisplay = false;
        public bool IsSubDialTotalizer30mHandImageDisplay
        {
            get { return _isSubDialTotalizer30mHandImageDisplay; }
            set
            {
                if (value == _isSubDialTotalizer30mHandImageDisplay) return;
                _isSubDialTotalizer30mHandImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _subDialTotalizer30mHandImage;
        public BitmapImage SubDialTotalizer30mHandImage
        {
            get { return _subDialTotalizer30mHandImage; }
            set
            {
                if (value == _subDialTotalizer30mHandImage) return;
                _subDialTotalizer30mHandImage = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubDialTotalizer12hDisplay = false;
        public bool IsSubDialTotalizer12hDisplay
        {
            get { return _isSubDialTotalizer12hDisplay; }
            set
            {
                if (value == _isSubDialTotalizer12hDisplay) return;
                _isSubDialTotalizer12hDisplay = value;
                OnPropertyChanged();
                OnPropertyChanged("SubDialTotalizer12hVisibility");
            }
        }

        //クロノグラフONかつインダイアルONなら表示する
        public Visibility SubDialTotalizer12hVisibility
        {
            get
            {
                if (IsChronographDisplay == true && IsSubDialTotalizer12hDisplay == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        private double _subDialTotalizer12hSize = 100;
        public double SubDialTotalizer12hSize
        {
            get { return _subDialTotalizer12hSize; }
            set
            {
                if (value == _subDialTotalizer12hSize) return;
                _subDialTotalizer12hSize = value;
                OnPropertyChanged();
            }
        }

        private double _subDialTotalizer12hCoordinateX = 0;
        public double SubDialTotalizer12hCoordinateX
        {
            get { return _subDialTotalizer12hCoordinateX; }
            set
            {
                if (value == _subDialTotalizer12hCoordinateX) return;
                var coordinate = new Thickness(value, SubDialTotalizer12hCoordinate.Top, value * -1, SubDialTotalizer12hCoordinate.Bottom);
                SubDialTotalizer12hCoordinate = coordinate;
                _subDialTotalizer12hCoordinateX = value;
                OnPropertyChanged();
            }
        }

        private double _subDialTotalizer12hCoordinateY = 0;
        public double SubDialTotalizer12hCoordinateY
        {
            get { return _subDialTotalizer12hCoordinateY; }
            set
            {
                if (value == _subDialTotalizer12hCoordinateY) return;
                var coordinate = new Thickness(SubDialTotalizer12hCoordinate.Left, value * -1, SubDialTotalizer12hCoordinate.Right, value);
                SubDialTotalizer12hCoordinate = coordinate;
                _subDialTotalizer12hCoordinateY = value;
                OnPropertyChanged();
            }
        }

        // CoordinateのX,Y値を、Thickness,Top,Right,Buttom値に変換する（Marginにバインドするため）
        private Thickness _subDialTotalizer12hCoordinate = new Thickness(0, 0, 0, 0);
        public Thickness SubDialTotalizer12hCoordinate
        {
            get => _subDialTotalizer12hCoordinate;
            set
            {
                if (value == _subDialTotalizer12hCoordinate) return;
                _subDialTotalizer12hCoordinate = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubDialTotalizer12hBackgroundImageDisplay = false;
        public bool IsSubDialTotalizer12hBackgroundImageDisplay
        {
            get { return _isSubDialTotalizer12hBackgroundImageDisplay; }
            set
            {
                if (value == _isSubDialTotalizer12hBackgroundImageDisplay) return;
                _isSubDialTotalizer12hBackgroundImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _subDialTotalizer12hBackgroundImage;
        public BitmapImage SubDialTotalizer12hBackgroundImage
        {
            get { return _subDialTotalizer12hBackgroundImage; }
            set
            {
                if (value == _subDialTotalizer12hBackgroundImage) return;
                _subDialTotalizer12hBackgroundImage = value;
                OnPropertyChanged();
            }
        }

        private bool _isSubDialTotalizer12hHandImageDisplay = false;
        public bool IsSubDialTotalizer12hHandImageDisplay
        {
            get { return _isSubDialTotalizer12hHandImageDisplay; }
            set
            {
                if (value == _isSubDialTotalizer12hHandImageDisplay) return;
                _isSubDialTotalizer12hHandImageDisplay = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _subDialTotalizer12hHandImage;
        public BitmapImage SubDialTotalizer12hHandImage
        {
            get { return _subDialTotalizer12hHandImage; }
            set
            {
                if (value == _subDialTotalizer12hHandImage) return;
                _subDialTotalizer12hHandImage = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
