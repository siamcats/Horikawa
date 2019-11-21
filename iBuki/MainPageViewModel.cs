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
using Windows.Services.Store;
using Microsoft.Graphics.Canvas.Text;

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

        #region License

        private bool _isLicensedDayDate = false;
        public bool IsLicensedDayDate
        {
            get => _isLicensedDayDate;
            set
            {
                if (value == _isLicensedDayDate) return;
                _isLicensedDayDate = value;
                OnPropertyChanged();
            }
        }

        private bool _isLicensedMoonPhase = false;
        public bool IsLicensedMoonPhase
        {
            get => _isLicensedMoonPhase;
            set
            {
                if (value == _isLicensedMoonPhase) return;
                _isLicensedMoonPhase = value;
                OnPropertyChanged();
            }
        }

        public void SetLicense(string storeId)
        {
            //SkuStoreIdは末尾に余分な文字列があるため曖昧比較する
            if (storeId.Contains(Const.STORE_ID_DAYDATE))
            {
                IsLicensedDayDate = true;
            }
            else if (storeId.Contains(Const.STORE_ID_MOONPHASE))
            {
                IsLicensedMoonPhase = true;
            }
        }

        #endregion

        /// <summary>
        /// Settingsオブジェクトをアプリに反映
        /// </summary>
        public async void ImportSettingsAsync(Settings settings, bool isAsset = false)
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
                SelectedBackgroundImageStretch = ConvertEnum<Stretch>(settings.BackgroundImageStretch);
                DesignConfig.BackgroundImageCoordinateX = settings.BackgroundImageCoordinateX;
                DesignConfig.BackgroundImageCoordinateY = settings.BackgroundImageCoordinateY;
            }
            // foreground
            DesignConfig.IsForegroundImageDisplay = settings.ForegroundImageDisplay;
            if (DesignConfig.IsForegroundImageDisplay)
            {
                try
                {
                    var bitmap = new BitmapImage();
                    var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Const.URI_CURRENT_FOREGROUND));
                    using (var stream = await file.OpenReadAsync())
                    {
                        await bitmap.SetSourceAsync(stream);
                    }
                    DesignConfig.ForegroundImage = bitmap;
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine(Const.URI_CURRENT_FOREGROUND + " Not Found");
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
                IndexType = (IndexType)Enum.Parse(typeof(IndexType), settings.IndexType);
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
            HandsType = (HandsType)Enum.Parse(typeof(HandsType), settings.HandsType);
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
            if (!isAsset && settings.DateDisplay) ///ライセンスチェック
            {
                if (!IsLicensedDayDate) settings.DateDisplay = false;
            }
            DesignConfig.IsDateDisplay = settings.DateDisplay;
            if (DesignConfig.IsDateDisplay)
            {
                DesignConfig.DateBackgroundColor = ConvertHexColor(settings.DateBackgroundColor);
                DesignConfig.DateWidth = settings.DateWidth;
                DesignConfig.DateHeight = settings.DateHeight;
                DesignConfig.DateCoordinateX = settings.DateCoordinateX;
                DesignConfig.DateCoordinateY = settings.DateCoordinateY;
                DesignConfig.DateBorderColor = ConvertHexColor(settings.DateBorderColor);
                DesignConfig.DateBorderThickness = settings.DateBorderThickness;
                DesignConfig.DateFontColor = ConvertHexColor(settings.DateFontColor);
                DesignConfig.DateFormat = settings.DateFormat;
                DesignConfig.DateFontFamily = settings.DateFontFamily;
                DesignConfig.DateFontSize = settings.DateFontSize;
            }
            // moon phase
            if (!isAsset && settings.MoonPhaseDisplay) ///ライセンスチェック
            {
                if (!IsLicensedMoonPhase) settings.MoonPhaseDisplay = false;
            }
            DesignConfig.IsMoonPhaseDisplay = settings.MoonPhaseDisplay;
            if (DesignConfig.IsMoonPhaseDisplay)
            {
                DesignConfig.MoonPhaseSize = settings.MoonPhaseSize;
                DesignConfig.MoonPhaseCoordinateX = settings.MoonPhaseCoordinateX;
                DesignConfig.MoonPhaseCoordinateY = settings.MoonPhaseCoordinateY;
                DesignConfig.MoonPhaseForegroundColor = ConvertHexColor(settings.MoonPhaseForegroundColor);
                DesignConfig.IsMoonPhaseBackgroundImageDisplay = settings.MoonPhaseBackgroundImageDisplay;
                if (DesignConfig.IsMoonPhaseBackgroundImageDisplay)
                {
                    try
                    {
                        var bitmap = new BitmapImage();
                        var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Const.URI_CURRENT_MOONPHASE_BACKGROUND));
                        using (var stream = await file.OpenReadAsync())
                        {
                            await bitmap.SetSourceAsync(stream);
                        }
                        DesignConfig.MoonPhaseBackgroundImage = bitmap;
                    }
                    catch (FileNotFoundException)
                    {
                        Debug.WriteLine(Const.URI_CURRENT_MOONPHASE_BACKGROUND + " Not Found");
                    }
                }
                DesignConfig.IsMoonPhaseForegroundImageDisplay = settings.MoonPhaseForegroundImageDisplay;
                if (DesignConfig.IsMoonPhaseForegroundImageDisplay)
                {
                    try
                    {
                        var bitmap = new BitmapImage();
                        var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Const.URI_CURRENT_MOONPHASE_FOREGROUND));
                        using (var stream = await file.OpenReadAsync())
                        {
                            await bitmap.SetSourceAsync(stream);
                        }
                        DesignConfig.MoonPhaseForegroundImage = bitmap;
                    }
                    catch (FileNotFoundException)
                    {
                        Debug.WriteLine(Const.URI_CURRENT_MOONPHASE_FOREGROUND + " Not Found");
                    }
                }
            }
        }

        /// <summary>
        /// 現在のアプリの設定をSettingsオブジェクトへ書き出し
        /// </summary>
        public Settings ExportSettings(string name, string author, string description = "")
        {
            var settings = new Settings();
            settings.Name = name;
            settings.Author = author;
            settings.Description = description;
            settings.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
            settings.TargetAppVersion = Const.APP_VERSION;
            // background
            settings.BackgroundColor = DesignConfig.BackgroundColor.ToString();
            settings.BackgroundImageDisplay = DesignConfig.IsBackgroundImageDisplay;
            if (DesignConfig.IsBackgroundImageDisplay)
            {
                settings.BackgroundImageStretch = Enum.GetName(typeof(Stretch), DesignConfig.BackgroundImageStretch);
                settings.BackgroundImageCoordinateX = DesignConfig.BackgroundImageCoordinateX;
                settings.BackgroundImageCoordinateY = DesignConfig.BackgroundImageCoordinateY;
            }
            settings.ForegroundImageDisplay = DesignConfig.IsForegroundImageDisplay;

            // scale
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
            settings.HandsType = Enum.GetName(typeof(HandsType), DesignConfig.HandsType);
            settings.HandsDisplay = DesignConfig.IsHandsDisplay;
            if (DesignConfig.IsHandsDisplay)
            {
                settings.HandsColor = DesignConfig.HandsColor.ToString();
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
                settings.MoonPhaseBackgroundImageDisplay = DesignConfig.IsMoonPhaseBackgroundImageDisplay;
                settings.MoonPhaseForegroundImageDisplay = DesignConfig.IsMoonPhaseForegroundImageDisplay;
            }


            return settings;
        }

        #region General / About

        public List<string> LanguageList = Const.LANGUAGE_LIST;

        public ObservableCollection<StoreProduct> AddOnList = new ObservableCollection<StoreProduct>();

        public ObservableCollection<StoreProduct> LicensedAddOnList = new ObservableCollection<StoreProduct>();

        public string AppVersion = Const.APP_VERSION;

        public string AppName = Const.APP_NAME;

        public string AppAuthor = Const.APP_AUTHOR;

        public Uri AppLogo = Const.APP_LOGO;

        #endregion

        #region Template

        public ObservableCollection<Settings> PresetTemplateList = new ObservableCollection<Settings>();

        public ObservableCollection<Settings> TemplateList = new ObservableCollection<Settings>();

        #endregion

        #region Background

        public List<string> BackgroundImageStretchList = EnumExtension.GetLocalizeList<StretchLocalize>();
        public Stretch SelectedBackgroundImageStretch
        {
            get => DesignConfig.BackgroundImageStretch;
            set
            {
                if (value == DesignConfig.BackgroundImageStretch) return;
                {
                    DesignConfig.BackgroundImageStretch = value;
                    OnPropertyChanged();
                }
            }
        }

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

        #region 雑多なprivateメソッド

        /// <summary>
        /// 16進数カラーコード文字列をColorに変換
        /// </summary>
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

        /// <summary>
        /// Enum名称をEnumに変換
        /// </summary>
        private T ConvertEnum<T>(string enumStr)
        {
            if (string.IsNullOrEmpty(enumStr)) return default(T); //設定なし
            return (T)Enum.Parse(typeof(T), enumStr);
        }

        #endregion
    }
}
