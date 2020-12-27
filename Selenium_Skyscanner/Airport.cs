using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Selenium_Skyscanner
{
    [DebuggerDisplay("{ToString()}")]
    class Airport
    {
        public Airport(string iATA, string location)
        {
            IATA = iATA;
            Location = location;
            DestinationAirports = new List<Airport>();
        }

        public string IATA { get; set; }
        public string Location { get; set; }
        public List<Airport> DestinationAirports { get; set; }
        public void AddDestinationAirport(Airport destAirport)
        {
            destAirport.DestinationAirports.Add(this);
            DestinationAirports.Add(destAirport);
        }

        public override int GetHashCode()
        {
            return IATA.GetHashCode();
        }

        public override string ToString()
        {
            return $"{IATA} : {Location}";
        }

        public List<Airport> GetCommonMidwayAirportsWithTargetAirport(Airport targetAirport)
        {
            List<Airport> commonAirports = new List<Airport>();
            foreach (Airport originDestinationAirport in this.DestinationAirports)
            {
                foreach (Airport targetDestinationAirport in targetAirport.DestinationAirports)
                {
                    if (targetDestinationAirport.IATA.Equals(originDestinationAirport.IATA))
                    {
                        commonAirports.Add(targetDestinationAirport);
                        break;
                    }
                }
            }
            return commonAirports;
        }
    }
}
