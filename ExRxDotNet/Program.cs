﻿using System;
using System.Collections.Generic;
using ClassLibrary;

namespace ExRxDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeWorker_ExRx chrome = new ChromeWorker_ExRx();
            Dictionary<string, string> groupsAndLinks = chrome.GetLinksForMajorGroupsFromMainPage();
        }
    }
}
