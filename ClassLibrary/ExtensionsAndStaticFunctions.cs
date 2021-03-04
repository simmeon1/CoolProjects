using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public static class ExtensionsAndStaticFunctions
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

        public static T ToObject<T>(this string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static T CloneObject<T>(this T inputObj)
        {
            if (inputObj == null) return default(T);
            //Dim settings As JsonSerializerSettings = New JsonSerializerSettings With {.TypeNameHandling = TypeNameHandling.All}
            T outputObj = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(inputObj));
            return outputObj;

        }

        public static bool MatchesRegex(this string str, string pattern, RegexOptions regexOptions = RegexOptions.None)
        {
            return Regex.IsMatch(str, pattern, regexOptions);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string GetDateTimeNowStringForFileName()
        {
            DateTime dateTimeNow = DateTime.Now;
            string dateTimeNowStr = dateTimeNow.ToString("s", CultureInfo.CreateSpecificCulture("en-US"));
            dateTimeNowStr = dateTimeNowStr.Replace(':', '-');
            return dateTimeNowStr;
        }
    }
}
