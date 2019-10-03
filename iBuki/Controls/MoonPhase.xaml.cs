using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace iBuki
{
    public sealed partial class MoonPhase : UserControl
    {

        public DateTimeOffset DateTime
        {
            get { return (DateTimeOffset)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register(
            "DateTime",
            typeof(DateTimeOffset),
            typeof(MoonPhase),
            new PropertyMetadata(DateTimeOffset.Now,
                (d, e) => { (d as MoonPhase).OnPropertyChanged(e); })
            );

        public MoonPhase()
        {
            InitializeComponent();
            background.Source = new BitmapImage(new Uri(Const.URI_ASSETS_MOONPHASE + Const.FILE_MOONPHASE_BACKGROUND));
            var moonAge = GetMoonAge(DateTime);
            angle.Rotation = moonAge * 6;
        }

        private void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {

            var moonAge = GetMoonAge(DateTime);
            angle.Rotation = moonAge * 6;
        }


        /// <summary>
        /// 簡易月齢計算法により月齢を求める
        /// 堀源一郎「おに・おに・にし-簡易月齢計算法」（『天文月報』1968年7月号、日本天文学会、1968年）。
        /// </summary>
        private double GetMoonAge(DateTimeOffset datetime)
        {
            double[] c = { 0, 0, 2, 0, 2, 2, 4, 5, 6, 7, 8, 9, 10 };
            var age = (((datetime.Year - 11) % 19) * 11 + c[datetime.Month] + datetime.Day) % 30;
            return age;
        }


        //　ユリウス日から直前の新月のユリウス日を差し引く計算の方が正確らしい
    }
}
