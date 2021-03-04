using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LeagueAPI_Classes
{
    public static class ExtensionMethods_Global
    {
        public static string ToJson(this object obj, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        public static string GetJsonFromKey(this JObject obj, string key, Formatting formatting = Formatting.Indented)
        {
            IEnumerable<KeyValuePair<string, JToken>> items = obj.Value<IEnumerable<KeyValuePair<string, JToken>>>(key);
            return JsonConvert.SerializeObject(items, formatting);
        }

        public static T ToObject<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
