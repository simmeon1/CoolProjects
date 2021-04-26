using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CodingChallenges
{
    [TestClass]
    public class D3018_Helper
    {
        [TestMethod]
        public void Test()
        {
            //Dim exporter As New GridDataToExcelExporter(Of LocationCLS, LocationImporter)(New LocationImporter(GetVariables, AddressOf ShowMessageDelegate, AddressOf GetMessageResultDelegate))
            //exporter.GoThroughExportDataProcess($"{LocalisationManager.GetExactReplacement("Location") }s_{Date.Now:yyyyMMdd}", gcLocations)

            string fullMethod = File.ReadAllText("C:\\Users\\Simeon\\Desktop\\D3018\\fullMethod.txt");

            Match nameMatch = Regex.Match(fullMethod, "Dim suggested\\w+ As String = (.*?)\r", RegexOptions.Multiline);
            string name = nameMatch.Success ? nameMatch.Groups[1].Value : "NAME_NOT_FOUND";
            
            Match classNameMatch = Regex.Match(fullMethod, "\\w+CLS", RegexOptions.Multiline);
            string className = classNameMatch.Success ? classNameMatch.Value : "CLS_NOT_FOUND";

            Match importerMatch = Regex.Match(fullMethod, "Dim importer As .*?(\\w+Importer)\\(", RegexOptions.Multiline);
            string importer = importerMatch.Success ? importerMatch.Groups[1].Value : "IMPORTER_NOT_FOUND";
            
            Match gcMatch = Regex.Match(fullMethod, "gc\\w+", RegexOptions.Multiline);
            string gc = gcMatch.Success ? gcMatch.Value : "GC_NOT_FOUND";

            string result = $"Dim exporter As New GridDataToExcelExporter(Of {className}, {importer})(New {importer}(GetVariables, AddressOf ShowMessageDelegate, AddressOf GetMessageResultDelegate))";
            result += Environment.NewLine;
            result += $"exporter.GoThroughExportDataProcess({name}, {gc})";

            Debug.WriteLine(result);
        }
    }
}
