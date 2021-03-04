using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueAPI_Classes 
{
    public static class ExtensionMethods_Enums
    {
        public static bool ItemIsMoreThan2000Gold(this ItemEnum item)
        {
            HashSet<int> expensiveItems = new HashSet<int>() { 1400, 1401, 1402, 1412, 1413, 1414, 1416, 1419, 2065, 3001, 3003, 3004, 3007, 3008, 3022, 3025, 3026, 3027, 3029, 3030, 3031, 3033, 3036, 3040, 3042, 3043, 3046, 3048, 3050, 3053, 3065, 3068, 3071, 3072, 3074, 3075, 3078, 3083, 3084, 3085, 3087, 3089, 3091, 3094, 3095, 3100, 3102, 3107, 3109, 3110, 3115, 3116, 3124, 3135, 3137, 3139, 3142, 3143, 3146, 3147, 3151, 3152, 3153, 3156, 3157, 3161, 3165, 3174, 3179, 3181, 3190, 3193, 3194, 3197, 3198, 3222, 3285, 3371, 3373, 3374, 3379, 3380, 3382, 3383, 3384, 3386, 3387, 3388, 3389, 3390, 3504, 3508, 3742, 3748, 3800, 3812, 3814, 3905, 3907 };
            return expensiveItems.Contains((int)item);
        }
        public static bool RuneIsKeystone(this RuneEnum rune)
        {
            HashSet<int> keystones = new HashSet<int>() { 8005, 8008, 8021, 8010, 8112, 8124, 8128, 9923, 8214, 8229, 8230, 8437, 8439, 8465, 3024, 8360 };
            return keystones.Contains((int)rune);
        }
    }
}
