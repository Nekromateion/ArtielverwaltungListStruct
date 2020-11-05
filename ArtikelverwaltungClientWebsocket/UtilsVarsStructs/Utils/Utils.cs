using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket;

namespace ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Utils
{
    public static class Utils
    {
        private static readonly Logger Logger =
            LogHandler.Logger;
        static int tableWidth = 90;

        private static void WriteLine(object obj, ConsoleColor? color = null)
        {
            Logger.AddLine("called");
            if (color != null)
                Console.ForegroundColor = color.Value;
            Console.WriteLine(obj);
            Console.ResetColor();
        }
        public static void PrintLine(ConsoleColor? color = null)
        {
            Logger.AddLine("called");
            WriteLine(new string('-', tableWidth - 1), color);
        }

        public static void PrintRow(ConsoleColor? color = null, params string[] columns)
        {
            Logger.AddLine("called");
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            WriteLine(row, color);
        }
        
        static string AlignCentre(string text, int width)
        {
            Logger.AddLine("called");
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
        
        public static void OpenBrowser(string url)
        {
            Logger.AddLine("called");
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}