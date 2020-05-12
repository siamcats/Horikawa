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
    public sealed partial class SubDial : UserControl
    {

        private HandModel hm = new HandModel(Clock.Second);

        public DateTimeOffset DateTime
        {
            get { return (DateTimeOffset)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register(
            "DateTime",
            typeof(DateTimeOffset),
            typeof(SubDial),
            new PropertyMetadata(DateTimeOffset.Now,
                (d, e) => { (d as SubDial).OnPropertyChanged(e); })
            );

        public TimeSpan TimeSpan
        {
            get { return (TimeSpan)GetValue(TimeSpanProperty); }
            set { SetValue(TimeSpanProperty, value); }
        }

        public static readonly DependencyProperty TimeSpanProperty = DependencyProperty.Register(
            "DateTime",
            typeof(TimeSpan),
            typeof(SubDial),
            new PropertyMetadata(TimeSpan.Zero,
                (d, e) => { (d as SubDial).OnPropertyChanged(e); })
            );

        public Movement Movement
        {
            get { return (Movement)GetValue(MovementProperty); }
            set { SetValue(MovementProperty, value); }
        }

        public static readonly DependencyProperty MovementProperty = DependencyProperty.Register(
            "Movement",
            typeof(Movement),
            typeof(SubDial),
            new PropertyMetadata(Movement.Mechanical,
                (d, e) => { (d as SubDial).OnPropertyChanged(e); })
            );
        public SubDialType Type
        {
            get { return (SubDialType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type",
            typeof(SubDialType),
            typeof(SubDial),
            new PropertyMetadata(SubDialType.Seconds,
                (d, e) => { (d as SubDial).OnPropertyChanged(e); })
            );

        public bool IsBackgroundImageDisplay
        {
            get { return (bool)GetValue(IsBackgroundImageDisplayProperty); }
            set { SetValue(IsBackgroundImageDisplayProperty, value); }
        }

        public static readonly DependencyProperty IsBackgroundImageDisplayProperty = DependencyProperty.Register(
            "IsBackgroundImageDisplay",
            typeof(bool),
            typeof(SubDial),
            new PropertyMetadata(false,
                (d, e) => { (d as SubDial).OnImagePropertyChanged(e); })
            );

        public BitmapImage BackgroundImage
        {
            get { return (BitmapImage)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        public static readonly DependencyProperty BackgroundImageProperty = DependencyProperty.Register(
            "BackgroundImage",
            typeof(BitmapImage),
            typeof(SubDial),
            new PropertyMetadata(new BitmapImage(),
                (d, e) => { (d as SubDial).OnImagePropertyChanged(e); })
            );

        public bool IsHandImageDisplay
        {
            get { return (bool)GetValue(IsHandImageDisplayProperty); }
            set { SetValue(IsHandImageDisplayProperty, value); }
        }

        public static readonly DependencyProperty IsHandImageDisplayProperty = DependencyProperty.Register(
            "IsHandImageDisplay",
            typeof(bool),
            typeof(SubDial),
            new PropertyMetadata(false,
                (d, e) => { (d as SubDial).OnImagePropertyChanged(e); })
            );

        public BitmapImage HandImage
        {
            get { return (BitmapImage)GetValue(HandImageProperty); }
            set { SetValue(HandImageProperty, value); }
        }

        public static readonly DependencyProperty HandImageProperty = DependencyProperty.Register(
            "HandImage",
            typeof(BitmapImage),
            typeof(SubDial),
            new PropertyMetadata(new BitmapImage(),
                (d, e) => { (d as SubDial).OnImagePropertyChanged(e); })
            );

        public SubDial()
        {
            InitializeComponent();

            background.Source = IsBackgroundImageDisplay
                ? BackgroundImage
                : Type == SubDialType.Seconds
                    ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_SECOND_BACKGROUND))
                    : Type == SubDialType.Totalizer30Minuts
                        ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_30M_BACKGROUND))
                        : Type == SubDialType.Totalizer12Hours
                            ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_SECOND_BACKGROUND))
                            : BackgroundImage;

            hand.Source = IsHandImageDisplay
                ? HandImage
                : Type == SubDialType.Seconds
                    ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_SECOND_HAND))
                    : Type == SubDialType.Totalizer30Minuts
                        ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_30M_HAND))
                        : Type == SubDialType.Totalizer12Hours
                            ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_SECOND_HAND))
                            : HandImage;
        }

        private void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if(Type == SubDialType.Seconds)
            {
                angle.Rotation = CalcAngleSecond();
            }
            else if(Type == SubDialType.Totalizer30Minuts)
            {
                angle.Rotation = CalcAngle30M();
            }
            else
            {
                angle.Rotation = CalcAngle12H();
            }
        }

        private void OnImagePropertyChanged(DependencyPropertyChangedEventArgs e)
        {

            background.Source = IsBackgroundImageDisplay
                ? BackgroundImage
                : Type == SubDialType.Seconds
                    ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_SECOND_BACKGROUND))
                    : Type == SubDialType.Totalizer30Minuts
                        ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_30M_BACKGROUND))
                        : Type == SubDialType.Totalizer12Hours
                            ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_12H_BACKGROUND))
                            : BackgroundImage;

            hand.Source = IsHandImageDisplay
                ? HandImage
                : Type == SubDialType.Seconds
                    ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_SECOND_HAND))
                    : Type == SubDialType.Totalizer30Minuts
                        ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_30M_HAND))
                        : Type == SubDialType.Totalizer12Hours
                            ? new BitmapImage(new Uri(Const.URI_ASSETS_SUBDIAL + Const.FILE_SUBDIAL_12H_HAND))
                            : HandImage;
        }

        private double CalcAngleSecond()
        {
            decimal ss = DateTime.Second;
            decimal fff = DateTime.Millisecond;
            decimal angle = Movement == Movement.Quartz
                ? 6 * ss
                : 6 * ss + fff * 6 / 1000;
            //Debug.WriteLine(angle);
            return decimal.ToDouble(angle);
        }

        private double CalcAngle30M()
        {
            decimal mm = TimeSpan.Minutes;
            decimal ss = TimeSpan.Seconds;
            decimal fff = TimeSpan.Milliseconds;
            decimal angle = Movement == Movement.Quartz
                ? 12 * mm
                : ss < 59
                    ? 12 * mm
                    : 12 * mm + fff * 12 / 1000;
            //Debug.WriteLine(mm + ":" + ss + ":" + fff + "|" + angle);
            return decimal.ToDouble(angle);
        }

        private double CalcAngle12H()
        {
            decimal hh = TimeSpan.Hours;
            decimal mm = TimeSpan.Minutes;
            decimal angle = Movement == Movement.Quartz
                ? 30 * hh
                : mm < 59
                    ? 30 * hh
                    : 30 * hh + mm * 30 / 60;
            //Debug.WriteLine(mm + ":" + ss + ":" + fff + "|" + angle);
            return decimal.ToDouble(angle);
        }
    }
}
