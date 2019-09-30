using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

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

        public static string GetAppName()
        {
            var package = Package.Current;
            var name = package.DisplayName;
            return name;
        }

        public static string GetAppAuthor()
        {
            var package = Package.Current;
            var author = package.PublisherDisplayName;
            return author;
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

        //◆InstalledFolder
        //┗◇Assets
        //　┗◇Templates
        //　　┗◇[Template Name]
        //　　　┣・Setings.json
        //　　　┣・Background.png　＊無いかも
        //　　　┗・Thumbnail.png
        //◆LocalFolder
        //┣・Background.png　←現在の背景
        //┗◇Templates　※無いかも
        //　┗◇[Template Name]
        //　　┣・Setings.json
        //　　┣・Background.png　＊無いかも
        //　　┗・Thumbnail.png　＊無い TODO:自動サムネイル保存

        public static readonly string KEY_CURRENT_SETTINGS = "CurrentSettings"; //現在の設定値はLoclSettingsにjson文字列で持つ。その項目キー
        public static readonly string URI_CURRENT_BACKGROUND = "ms-appdata:///local/Background.png";
        public static readonly string URI_ASSETS_MOONPHASE = "ms-appx:///Assets/MoonPhase/";

        public static readonly string FOLDER_ASSETS = "Assets";
        public static readonly string FOLDER_TEMPLATES = "Templates";
        public static readonly string FILE_SETTINGS = "Settings.json";
        public static readonly string FILE_BACKGROUND = "Background.png";
        public static readonly string FILE_THUMBNAIL = "Thumbnail.png";
        public static readonly string FILE_MOONPHASE_BACKGROUND = "MoonPhaseBackground.png";

        public static readonly string StartUpTaskId = "ChronocciStartupId";

    }
}
