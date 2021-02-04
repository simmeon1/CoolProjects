using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public partial class ChromeWorker_Music
    {
        /// <summary>
        /// Example link: https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1980
        /// </summary>
        /// <returns></returns>
        private static List<string> GetWikipediaLinks()
        {
            List<string> links = new List<string>();
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1958");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1959");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1960");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1961");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1962");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1963");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1964");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1965");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1966");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1967");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1968");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1969");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1970");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1971");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1972");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1973");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1974");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1975");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1976");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1977");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1978");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1979");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1980");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1981");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1982");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1983");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1984");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1985");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1986");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1987");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1988");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1989");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1990");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1991");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1992");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1993");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1994");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1995");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1996");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1997");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1998");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_1999");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2000");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2001");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2002");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2003");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2004");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2005");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2006");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2007");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2008");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2009");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2010");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2011");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2012");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2013");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2014");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2015");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2016");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2017");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2018");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2019");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2020");
            links.Add("https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles_in_2021");
            return links;
        }
    }
}
