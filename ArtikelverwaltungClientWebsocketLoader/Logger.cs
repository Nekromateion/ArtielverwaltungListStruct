using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ArtikelverwaltungClientWebsocketLoader
{
    public class Logger
    {
        private static List<string> _queue = new List<string>();
        
        private Thread _queueHandler = new Thread(() =>
        {
            while (true)
            {
                if (_queue.Count != 0)
                {
                    if (_queue[0] == "empty")
                    {
                        File.AppendAllText(LogFile, Environment.NewLine);
                        _queue.Remove(_queue[0]);
                    }
                    else
                    {
                        File.AppendAllText(LogFile, _queue[0] + Environment.NewLine);
                        _queue.Remove(_queue[0]);
                    }
                }
                Thread.Sleep(1000);
            }
        });

        private static string LogFile = String.Empty;

        public void Init()
        {
            Directory.CreateDirectory("Logs");
            LogFile = Path.Combine("Logs", (DateTime.Now + ".log").Replace('/', '-').Replace(':', '-').Replace(' ', '_'));
            _queueHandler.Start();
            AddLines(new string[]{"<=================================================================>", "                           Log start", "<=================================================================>"});
            AddEmpty();
        }

        public void AddEmpty()
        {
            _queue.Add("empty");
            //File.AppendAllText(LogFile, Environment.NewLine);
        }
        
        public void AddLines(string[] lines, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = 0, [CallerFilePath] string callerPath = "")
        {
            int pos = callerPath.LastIndexOf(@"\") + 1;
            callerPath = callerPath.Substring(pos, callerPath.Length - pos);
            foreach (string line in lines)
            {
                _queue.Add($"[{DateTime.Now.ToString("hh.mm.ss.ffffff")}] : [{callerPath}/{callerName}/{callerLine}] {line}");
                //File.AppendAllText(LogFile, $"[{DateTime.Now.ToString("hh.mm.ss.ffffff")}] : [{callerPath}/{callerName}/{callerLine}] {line}" + Environment.NewLine);
            }
        }
        
        public void AddLine(string line, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = 0, [CallerFilePath] string callerPath = "")
        {
            int pos = callerPath.LastIndexOf(@"\") + 1;
            callerPath = callerPath.Substring(pos, callerPath.Length - pos);
            _queue.Add($"[{DateTime.Now.ToString("hh.mm.ss.ffffff")}] : [{callerPath}/{callerName}/{callerLine}] {line}");
            //File.AppendAllText(LogFile, $"[{DateTime.Now.ToString("hh.mm.ss.ffffff")}] : [{callerPath}/{callerName}/{callerLine}] {line}" + Environment.NewLine);
        }
    }
}