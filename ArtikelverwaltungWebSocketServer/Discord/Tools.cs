using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ArtikelverwaltungWebSocketServer.Discord.Env;
using ArtikelverwaltungWebSocketServer.Structs;
using Discord.WebSocket;

namespace ArtikelverwaltungWebSocketServer.Discord
{
    public static class Tools
    {
        internal static string ReadList(List<Article> artList)
        {
            var text = "```" + Environment.NewLine;
            text += Utils.PrintLine();
            text += Utils.PrintRow("ID", "Name", "Price", "Count");
            foreach (var art in artList)
                text += Utils.PrintRow(Convert.ToString(art.Id), art.Name,
                    Convert.ToString(art.Price, CultureInfo.InvariantCulture), Convert.ToString(art.Count));

            if (text.Length > 1890) text = text.Substring(0, 1890);
            text += Utils.PrintLine();
            text += "```";
            return text;
        }

        internal static string SortById(List<Article> artList)
        {
            var text = "```" + Environment.NewLine;
            artList = artList.OrderBy(x => x.Id).ToList();
            text += Utils.PrintLine();
            text += Utils.PrintRow("^ID^", "Name", "Price", "Count");
            foreach (var art in artList)
                text += Utils.PrintRow(Convert.ToString(art.Id), art.Name,
                    Convert.ToString(art.Price, CultureInfo.InvariantCulture), Convert.ToString(art.Count));

            if (text.Length > 1890) text = text.Substring(0, 1890);
            text += Utils.PrintLine();
            text += "```";
            return text;
        }

        internal static string SortByName(List<Article> artList)
        {
            var text = "```" + Environment.NewLine;
            artList = artList.OrderBy(x => x.Name).ToList();
            text += Utils.PrintLine();
            text += Utils.PrintRow("ID", "^Name^", "Price", "Count");
            foreach (var art in artList)
                text += Utils.PrintRow(Convert.ToString(art.Id), art.Name,
                    Convert.ToString(art.Price, CultureInfo.InvariantCulture), Convert.ToString(art.Count));

            if (text.Length > 1890) text = text.Substring(0, 1890);
            text += Utils.PrintLine();
            text += "```";
            return text;
        }

        internal static string SortByPrice(List<Article> artList)
        {
            var text = "```" + Environment.NewLine;
            artList = artList.OrderBy(x => x.Price).ToList();
            text += Utils.PrintLine();
            text += Utils.PrintRow("ID", "Name", "^Price^", "Count");
            foreach (var art in artList)
                text += Utils.PrintRow(Convert.ToString(art.Id), art.Name,
                    Convert.ToString(art.Price, CultureInfo.InvariantCulture), Convert.ToString(art.Count));

            if (text.Length > 1890) text = text.Substring(0, 1890);
            text += Utils.PrintLine();
            text += "```";
            return text;
        }

        internal static string SortByCount(List<Article> artList)
        {
            var text = "```" + Environment.NewLine;
            artList = artList.OrderBy(x => x.Count).ToList();
            text += Utils.PrintLine();
            text += Utils.PrintRow("ID", "Name", "Price", "^Count^");
            foreach (var art in artList)
                text += Utils.PrintRow(Convert.ToString(art.Id), art.Name,
                    Convert.ToString(art.Price, CultureInfo.InvariantCulture), Convert.ToString(art.Count));

            if (text.Length > 1890) text = text.Substring(0, 1890);
            text += Utils.PrintLine();
            text += "```";
            return text;
        }

        internal static string SearchById(List<Article> artList, string searchFor)
        {
            var foundCount = 0;
            var text = "```" + Environment.NewLine;

            text += Utils.PrintLine();
            text += Utils.PrintRow("->ID<-", "Name", "Price", "Count");
            foreach (var art in artList)
                if (art.Id == Convert.ToInt32(searchFor))
                {
                    foundCount++;
                    text += Utils.PrintRow(Convert.ToString(art.Id), art.Name,
                        Convert.ToString(art.Price, CultureInfo.InvariantCulture), Convert.ToString(art.Count));
                }

            if (text.Length > 1890) text = text.Substring(0, 1890);
            text += Utils.PrintLine();
            text += "```";
            if (foundCount == 0) text = "No results found for your search quarry";
            return text;
        }

        internal static string SearchByName(List<Article> artList, string searchFor)
        {
            var foundCount = 0;
            var text = "```" + Environment.NewLine;

            text += Utils.PrintLine();
            text += Utils.PrintRow("ID", "->Name<-", "Price", "Count");
            foreach (var art in artList)
                if (art.Name.Contains(searchFor))
                {
                    foundCount++;
                    text += Utils.PrintRow(Convert.ToString(art.Id), art.Name,
                        Convert.ToString(art.Price, CultureInfo.InvariantCulture), Convert.ToString(art.Count));
                }

            if (text.Length > 1890) text = text.Substring(0, 1890);
            text += Utils.PrintLine();
            text += "```";
            if (foundCount == 0) text = "No results found for your search quarry";
            return text;
        }

        internal static string SearchByPrice(List<Article> artList, string searchFor)
        {
            var foundCount = 0;
            var text = "```" + Environment.NewLine;

            text += Utils.PrintLine();
            text += Utils.PrintRow("ID", "Name", "->Price<-", "Count");
            foreach (var art in artList)
                if (Math.Abs(art.Price - Convert.ToDouble(searchFor)) < 0.25)
                {
                    foundCount++;
                    text += Utils.PrintRow(Convert.ToString(art.Id), art.Name,
                        Convert.ToString(art.Price, CultureInfo.InvariantCulture), Convert.ToString(art.Count));
                }

            if (text.Length > 1890) text = text.Substring(0, 1890);
            text += Utils.PrintLine();
            text += "```";
            if (foundCount == 0) text = "No results found for your search quarry";
            return text;
        }

        internal static string SearchByCount(List<Article> artList, string searchFor)
        {
            var foundCount = 0;
            var text = "```" + Environment.NewLine;

            text += Utils.PrintLine();
            text += Utils.PrintRow("ID", "Name", "Price", "->Count<-");
            foreach (var art in artList)
                if (art.Count == Convert.ToInt32(searchFor))
                {
                    foundCount++;
                    text += Utils.PrintRow(Convert.ToString(art.Id), art.Name,
                        Convert.ToString(art.Price, CultureInfo.InvariantCulture), Convert.ToString(art.Count));
                }

            if (text.Length > 1890) text = text.Substring(0, 1890);
            text += Utils.PrintLine();
            text += "```";
            if (foundCount == 0) text = "No results found for your search quarry";
            return text;
        }

        internal static string SetId(List<UserAdd> tempArts, string value, SocketUser user)
        {
            var isFound = false;
            foreach (var t in tempArts)
                if (t.UserId == user.Id)
                    isFound = true;
            if (isFound)
            {
                var content = tempArts.Where(x => x.UserId == user.Id).ToList().FirstOrDefault();
                content.Id = Convert.ToInt32(value);
                tempArts.Remove(content);
                tempArts.Add(content);
                Vars.TemporaryArticles = tempArts;
                return "Set value ID of cached content for you to " + value;
            }
            else
            {
                var content = new UserAdd {UserId = user.Id, Id = Convert.ToInt32(value)};
                Vars.TemporaryArticles.Add(content);
                return "There was no entry in the cache found for you, so a new one has been created." +
                       Environment.NewLine + "Set value ID for your cache to " + value;
            }
        }

        internal static string SetName(List<UserAdd> tempArts, string value, SocketUser user)
        {
            var isFound = false;
            foreach (var t in tempArts)
                if (t.UserId == user.Id)
                    isFound = true;
            if (isFound)
            {
                var content = tempArts.Where(x => x.UserId == user.Id).ToList().FirstOrDefault();
                content.Name = value;
                tempArts.Remove(content);
                tempArts.Add(content);
                Vars.TemporaryArticles = tempArts;
                return "Set value Name of cached content for you to " + value;
            }
            else
            {
                var content = new UserAdd {UserId = user.Id, Name = value};
                Vars.TemporaryArticles.Add(content);
                return "There was no entry in the cache found for you, so a new one has been created." +
                       Environment.NewLine + "Set value Name for your cache to " + value;
            }
        }

        internal static string SetPrice(List<UserAdd> tempArts, string value, SocketUser user)
        {
            var isFound = false;
            foreach (var t in tempArts)
                if (t.UserId == user.Id)
                    isFound = true;
            if (isFound)
            {
                var content = tempArts.Where(x => x.UserId == user.Id).ToList().FirstOrDefault();
                content.Price = Convert.ToDouble(value);
                tempArts.Remove(content);
                tempArts.Add(content);
                Vars.TemporaryArticles = tempArts;
                return "Set value Price of cached content for you to " + value;
            }
            else
            {
                var content = new UserAdd {UserId = user.Id, Price = Convert.ToDouble(value)};
                Vars.TemporaryArticles.Add(content);
                return "There was no entry in the cache found for you, so a new one has been created." +
                       Environment.NewLine + "Set value Price for your cache to " + value;
            }
        }

        internal static string SetCount(List<UserAdd> tempArts, string value, SocketUser user)
        {
            var isFound = false;
            foreach (var t in tempArts)
                if (t.UserId == user.Id)
                    isFound = true;
            if (isFound)
            {
                var content = tempArts.Where(x => x.UserId == user.Id).ToList().FirstOrDefault();
                content.Count = Convert.ToInt32(value);
                tempArts.Remove(content);
                tempArts.Add(content);
                Vars.TemporaryArticles = tempArts;
                return "Set value Count of cached content for you to " + value;
            }
            else
            {
                var content = new UserAdd {UserId = user.Id, Count = Convert.ToInt32(value)};
                Vars.TemporaryArticles.Add(content);
                return "There was no entry in the cache found for you, so a new one has been created." +
                       Environment.NewLine + "Set value Count for your cache to " + value;
            }
        }
    }
}