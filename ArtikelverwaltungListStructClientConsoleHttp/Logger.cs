using System;
using System.IO;

namespace ArtikelverwaltungListStructClientConsole
{
    public class Logger
    {
        public static string LogName { get; set; }

        private static string LogFile = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NW"), "Artikelverwaltung"), LogName);

        private static void Init()
        {
            
        }
    }
}