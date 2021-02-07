using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Music
{
    public enum ListTypes
    {
        Unknown,
        TopTenUKSingles,
        TopTenUSSingles,
        TopTenUKandUSSingles
    }

    public static class JsonHelper
    {
        public static Dictionary<ListTypes, string> ListTypesAndJsonNames = new Dictionary<ListTypes, string>(
            new List<KeyValuePair<ListTypes, string>>() {
                new KeyValuePair<ListTypes, string>(ListTypes.Unknown, ListTypes.Unknown.ToString()),
                new KeyValuePair<ListTypes, string>(ListTypes.TopTenUKSingles, ListTypes.TopTenUKSingles.ToString()),
                new KeyValuePair<ListTypes, string>(ListTypes.TopTenUSSingles, ListTypes.TopTenUSSingles.ToString()),
                new KeyValuePair<ListTypes, string>(ListTypes.TopTenUKandUSSingles, ListTypes.TopTenUKandUSSingles.ToString()),
            }
        );

        public static Dictionary<int, List<WikipediaSong>> ReadJsonFile_Dict(ListTypes listType)
        {
            return ReadJsonFile<Dictionary<int, List<WikipediaSong>>>(listType);
        }
        
        public static List<WikipediaSong> ReadJsonFile_List(ListTypes listType)
        {
            return ReadJsonFile<List<WikipediaSong>>(listType);
        }

        private static T ReadJsonFile<T>(ListTypes listType)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(GetNameForJson(listType)));
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        private static string GetNameForJson(ListTypes listType)
        {
            return $"{ListTypesAndJsonNames[listType]}.json";
        }

        public static void WriteJsonFile(this string json, ListTypes listType)
        {
            try
            {
                File.WriteAllText(GetNameForJson(listType), json);
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
