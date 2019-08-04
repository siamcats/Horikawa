using System;
using System.Runtime.Serialization;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace iBuki
{
    [DataContract]
    class Settings
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string TargetAppVersion { get; set; }

        [DataMember]
        public Hands HandsType { get; set; }

        [DataMember]
        public string HandsColor { get; set; }

        [DataMember]
        public bool IsDateDisplay { get; set; }

        [DataMember]
        public string DateDisplayFormat { get; set; }
        
        public SolidColorBrush GetBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, r, g, b));
            return brush;
        }
    }
}
