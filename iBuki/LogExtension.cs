﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iBuki
{
    static class LogExtension
    {
        [Conditional("DEBUG")]
        public static void DebugLog<T>(this T obj, string name)
        {
            try
            {
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                Debug.WriteLine("{0} = {1}", name, json);
            }
            catch
            {
                Debug.WriteLine("{0} : Debug Log Error!", name);
            }
        }
    }

}
