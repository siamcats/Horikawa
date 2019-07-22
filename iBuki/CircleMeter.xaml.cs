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

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace iBuki
{
    public sealed partial class CircleMeter : UserControl
    {

        public static readonly DependencyProperty CircleColorProperty =
            DependencyProperty.Register(
                "CircleColor", // プロパティ名を指定
                typeof(Color), // プロパティの型を指定
                typeof(CircleMeter), // プロパティを所有する型を指定
                new PropertyMetadata(Color.FromArgb(255, 90, 117, 153),
                    (d, e) => { (d as CircleMeter).OnCircleColorPropertyChanged(e); }));

        public Color CircleColor
        {
            get { return (Color)GetValue(CircleColorProperty); }
            set { SetValue(CircleColorProperty, value); }
        }



        public CircleMeter()
        {
            this.InitializeComponent();

            double cx = 50.0;
            double cy = 50.0;
            double r = 45.0; //半径
            int cnt = 240; //線の数
            double deg = 360.0 / (double)cnt;
            double degS = deg * 0.8; //間隔
            for (int i = 0; i < cnt; ++i)
            {
                var si1 = Math.Sin((270.0 - (double)i * deg) / 180.0 * Math.PI);
                var co1 = Math.Cos((270.0 - (double)i * deg) / 180.0 * Math.PI);
                var si2 = Math.Sin((270.0 - (double)(i + 1) * deg + degS) / 180.0 * Math.PI);
                var co2 = Math.Cos((270.0 - (double)(i + 1) * deg + degS) / 180.0 * Math.PI);
                var x1 = r * co1 + cx;
                var y1 = r * si1 + cy;
                var x2 = r * co2 + cx;
                var y2 = r * si2 + cy;

                var pathStr = string.Format("M {0},{1} A {2},{2} 0 0 0 {3},{4}", x1, y1, r, x2, y2);
                Path path = XamlReader.Load($"<Path xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><Path.Data>{pathStr}</Path.Data></Path>") as Path;
                path.Stroke = new SolidColorBrush(Color.FromArgb(CircleColor.A, CircleColor.R, CircleColor.G, CircleColor.B));
                if (i % 4 == 0)
                {
                    path.StrokeThickness = 4.0;
                }
                else
                {
                    path.StrokeThickness = 2.0;
                }
                mainCanvas.Children.Add(path);
            }
        }

        public void OnCircleColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (null == mainCanvas) return;
            if (null == mainCanvas.Children) return;

            foreach (var child in mainCanvas.Children)
            {
                var shp = child as Shape;
                var sb = shp.Stroke as SolidColorBrush;
                var a = sb.Color.A;
                shp.Stroke = new SolidColorBrush(Color.FromArgb(CircleColor.A, CircleColor.R, CircleColor.G, CircleColor.B));
            }
        }
    }
}
