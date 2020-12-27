using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Selenium_Skyscanner
{
    [DebuggerDisplay("{ToString()}")]
    public class Airport : IEquatable<Airport>
    {
        public Airport(string iATA, string location)
        {
            IATA = iATA;
            Location = location;
            DestinationAirports = new AirportCollection();
        }

        public string IATA { get; set; }
        public string Location { get; set; }
        public AirportCollection DestinationAirports { get; set; }
        public void AddDestinationAirport(Airport destAirport)
        {
            destAirport.DestinationAirports.Add(this);
            DestinationAirports.Add(destAirport);
        }

        public override string ToString()
        {
            return $"{IATA} : {Location}";
        }

        public AirportCollection GetCommonMidwayAirportsWithTargetAirport(Airport targetAirport)
        {
            AirportCollection commonAirports = new AirportCollection();
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

        public bool Equals([AllowNull] Airport other)
        {
            if (other == null) return false;
            return (this.IATA.Equals(other.IATA));
        }
    }
}
