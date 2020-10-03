using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace ArtikelverwaltungListStructClientConsole
{
    public class Logger
    {
        public static string LogName { get; set; }

        private static string LogFile = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NW"), "Artikelverwaltung"), LogName);

        private static void Init()
        {
            AddLines(new string[]{"<=================================================================>", "                           Log start", "<=================================================================>"});
            AddEmpty();
        }

        private static void AddEmpty()
        {
            File.AppendAllText(LogFile, Environment.NewLine);
        }
        
        private static void AddLines(string[] lines, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = 0, [CallerFilePath] string callerPath = "")
        {
            int pos = callerPath.LastIndexOf(@"\") + 1;
            callerPath = callerPath.Substring(pos, callerPath.Length - pos);
            foreach (string line in lines)
            {
                File.AppendAllText(LogFile, $"[{DateTime.Now.ToString("hh.mm.ss.ffffff")}] : [{callerPath}/{callerName}/{callerLine}] {line}" + Environment.NewLine);
            }
        }
        
        public void AddLine(string line, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = 0, [CallerFilePath] string callerPath = "")
        {
            int pos = callerPath.LastIndexOf(@"\") + 1;
            callerPath = callerPath.Substring(pos, callerPath.Length - pos);
            File.AppendAllText(LogFile, $"[{DateTime.Now.ToString("hh.mm.ss.ffffff")}] : [{callerPath}/{callerName}/{callerLine}] {line}" + Environment.NewLine);
        }
    }
}