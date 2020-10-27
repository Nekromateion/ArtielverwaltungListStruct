﻿using System;

namespace ArtikelverwaltungWebSocketServer.Discord.Env
{
    public class Utils
    {
        static int tableWidth = 90;

        public static string WriteLine(object obj)
        {
            return (string) obj + Environment.NewLine;
        }

        public static string PrintLine()
        {
            return (new string('-', tableWidth - 1));
        }

        public static string PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            return (row);
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