using ClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueAPI_Classes
{
    public class LeagueAPI_Variables
    {
        public const string varsFile = "LeagueAPI_Vars.json";
        public bool DataIsBeingCollected { get; set; }
        public bool StopCollectingData { get; set; }
        public bool WriteCurrentlyCollectedData { get; set; }
        public string CurrentProgress { get; set; }

        public void InitialiseForCollectingData()
        {
            DataIsBeingCollected = true;
            StopCollectingData = false;
            CurrentProgress = "";
            WriteCurrentlyCollectedData = false;
        }

        public void DataCollectionFinished()
        {
            DataIsBeingCollected = false;
            StopCollectingData = false;
            CurrentProgress = "";
            WriteCurrentlyCollectedData = false;
        }

        public static LeagueAPI_Variables ReadLocalVarsFile()
        {
            if (!File.Exists(varsFile)) return null;
            return File.ReadAllText(varsFile).ToObject<LeagueAPI_Variables>();
        }

        public static void UpdateLocalVarsFile(LeagueAPI_Variables aPIVars)
        {
            File.WriteAllText(varsFile, aPIVars.ToJson());
        }
    }
}
