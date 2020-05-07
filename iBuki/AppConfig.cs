using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using System.Runtime.CompilerServices;
using Windows.UI.ViewManagement;
using Windows.Globalization;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace iBuki
{
    /// <summary>
    /// LocalSettingsに保存して保持する情報
    /// </summary>
    public class AppConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ApplicationDataContainer DataContainer { get; set; }

        public AppConfig()
        {
            DataContainer = ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// LocalSetting保存
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="key">キーは指定不要、呼出し元のプロパティ名を使う</param>
        private void Save(object value, [CallerMemberName]string key = null)
        {
            //Debug.WriteLine("Save - " + key + " : " + value);
            DataContainer.Values[key] = value;
        }

        /// <summary>
        /// LocalSetting取得
        /// </summary>
        /// <param name="defaultValue">キーが存在しない場合の返却値</param>
        /// <param name="key">キーは指定不要、呼出し元のプロパティ名を使う</param>
        private T Load<T>(T defaultValue, [CallerMemberName]string key = null)
        {
            if (DataContainer.Values.ContainsKey(key))
            {
                //Debug.WriteLine("Load - " + key + " : " + DataContainer.Values[key].ToString());
                return (T)DataContainer.Values[key];
            }
            //Debug.WriteLine("Cannot Load - " + key);
            // キーがなければ指定したデフォルト値を返す
            if (null != defaultValue)
            {
                return defaultValue;
            }
            return default(T);
        }

        private double _windowSize = 500;
        public double WindowSize
        {
            get { return _windowSize; }
            set
            {
                if (value == _windowSize) return;
                _windowSize = value;
                OnPropertyChanged();
            }
        }

        public ElementTheme SystemTheme
        {
            get { return (ElementTheme)Load((int)ElementTheme.Dark); }
            set
            {
                if ((int)value == Load((int)ElementTheme.Dark)) return;
                Save((int)value);
                OnPropertyChanged();
            }
        }

        public Color AccentColor
        {
            get
            {
                var str = Load("#FFFFFFFF");
                return ConvertHexColor(str);
            }
            set
            {
                var str = "#" + value.A.ToString("X2") + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2");
                if (str == Load("#FFFFFFFF")) return;
                Save(str);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 16進数カラーコード文字列をBrushに変換
        /// </summary>
        private Color ConvertHexColor(string hexCode)
        {

            if(string.IsNullOrEmpty(hexCode)||!hexCode.Contains("#")) return Color.FromArgb(255, 255, 255, 255);
            hexCode = hexCode.Replace("#", string.Empty);
            if (hexCode.Length != 8) return Color.FromArgb(255, 255, 255, 255);
            byte a = (byte)(Convert.ToUInt32(hexCode.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hexCode.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hexCode.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hexCode.Substring(6, 2), 16));
            return Color.FromArgb(a, r, g, b);
        }

        public Movement Movement
        {
            get { return (Movement)Load((int)Movement.Quartz); }
            set
            {
                if ((int)value == Load((int)Movement.Quartz)) return;
                Save((int)value);
                OnPropertyChanged();
            }
        }

        public bool IsTopMost
        {
            get { return Load(false); }
            set
            {
                if (value == Load(false)) return;
                Save(value);
                OnPropertyChanged();
            }
        }

        public string Language
        {
            get { return Load("en-US"); }
            set
            {
                if (value == Load("en-US")) return;
                Save(value);
                OnPropertyChanged();
            }
        }

        public bool IsLicensedDayDate
        {
            get { return Load(false); }
            set
            {
                if (value == Load(false)) return;
                Save(value);
                OnPropertyChanged();
            }
        }

        public bool IsLicensedMoonPhase
        {
            get { return Load(false); }
            set
            {
                if (value == Load(false)) return;
                Save(value);
                OnPropertyChanged();
            }
        }

        public bool IsLicensedPowerReserve
        {
            get { return Load(false); }
            set
            {
                if (value == Load(false)) return;
                Save(value);
                OnPropertyChanged();
            }
        }

        public bool IsLicensedChronograph
        {
            get { return Load(false); }
            set
            {
                if (value == Load(false)) return;
                Save(value);
                OnPropertyChanged();
            }
        }
    }
}
