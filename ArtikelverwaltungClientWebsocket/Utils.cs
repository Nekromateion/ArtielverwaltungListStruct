using System;

namespace ArtikelverwaltungClientWebsocket
{
    public class Utils
    {
        static int tableWidth = 90;
        public static void WriteLine(object obj, ConsoleColor? color = null)
        {
            if (color != null)
                Console.ForegroundColor = color.Value;
            Console.WriteLine(obj);
            Console.ResetColor();
        }
        public static void PrintLine(ConsoleColor? color = null)
        {
            WriteLine(new string('-', tableWidth - 1), color);
        }

        public static void PrintRow(ConsoleColor? color = null, params string[] columns)
        {
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
    }
}