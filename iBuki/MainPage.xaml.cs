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
using Windows.ApplicationModel;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using System.Text.RegularExpressions;
using Windows.Services.Store;
using System.Collections.Generic;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace iBuki
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel vm = new MainPageViewModel();
        private DispatcherTimer timer;

        #region lifecycle

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            DataContext = vm.AppConfig;

            GetLicenseInfo();

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
            DebugLocalFolder();
            SetTitleBar();

            // イベントハンドラの設定
            Application.Current.Suspending += OnSuspending; //アプリ終了イベント
            Application.Current.Resuming += OnResuming; //アプリ復帰イベント
            /// タイマーイベント
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.125);
            timer.Tick += Timer_Tick;
            timer.Start();

            // プリセットテンプレートの読み取り
            GetAssetsTemplate();

            GetTemplateList();

            // 設定の読み込み・反映
            /// クリップ状態
            if (vm.AppConfig.IsTopMost)
            { StartOverlay(); }
            else
            { StopOverlay(); }
            /// デザイン
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Const.KEY_CURRENT_SETTINGS))
            {
                var json = (string)ApplicationData.Current.LocalSettings.Values[Const.KEY_CURRENT_SETTINGS];
                vm.ImportSettingsAsync(Deserialize(json));
            }
            else
            {
                // 保存された設定がなければAssetsのデフォルトテーマを使用
                Debug.WriteLine("起動時初期値");
                SetInitSettings();
            }
            SetupStartupToggle();
            WindowSizeChange();
        }

        /// <summary>
        /// （イベント）復帰
        /// </summary>
        private void OnResuming(object sender, object e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Const.KEY_CURRENT_SETTINGS))
            {
                var json = (string)ApplicationData.Current.LocalSettings.Values[Const.KEY_CURRENT_SETTINGS];
                vm.ImportSettingsAsync(Deserialize(json));
            }
        }

        /// <summary>
        /// （イベント）停止
        /// </summary>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var settings = vm.ExportSettings(Const.KEY_CURRENT_SETTINGS, Const.KEY_CURRENT_SETTINGS, "Description");
            var json = Serialize(settings);
            Debug.WriteLine("終了時保存 - " + json);
            ApplicationData.Current.LocalSettings.Values[Const.KEY_CURRENT_SETTINGS] = json;
        }

        #endregion

        #region 画像関連

        /// <summary>
        ///（イベント）画像取り込みボタンタップ
        /// </summary>
        private async void DialImagePicker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();

            //filePicker.FileTypeFilter.Add(".jpg");
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
                //LocalFolder/Background.pngに配置
                await file.CopyAsync(ApplicationData.Current.LocalFolder, Const.FILE_BACKGROUND, NameCollisionOption.ReplaceExisting);
                //アプリデザインに反映
                vm.DesignConfig.BackgroundImage = bitmap;
            }
        }
        private async void ForegroundImagePicker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".png");

            // 単一ファイルの選択
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                var bitmap = new BitmapImage();
                using (var stream = await file.OpenReadAsync())
                {
                    await bitmap.SetSourceAsync(stream);
                }
                //LocalFolder/Background.pngに配置
                await file.CopyAsync(ApplicationData.Current.LocalFolder, Const.FILE_FOREGROUND, NameCollisionOption.ReplaceExisting);
                //アプリデザインに反映
                vm.DesignConfig.ForegroundImage = bitmap;
            }
        }

        /// <summary>
        ///（イベント）ムーンフェイズ画像取り込みボタンタップ
        /// </summary>
        private async void MoonPhaseImagePicker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".png");

            // 単一ファイルの選択
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                var bitmap = new BitmapImage();
                using (var stream = await file.OpenReadAsync())
                {
                    await bitmap.SetSourceAsync(stream);
                }
                //LocalFolder/MoonPhaseBackground.pngに配置
                await file.CopyAsync(ApplicationData.Current.LocalFolder, Const.FILE_MOONPHASE_BACKGROUND, NameCollisionOption.ReplaceExisting);
                //アプリデザインに反映
                vm.DesignConfig.MoonPhaseBackgroundImage = bitmap;
            }
        }

        /// <summary>
        ///（イベント）ムーンフェイズ画像取り込みボタンタップ
        /// </summary>
        private async void MoonPhaseForegroundImagePicker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();

            //filePicker.FileTypeFilter.Add(".jpg");
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
                //LocalFolder/MoonPhaseBackground.pngに配置
                await file.CopyAsync(ApplicationData.Current.LocalFolder, Const.FILE_MOONPHASE_FOREGROUND, NameCollisionOption.ReplaceExisting);
                //アプリデザインに反映
                vm.DesignConfig.MoonPhaseForegroundImage = bitmap;
            }
        }

        /// <summary>
        /// （イベント）カラー選択ボタン
        /// </summary>
        private void ColorButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        #endregion

        #region 針の描写更新

        DateTime beforeDate = new DateTime(); //現在日退避
        readonly DateTime defaultDate = new DateTime(); //比較用の初期値日付

        /// <summary>
        /// （イベント）チックで針描画
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            DateTime localDate;
            if (stopTickToggle.IsOn)
            {
                localDate = DateTime.Parse("2019/12/12 10:08:37");
            }
            else
            {
                localDate = DateTime.Now;
            }
            //textBlock.Text = localDate.ToString("hh:mm:ss.fff");

            hourHandAngle.Angle = CalcAngleHour(localDate);
            minuteHandAngle.Angle = CalcAngleMinute(localDate);
            secondHandAngle.Angle = CalcAngleSecond(localDate);
            dateDisplay.Text = CalcDate(localDate); //デイトは秒表示もできるから日替わり処理じゃなくてここでする

            // 退避日と日付が異なれば日替わり処理を起こす
            if (beforeDate == defaultDate || localDate.Date != beforeDate.Date)
            {
                moonPhase.DateTime = localDate;
                beforeDate = localDate; //現在日退避
            }
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
            var movement = vm.AppConfig.Movement;
            if (movement == Movement.Chronograph) return 0;

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
            try
            {
                var ss = now.ToString(vm.DesignConfig.DateFormat, new CultureInfo("en-US"));
                return ss;
            }
            catch (FormatException e)
            {
                Debug.WriteLine(e.Message);
                vm.DesignConfig.DateFormat = "";
                return "";
            }
        }

        #endregion

        #region 設定ファイル関連の操作

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

        #endregion

        #region ウィンドウサイズの制御

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //WindowSizeChange();
        }

        private void WindowSizeChange()
        {
            var view = ApplicationView.GetForCurrentView();
            var size = new Size(vm.AppConfig.WindowSize, vm.AppConfig.WindowSize);
            if (!view.TryResizeView(size))
            {
                Debug.WriteLine("TryResizeWindow 失敗");
            }
            else
            {
                Debug.WriteLine("TryResizeWindow : " + vm.AppConfig.WindowSize);
            }
            
        }

        #endregion

        #region 言語選択

        /// <summary>
        /// （イベント）言語選択
        /// </summary>
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //言語選択したら再起動を促す
            restartLink.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// （イベント）言語選択後は再起動でアプリに反映させる
        /// </summary>
        private async void HyperlinkButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AppRestartFailureReason result = await CoreApplication.RequestRestartAsync("");
        }

        /// <summary>
        /// （イベント）設定パネル
        /// </summary>
        private void ConfigButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //設定パネル押したら再起動しますか？表示は消しとく
            restartLink.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region デスクトップ最前面表示の制御

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

        /// <summary>
        /// 最前面表示にする
        /// </summary>
        private async void StartOverlay()
        {
            var compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            var size = new Size(vm.AppConfig.WindowSize, vm.AppConfig.WindowSize);
            compactOptions.CustomSize = size;
            var result = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
            if (result) vm.AppConfig.IsTopMost = true;
        }

        /// <summary>
        /// 最前面表示を解除する
        /// </summary>
        private async void StopOverlay()
        {
            var result = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
            if (result) vm.AppConfig.IsTopMost = true; vm.AppConfig.IsTopMost = false;
        }

        #endregion

        #region ウィンドウタイトルバー関連の制御

        /// <summary>
        /// （イベント）ウィンドウのアクティブ状態変化
        /// </summary>
        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState != CoreWindowActivationState.Deactivated)
            {
                // アクティブ時のみタイトルバーを表示
                myTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                // 非アクティブ
                myTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///  ウィンドウタイトルバーの設定
        /// </summary>
        private void SetTitleBar()
        {
            // タイトルバーの領域を指定する
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            // タイトルバーの領域までアプリの表示を拡張する
            coreTitleBar.ExtendViewIntoTitleBar = true;

            // ［×］ボタンなどの色を設定する
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            appTitleBar.ButtonBackgroundColor = Colors.Transparent;
            appTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            appTitleBar.ButtonInactiveForegroundColor = Colors.Transparent; //効かないっぽい

            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;　//なんで必要なんだっけ？思い出したら復活させる

            Window.Current.SetTitleBar(moveButton);
            Window.Current.Activated += Current_Activated;
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
        /// デバッグ機能用、タイトルバーを隠す
        /// </summary>
        private void HideTitlebarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            myTitleBar.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region テンプレート関連の制御

        /// <summary>
        /// Assetsのテンプレートファイルをテンプレートリストに反映
        /// </summary>
        private async void GetAssetsTemplate()
        {
            var installedFolder = Package.Current.InstalledLocation;
            var assetsFolder = await installedFolder.GetFolderAsync(Const.FOLDER_ASSETS);
            var templatesFolder = await assetsFolder.GetFolderAsync(Const.FOLDER_TEMPLATES);
            var templateFolderList = await templatesFolder.GetFoldersAsync();

            foreach (var folder in templateFolderList)
            {
                // 設定ファイル→SettingsObj化
                var settingFile = await folder.GetFileAsync(Const.FILE_SETTINGS);
                var json = await FileIO.ReadTextAsync(settingFile);
                var settings = Deserialize(json);

                // サムネイル画像→SettingsObj化
                var bitmap = new BitmapImage();
                try
                {
                    var thumbFile = await folder.GetFileAsync(Const.FILE_THUMBNAIL);
                    using (var stream = await thumbFile.OpenReadAsync())
                    {
                        await bitmap.SetSourceAsync(stream);
                    }
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine(folder.Name + "/" + Const.FILE_THUMBNAIL + "not Found");
                }
                settings.Thumbnail = bitmap;
                vm.PresetTemplateList.Add(settings);
            }
        }

        /// <summary>
        /// LocalFolderのテンプレートファイルをテンプレートリストに反映
        /// </summary>
        private async void GetTemplateList()
        {
            var templatesFolder = await GetLocalTemplatesFolder();
            var folderList = await templatesFolder.GetFoldersAsync();

            foreach (var folder in folderList)
            {
                // 設定ファイル→SettingsObj化
                try
                {
                    var settingFile = await folder.GetFileAsync(Const.FILE_SETTINGS);
                    var json = await FileIO.ReadTextAsync(settingFile);
                    var settings = Deserialize(json);
                    vm.TemplateList.Add(settings);
                }
                catch (FileNotFoundException e)
                {
                    Debug.WriteLine(e.FileName);
                }

                // サムネイル画像→SettingsObj化
                //var bitmap = new BitmapImage();
                //var thumbFile = await folder.GetFileAsync(Const.FILE_THUMBNAIL);
                //using (var stream = await thumbFile.OpenReadAsync())
                //{
                //    await bitmap.SetSourceAsync(stream);
                //}
                //settings.Thumbnail = bitmap;
            }
        }

        /// <summary>
        /// テンプレート選択（プリセット）
        /// </summary>
        private async void PresetTemplateList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var settings = presetTemplateList.SelectedItem as Settings;
            var installedFolder = Package.Current.InstalledLocation;
            var assetsFolder = await installedFolder.GetFolderAsync(Const.FOLDER_ASSETS);
            var templatesFolder = await assetsFolder.GetFolderAsync(Const.FOLDER_TEMPLATES);
            var templateFolder = await templatesFolder.GetFolderAsync(settings.Name);

            //Assetフォルダの背景画像を現在設定用にコピー（上書き）
            if (settings.BackgroundImageDisplay)
            {
                try
                {
                    var bgFile = await templateFolder.GetFileAsync(Const.FILE_BACKGROUND);

                    var localFolder = ApplicationData.Current.LocalFolder;
                    await bgFile.CopyAsync(localFolder, Const.FILE_BACKGROUND, NameCollisionOption.ReplaceExisting);
                }
                catch (Exception)
                {
                    Debug.WriteLine(settings.Name + ":背景画像のコピーに失敗");
                }
            }
            //前景画像も同様
            if (settings.ForegroundImageDisplay)
            {
                try
                {
                    var fgFile = await templateFolder.GetFileAsync(Const.FILE_FOREGROUND);

                    var localFolder = ApplicationData.Current.LocalFolder;
                    await fgFile.CopyAsync(localFolder, Const.FILE_FOREGROUND, NameCollisionOption.ReplaceExisting);
                }
                catch (Exception)
                {
                    Debug.WriteLine(settings.Name + ":前景画像のコピーに失敗");
                }
            }
            vm.ImportSettingsAsync(settings);

            presetTemplateList.SelectedItem = null;
        }

        /// <summary>
        /// テンプレートの選択（初回起動時）
        /// </summary>
        private async void SetInitSettings()
        {
            var installedFolder = Package.Current.InstalledLocation;
            var assetsFolder = await installedFolder.GetFolderAsync(Const.FOLDER_ASSETS);
            var templatesFolder = await assetsFolder.GetFolderAsync(Const.FOLDER_TEMPLATES);
            var defaultTemplateFolder = await templatesFolder.GetFolderAsync(Const.DEFAULT_TEMPLATE_NAME);
            var jsonFile = await defaultTemplateFolder.GetFileAsync(Const.FILE_SETTINGS);
            var json = await FileIO.ReadTextAsync(jsonFile);
            var settings = Deserialize(json);
            if (settings.BackgroundImageDisplay)
            {
                try
                {
                    //Assetフォルダの背景画像を現在設定用にコピー（上書き）
                    
                    var bgFile = await defaultTemplateFolder.GetFileAsync(Const.FILE_BACKGROUND);

                    var localFolder = ApplicationData.Current.LocalFolder;
                    await bgFile.CopyAsync(localFolder, Const.FILE_BACKGROUND, NameCollisionOption.ReplaceExisting);
                }
                catch (Exception)
                {
                    Debug.WriteLine(settings.Name + ":背景画像のコピーに失敗");
                }
            }
            vm.ImportSettingsAsync(settings);

            presetTemplateList.SelectedItem = null;
        }

        /// <summary>
        /// テンプレート選択
        /// </summary>
        private async void TemplateList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var settings = templateList.SelectedItem as Settings;
            if (settings == null) return; //deleteした後もこのイベントが走るのでここで抜ける

            var templatesFolder = await GetLocalTemplatesFolder();
            var templateFolder = await templatesFolder.GetFolderAsync(settings.Name);
            //テンプレートフォルダの背景画像を現在設定用にコピー（上書き）
            if (settings.BackgroundImageDisplay)
            {
                try
                {
                    var bgFile = await templateFolder.GetFileAsync(Const.FILE_BACKGROUND);

                    var localFolder = ApplicationData.Current.LocalFolder;
                    await bgFile.CopyAsync(localFolder, Const.FILE_BACKGROUND, NameCollisionOption.ReplaceExisting);
                }
                catch (Exception)
                {
                    Debug.WriteLine(settings.Name + ":背景/前景画像のコピーに失敗");
                }
            }
            //前景画像も同様
            if (settings.ForegroundImageDisplay)
            {
                try
                {
                    //テンプレートフォルダの背景画像を現在設定用にコピー（上書き）
                    var fgFile = await templateFolder.GetFileAsync(Const.FILE_FOREGROUND);

                    var localFolder = ApplicationData.Current.LocalFolder;
                    await fgFile.CopyAsync(localFolder, Const.FILE_FOREGROUND, NameCollisionOption.ReplaceExisting);
                }
                catch (Exception)
                {
                    Debug.WriteLine(settings.Name + ":背景/前景画像のコピーに失敗");
                }
            }
            vm.ImportSettingsAsync(settings);

            templateList.SelectedItem = null;

        }

        /// <summary>
        /// （イベント）テンプレート保存ボタン
        /// </summary>
        private async void SaveTemplateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var newTemplateName = inputNameTextBox.Text;

            if (newTemplateName == "") return; //テンプレート名は必須

            //禁止文字
            if (Regex.IsMatch(newTemplateName, "[\\\\\\/:\\*\\?\"<>\\|]"))
            {
                var loader = new ResourceLoader();
                var title = loader.GetString("dialogCannotSaveTemplate");
                var message = loader.GetString("dialogCannotSaveTemplateValidate");
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                return;
            }

            //同名のテンプレートが存在したらエラー出して終わり
            var templatesFolder = await GetLocalTemplatesFolder();
            var existFolder = await templatesFolder.TryGetItemAsync(newTemplateName);
            if (existFolder != null)
            {
                var loader = new ResourceLoader();
                var title = loader.GetString("dialogCannotSaveTemplate");
                var message = loader.GetString("dialogCannotSaveTemplateSameName");
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                return;
            }

            //現在の設定値をテンプレートリストに追加
            var settings = vm.ExportSettings(newTemplateName, inputAuthorTextBox.Text, inputDescriptionTextBox.Text);
            vm.TemplateList.Add(settings);

            //テンプレート用のフォルダを作って、
            var templateFolder = await templatesFolder.CreateFolderAsync(newTemplateName);

            ///Settings.jsonを保存
            var json = Serialize(settings);
            var localSettingsFile = await templateFolder.CreateFileAsync(Const.FILE_SETTINGS);
            await FileIO.WriteTextAsync(localSettingsFile, json);

            ///画像を保存
            if (settings.BackgroundImageDisplay)
            {
                try
                {
                    //現在設定をコピーする
                    var bgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Const.URI_CURRENT_BACKGROUND));
                    var bgFileCopied = await bgFile.CopyAsync(templateFolder, Const.FILE_BACKGROUND, NameCollisionOption.ReplaceExisting);
                }
                catch (FileNotFoundException ex)
                {
                    Debug.WriteLine(ex.FileName);
                }
            }
            if (settings.ForegroundImageDisplay)
            {
                try
                {
                    //現在設定をコピーする
                    var fgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Const.URI_CURRENT_FOREGROUND));
                    var fgFileCopied = await fgFile.CopyAsync(templateFolder, Const.FILE_FOREGROUND, NameCollisionOption.ReplaceExisting);
                }
                catch (FileNotFoundException ex)
                {
                    Debug.WriteLine(ex.FileName);
                }
            }

            inputNameTextBox.Text = "";
            inputAuthorTextBox.Text = "";
            inputDescriptionTextBox.Text = "";
        }

        /// <summary>
        /// （イベント）テンプレート削除ボタン
        /// </summary>
        private async void TemplateDeleteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var loader = new ResourceLoader();
            var title = loader.GetString("dialogDeleteTemplate");
            var message = loader.GetString("dialogDeleteTemplateReally");
            var primaryText = loader.GetString("dialogDelete");
            var closeText = loader.GetString("dialogCancel");
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = primaryText,
                CloseButtonText = closeText,
                DefaultButton = ContentDialogButton.Primary
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.None) return; //キャンセルなら抜ける


            //親のDataContext（＝settings）を拾う
            if (sender is Control ctl && ctl.DataContext is Settings settings)
            {
                //index指定でリストから削除
                templateList.SelectedItem = settings;
                var templateName = settings.Name;
                var list = (ObservableCollection<Settings>)templateList.ItemsSource;
                var index = list.IndexOf(settings);
                vm.TemplateList.RemoveAt(index);

                // Localのテンプレートフォルダを消す
                var templatesFolder = await GetLocalTemplatesFolder();
                var isExistFolder = await templatesFolder.TryGetItemAsync(templateName);
                if (isExistFolder == null)
                {
                    Debug.WriteLine(templateName + "フォルダが存在しない（想定外）");
                }
                else
                {
                    await isExistFolder.DeleteAsync();
                }
            }
        }
        
        /// <summary>
        /// テンプレートファイルをエクスポート
        /// </summary>
        private async void TemplateExportButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Control ctl && ctl.DataContext is Settings settings) //親のDataContext（＝settings）を拾う
            {
                // 対象のテンプレートフォルダ取得
                var templatesFolder = await GetLocalTemplatesFolder();
                var templateFolder = await templatesFolder.TryGetItemAsync(settings.Name);
                if (templateFolder == null) return; //テンプレートフォルダ存在しない→抜ける（想定外）

                // 名前を付けて保存ダイアログを表示
                var filePicker = new FileSavePicker();
                filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                filePicker.FileTypeChoices.Add("デザインテンプレートファイル", new List<string>() { Const.TEMPLATE_FILE_EXTENSION });
                filePicker.SuggestedFileName = settings.Name;
                var file = await filePicker.PickSaveFileAsync();
                if (file == null) return; // ファイル選択無し→抜ける

                // ※Pickerで選択したFileを直接ZipFileで操作することができないため、
                // ※一旦LocalDirectoryで作成した後にstreamでFileへ書き出す。
                
                // 既にtemp.zipがあれば消しておく（次のzip作成で落ちるため）
                var temporaryFile = await templatesFolder.TryGetItemAsync("temp.zip");
                if (temporaryFile != null) await temporaryFile.DeleteAsync();

                // テンプレートフォルダをtemp.zipに圧縮してLocalDirectoryに配置する
                var temporaryFilePath = templatesFolder.Path + "\\temp.zip";
                ZipFile.CreateFromDirectory(templateFolder.Path, temporaryFilePath);

                // LocalDirectoryに出来上がったzipを、Pickerで選択したファイルに置き換える
                using (Stream fromStream = File.OpenRead(temporaryFilePath))
                {
                    using (var stream = await file.OpenStreamForWriteAsync())
                    {
                        fromStream.CopyTo(stream);
                    }
                }
            }
        }

        /// <summary>
        /// テンプレートファイルをインポート
        /// </summary>
        private async void ImportTemplateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".chronocci");
            var file = await filePicker.PickSingleFileAsync();
            if (file == null) return; // ファイル選択無し→抜ける

            var templatesFolder = await GetLocalTemplatesFolder();
            var temporaryFile = await templatesFolder.TryGetItemAsync("temp.zip");
            if (temporaryFile == null) await templatesFolder.CreateFileAsync("temp.zip");

            // Pickerで選択したファイルをLocalDirectoryのzipに置き換える
            var temporaryFilePath = templatesFolder.Path + "\\temp.zip";
            using (Stream toStream = File.OpenWrite(temporaryFilePath))
            {
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    stream.CopyTo(toStream);
                }
            }

        }

        #endregion

        #region スタートアップの制御

        private async void SetupStartupToggle()
        {
            var startupTask = await StartupTask.GetAsync(Const.START_UP_TAST_ID);

            switch (startupTask.State)
            {
                case StartupTaskState.Disabled:
                    // トグル OFF、変更可能
                    startupToggle.IsOn = false;
                    startupToggle.IsEnabled = true;
                    break;
                case StartupTaskState.DisabledByUser:
                    // トグル OFF、変更不可
                    startupToggle.IsOn = false;
                    startupToggle.IsEnabled = false;
                    break;
                case StartupTaskState.DisabledByPolicy:
                    // トグル OFF、変更不可
                    startupToggle.IsOn = false;
                    startupToggle.IsEnabled = false;
                    break;
                case StartupTaskState.Enabled:
                    // トグル ON、変更可能
                    startupToggle.IsOn = true;
                    startupToggle.IsEnabled = true;
                    break;
                case StartupTaskState.EnabledByPolicy:
                    // トグル ON、変更不可
                    startupToggle.IsOn = true;
                    startupToggle.IsEnabled = false;
                    break;
                default:
                    startupToggle.IsOn = false;
                    startupToggle.IsEnabled = false;
                    break;
            }
        }

        // ToggleSwitchを切り替えたときのイベントハンドラー
        private async void StartupToggle_Toggled(object sender, RoutedEventArgs e)
        {
            var startupTask = await StartupTask.GetAsync(Const.START_UP_TAST_ID);

            if ((sender as ToggleSwitch).IsOn)
            {
                var state = await startupTask.RequestEnableAsync();
            }
            else
            {
                startupTask.Disable();
            }

            SetupStartupToggle();
        }

        #endregion

        #region Microsoftストア関連

        private StoreContext context = null;

        private void LicenseUpdateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GetAddOnInfo();
        }

        private void AddOnPanel_Loaded(object sender, RoutedEventArgs e)
        {
            GetAddOnInfo();
        }

        private void AddOnList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var addOn = addOnList.SelectedItem as StoreProduct;
            Purchase(addOn.StoreId);
            GetAddOnInfo();
        }

        //アプリ情報取得
        public async void GetAppInfo()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }
            StoreProductResult queryResult = await context.GetStoreProductForCurrentAppAsync();

            if (queryResult.Product == null)
            {
                // The Store catalog returned an unexpected result.
                Debug.WriteLine("Something went wrong, and the product was not returned.");

                // Show additional error info if it is available.
                if (queryResult.ExtendedError != null)
                {
                    Debug.WriteLine(queryResult.ExtendedError.Message);
                }
                return;
            }
        }

        //アドオン情報取得
        public async void GetAddOnInfo()
        {
            licenseUpdateProgress.IsActive = true;

            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            //これはオフラインだとエラーになる
            string[] productKinds = { "Durable", "Consumable", "UnmanagedConsumable" };
            var filterList = new List<string>(productKinds);
            var queryResult = await context.GetAssociatedStoreProductsAsync(filterList);

            if (queryResult.ExtendedError != null)
            {
                Debug.WriteLine($"ExtendedError: {queryResult.ExtendedError.Message}");
                var loader = new ResourceLoader();
                var title = loader.GetString("dialogError");
                var message = loader.GetString("dialogRetrieveAddOnError");
                //var message = queryResult.ExtendedError.Message;
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                licenseUpdateProgress.IsActive = false;
                return;
            }

            vm.LicensedAddOnList.Clear();
            vm.AddOnList.Clear();

            foreach (KeyValuePair<string, StoreProduct> item in queryResult.Products)
            {
                var product = item.Value;
                Debug.WriteLine(product.StoreId + " : " + product.Title + " - " + product.IsInUserCollection);

                if (product.IsInUserCollection)
                {
                    vm.LicensedAddOnList.Add(product);
                }
                else
                {
                    vm.AddOnList.Add(product);
                }
            }

            licenseUpdateProgress.IsActive = false;
        }

        //アドオン情報取得（購入済みのみ）
        public async void GetAddOnInfoPurchased()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
                // If your app is a desktop app that uses the Desktop Bridge, you
                // may need additional code to configure the StoreContext object.
                // For more info, see https://aka.ms/storecontext-for-desktop.
            }

            // Specify the kinds of add-ons to retrieve.
            string[] productKinds = { "Durable" };
            List<String> filterList = new List<string>(productKinds);

            //workingProgressRing.IsActive = true;
            StoreProductQueryResult queryResult = await context.GetUserCollectionAsync(filterList);
            //workingProgressRing.IsActive = false;

            if (queryResult.ExtendedError != null)
            {
                // The user may be offline or there might be some other server failure.
                Debug.WriteLine($"ExtendedError: {queryResult.ExtendedError.Message}");
                return;
            }

            Debug.WriteLine("購入済:");
            foreach (KeyValuePair<string, StoreProduct> item in queryResult.Products)
            {
                StoreProduct product = item.Value;
                Debug.WriteLine(" - " + product.Title + "/" + product.Description);
                // Use members of the product object to access info for the product...
            }
        }
        
        //アドオン購入
        private async void Purchase(string storeId)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }
            var result = await context.RequestPurchaseAsync(storeId);

            var loader = new ResourceLoader();
            var title = string.Empty;
            var message = string.Empty;
            var messageEx = string.Empty;
            if (result.ExtendedError != null)
            {
                messageEx = "\n" + result.ExtendedError.Message;
            }

            switch (result.Status)
            {
                case StorePurchaseStatus.AlreadyPurchased:
                    title = loader.GetString("dialogError");
                    message = loader.GetString("dialogPurchasedAlready");
                    break;

                case StorePurchaseStatus.Succeeded:
                    message = loader.GetString("dialogPurchasedSucceeded");
                    break;

                case StorePurchaseStatus.NotPurchased:
                    title = loader.GetString("dialogCancel");
                    message = loader.GetString("dialogPurchasedCancel") + messageEx;
                    break;

                case StorePurchaseStatus.NetworkError:
                    title = loader.GetString("dialogError");
                    message = loader.GetString("dialogPurchasedNetworkError") + messageEx;
                    break;

                case StorePurchaseStatus.ServerError:
                    title = loader.GetString("dialogError");
                    message = loader.GetString("dialogPurchasedServerError") + messageEx;
                    break;

                default:
                    title = loader.GetString("dialogError");
                    message = loader.GetString("dialogPurchasedUnknownError") + messageEx;
                    break;
            }

            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK"
            };
            await dialog.ShowAsync();
        }

        //ライセンス情報取得
        public async void GetLicenseInfo()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            //これはオフラインでもＯＫ
            var appLicense = await context.GetAppLicenseAsync();

            if (appLicense == null)
            {
                var loader = new ResourceLoader();
                var title = loader.GetString("dialogError");
                var message = loader.GetString("dialogRetrieveLicenseError");
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                return;
            }

            foreach (KeyValuePair<string, StoreLicense> item in appLicense.AddOnLicenses)
            {
                var addOnLicense = item.Value;
                vm.SetLicense(addOnLicense.SkuStoreId);
            }
        }

        #endregion

        #region 雑多なprivateメソッド

        /// <summary>
        /// テンプレートフォルダを開く（存在してなかったら作る）
        /// </summary>
        private async Task<StorageFolder> GetLocalTemplatesFolder()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var isExistFolder = await localFolder.TryGetItemAsync(Const.FOLDER_TEMPLATES);
            if (isExistFolder == null)
            {
                await localFolder.CreateFolderAsync(Const.FOLDER_TEMPLATES);
            }
            return await localFolder.GetFolderAsync(Const.FOLDER_TEMPLATES);
        }


        private async void DebugLocalFolder()
        {
            var localfolders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();
            foreach (StorageFolder folder in localfolders)
            {
                Debug.WriteLine("◇" + folder.Name);
                var localfolders2 = await folder.GetFoldersAsync();
                foreach (StorageFolder folder2 in localfolders2)
                {
                    Debug.WriteLine("├◇" + folder2.Name);

                    var localfiles3 = await folder2.GetFilesAsync();
                    foreach (StorageFile file3 in localfiles3)
                    {
                        Debug.WriteLine("　├・" + file3.Name);
                    }
                }
                var localfiles2 = await folder.GetFilesAsync();
                foreach (StorageFile file2 in localfiles2)
                {
                    Debug.WriteLine("├・" + file2.Name);
                }
            }

            var localfiles = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            foreach (StorageFile file in localfiles)
            {
                Debug.WriteLine("・" + file.Name);
            }


            //Debug.WriteLine("★AssetsFolder");
            //StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //StorageFolder assets = await appInstalledFolder.GetFolderAsync("Assets");
            //var assetsfiles = await assets.GetFoldersAsync();
            //for (int i = 0; i < assetsfiles.Count; i++)
            //{
            //    // do something with the name of each file
            //    Debug.WriteLine(assetsfiles[i].Name);
            //}
        }


        #endregion

        private void PositionResetLink_Tapped(object sender, TappedRoutedEventArgs e)
        {
            vm.DesignConfig.BackgroundImageCoordinateX = 0;
            vm.DesignConfig.BackgroundImageCoordinateY = 0;
        }

        //private void BackgroundImage_ImageOpened(object sender, RoutedEventArgs e)
        //{
        //    backgroundImage.Height = backgroundImage.ActualHeight;
        //    backgroundImage.Width = backgroundImage.ActualWidth;
        //    Debug.WriteLine(backgroundImage.ActualHeight);
        //    Debug.WriteLine(backgroundImage.ActualWidth);
        //}
    }
}
