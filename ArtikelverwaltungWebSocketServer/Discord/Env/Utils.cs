using System;

namespace ArtikelverwaltungWebSocketServer.Discord.Env
{
    public static class Utils
    {
        private static readonly int tableWidth = 90;

        public static string PrintLine()
        {
            return new string('-', tableWidth - 1) + Environment.NewLine;
        }

        public static string PrintRow(params string[] columns)
        {
            var width = (tableWidth - columns.Length) / columns.Length;
            var row = "|";

            foreach (var column in columns) row += AlignCentre(column, width) + "|";

            return row + Environment.NewLine;
        }

        private static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
                return new string(' ', width);
            return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }
    }
}