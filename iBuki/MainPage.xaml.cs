using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace iBuki
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public AppConfig AppConfig { get; set; } = new AppConfig();
        public DesignConfig DesignConfig { get; set; } = new DesignConfig();

        private StorageFolder _storageFolder = ApplicationData.Current.LocalFolder;
        private DispatcherTimer _timer;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            DataContext = AppConfig;
            DataContext = DesignConfig;

            // デフォルトサイズは500です
            var size = new Size(AppConfig.WindowSize, AppConfig.WindowSize);
            ApplicationView.PreferredLaunchViewSize = size;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            var view = ApplicationView.GetForCurrentView();
            view.SetPreferredMinSize(size);


            // ウインドウが初めてアクティブになったとき、 CompactOverlay にする。
            bool isFirstActivate = true;
            Window.Current.Activated += async (s, e) =>
            {
                if (e.WindowActivationState == CoreWindowActivationState.CodeActivated && isFirstActivate)
                {
                    isFirstActivate = false;
                    StartOverlay();
                }
            };
        }

        private async void StartOverlay()
        {
            var compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            var size = new Size(AppConfig.WindowSize, AppConfig.WindowSize);
            compactOptions.CustomSize = size;
            var result = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
            if (result) AppConfig.IsTopMost = true;
        }

        private async void StopOverlay()
        {
            var result = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
            if (result) AppConfig.IsTopMost = true; AppConfig.IsTopMost = false;
        }

        /// <summary>
        /// 遷移時
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            // タイトルバーの領域までアプリの表示を拡張する
            coreTitleBar.ExtendViewIntoTitleBar = true;

            // ［×］ボタンなどの背景色を設定する
            appTitleBar.ButtonBackgroundColor = Colors.Transparent;
            appTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            appTitleBar.ButtonForegroundColor = Colors.White;

            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            Window.Current.SetTitleBar(moveButton);
            Window.Current.Activated += Current_Activated;

            // タイマーイベントの間隔を指定します。。
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.125);
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }

        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState != Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                // フォーカスイン
                myTitleBar.Visibility = Visibility.Visible;
                myTitleBar.Background.Opacity = 1.0;
            }
            else
            {
                // フォーカスアウト
                myTitleBar.Visibility = Visibility.Collapsed;
                myTitleBar.Background.Opacity = 0.0;
            }
        }

        // タイトルバーの寸法が変わったとき
        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            // タイトルバーの高さ
            myTitleBar.Height = sender.Height;

            // タイトルバーの左右に確保するスペース
            myTitleBar.Padding = new Thickness(sender.SystemOverlayLeftInset,0.0, sender.SystemOverlayRightInset, 0.0);
            ConfigPanel.Margin = new Thickness(0,sender.Height,0,0);
        }

        /// <summary>
        /// チック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, object e)
        {
            DateTime localDate = DateTime.Now;
            //textBlock.Text = localDate.ToString("hh:mm:ss.fff");

            hourHandAngle.Angle = CalcAngleHour(localDate);
            minuteHandAngle.Angle = CalcAngleMinute(localDate);
            secondHandAngle.Angle = CalcAngleSecond(localDate);
            dateDisplay.Text = CalcDate(localDate);
        }


        private async void DialImagePicker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var filePicker = new Windows.Storage.Pickers.FileOpenPicker();

            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".png");
            //filePicker.FileTypeFilter.Add("*");

            // 単一ファイルの選択
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                var bitmap = new BitmapImage();
                using (var stream = await file.OpenReadAsync())
                {
                    await bitmap.SetSourceAsync(stream);
                }
                DesignConfig.DialImage = bitmap;
            }
        }

        //public async Task GetZipFileInformation(Stream stream)
        //{
        //   ZipArchive zip = new ZipArchive(stream);
        //    var firstFile = zip.Entries.FirstOrDefault();
        //    if (firstFile != null)
        //    { }
        //}

        private void WindowSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            WindowSizeChange();
        }

        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ImportSetting("Porto");
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WindowSizeChange();
            Serialize();
        }

        private double CalcAngleHour(DateTime now)
        {
            var hh = Convert.ToDecimal(now.ToString("hh"));
            var mm = Convert.ToDecimal(now.ToString("mm"));
            var ss = Convert.ToDecimal(now.ToString("ss"));
            Decimal angle = hh * 360 / 12 + mm * 360 / 12 / 60 + ss * 360 / 12 / 60 / 60;
            //Debug.WriteLine(angle.ToString());
            return decimal.ToDouble(angle);
        }

        private double CalcAngleMinute(DateTime now)
        {
            var mm = Convert.ToDecimal(now.ToString("mm"));
            var ss = Convert.ToDecimal(now.ToString("ss"));
            var angle = mm * 360 / 60 + ss * 360 / 60 / 60;
            //Debug.WriteLine(angle.ToString());
            return decimal.ToDouble(angle);
        }

        private double CalcAngleSecond(DateTime now)
        {
            var ss = Convert.ToDecimal(now.ToString("ss"));
            var fff = Convert.ToDecimal(now.ToString("fff"));
            //var angle = 6 * ss;
            //var angle2 = Convert.ToDouble(fff)/1000*6;
            //Debug.WriteLine(angle + angle2);
            var angle = AppConfig.Movement == Movement.Quartz
                ? 6 * ss
                : 6 * ss + fff / 1000 * 6;
            return decimal.ToDouble(angle);
        }

        private string CalcDate(DateTime now)
        {
            var ss = now.ToString(DesignConfig.DateDisplayFormat, new CultureInfo("en-US"));
            return ss;
        }

        private void WindowSizeChange()
        {
            var view = ApplicationView.GetForCurrentView();
            var size = new Size(AppConfig.WindowSize, AppConfig.WindowSize);
            if (!view.TryResizeView(size))
            {
                Debug.WriteLine("Try Resize Window 失敗");
            }
        }

        private async void SaveSetting()
        {
            StorageFile currentSetting = await _storageFolder.CreateFileAsync("CurrentSetting.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(currentSetting, "");
        }


        private void Serialize()
        {

            var setting = new Settings()
            {
                Author = "aaa",
                //BackgroundColor = new SolidColorBrush(Windows.UI.Colors.Blue)
            };

            var serializer2 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Settings));

            using (var stream = new MemoryStream())
            {
                serializer2.WriteObject(stream, setting);
                string jsonData = System.Text.Encoding.UTF8.GetString(stream.ToArray());
                Debug.WriteLine(jsonData);
            }
        }

        private async void ImportSetting(string name)
        {
            var filePicker = new Windows.Storage.Pickers.FileOpenPicker();

            filePicker.FileTypeFilter.Add(".json");
            //filePicker.FileTypeFilter.Add("*");

            // 単一ファイルの選択
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                // ファイルの読み込み
                var settings = new Settings();
                string json = await FileIO.ReadTextAsync(file);
                using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
                {
                    // 変換できるシリアライザーを作成
                    var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Settings));
                    // クラスにデータを読み込む
                    settings = serializer.ReadObject(stream) as Settings;
                    Debug.WriteLine(settings);
                }

                DesignConfig.IsDateDisplay = settings.IsDateDisplay;
                DesignConfig.DateDisplayFormat = settings.DateDisplayFormat;
                DesignConfig.HandsColor = settings.GetBrush(settings.HandsColor);
            }
        }

        private void ClipButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AppConfig.IsTopMost)
            {
                StartOverlay();
            }
            else
            {
                StopOverlay();
            }
        }

        //private async void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //}
    }
}