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
    public sealed partial class CircleIndex : UserControl
    {
        public IndexType Type
        {
            get { return (IndexType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

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

        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }
        public new double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        public new string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", // プロパティ名を指定
            typeof(IndexType), // プロパティの型を指定
            typeof(CircleIndex), // プロパティを所有する型を指定
            new PropertyMetadata(IndexType.Arabic,
                (d, e) => { (d as CircleIndex).OnPropertyChanged(e); })
            );

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", // プロパティ名を指定
            typeof(Color), // プロパティの型を指定
            typeof(CircleIndex), // プロパティを所有する型を指定
            new PropertyMetadata(Color.FromArgb(255, 90, 117, 153),
                (d, e) => { (d as CircleIndex).OnFontPropertyChanged(e); })
            );

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius",
            typeof(double),
            typeof(CircleIndex),
            new PropertyMetadata((double)42,
                (d, e) => { (d as CircleIndex).OnPropertyChanged(e); })
            );

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
            "Length",
            typeof(double),
            typeof(CircleIndex),
            new PropertyMetadata((double)8,
                (d, e) => { (d as CircleIndex).OnPropertyChanged(e); })
            );

        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize",
            typeof(double),
            typeof(CircleIndex),
            new PropertyMetadata((double)10,
                (d, e) => { (d as CircleIndex).OnFontPropertyChanged(e); })
            );

        public new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(
            "FontFamily",
            typeof(string),
            typeof(CircleIndex),
            new PropertyMetadata("Century Gothic",
                (d, e) => { (d as CircleIndex).OnFontPropertyChanged(e); })
            );

        public CircleIndex()
        {
            InitializeComponent();
            Draw();
        }

        readonly string[] ARABIC = { "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "1", "2" };
        readonly string[] ROMAN = { "III", "IIII", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "I", "II" };

        private void OnFontPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            for (int i = 0; i < 12; i++)
            {
                var container = (Border)VisualTreeHelper.GetChild(canvas, i);
                var text = (TextBlock)VisualTreeHelper.GetChild(container, 0);

                text.FontSize = FontSize;
                text.FontFamily = new FontFamily(FontFamily);
                text.Foreground = new SolidColorBrush(Color);
            }
        }

        private void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            var index = Type == IndexType.Arabic ?
               ARABIC :
               ROMAN ;

            double deg = 360.0 / index.Length;
            double cx = 50;
            double cy = 50;

            //rcos45 = 100 * 1 / √ 2 = 70.71、
            //rsin45 = 100 * 1 / √ 2 = 70.71、

            for (int i = 0; i < index.Length; i++)
            {
                var container = (Border)VisualTreeHelper.GetChild(canvas, i);
                var text = (TextBlock)VisualTreeHelper.GetChild(container, 0);

                text.Text = index[i];
                text.FontSize = FontSize;
                text.FontFamily = new FontFamily(FontFamily);
                text.Foreground = new SolidColorBrush(Color);

                var cxAdj = container.Width / 2;
                var cyAdj = container.Height / 2;
                double x = cx - cxAdj + Radius * Math.Cos(deg * i / 180.0 * Math.PI);
                double y = cy - cyAdj + Radius * Math.Sin(deg * i / 180.0 * Math.PI);
                Debug.WriteLine(deg * i + ": " + cxAdj + "," + cyAdj);

                Canvas.SetLeft(container, x);
                Canvas.SetTop(container, y);

                if(Type is IndexType.Roman)
                {
                    container.RenderTransform = new CompositeTransform { Rotation = deg * i + 90 };
                }
                else
                {
                    container.RenderTransform = new CompositeTransform { Rotation = 0 };
                }

            }
        }
    }
}
