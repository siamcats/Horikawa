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
        public bool IsAlterScale
        {
            get { return (bool)GetValue(IsAlterScaleProperty); }
            set { SetValue(IsAlterScaleProperty, value); }
        }

        public int AlterInterval
        {
            get { return (int)GetValue(AlterIntervalProperty); }
            set { SetValue(AlterIntervalProperty, value); }
        }
        public double AlterRadius
        {
            get { return (double)GetValue(AlterRadiusProperty); }
            set { SetValue(AlterRadiusProperty, value); }
        }

        public double AlterThickness
        {
            get { return (double)GetValue(AlterThicknessProperty); }
            set { SetValue(AlterThicknessProperty, value); }
        }

        public double AlterLength
        {
            get { return (double)GetValue(AlterLengthProperty); }
            set { SetValue(AlterLengthProperty, value); }
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

        public static readonly DependencyProperty IsAlterScaleProperty = DependencyProperty.Register(
            "IsAlterScale",
            typeof(bool),
            typeof(CircleMeter),
            new PropertyMetadata(true,
                (d, e) => { (d as CircleMeter).OnPropertyChanged(e); })
            );

        public static readonly DependencyProperty AlterIntervalProperty = DependencyProperty.Register(
            "AlterInterval",
            typeof(int),
            typeof(CircleMeter),
            new PropertyMetadata((int)60,
                (d, e) => { (d as CircleMeter).OnPropertyChanged(e); })
            );

        public static readonly DependencyProperty AlterRadiusProperty = DependencyProperty.Register(
            "AlterRadius",
            typeof(double),
            typeof(CircleMeter),
            new PropertyMetadata((double)42,
                (d, e) => { (d as CircleMeter).OnPropertyChanged(e); })
            );

        public static readonly DependencyProperty AlterThicknessProperty = DependencyProperty.Register(
            "AlterThickness",
            typeof(double),
            typeof(CircleMeter),
            new PropertyMetadata((double)2,
                (d, e) => { (d as CircleMeter).OnPropertyChanged(e); })
            );

        public static readonly DependencyProperty AlterLengthProperty = DependencyProperty.Register(
            "AlterLength",
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
            double deg = 360.0 / Count;

            double c = 2 * Radius * Math.PI;
            double padding = (Count * Thickness < c) ?    //実線部
                Count * Thickness :
                c;
            double margin = (c - Count * Thickness > 0) ? //空白部
                c - Count * Thickness :
                0;
            double ratio = margin / (padding + margin);   //空白部が占める割合
            double degS = deg * ratio;
            double degAdj = Thickness / 2;
            //double degS = deg * 0.9; //間隔
            //Debug.WriteLine("   c " + c + " | padding " + padding + " | margin " + margin + " | deg " + degS + " | degAdj" + degAdj + " | radius" + Radius);
            //Debug.WriteLine(1 - (Count * Thickness / (c - Count * Thickness)));
            //Debug.WriteLine(ratio);

            for (int i = 0; i < Count; ++i)
            {
                if (IsAlterScale == false || AlterInterval == 0 || i % AlterInterval != 0)
                {
                    var si1 = Math.Sin((270.0 - (double)i * deg + degAdj) / 180.0 * Math.PI);
                    var co1 = Math.Cos((270.0 - (double)i * deg + degAdj) / 180.0 * Math.PI);
                    var si2 = Math.Sin((270.0 - (double)(i + 1) * deg + degS + degAdj) / 180.0 * Math.PI);
                    var co2 = Math.Cos((270.0 - (double)(i + 1) * deg + degS + degAdj) / 180.0 * Math.PI);
                    var x1 = Radius * co1 + cx;
                    var y1 = Radius * si1 + cy;
                    var x2 = Radius * co2 + cx;
                    var y2 = Radius * si2 + cy;
                    var pathStr = string.Format("M {0},{1} A {2},{2} 0 0 0 {3},{4}", x1, y1, Radius, x2, y2);
                    var path = new Path
                    {
                        Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), pathStr),
                        Stroke = new SolidColorBrush(Color.FromArgb(Color.A, Color.R, Color.G, Color.B)),
                    };
                    path.StrokeThickness = Length;
                    mainCanvas.Children.Add(path);
                }
            }

            //一部の線だけ変える
            if (IsAlterScale && AlterInterval != 0)
            {
                double altC = 2 * AlterRadius * Math.PI;
                double altPadding = (Count * AlterThickness < altC) ?    //実線部
                    Count * AlterThickness :
                    altC;
                double altMargin = (altC - Count * AlterThickness > 0) ? //空白部
                    altC - Count * AlterThickness :
                    0;
                double altRatio = altMargin / (altPadding + altMargin);  //空白部が占める割合
                double altDegS = deg * altRatio;
                double altDegAdj = AlterThickness / 2;
                //Debug.WriteLine("altc " + altC + " | padding " + altPadding + " | margin " + altMargin + " | deg " + altDegS + " | degAdj " + altDegAdj + " | radius" + AlterRadius);

                for (int i = 0; i < Count; ++i)
                {
                    if (i % AlterInterval == 0)
                    {
                        var si1 = Math.Sin((270.0 - (double)i * deg + altDegAdj) / 180.0 * Math.PI);
                        var co1 = Math.Cos((270.0 - (double)i * deg + altDegAdj) / 180.0 * Math.PI);
                        var si2 = Math.Sin((270.0 - (double)(i + 1) * deg + altDegS + altDegAdj) / 180.0 * Math.PI);
                        var co2 = Math.Cos((270.0 - (double)(i + 1) * deg + altDegS + altDegAdj) / 180.0 * Math.PI);
                        var x1 = AlterRadius * co1 + cx;
                        var y1 = AlterRadius * si1 + cy;
                        var x2 = AlterRadius * co2 + cx;
                        var y2 = AlterRadius * si2 + cy;
                        var pathStr = string.Format("M {0},{1} A {2},{2} 0 0 0 {3},{4}", x1, y1, AlterRadius, x2, y2);
                        var path = new Path
                        {
                            Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), pathStr),
                            Stroke = new SolidColorBrush(Color.FromArgb(Color.A, Color.R, Color.G, Color.B)),
                        };
                        path.StrokeThickness = AlterLength;
                        mainCanvas.Children.Add(path);
                    }
                }
            }


        }
    }
}
