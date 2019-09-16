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
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M242,5 L240,300 260,300 258,5");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 0;
                                break;
                            case Clock.Minute:
                                margin = 40;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 17;
                                Height = 250;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M242,5 L240,300 260,300 258,5");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 0;
                                break;
                            case Clock.Second:
                                margin = 70;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 6;
                                Height = 230;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M242,5 L240,300 260,300 258,5");
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
                                Width = 35;
                                Height = 231;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M300,-55 L280,230 300,300 320,230 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 0;
                                break;
                            case Clock.Second:
                                margin = 58;
                                Margin = new Thickness(0, margin, 0, 0);
                                Width = 10;
                                Height = 260;
                                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M159.25256,110.00066 L158.25434,111.00056 157.25519,263.98578 156.25604,268.98557 155.25662,314.9812 C155.25689,316.98045 163.25007,316.98062 163.24981,314.98122 L162.25093,268.98557 161.25178,263.98578 160.25263,111.00056 z");
                                CenterX = Width / 2;
                                CenterY = Height - ((Height + margin) - 250);
                                Pivot = 20;
                                break;
                        }
                        break;
                    case HandsType.Breguet:
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
