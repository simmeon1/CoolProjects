using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Selenium_Skyscanner
{
    [DebuggerDisplay("Count = {Paths.Count}")]
    public class AirportToAirportPaths : IEnumerable<AirportCollection>
    {
        public List<AirportCollection> Paths { get; set; }
        public AirportToAirportPaths(List<AirportCollection> paths)
        {
            Paths = paths;
        }

        public string CreateSkyscannerJSFunctionToLookPaths(int maxPathsToInclude = 0)
        {
            string func = "function searchForPaths() {";
            if (maxPathsToInclude > Paths.Count) maxPathsToInclude = Paths.Count;
            int maxPaths = maxPathsToInclude == 0 ? Paths.Count : maxPathsToInclude;
            int linesAdded = 0;
            for (int i = 0; i < maxPaths; i++)
            {
                AirportCollection path = Paths[i];
                for (int j = 0; j < path.Count - 1; j++)
                {
                    func += $"setTimeout(window.open('https://www.skyscanner.net/transport/flights/{path[j].IATA}/{path[j + 1].IATA}?adultsv2=1&cabinclass=economy&childrenv2=&inboundaltsenabled=false&iym=&outboundaltsenabled=false&oym=2101&preferdirects=false&rtn=0&selectedoday=01', 'wp{linesAdded}'), 2500);";
                    linesAdded++;
                }
            }
            func += "}";
            return func;
        }

        public string GetCollectionsAsPaths()
        {
            StringBuilder sb = new StringBuilder("");
            foreach (AirportCollection path in Paths)
            {
                if (sb.Length > 0) sb.Append($"{Environment.NewLine}");
                sb.Append(path.PrintCollectionAsPath());
            }
            return sb.ToString();
        }

        public IEnumerator<AirportCollection> GetEnumerator()
        {
            return ((IEnumerable<AirportCollection>)Paths).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Paths).GetEnumerator();
        }

        public void Insert(int index, AirportCollection collection)
        {
            Paths.Insert(index, collection);
        }
    }
}
