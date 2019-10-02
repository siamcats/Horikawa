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
using System.Collections.ObjectModel;

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
        /// Settingsオブジェクトをアプリに反映
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
                    var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Const.URI_CURRENT_BACKGROUND));
                    using (var stream = await file.OpenReadAsync())
                    {
                        await bitmap.SetSourceAsync(stream);
                    }
                    DesignConfig.BackgroundImage = bitmap;
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine(Const.URI_CURRENT_BACKGROUND + " Not Found");
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
                DesignConfig.IndexInterval = settings.indexInterval;
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
                DesignConfig.HandsStrokeColor = ConvertHexColor(settings.HandsStrokeColor);
                DesignConfig.HandsStrokeThickness = settings.HandsStrokeThickness;
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
                DesignConfig.DateCoordinateX = settings.DateCoordinateX;
                DesignConfig.DateCoordinateY = settings.DateCoordinateY;
                DesignConfig.DateWidth = settings.DateWidth;
                DesignConfig.DateHeight = settings.DateHeight;
                DesignConfig.DateBorderColor = ConvertHexColor(settings.DateBorderColor);
                DesignConfig.DateBorderThickness = settings.DateBorderThickness;
                DesignConfig.DateFontColor = ConvertHexColor(settings.DateFontColor);
                DesignConfig.DateFormat = settings.DateFormat;
                DesignConfig.DateFontFamily = settings.DateFontFamily;
                DesignConfig.DateFontSize = settings.DateFontSize;
            }
            // moon phase
            DesignConfig.IsMoonPhaseDisplay = settings.MoonPhaseDisplay;
            if (DesignConfig.IsMoonPhaseDisplay)
            {
                DesignConfig.MoonPhaseSize = settings.MoonPhaseSize;
                DesignConfig.MoonPhaseCoordinateX = settings.MoonPhaseCoordinateX;
                DesignConfig.MoonPhaseCoordinateY = settings.MoonPhaseCoordinateY;
                DesignConfig.MoonPhaseForegroundColor = ConvertHexColor(settings.MoonPhaseForegroundColor);
            }
        }

        /// <summary>
        /// 現在のアプリの設定をSettingsオブジェクトへ書き出し
        /// </summary>
        public Settings ExportSettings(string name, string author, string description = "" )
        {
            var settings = new Settings();
            settings.Name = name;
            settings.Author = author;
            settings.Description = description;
            settings.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
            settings.TargetAppVersion = Const.APP_VERSION;
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
                settings.indexInterval = DesignConfig.IndexInterval;
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
                settings.HandsStrokeColor = DesignConfig.HandsStrokeColor.ToString();
                settings.HandsStrokeThickness = DesignConfig.HandsStrokeThickness;
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
                settings.DateBorderThickness = DesignConfig.DateBorderThickness;
                settings.DateFontColor = DesignConfig.DateFontColor.ToString();
                settings.DateFormat = DesignConfig.DateFormat;
                settings.DateFontFamily = DesignConfig.DateFontFamily;
                settings.DateFontSize = DesignConfig.DateFontSize;
            }

            // moon phase
            settings.MoonPhaseDisplay = DesignConfig.IsMoonPhaseDisplay;
            if (DesignConfig.IsMoonPhaseDisplay)
            {
                settings.MoonPhaseSize = DesignConfig.MoonPhaseSize;
                settings.MoonPhaseCoordinateX = DesignConfig.MoonPhaseCoordinateX;
                settings.MoonPhaseCoordinateY = DesignConfig.MoonPhaseCoordinateY;
                settings.MoonPhaseForegroundColor = DesignConfig.MoonPhaseForegroundColor.ToString();
            }


            return settings;
        }

        #region General

        public List<string> LanguageList = Const.LANGUAGE_LIST;

        public string AppVersion = Const.APP_VERSION;

        public string AppName = Const.GetAppName();

        public string AppAuthor = Const.GetAppAuthor();

        public Uri AppLogo = Const.APP_LOGO;

        #endregion

        #region Template

        public ObservableCollection<Settings> PresetTemplateList = new ObservableCollection<Settings>();

        public ObservableCollection<Settings> TemplateList = new ObservableCollection<Settings>();

        #endregion

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

        #region Day Date

        public string[] FontList = CanvasTextFormat.GetSystemFontFamilies();

        #endregion

        #region Moon Phase

        #endregion

        private Color ConvertHexColor(string hexCode)
        {
            if(string.IsNullOrEmpty(hexCode)||!hexCode.Contains("#")) return Color.FromArgb(255, 255, 255, 255);
            hexCode = hexCode.Replace("#", string.Empty);
            if(hexCode.Length!=8) return Color.FromArgb(255, 255, 255, 255);
            byte a = (byte)(Convert.ToUInt32(hexCode.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hexCode.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hexCode.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hexCode.Substring(6, 2), 16));
            return Color.FromArgb(a, r, g, b);
        }

    }
}
