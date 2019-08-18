using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iBuki
{
    class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private AppConfig _appConfig = new AppConfig();
        public AppConfig AppConfig
        {
            get => _appConfig;
            set
            {
                if (value == _appConfig) return;
                _appConfig = value;
                OnPropertyChanged(); 

            }
        }

        private DesignConfig _designConfig = new DesignConfig();
        public DesignConfig DesignConfig
        {
            get => _designConfig;
            set
            {
                if (value == _designConfig) return;
                _designConfig = value;
                OnPropertyChanged();
            }
        }

        #region Dial

        public bool IsScaleDisplay
        {
            get => DesignConfig.IsScaleDisplay;
            set
            {
                if (value == DesignConfig.IsScaleDisplay) return;
                {
                    DesignConfig.IsScaleDisplay = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsAlterScaleEnable");
                }
            }
        }

        public bool IsAlterScale
        {
            get => DesignConfig.IsAlterScale;
            set
            {
                if (value == DesignConfig.IsAlterScale) return;
                {
                    DesignConfig.IsAlterScale = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsAlterScaleEnable");
                }
            }
        }

        public bool IsAlterScaleEnable
        {
            get
            {
                if (DesignConfig.IsScaleDisplay && DesignConfig.IsAlterScale) return true;
                return false;
            }
        }


        public List<string> IndexTypeList = EnumExtension.GetLocalizeList<IndexType>();
        public IndexType IndexType
        {
            get => DesignConfig.IndexType;
            set
            {
                if (value == DesignConfig.IndexType) return;
                {
                    DesignConfig.IndexType = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IndexBarVisible");
                    OnPropertyChanged("IndexVisible");
                }
            }
        }

        public bool IsIndexDisplay
        {
            get => DesignConfig.IsIndexDisplay;
            set
            {
                if (value == DesignConfig.IsIndexDisplay) return;
                {
                    DesignConfig.IsIndexDisplay = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IndexBarVisible");
                    OnPropertyChanged("IndexVisible");
                }
            }
        }

        public Visibility IndexBarVisible
        {
            get
            {
                if (DesignConfig.IsIndexDisplay && IndexType == IndexType.Bar) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public Visibility IndexVisible
        {
            get
            {
                if (DesignConfig.IsIndexDisplay && IndexType != IndexType.Bar) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        
        #endregion


        #region Hands

        public List<string> HandsTypeList = EnumExtension.GetLocalizeList<HandsType>();

        public HandsType HandsType
        {
            get => DesignConfig.HandsType;
            set
            {
                if (value == DesignConfig.HandsType) return;
                {
                    DesignConfig.HandsType = value;
                    HourHand.Type = value;
                    MinuteHand.Type = value;
                    SecondHand.Type = value;
                    OnPropertyChanged();
                }
            }
        }

        private HandModel _hourHand = new HandModel(Clock.Hour);
        public HandModel HourHand
        {
            get => _hourHand;
            set
            {
                if (value == _hourHand) return;
                _hourHand = value;
                OnPropertyChanged();
            }
        }

        private HandModel _minuteHand = new HandModel(Clock.Minute);
        public HandModel MinuteHand
        {
            get => _minuteHand;
            set
            {
                if (value == _minuteHand) return;
                _minuteHand = value;
                OnPropertyChanged();
            }
        }

        private HandModel _secondHand = new HandModel(Clock.Second);
        public HandModel SecondHand
        {
            get => _secondHand;
            set
            {
                if (value == _secondHand) return;
                _secondHand = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Date

        public string[] FontList = CanvasTextFormat.GetSystemFontFamilies();

        private Thickness _dateCoordinate = new Thickness(300,300,0,0);
        public Thickness DateCoordinate
        {
            get => _dateCoordinate;
            set
            {
                if (value == _dateCoordinate) return;
                _dateCoordinate = value;
                OnPropertyChanged();
            }
        }

        private double _dateCoordinateX = 300;
        public double DateCoordinateX
        {
            get { return _dateCoordinateX; }
            set
            {
                if (value == _dateCoordinateX) return;
                DesignConfig.DateCoordinateX = value;
                var Y = DateCoordinate.Top;
                var coordinate = new Thickness(value, Y, 0, 0);
                DateCoordinate = coordinate;
                _dateCoordinateX = value;
                OnPropertyChanged();
            }
        }

        private double _dateCoordinateY = 300;
        public double DateCoordinateY
        {
            get { return _dateCoordinateY; }
            set
            {
                if (value == _dateCoordinateY) return;
                DesignConfig.DateCoordinateY = value;
                var X = DateCoordinate.Left;
                var coordinate = new Thickness(X, value, 0, 0);
                DateCoordinate = coordinate;
                _dateCoordinateY = value;
                OnPropertyChanged();
            }
        }

        #endregion

    }
}
