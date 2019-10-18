using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace iBuki
{
    [DataContract]
    public class Settings { 

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// ISO 8601 Format(2019-09-22T00:00Z)
        /// </summary>
        [DataMember]
        public string CreatedAt { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string TargetAppVersion { get; set; }

        [DataMember]
        public string BackgroundColor { get; set; }

        [DataMember]
        public bool BackgroundImageDisplay { get; set; }

        [DataMember]
        public string BackgroundImageStretch { get; set; }

        [DataMember]
        public double BackgroundImageCoordinateX { get; set; }

        [DataMember]
        public double BackgroundImageCoordinateY { get; set; }

        [DataMember]
        public bool ScaleDisplay { get; set; }

        [DataMember]
        public string ScaleColor { get; set; }

        [DataMember]
        public int ScaleCount { get; set; }

        [DataMember]
        public double ScaleRadius { get; set; }

        [DataMember]
        public double ScaleLength { get; set; }

        [DataMember]
        public double ScaleThickness { get; set; }

        [DataMember]
        public bool AlterScale { get; set; }

        [DataMember]
        public int AlterScaleInterval { get; set; }

        [DataMember]
        public double AlterScaleRadius { get; set; }

        [DataMember]
        public double AlterScaleLength { get; set; }

        [DataMember]
        public double AlterScaleThickness { get; set; }

        [DataMember]
        public bool IndexDisplay { get; set; }

        [DataMember]
        public string IndexType { get; set; }

        [DataMember]
        public string IndexColor { get; set; }

        [DataMember]
        public double IndexRadius { get; set; }

        [DataMember]
        public double IndexLength { get; set; }

        [DataMember]
        public double IndexThickness { get; set; }

        [DataMember]
        public int indexInterval { get; set; }

        [DataMember]
        public string IndexFontFamily { get; set; }

        [DataMember]
        public double IndexFontSize { get; set; }

        [DataMember]
        public string HandsType { get; set; }

        [DataMember]
        public bool HandsDisplay { get; set; }

        [DataMember]
        public string HandsColor { get; set; }

        [DataMember]
        public string HandsStrokeColor { get; set; }

        [DataMember]
        public double HandsStrokeThickness { get; set; }

        [DataMember]
        public bool SecondHandDisplay { get; set; }

        [DataMember]
        public string SecondHandColor { get; set; }

        [DataMember]
        public bool DateDisplay { get; set; }

        [DataMember]
        public string DateBackgroundColor { get; set; }

        [DataMember]
        public double DateCoordinateX { get; set; }

        [DataMember]
        public double DateCoordinateY { get; set; }

        [DataMember]
        public double DateWidth { get; set; }

        [DataMember]
        public double DateHeight { get; set; }

        [DataMember]
        public string DateBorderColor { get; set; }

        [DataMember]
        public double DateBorderThickness { get; set; }

        [DataMember]
        public string DateFontColor { get; set; }

        [DataMember]
        public string DateFormat { get; set; }

        [DataMember]
        public string DateFontFamily { get; set; }

        [DataMember]
        public double DateFontSize { get; set; }

        [DataMember]
        public bool MoonPhaseDisplay { get; set; }

        [DataMember]
        public double MoonPhaseSize { get; set; }

        [DataMember]
        public double MoonPhaseCoordinateX { get; set; }

        [DataMember]
        public double MoonPhaseCoordinateY { get; set; }

        [DataMember]
        public bool MoonPhaseBackgroundImageDisplay { get; set; }

        [DataMember]
        public string MoonPhaseForegroundColor { get; set; }

        [DataMember]
        public bool MoonPhaseForegroundImageDisplay { get; set; }

        public BitmapSource Thumbnail { get; set; }

        public string CreatedAtDateFormat { get
            {
                if (CreatedAt == null) return "";
                var datetime = DateTime.Parse(CreatedAt);
                return datetime.ToString("yyyy-MM-dd");
            }
        }
    }
}
