using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace iBuki
{
    public static class Const
    {
        public static readonly List<string> LANGUAGE_LIST = new List<string>{
            "en-US",
            "jp-JP"
        };

        public static string APP_VERSION
        {
            get
            {
                var package = Package.Current;
                var packageId = package.Id;
                var version = packageId.Version;
                return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }

        public static string APP_NAME
        {
            get
            {
                var package = Package.Current;
                var name = package.DisplayName;
                return name;
            }
        }

        public static string APP_AUTHOR
        {
            get
            {
                var package = Package.Current;
                var author = package.PublisherDisplayName;
                return author;
            }
        }

        public static Uri APP_LOGO
        {
            get
            {
                var package = Package.Current;
                var packageId = package.Id;
                var logo = package.Logo;
                return logo;
            }
        }

        //◆InstalledFolder(ms-appx:///)
        //┗◇Assets
        //　┣◇Templates
        //　┃┗◇[Template Name]
        //　┃　┣・Setings.json
        //　┃　┣・Background.png　＊無いかも
        //　┃　┗・Thumbnail.png
        //　┗◇MoonPhase
        //　　┗・MoonPhaseBackground.png
        //◆LocalFolder(ms-appdata:///)
        //┣・Background.png　←現在の背景
        //┣・MoonPhaseBackground.png　←現在のムーンフェイズの背景
        //┣◇Templates　※無いかも
        //┃┗◇[Template Name]
        //┃　┣・Setings.json
        //┃　┣・Background.png　＊無いかも
        //┃　┗・Thumbnail.png　＊無い TODO:自動サムネイル保存
        //┗☆Temporary

        public static readonly string KEY_CURRENT_SETTINGS = "CurrentSettings"; //現在の設定値はLoclSettingsにjson文字列で持つ。その項目キー
        public static readonly string URI_CURRENT_BACKGROUND = "ms-appdata:///local/Background.png";
        public static readonly string URI_CURRENT_FOREGROUND = "ms-appdata:///local/Foreground.png";
        public static readonly string URI_CURRENT_MOONPHASE_BACKGROUND = "ms-appdata:///local/MoonPhaseBackground.png";
        public static readonly string URI_CURRENT_MOONPHASE_FOREGROUND = "ms-appdata:///local/MoonPhaseForeground.png";
        public static readonly string URI_CURRENT_SUBDIAL_SECOND_BACKGROUND = "ms-appdata:///local/SubDialSecondBackground.png";
        public static readonly string URI_CURRENT_SUBDIAL_SECOND_HAND = "ms-appdata:///local/SubDialSecondHand.png";
        public static readonly string URI_CURRENT_SUBDIAL_30M_BACKGROUND = "ms-appdata:///local/SubDial30mBackground.png";
        public static readonly string URI_CURRENT_SUBDIAL_30M_HAND = "ms-appdata:///local/SubDial30mHand.png";
        public static readonly string URI_CURRENT_SUBDIAL_12H_BACKGROUND = "ms-appdata:///local/SubDial12hBackground.png";
        public static readonly string URI_CURRENT_SUBDIAL_12H_HAND = "ms-appdata:///local/SubDial12hHand.png";
        public static readonly string URI_ASSETS_MOONPHASE = "ms-appx:///Assets/MoonPhase/";
        public static readonly string URI_ASSETS_SUBDIAL = "ms-appx:///Assets/SubDial/";

        public static readonly string FOLDER_ASSETS = "Assets";
        public static readonly string FOLDER_TEMPLATES = "Templates";
        public static readonly string FOLDER_TEMPORARY = "Temporary";
        public static readonly string FILE_SETTINGS = "Settings.json";
        public static readonly string FILE_BACKGROUND = "Background.png";
        public static readonly string FILE_FOREGROUND = "Foreground.png";
        public static readonly string FILE_THUMBNAIL = "Thumbnail.png";
        public static readonly string FILE_MOONPHASE_BACKGROUND = "MoonPhaseBackground.png";
        public static readonly string FILE_MOONPHASE_FOREGROUND = "MoonPhaseForeground.png";
        public static readonly string FILE_SUBDIAL_SECOND_BACKGROUND = "SubDialSecondBackground.png";
        public static readonly string FILE_SUBDIAL_SECOND_HAND = "SubDialSecondHand.png";
        public static readonly string FILE_SUBDIAL_30M_BACKGROUND = "SubDial30mBackground.png";
        public static readonly string FILE_SUBDIAL_30M_HAND = "SubDial30mHand.png";
        public static readonly string FILE_SUBDIAL_12H_BACKGROUND = "SubDial12hBackground.png";
        public static readonly string FILE_SUBDIAL_12H_HAND = "SubDial12hHand.png";

        public static readonly string DEFAULT_TEMPLATE_NAME = "Chronocci";

        public static readonly string START_UP_TAST_ID = "ChronocciStartupId";
        public static readonly string TEMPLATE_FILE_EXTENSION = ".chronocci";

        public static readonly string VALIDATE_REGEX_FILENAME = "[\\\\\\/:\\*\\?\"<>\\|]";
        public static string TEMPLATE_FILE_EXTENSION_DISCRIPTION
        {
            get
            {
                var loader = new ResourceLoader();
                return loader.GetString("templateFileExtension");
            }
        }
 
        public const string STORE_ID_DAYDATE      = "9PC9BV184X42";
        public const string STORE_ID_MOONPHASE    = "9N2670BTRV8R";
        public const string STORE_ID_POWERRESERVE = "9N28RLNQFZPB";
        public const string STORE_ID_CHRONOGRAPH  = "9NRN57BWR5N0";

        public static Visibility VISIBLE_ON_DEBUG
        {
#if DEBUG
            get { return Visibility.Visible; }
#else
            get { return Visibility.Collapsed; }
#endif
        }

    }
}
