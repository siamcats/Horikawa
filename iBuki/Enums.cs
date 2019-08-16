using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;

namespace iBuki
{
    public enum Movement
    {
        Quartz,
        Mechanical
    }
    
    public enum Clock
    {
        Hour,Minute,Second
    }

    public enum HandsType
    {
        [LocalizeName("EnumHandBar")]
        Bar,
        [LocalizeName("EnumHandLeaf")]
        Leaf,
        [LocalizeName("EnumHandDolphin")]
        Dolphin,
        [LocalizeName("EnumHandBreguet")]
        Breguet
    }

    public enum ScaleType
    {
        PerSecond
    }

    public enum IndexType
    {
        [LocalizeName("EnumIndexBar")]
        Bar,
        [LocalizeName("EnumIndexArabic")]
        Arabic,
        [LocalizeName("EnumIndexRomen")]
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

    public class EnumSourceProvider<T> : MarkupExtension
    {
        private static string DisplayName(T value)
        {
            var fileInfo = value.GetType().GetField(value.ToString());
            var descriptionAttribute = (DescriptionAttribute)fileInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault();
            return descriptionAttribute.Description;
        }

        public IEnumerable Source { get; }
            = typeof(T).GetEnumValues()
                .Cast<T>()
                .Select(value => new { Code = value, Name = DisplayName(value) });

        //public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
