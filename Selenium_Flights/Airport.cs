using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Selenium_Flights
{
    [DebuggerDisplay("{ToString()}, Destinations = {DestinationAirports.Count}")]
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

        public override string ToString()
        {
            return $"{IATA} : {Location}";
        }

        public bool Equals([AllowNull] Airport other)
        {
            if (other == null) return false;
            return (this.IATA.Equals(other.IATA));
        }
    }
}
