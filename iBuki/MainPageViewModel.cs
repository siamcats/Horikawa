using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;

namespace iBuki
{
    class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private AppConfig _appConfig = new AppConfig();
        public AppConfig AppConfig
        {
            get => _appConfig;
            set
            {
                if (value == _appConfig) return;
                _appConfig = value;
                OnPropertyChanged(); 

            }
        }

        public List<string> LanguageList = new List<string> { "en-US", "jp-JP" };

        private DesignConfig _designConfig = new DesignConfig();
        public DesignConfig DesignConfig
        {
            get => _designConfig;
            set
            {
                if (value == _designConfig) return;
                _designConfig = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Settingsオブジェクトをアプリ設定に反映
        /// </summary>
        public async void ImportSettingsAsync(Settings settings)
        {
            // background
            DesignConfig.BackgroundColor = ConvertHexColor(settings.BackgroundColor);
            DesignConfig.IsBackgroundImageDisplay = settings.BackgroundImageDisplay;
            if (DesignConfig.IsBackgroundImageDisplay)
            {
                try
                {
                    var bitmap = new BitmapImage();
                    var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Const.URI_LOCAL + Const.FILE_BACKGROUND));
                    using (var stream = await file.OpenReadAsync())
                    {
                        await bitmap.SetSourceAsync(stream);
                    }
                    DesignConfig.BackgroundImage = bitmap;
                }
                catch (FileNotFoundException e)
                {
                    Debug.WriteLine(Const.URI_LOCAL + Const.FILE_BACKGROUND + " Not Found");
                }
            }
            // scale
            IsScaleDisplay = settings.ScaleDisplay;
            if (IsScaleDisplay)
            {
                DesignConfig.ScaleColor = ConvertHexColor(settings.ScaleColor);
                DesignConfig.ScaleCount = settings.ScaleCount;
                DesignConfig.ScaleRadius = settings.ScaleRadius;
                DesignConfig.ScaleLength = settings.ScaleLength;
                DesignConfig.ScaleThickness = settings.ScaleThickness;
                IsAlterScale = settings.AlterScale;
                if (IsAlterScale)
                {
                    DesignConfig.AlterScaleInterval = settings.AlterScaleInterval;
                    DesignConfig.AlterScaleRadius = settings.AlterScaleRadius;
                    DesignConfig.AlterScaleLength = settings.AlterScaleLength;
                    DesignConfig.AlterScaleThickness = settings.AlterScaleThickness;
                }
            }
            // index
            IsIndexDisplay = settings.IndexDisplay;
            if (IsIndexDisplay)
            {
                IndexType = (IndexType)Enum.Parse(typeof(IndexType),settings.IndexType);
                DesignConfig.IndexColor = ConvertHexColor(settings.IndexColor);
                DesignConfig.IndexRadius = settings.IndexRadius;
                if (DesignConfig.IndexType == IndexType.Bar)
                {
                    DesignConfig.IndexLength = settings.IndexLength;
                    DesignConfig.IndexThickness = settings.IndexThickness;
                }
                else
                {
                    DesignConfig.IndexFontFamily = settings.IndexFontFamily;
                    DesignConfig.IndexFontSize = settings.IndexFontSize;
                }
            }
            // hands
            HandsType = (HandsType)Enum.Parse(typeof(HandsType),settings.HandsType);
            DesignConfig.IsHandsDisplay = settings.HandsDisplay;
            if (DesignConfig.IsHandsDisplay)
            {
                DesignConfig.HandsColor = ConvertHexColor(settings.HandsColor);
            }
            DesignConfig.IsSecondHandDisplay = settings.SecondHandDisplay;
            if (DesignConfig.IsSecondHandDisplay)
            {
                DesignConfig.SecondHandColor = ConvertHexColor(settings.SecondHandColor);
            }
            // day date
            DesignConfig.IsDateDisplay = settings.DateDisplay;
            if (DesignConfig.IsDateDisplay)
            {
                DesignConfig.DateBackgroundColor = ConvertHexColor(settings.DateBackgroundColor);
                DateCoordinateX = settings.DateCoordinateX;
                DateCoordinateY = settings.DateCoordinateY;
                DesignConfig.DateWidth = settings.DateWidth;
                DesignConfig.DateHeight = settings.DateHeight;
                DesignConfig.DateBorderColor = ConvertHexColor(settings.DateBorderColor);
                DesignConfig.DateFontColor = ConvertHexColor(settings.DateFontColor);
                DesignConfig.DateFormat = settings.DateFormat;
                DesignConfig.DateFontFamily = settings.DateFontFamily;
                DesignConfig.DateFontSize = settings.DateFontSize;
            }
        }

        /// <summary>
        /// アプリ設定をSettingsオブジェクトへ書き出し
        /// </summary>
        public Settings ExportSettings(string name, string author, string version)
        {
            var settings = new Settings();
            settings.Name = name;
            settings.Author = author;
            settings.Version = version;
            settings.TargetAppVersion = GetAppVersion();
            settings.BackgroundColor = DesignConfig.BackgroundColor.ToString();
            settings.BackgroundImageDisplay = DesignConfig.IsBackgroundImageDisplay;
            if (DesignConfig.IsBackgroundImageDisplay)
            {

            }
            settings.ScaleDisplay = DesignConfig.IsScaleDisplay;
            if (DesignConfig.IsScaleDisplay)
            {
                settings.ScaleColor = DesignConfig.ScaleColor.ToString();
                settings.ScaleCount = DesignConfig.ScaleCount;
                settings.ScaleRadius = DesignConfig.ScaleRadius;
                settings.ScaleLength = DesignConfig.ScaleLength;
                settings.ScaleThickness = DesignConfig.ScaleThickness;
                settings.AlterScale = DesignConfig.IsAlterScale;
                if (DesignConfig.IsAlterScale)
                {
                    settings.AlterScaleInterval = DesignConfig.AlterScaleInterval;
                    settings.AlterScaleRadius = DesignConfig.AlterScaleRadius;
                    settings.AlterScaleLength = DesignConfig.AlterScaleLength;
                    settings.AlterScaleThickness = DesignConfig.AlterScaleThickness;
                }
            }
            // index
            settings.IndexDisplay = DesignConfig.IsIndexDisplay;
            if (DesignConfig.IsIndexDisplay)
            {
                settings.IndexType = Enum.GetName(typeof(IndexType), DesignConfig.IndexType);
                settings.IndexColor = DesignConfig.IndexColor.ToString();
                settings.IndexRadius = DesignConfig.IndexRadius;
                if (DesignConfig.IndexType == IndexType.Bar)
                {
                    settings.IndexLength = DesignConfig.IndexLength;
                    settings.IndexThickness = DesignConfig.IndexThickness;
                }
                else
                {
                    settings.IndexFontFamily = DesignConfig.IndexFontFamily;
                    settings.IndexFontSize = DesignConfig.IndexFontSize;
                }
            }
            // hands
            settings.HandsType = Enum.GetName(typeof(HandsType),DesignConfig.HandsType);
            settings.HandsDisplay = DesignConfig.IsHandsDisplay;
            if (DesignConfig.IsHandsDisplay)
            {
                settings.HandsColor =DesignConfig.HandsColor.ToString();
            }
            settings.SecondHandDisplay = DesignConfig.IsSecondHandDisplay;
            if (DesignConfig.IsSecondHandDisplay)
            {
                settings.SecondHandColor = DesignConfig.SecondHandColor.ToString();
            }
            // day date
            settings.DateDisplay = DesignConfig.IsDateDisplay;
            if (DesignConfig.IsDateDisplay)
            {
                settings.DateBackgroundColor = DesignConfig.DateBackgroundColor.ToString();
                settings.DateCoordinateX = DesignConfig.DateCoordinateX;
                settings.DateCoordinateY = DesignConfig.DateCoordinateY;
                settings.DateWidth = DesignConfig.DateWidth;
                settings.DateHeight = DesignConfig.DateHeight;
                settings.DateBorderColor = DesignConfig.DateBorderColor.ToString();
                settings.DateFontColor = DesignConfig.DateFontColor.ToString();
                settings.DateFormat = DesignConfig.DateFormat;
                settings.DateFontFamily = DesignConfig.DateFontFamily;
                settings.DateFontSize = DesignConfig.DateFontSize;
            }

            return settings;
        }

        #region Dial

        public bool IsScaleDisplay
        {
            get => DesignConfig.IsScaleDisplay;
            set
            {
                if (value == DesignConfig.IsScaleDisplay) return;
                {
                    DesignConfig.IsScaleDisplay = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsAlterScaleEnable");
                }
            }
        }

        public bool IsAlterScale
        {
            get => DesignConfig.IsAlterScale;
            set
            {
                if (value == DesignConfig.IsAlterScale) return;
                {
                    DesignConfig.IsAlterScale = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsAlterScaleEnable");
                }
            }
        }

        public bool IsAlterScaleEnable
        {
            get
            {
                if (DesignConfig.IsScaleDisplay && DesignConfig.IsAlterScale) return true;
                return false;
            }
        }


        public List<string> IndexTypeList = EnumExtension.GetLocalizeList<IndexType>();
        public IndexType IndexType
        {
            get => DesignConfig.IndexType;
            set
            {
                if (value == DesignConfig.IndexType) return;
                {
                    DesignConfig.IndexType = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IndexBarVisible");
                    OnPropertyChanged("IndexVisible");
                }
            }
        }

        public bool IsIndexDisplay
        {
            get => DesignConfig.IsIndexDisplay;
            set
            {
                if (value == DesignConfig.IsIndexDisplay) return;
                {
                    DesignConfig.IsIndexDisplay = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IndexBarVisible");
                    OnPropertyChanged("IndexVisible");
                }
            }
        }

        public Visibility IndexBarVisible
        {
            get
            {
                if (DesignConfig.IsIndexDisplay && IndexType == IndexType.Bar) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public Visibility IndexVisible
        {
            get
            {
                if (DesignConfig.IsIndexDisplay && IndexType != IndexType.Bar) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        
        #endregion

        #region Hands

        public List<string> HandsTypeList = EnumExtension.GetLocalizeList<HandsType>();

        public HandsType HandsType
        {
            get => DesignConfig.HandsType;
            set
            {
                //if (value == DesignConfig.HandsType) return;
                DesignConfig.HandsType = value;
                HourHand.Type = value;
                MinuteHand.Type = value;
                SecondHand.Type = value;
                OnPropertyChanged();
            }
        }

        private HandModel _hourHand = new HandModel(Clock.Hour);
        public HandModel HourHand
        {
            get => _hourHand;
        }

        private HandModel _minuteHand = new HandModel(Clock.Minute);
        public HandModel MinuteHand
        {
            get => _minuteHand;
        }

        private HandModel _secondHand = new HandModel(Clock.Second);
        public HandModel SecondHand
        {
            get => _secondHand;
        }


        #endregion

        #region Date

        public string[] FontList = CanvasTextFormat.GetSystemFontFamilies();

        private Thickness _dateCoordinate = new Thickness(300,300,0,0);
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

        private double _dateCoordinateX = 300;
        public double DateCoordinateX
        {
            get { return _dateCoordinateX; }
            set
            {
                if (value == _dateCoordinateX) return;
                DesignConfig.DateCoordinateX = value;
                var Y = DateCoordinate.Top;
                var coordinate = new Thickness(value, Y, 0, 0);
                DateCoordinate = coordinate;
                _dateCoordinateX = value;
                OnPropertyChanged();
            }
        }

        private double _dateCoordinateY = 300;
        public double DateCoordinateY
        {
            get { return _dateCoordinateY; }
            set
            {
                if (value == _dateCoordinateY) return;
                DesignConfig.DateCoordinateY = value;
                var X = DateCoordinate.Left;
                var coordinate = new Thickness(X, value, 0, 0);
                DateCoordinate = coordinate;
                _dateCoordinateY = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private Color ConvertHexColor(string hexCode)
        {
            hexCode = hexCode.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hexCode.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hexCode.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hexCode.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hexCode.Substring(6, 2), 16));
            return Color.FromArgb(a, r, g, b);
        }


        private BitmapSource ConvertImage(string path)
        {
            try
            {
                var bitmap = new BitmapImage(new Uri(path));
                return bitmap;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        private string GetAppVersion()
        {
            var version = Package.Current.Id.Version;
            return version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
        }

    }
}
