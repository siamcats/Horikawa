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
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace iBuki
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel vm = new MainPageViewModel();
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
            SetTitleBar();

            // イベントハンドラの設定
            Application.Current.Suspending += OnSuspending; //アプリ終了イベント
            Application.Current.Resuming += OnResuming; //アプリ復帰イベント
            /// タイマーイベント
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.125);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // 設定の読み込み・反映
            /// クリップ状態
            if (vm.AppConfig.IsTopMost)
            { StartOverlay(); }
            else
            { StopOverlay(); }
            /// デザイン
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Const.THEME_CURRENT))
            {
                var json = (string)ApplicationData.Current.LocalSettings.Values[Const.THEME_CURRENT];
                vm.ImportSettingsAsync(Deserialize(json));
            }
            else
            {
                // 保存された設定がなければAssetsのデフォルトテーマを使用
                ImportAssetsTheme(Const.THEME_DEFAULT);
                Debug.WriteLine("起動時初期値");
            }
        }

        /// <summary>
        /// （イベント）復帰
        /// </summary>
        private void OnResuming(object sender, object e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Const.THEME_CURRENT))
            {
                var json = (string)ApplicationData.Current.LocalSettings.Values[Const.THEME_CURRENT];
                vm.ImportSettingsAsync(Deserialize(json));
            }
            // 設定がなければAssetsのデフォルトテーマを使用
            ImportAssetsTheme(Const.THEME_DEFAULT);
        }

        /// <summary>
        /// （イベント）停止
        /// </summary>
        private void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var settings = vm.ExportSettings(Const.THEME_CURRENT, Const.THEME_CURRENT, Const.GetAppVersion());
            var json = Serialize(settings);
            Debug.WriteLine("終了時保存 - " + json);
            ApplicationData.Current.LocalSettings.Values[Const.THEME_CURRENT] = json;
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
            var localDate = DateTime.Now;
            //var localDate = DateTime.Parse("2019/12/12 10:08:42");
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
            var filePicker = new FileOpenPicker();

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
                await file.CopyAsync(ApplicationData.Current.LocalFolder, Const.FILE_BACKGROUND, NameCollisionOption.ReplaceExisting);
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

        /// <summary>
        /// Assetからの設定ファイルコピー
        /// </summary>
        /// <param name="name"></param>
        private async void ImportAssetsTheme(string name)
        {
            string json = "";

            // 設定ファイル
            try
            {
                var settingFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Const.URI_ASSETS + name + "/" + Const.FILE_SETTINGS));
                json = await FileIO.ReadTextAsync(settingFile);
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine(Const.URI_ASSETS + name + "/" + Const.FILE_SETTINGS + " Not Found");
            }
            var settings = Deserialize(json);

            // 画像ファイル
            try
            {
                if (settings.BackgroundImageDisplay)
                {
                    var bgimageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Const.URI_ASSETS + name + "/" + Const.FILE_BACKGROUND));
                    var bgimageFileCopied = await bgimageFile.CopyAsync(ApplicationData.Current.LocalFolder, Const.FILE_BACKGROUND, NameCollisionOption.ReplaceExisting);
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine(Const.URI_ASSETS + name + "/" + Const.FILE_BACKGROUND + " Not Found");
            }

            vm.ImportSettingsAsync(settings);
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
            ImportAssetsTheme(item);
        }

        /// <summary>
        ///  タイトルバーの設定を行う
        /// </summary>
        private void SetTitleBar()
        {
            // タイトルバーの領域を指定する
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
        }
    }
}