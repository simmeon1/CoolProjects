using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium_Skyscanner
{
    static class Airports_FlightsFromDotCom
    {
        public static Airport GetAberdeenAirport()
        {
            Airport aberdeenAirport = new Airport("ABZ", "Dyce");
            aberdeenAirport.AddDestinationAirport(new Airport("ALC", "Alicante"));
            aberdeenAirport.AddDestinationAirport(new Airport("AMS", "Amsterdam"));
            aberdeenAirport.AddDestinationAirport(new Airport("BHD", "Belfast"));
            aberdeenAirport.AddDestinationAirport(new Airport("BGO", "Bergen"));
            aberdeenAirport.AddDestinationAirport(new Airport("BHX", "Birmingham"));
            aberdeenAirport.AddDestinationAirport(new Airport("BOJ", "Bourgas"));
            aberdeenAirport.AddDestinationAirport(new Airport("BRS", "Bristol"));
            aberdeenAirport.AddDestinationAirport(new Airport("CPH", "Copenhagen"));
            aberdeenAirport.AddDestinationAirport(new Airport("MME", "Darlington"));
            aberdeenAirport.AddDestinationAirport(new Airport("DUB", "Dublin"));
            aberdeenAirport.AddDestinationAirport(new Airport("EBJ", "Esbjerg"));
            aberdeenAirport.AddDestinationAirport(new Airport("FAO", "Faro"));
            aberdeenAirport.AddDestinationAirport(new Airport("GDN", "Gdansk"));
            aberdeenAirport.AddDestinationAirport(new Airport("GVA", "Geneva"));
            aberdeenAirport.AddDestinationAirport(new Airport("HUY", "Humberside"));
            aberdeenAirport.AddDestinationAirport(new Airport("INV", "Inverness"));
            aberdeenAirport.AddDestinationAirport(new Airport("KOI", "Kirkwall"));
            aberdeenAirport.AddDestinationAirport(new Airport("LHR", "London"));
            aberdeenAirport.AddDestinationAirport(new Airport("LTN", "London"));
            aberdeenAirport.AddDestinationAirport(new Airport("AGP", "Malaga"));
            aberdeenAirport.AddDestinationAirport(new Airport("MAN", "Manchester"));
            aberdeenAirport.AddDestinationAirport(new Airport("NCL", "Newcastle"));
            aberdeenAirport.AddDestinationAirport(new Airport("NWI", "Norwich"));
            aberdeenAirport.AddDestinationAirport(new Airport("OSL", "Oslo"));
            aberdeenAirport.AddDestinationAirport(new Airport("CDG", "Paris"));
            aberdeenAirport.AddDestinationAirport(new Airport("RIX", "Riga"));
            aberdeenAirport.AddDestinationAirport(new Airport("SVG", "Stavanger"));
            aberdeenAirport.AddDestinationAirport(new Airport("LSI", "Sumburgh"));
            return aberdeenAirport;
        }

        public static Airport GetSofiaAirport()
        {
            Airport sofiaAirport = new Airport("SOF", "Sofia");
            sofiaAirport.AddDestinationAirport(new Airport("AUH", "Abu Dhabi"));
            sofiaAirport.AddDestinationAirport(new Airport("ALC", "Alicante"));
            sofiaAirport.AddDestinationAirport(new Airport("AMS", "Amsterdam"));
            sofiaAirport.AddDestinationAirport(new Airport("AQJ", "Aqaba"));
            sofiaAirport.AddDestinationAirport(new Airport("ATH", "Athens"));
            sofiaAirport.AddDestinationAirport(new Airport("BCN", "Barcelona"));
            sofiaAirport.AddDestinationAirport(new Airport("BRI", "Bari"));
            sofiaAirport.AddDestinationAirport(new Airport("BSL", "Basel"));
            sofiaAirport.AddDestinationAirport(new Airport("BVA", "Beauvais"));
            sofiaAirport.AddDestinationAirport(new Airport("BEG", "Beograd"));
            sofiaAirport.AddDestinationAirport(new Airport("BGY", "Bergamo"));
            sofiaAirport.AddDestinationAirport(new Airport("BER", "Berlin"));
            sofiaAirport.AddDestinationAirport(new Airport("SXF", "Berlin"));
            sofiaAirport.AddDestinationAirport(new Airport("TXL", "Berlin"));
            sofiaAirport.AddDestinationAirport(new Airport("BLL", "Billund"));
            sofiaAirport.AddDestinationAirport(new Airport("BHX", "Birmingham"));
            sofiaAirport.AddDestinationAirport(new Airport("BLQ", "Bologna"));
            sofiaAirport.AddDestinationAirport(new Airport("BOJ", "Bourgas"));
            sofiaAirport.AddDestinationAirport(new Airport("BTS", "Bratislava"));
            sofiaAirport.AddDestinationAirport(new Airport("BRS", "Bristol"));
            sofiaAirport.AddDestinationAirport(new Airport("BRU", "Brussels"));
            sofiaAirport.AddDestinationAirport(new Airport("CRL", "Brussels"));
            sofiaAirport.AddDestinationAirport(new Airport("OTP", "Bucharest"));
            sofiaAirport.AddDestinationAirport(new Airport("BUD", "Budapest"));
            sofiaAirport.AddDestinationAirport(new Airport("CTA", "Catania"));
            sofiaAirport.AddDestinationAirport(new Airport("CHQ", "Chania"));
            sofiaAirport.AddDestinationAirport(new Airport("CGN", "Cologne"));
            sofiaAirport.AddDestinationAirport(new Airport("CPH", "Copenhagen"));
            sofiaAirport.AddDestinationAirport(new Airport("DOH", "Doha"));
            sofiaAirport.AddDestinationAirport(new Airport("DTM", "Dortmund"));
            sofiaAirport.AddDestinationAirport(new Airport("DXB", "Dubai"));
            sofiaAirport.AddDestinationAirport(new Airport("DUB", "Dublin"));
            sofiaAirport.AddDestinationAirport(new Airport("DUS", "Dusseldorf"));
            sofiaAirport.AddDestinationAirport(new Airport("EDI", "Edinburgh"));
            sofiaAirport.AddDestinationAirport(new Airport("ETM", "Eilat"));
            sofiaAirport.AddDestinationAirport(new Airport("EIN", "Eindhoven"));
            sofiaAirport.AddDestinationAirport(new Airport("FRA", "Frankfurt"));
            sofiaAirport.AddDestinationAirport(new Airport("GVA", "Geneva"));
            sofiaAirport.AddDestinationAirport(new Airport("HHN", "Hahn"));
            sofiaAirport.AddDestinationAirport(new Airport("HAM", "Hamburg"));
            sofiaAirport.AddDestinationAirport(new Airport("HRG", "Hurghada"));
            sofiaAirport.AddDestinationAirport(new Airport("IST", "Istanbul"));
            sofiaAirport.AddDestinationAirport(new Airport("FKB", "Karlsruhe"));
            sofiaAirport.AddDestinationAirport(new Airport("KBP", "Kiev"));
            sofiaAirport.AddDestinationAirport(new Airport("LCA", "Larnaca"));
            sofiaAirport.AddDestinationAirport(new Airport("LIS", "Lisbon"));
            sofiaAirport.AddDestinationAirport(new Airport("LPL", "Liverpool"));
            sofiaAirport.AddDestinationAirport(new Airport("LGW", "London"));
            sofiaAirport.AddDestinationAirport(new Airport("LHR", "London"));
            sofiaAirport.AddDestinationAirport(new Airport("LTN", "London"));
            sofiaAirport.AddDestinationAirport(new Airport("STN", "London"));
            sofiaAirport.AddDestinationAirport(new Airport("MAD", "Madrid"));
            sofiaAirport.AddDestinationAirport(new Airport("AGP", "Malaga"));
            sofiaAirport.AddDestinationAirport(new Airport("MAN", "Manchester"));
            sofiaAirport.AddDestinationAirport(new Airport("FMM", "Memmingen"));
            sofiaAirport.AddDestinationAirport(new Airport("MXP", "Milano"));
            sofiaAirport.AddDestinationAirport(new Airport("SVO", "Moscow"));
            sofiaAirport.AddDestinationAirport(new Airport("MUC", "Munich"));
            sofiaAirport.AddDestinationAirport(new Airport("JMK", "Mykonos"));
            sofiaAirport.AddDestinationAirport(new Airport("NAP", "Naples"));
            sofiaAirport.AddDestinationAirport(new Airport("NCE", "Nice"));
            sofiaAirport.AddDestinationAirport(new Airport("EMA", "Nottingham"));
            sofiaAirport.AddDestinationAirport(new Airport("PMI", "Palma De Mallorca"));
            sofiaAirport.AddDestinationAirport(new Airport("CDG", "Paris"));
            sofiaAirport.AddDestinationAirport(new Airport("PRG", "Prague"));
            sofiaAirport.AddDestinationAirport(new Airport("CIA", "Rome"));
            sofiaAirport.AddDestinationAirport(new Airport("FCO", "Rome"));
            sofiaAirport.AddDestinationAirport(new Airport("LED", "Sankt Petersburg"));
            sofiaAirport.AddDestinationAirport(new Airport("SSH", "Sharm El-Sheikh"));
            sofiaAirport.AddDestinationAirport(new Airport("STR", "Stuttgart"));
            sofiaAirport.AddDestinationAirport(new Airport("TLV", "Tel Aviv"));
            sofiaAirport.AddDestinationAirport(new Airport("TSF", "Treviso"));
            sofiaAirport.AddDestinationAirport(new Airport("VLC", "Valencia"));
            sofiaAirport.AddDestinationAirport(new Airport("MLA", "Valletta"));
            sofiaAirport.AddDestinationAirport(new Airport("VAR", "Varna"));
            sofiaAirport.AddDestinationAirport(new Airport("VIE", "Vienna"));
            sofiaAirport.AddDestinationAirport(new Airport("WAW", "Warsaw"));
            sofiaAirport.AddDestinationAirport(new Airport("ZRH", "Zurich"));
            return sofiaAirport;
        }
    }
}
