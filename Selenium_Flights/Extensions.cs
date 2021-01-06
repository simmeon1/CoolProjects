using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium_Flights
{
    static class Extensions
    {
        public static string ToJson(this object obj, Formatting formatting = Formatting.Indented)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, formatting, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                return $"Object could not be serialized due to the following reason: {ex.Message}";
            }
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
