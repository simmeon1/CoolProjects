using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MusicClasses
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
        public static Dictionary<ListTypes, string> ListTypesAndJsonNames = GetListTypesAndJsonNames();

        private static Dictionary<ListTypes, string> GetListTypesAndJsonNames()
        {
            Dictionary<ListTypes, string> dict = new Dictionary<ListTypes, string>();
            dict.Add(ListTypes.Unknown, ListTypes.Unknown.ToString());
            dict.Add(ListTypes.TopTenUKSingles, ListTypes.TopTenUKSingles.ToString());
            dict.Add(ListTypes.TopTenUSSingles, ListTypes.TopTenUSSingles.ToString());
            dict.Add(ListTypes.TopTenUKandUSSingles, ListTypes.TopTenUKandUSSingles.ToString());
            return dict;
        }

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
