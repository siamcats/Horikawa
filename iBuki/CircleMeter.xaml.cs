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
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }
        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }
        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", // プロパティ名を指定
            typeof(Color), // プロパティの型を指定
            typeof(CircleMeter), // プロパティを所有する型を指定
            new PropertyMetadata(Color.FromArgb(255, 90, 117, 153),
                (d, e) => { (d as CircleMeter).OnColorPropertyChanged(e); })
            );

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
            "Count", 
            typeof(int),
            typeof(CircleMeter),
            new PropertyMetadata(12,
                (d, e) => { (d as CircleMeter).OnPropertyChanged(e); })
            );

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius",
            typeof(double),
            typeof(CircleMeter),
            new PropertyMetadata((double)42,
                (d, e) => { (d as CircleMeter).OnPropertyChanged(e); })
            );

        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
            "Thickness",
            typeof(double),
            typeof(CircleMeter),
            new PropertyMetadata((double)1,
                (d, e) => { (d as CircleMeter).OnPropertyChanged(e); })
            );

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
            "Length",
            typeof(double),
            typeof(CircleMeter),
            new PropertyMetadata((double)8,
                (d, e) => { (d as CircleMeter).OnPropertyChanged(e); })
            );

        public CircleMeter()
        {
            InitializeComponent();

            Draw();
        }

        private void OnColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (null == mainCanvas) return;
            if (null == mainCanvas.Children) return;

            foreach (var child in mainCanvas.Children)
            {
                var shp = child as Shape;
                var sb = shp.Stroke as SolidColorBrush;
                var a = sb.Color.A;
                shp.Stroke = new SolidColorBrush(Color.FromArgb(Color.A, Color.R, Color.G, Color.B));
            }
        }

        private void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            mainCanvas.Children.Clear();
            Draw();
        }

        private void Draw()
        {
            double cx = 50.0;
            double cy = 50.0;
            double c = 2 * Radius * Math.PI;
            double deg = 360.0 / Count;
            double degS = deg * (1 - (Count * Thickness / (c - Count * Thickness))); //間隔
            //double degS = deg * 0.9; //間隔
            for (int i = 0; i < Count; ++i)
            {
                var si1 = Math.Sin((270.0 - (double)i * deg) / 180.0 * Math.PI);
                var co1 = Math.Cos((270.0 - (double)i * deg) / 180.0 * Math.PI);
                var si2 = Math.Sin((270.0 - (double)(i + 1) * deg + degS) / 180.0 * Math.PI);
                var co2 = Math.Cos((270.0 - (double)(i + 1) * deg + degS) / 180.0 * Math.PI);
                var x1 = Radius * co1 + cx;
                var y1 = Radius * si1 + cy;
                var x2 = Radius * co2 + cx;
                var y2 = Radius * si2 + cy;

                var pathStr = string.Format("M {0},{1} A {2},{2} 0 0 0 {3},{4}", x1, y1, Radius, x2, y2);
                Path path = XamlReader.Load($"<Path xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><Path.Data>{pathStr}</Path.Data></Path>") as Path;
                path.Stroke = new SolidColorBrush(Color.FromArgb(Color.A, Color.R, Color.G, Color.B));
                path.StrokeThickness = Length;
                //if (i % 4 == 0)
                //{
                //    path.StrokeThickness = 4.0;
                //}
                //else
                //{
                //    path.StrokeThickness = 2.0;
                //}
                mainCanvas.Children.Add(path);
            }
            transform.Rotation = Thickness;
        }
    }
}
