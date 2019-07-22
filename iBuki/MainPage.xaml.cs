using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace iBuki
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public AppConfig AppConfig { get; set; } = new AppConfig();

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(500, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            DataContext = AppConfig;

            Debug.WriteLine("run");
        }


        private DispatcherTimer _timer;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this._timer = new DispatcherTimer();

            // タイマーイベントの間隔を指定。
            // ここでは1秒おきに実行する
            this._timer.Interval = TimeSpan.FromSeconds(0.125);

            this._timer.Tick += _timer_Tick;

            this._timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            // カウントを1加算
            DateTime localDate = DateTime.Now;

            // TextBlockにカウントを表示
            textBlock.Text = localDate.ToString("hh:mm:ss.fff");

            hourHandAngle.Angle = CalcAngleHour(localDate);
            minuteHandAngle.Angle = CalcAngleMinute(localDate);
            secondHandAngle.Angle = CalcAngleSecond(localDate);

            Debug.WriteLine(AppConfig.Movement.ToString());
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
            var angle = mm * 360 / 60 + ss * 360 / 60 / 60 ;
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

        private async void ImagePicker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var filePicker = new Windows.Storage.Pickers.FileOpenPicker();

            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.FileTypeFilter.Add("*");

            // 単一ファイルの選択
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                //var dlg = new MessageDialog(file.Name);
                //await dlg.ShowAsync();

                var bitmap = new BitmapImage();
                using (var stream = await file.OpenReadAsync())
                {
                    await bitmap.SetSourceAsync(stream);
                }
                bgImage.Source = bitmap;
            }
        }
    }
}
