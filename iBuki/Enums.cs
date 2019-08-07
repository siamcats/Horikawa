using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBuki
{
    public enum Movement
    {
        Quartz,
        Mechanical
    }

    public enum Hands
    {
        [LocalizeName("HandBar")]
        Bar,
        [LocalizeName("HandLeaf")]
        Leaf,
        [LocalizeName("HandDolphin")]
        Dolphin,
        [LocalizeName("HandBreguet")]
        Breguet
    }

    public enum Scale
    {
        PerSecond
    }

    public enum Index
    {
        [LocalizeName("IndexBar")]
        Bar,
        [LocalizeName("IndexArabic")]
        Arabic,
        [LocalizeName("IndexRomen")]
        Roman
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class LocalizeNameAttribute : Attribute
    {
        public string Name { get; set; }

        public LocalizeNameAttribute(string name)
        {
            Name = name;
        }
    }

    public static class EnumExtension
    {
        public static string GetLocalizeName<T>(this T enumValue)
        {
            var field = typeof(T).GetField(enumValue.ToString());
            var attrType = typeof(LocalizeNameAttribute);
            var attribute = Attribute.GetCustomAttribute(field, attrType);
            var key = (attribute as LocalizeNameAttribute)?.Name;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            return resourceLoader.GetString(key);
            //return resourceLoader.GetString(enumValue.ToString());
        }

        public static List<string> GetLocalizeList<T>()
        {
            var type = typeof(T);
            var values = Enum.GetValues(type).Cast<T>().ToList();
            var count = values.Count;
            var list = new List<string>();
            for (int i = 0; i < count; ++i)
            {
                var value = values[i];
                var str = value.GetLocalizeName();
                list.Add(str);
            }
            return list;
        }
    }
}
