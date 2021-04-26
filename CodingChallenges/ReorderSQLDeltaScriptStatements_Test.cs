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
    public class ReorderSQLDeltaScriptStatements_Test
    {
        [TestMethod]
        public void Test()
        {
            //.*BEGIN TRANSACTION(.|\n)*?COMMIT TRANSACTION\r\nGO
            string script = File.ReadAllText("C:\\Users\\Simeon\\Desktop\\SD_TAQA_Dev.sql");
            MatchCollection matches = Regex.Matches(script, ".*BEGIN TRANSACTION(.|\n)*?COMMIT TRANSACTION\r\nGO", RegexOptions.Multiline);
            Debug.WriteLine(matches.Count);

            List<string> procStatements = new();
            List<string> viewStatements = new();
            List<string> tableStatements = new();
            List<string> otherStatements = new();
            foreach (Match match in matches)
            {
                string statement = match.Value;
                string statementLower = statement.ToLower();
                if (statementLower.Contains("create procedure") || statementLower.Contains("alter procedure")) procStatements.Add(statement);
                else if (statementLower.Contains("create view") || statementLower.Contains("alter view")) viewStatements.Add(statement);
                else if (statementLower.Contains("create table") || statementLower.Contains("alter table")) tableStatements.Add(statement);
                else otherStatements.Add(statement);
            }

            string reorderedStatements = "";
            List<string> allStatements = new();
            allStatements.AddRange(tableStatements);
            allStatements.AddRange(viewStatements);
            allStatements.AddRange(procStatements);
            allStatements.AddRange(otherStatements);
            foreach (string statement in allStatements) reorderedStatements += $"{statement}{Environment.NewLine}";
        }
    }
}
