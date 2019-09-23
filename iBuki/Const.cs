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

        public static string GetAppVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
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

        public static readonly string KEY_CURRENT_SETTINGS = "CurrentSettings";

        public static readonly List<string> TEMPLATE_LIST = new List<string>()
        {
            "Default","Station","Portofino"
        };

        public static readonly string URI_ASSETS = "ms-appx:///Assets/Themes/";
        public static readonly string URI_LOCAL = "ms-appdata:///local/";

        public static readonly string FOLDER_ASSETS = "Assets";
        public static readonly string FOLDER_THEMES = "Themes";
        public static readonly string FILE_SETTINGS = "Settings.json";
        public static readonly string FILE_THUMBNAIL = "Thumbnail.png";
        public static readonly string FILE_BACKGROUND = "Background.png";
    }
}
