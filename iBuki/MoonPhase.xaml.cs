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

        public double Size
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Size",
            typeof(double),
            typeof(MoonPhase),
            new PropertyMetadata((double)100,
                (d, e) => { (d as MoonPhase).OnPropertyChanged(e); })
            );

        public MoonPhase()
        {
            InitializeComponent();

            background.Source = new BitmapImage(new Uri(Const.URI_ASSETS_MOONPHASE + Const.FILE_MOONPHASE_BACKGROUND));
            foreground.Source = new BitmapImage(new Uri(Const.URI_ASSETS_MOONPHASE + Const.FILE_MOONPHASE_FOREGROUND));
        }

        private void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {

        }

    }
}
