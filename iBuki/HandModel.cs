using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace iBuki
{
    /// <summary>
    /// 針タイプで可変となる設定項目はここで定義する
    /// </summary>
    public class HandModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Clock _clock;

        public HandModel(Clock clock)
        {
            _clock = clock;
        }

        private HandsType _type = HandsType.Bar;
        public HandsType Type
        {
            get => _type;
            set
            {
                //if (value == _type) return;
                _type = value;
                double margin;
                switch (_type)
                {
                    case HandsType.Bar:
                        switch (_clock)
                        {
                            case Clock.Hour:
                                margin = 100;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 18;
                                Height = 190;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M242,5 L240,300 260,300 258,5 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 0;
                                break;
                            case Clock.Minute:
                                margin = 40;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 17;
                                Height = 250;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M242,5 L240,300 260,300 258,5 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 0;
                                break;
                            case Clock.Second:
                                margin = 70;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 6;
                                Height = 230;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M242,5 L240,300 260,300 258,5 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 12;
                                break;
                        }
                        break;
                    case HandsType.Leaf:
                        switch (_clock)
                        {
                            case Clock.Hour:
                                margin = 120;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 9;
                                Height = 130;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M259.42356,3.5819738 C254.42137,3.5819738 252.54609,145.50599 259.42384,208.58333 266.30133,145.50599 264.4254,3.5819738 259.42356,3.5819738 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 19;
                                break;
                            case Clock.Minute:
                                margin = 50;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 8;
                                Height = 200;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M259.42356,3.5819738 C254.42137,3.5819738 254.42206,147.08292 259.42384,208.58333 264.42529,147.08292 264.4254,3.5819738 259.42356,3.5819738 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 18;
                                break;
                            case Clock.Second:
                                margin = 40;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 2;
                                Height = 250;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M240,45 C239.34443,45 239.43768,211.66667 239.15652,295 L240.8435,295 C240.56233,211.66667 240.65557,45 240,45 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 16;
                                break;
                        }
                        break;
                    case HandsType.Dolphin:
                        switch (_clock)
                        {
                            case Clock.Hour:
                                margin =132;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 35;
                                Height = 168;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M300,-60 L280,200 300,300 320,200 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 0;
                                break;
                            case Clock.Minute:
                                margin = 69;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 32;
                                Height = 231;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M300,-55 L280,230 300,300 320,230 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 0;
                                break;
                            case Clock.Second:
                                margin = 35;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 10;
                                Height = 280;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M159.25256,110.00066 L158.25434,111.00056 157.25519,263.98578 156.25604,268.98557 155.25662,314.9812 C155.25689,316.98045 163.25007,316.98062 163.24981,314.98122 L162.25093,268.98557 161.25178,263.98578 160.25263,111.00056 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 20;
                                break;
                        }
                        break;
                    case HandsType.Breguet:
                        switch (_clock)
                        {
                            case Clock.Hour:
                                margin = 120;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 28;
                                Height = 130;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M13.022,20.3 C7.223011,20.299999 2.5220008,25.001009 2.5220011,30.799998 2.5220008,36.598989 7.223011,41.299999 13.022,41.299999 18.820991,41.299999 23.522001,36.598989 23.522001,30.799998 23.522001,25.001009 18.820991,20.299999 13.022,20.3 z M13.000003,0 L15.75652,19.295632 16.248902,19.409274 C21.856449,20.852031 26.000001,25.942126 26.000001,31.999999 26.000001,38.73097 20.884504,44.267153 14.329177,44.932882 L14.03747,44.955063 16.999994,121 8.999999,121 11.963054,44.955103 11.670825,44.932882 C5.1154974,44.267153 7.1525574E-07,38.73097 0,31.999999 7.1525574E-07,25.942126 4.143553,20.852031 9.7510991,19.409274 L10.243482,19.295632 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 20;
                                break;
                            case Clock.Minute:
                                margin = 62;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 23;
                                Height = 188;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M10.499999,41 C6.3578634,41 2.999999,44.357864 2.9999986,48.500001 2.999999,52.642136 6.3578634,56 10.499999,55.999999 14.642135,56 17.999999,52.642136 17.999999,48.500001 17.999999,44.357864 14.642135,41 10.499999,41 z M10.499999,0 L10.999999,1 12.964248,39.302837 13.622378,39.47206 C17.896599,40.801479 21.000001,44.788321 20.999999,49.5 21.000001,54.755334 17.139112,59.10891 12.099045,59.879017 L12.015606,59.88962 13.999997,173 7.000001,173 8.9843917,59.88962 8.9009528,59.879017 C3.8608867,59.10891 -1.9073486E-06,54.755334 0,49.5 -1.9073486E-06,44.788321 3.1033998,40.801479 7.3776197,39.47206 L8.0357499,39.302837 9.9999995,1 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 15;
                                break;
                            case Clock.Second:
                                margin = 60;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 14;
                                Height = 251;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M7.0003641,222 C4.2392919,222 2.0010006,224.23858 2.0010007,227 2.0010006,229.76142 4.2392919,232 7.0003641,232 9.7614367,232 11.999727,229.76142 11.999727,227 11.999727,224.23858 9.7614367,222 7.0003641,222 z M6.9999993,0 L7.9999998,1 7.9999998,219.07955 8.4105814,219.14222 C11.600299,219.79494 13.999727,222.61726 13.999728,226 13.999727,229.86599 10.865781,233 6.9998639,233 3.1339464,233 1.1920929E-06,229.86599 0,226 1.1920929E-06,222.61726 2.399428,219.79494 5.5891469,219.14222 L5.999999,219.07951 5.999999,1 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 10;
                                break;
                        }
                        break;
                    default:
                        break;
                }
                OnPropertyChanged();
            }
        }

        private Thickness _margin;
        public Thickness Margin
        {
            get => _margin;
            set
            {
                if (value == _margin) return;
                _margin = value;
                OnPropertyChanged();
            }
        }

        private double _width;
        public double Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        private Geometry _data;
        public Geometry Data
        {
            get => _data;
            set
            {
                if (value == _data) return;
                _data = value;
                OnPropertyChanged();
            }
        }

        private double _centerX;
        public double CenterX
        {
            get => _centerX;
            set
            {
                if (value == _centerX) return;
                _centerX = value;
                OnPropertyChanged();
            }
        }

        private double _centerY;
        public double CenterY
        {
            get => _centerY;
            set
            {
                if (value == _centerY) return;
                _centerY = value;
                OnPropertyChanged();
            }
        }

        private double _pivot;
        public double Pivot
        {
            get => _pivot;
            set
            {
                if (value == _pivot) return;
                _pivot = value;
                OnPropertyChanged();
            }
        }
    }
}
