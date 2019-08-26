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
using Windows.Globalization;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml.Markup;
using Windows.Storage.Pickers;
using System.Runtime.Serialization.Json;
using System.Text;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace iBuki
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel vm = new MainPageViewModel();

        private StorageFolder _storageFolder = ApplicationData.Current.LocalFolder;
        private DispatcherTimer _timer;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            DataContext = vm.AppConfig;

            // デフォルトサイズは500固定
            var size = new Size(vm.AppConfig.WindowSize, vm.AppConfig.WindowSize);
            ApplicationView.PreferredLaunchViewSize = size;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            var view = ApplicationView.GetForCurrentView();
            view.SetPreferredMinSize(size);

        }

        /// <summary>
        /// （イベント）初期遷移時
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

            // Suspending・Resumingイベントハンドラ
            Application.Current.Suspending += OnSuspending;
            Application.Current.Resuming += OnResuming;

            // タイマーイベントの間隔を指定します。。
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.125);
            _timer.Tick += Timer_Tick;
            _timer.Start();


            // デフォルトのクリップ状態を反映
            if (vm.AppConfig.IsTopMost)
            { StartOverlay(); }
            else
            { StopOverlay(); }

            // 前回設定を読み込み反映
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("CurrentSettings"))
            {
                var json = (string)ApplicationData.Current.LocalSettings.Values["CurrentSettings"];
                vm.ImportSettings(Deserialize(json));
                Debug.WriteLine("起動時復元 - " + json);
            }
            else
            {
                // 保存された設定がなければAssetsのデフォルトテーマを使用
                ImportAssetsSetting("Default");
                Debug.WriteLine("起動時初期値");
            }
        }

        /// <summary>
        /// （イベント）復帰
        /// </summary>
        private void OnResuming(object sender, object e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("CurrentSettings"))
            {
                var json = (string)ApplicationData.Current.LocalSettings.Values["CurrentSettings"];
                vm.ImportSettings(Deserialize(json));
            }
            // 設定がなければAssetsのデフォルトテーマを使用
            ImportAssetsSetting("Default");
        }

        /// <summary>
        /// （イベント）停止
        /// </summary>
        private void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var json = Serialize(vm.ExportSettings("local","local","1.0.0"));
            Debug.WriteLine("終了時保存 - " + json);
            ApplicationData.Current.LocalSettings.Values["CurrentSettings"] = json;
        }

        /// <summary>
        /// （イベント）ウィンドウのアクティブ状態変化
        /// </summary>
        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState != CoreWindowActivationState.Deactivated)
            {
                // アクティブ
                myTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                // 非アクティブ
                myTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// （イベント）タイトルバーの大きさが変わった時
        /// </summary>
        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            // タイトルバーの高さ
            myTitleBar.Height = sender.Height;

            // タイトルバーの左右に確保するスペース
            myTitleBar.Padding = new Thickness(sender.SystemOverlayLeftInset, 0.0, sender.SystemOverlayRightInset, 0.0);
            ConfigPanel.Margin = new Thickness(0, sender.Height, 0, 0);            
        }

        /// <summary>
        /// （イベント）チックで針描画
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            DateTime localDate = DateTime.Now;
            //textBlock.Text = localDate.ToString("hh:mm:ss.fff");

            hourHandAngle.Angle = CalcAngleHour(localDate);
            minuteHandAngle.Angle = CalcAngleMinute(localDate);
            secondHandAngle.Angle = CalcAngleSecond(localDate);
            dateDisplay.Text = CalcDate(localDate);
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
            //var angle = 6 * ss;
            var angle = vm.AppConfig.Movement == Movement.Quartz
                ? 6 * ss
                : 6 * ss + fff / 1000 * 6;
            return decimal.ToDouble(angle);
        }

        private string CalcDate(DateTime now)
        {
            var ss = now.ToString(vm.DesignConfig.DateFormat, new CultureInfo("en-US"));
            return ss;
        }

        /// <summary>
        ///（イベント）画像取り込みボタンタップ
        /// </summary>
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
                vm.DesignConfig.BackgroundImage = bitmap;
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WindowSizeChange();
        }

        private void WindowSizeChange()
        {
            var view = ApplicationView.GetForCurrentView();
            var size = new Size(vm.AppConfig.WindowSize, vm.AppConfig.WindowSize);
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

        /// <summary>
        /// Assetからの設定ファイル読み込み
        /// </summary>
        /// <param name="name"></param>
        private async void ImportAssetsSetting(string name)
        {
            var uri = "ms-appx:///Assets/Themes/" + name + "/Settings.json";

            try
            {
                // Assetsからのファイル取り出し
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));
                string json = await FileIO.ReadTextAsync(file);
                // 設定反映
                vm.ImportSettings(Deserialize(json));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Json文字列をSettingsオブジェクトにデシリアライズ
        /// </summary>
        private Settings Deserialize(string json)
        {
            Settings settings;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(Settings));
                settings = serializer.ReadObject(stream) as Settings;
            }
            //settings.DebugLog("settings");
            return settings;
        }

        /// <summary>
        /// SettingsオブジェクトをJson文字列にシリアライズ
        /// </summary>
        private string Serialize(Settings settings)
        {
            var serializer = new DataContractJsonSerializer(typeof(Settings));
            string json;
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, settings);
                json = Encoding.UTF8.GetString(stream.ToArray());
                //Debug.WriteLine(json);
            }
            return json;
        }

        /// <summary>
        /// （イベント）クリップボタンタップ
        /// </summary>
        private void ClipButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (vm.AppConfig.IsTopMost)
            { StartOverlay(); }
            else
            { StopOverlay(); }
        }

        private async void StartOverlay()
        {
            var compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            var size = new Size(vm.AppConfig.WindowSize, vm.AppConfig.WindowSize);
            compactOptions.CustomSize = size;
            var result = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
            if (result) vm.AppConfig.IsTopMost = true;
        }

        private async void StopOverlay()
        {
            var result = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
            if (result) vm.AppConfig.IsTopMost = true; vm.AppConfig.IsTopMost = false;
        }

        private async void HyperlinkButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AppRestartFailureReason result = await CoreApplication.RequestRestartAsync("");
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            restartLink.Visibility = Visibility.Visible;
        }

        private void ConfigButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            restartLink.Visibility = Visibility.Collapsed;
        }

        private void ColorButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void TemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = templateList.SelectedItem as string;
            ImportAssetsSetting(item);
        }
    }
}