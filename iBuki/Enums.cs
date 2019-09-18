﻿using System;
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
        Mechanical,
        Chronograph
    }
    
    public enum Clock
    {
        Hour,Minute,Second
    }

    public enum HandsType
    {
        [LocalizeName("barHandEnum")]
        Bar,
        [LocalizeName("leafHandEnum")]
        Leaf,
        [LocalizeName("dolphinHandEnum")]
        Dolphin,
        [LocalizeName("breguetHandEnum")]
        Breguet
    }

    public enum IndexType
    {
        [LocalizeName("barIndexEnum")]
        Bar,
        [LocalizeName("arabicIndexEnum")]
        Arabic,
        [LocalizeName("romanIndexEnum")]
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
            if (string.IsNullOrEmpty(key)) return null; //LocalizeNameの定義が無ければnull文字を返却
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            return resourceLoader.GetString(key);
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
