using System;
using System.Linq;
using System.Collections.Generic;
using Discord.WebSocket;

namespace ArtikelverwaltungWebSocketServer.Discord
{
    public class Tools
    {
        internal static string ReadList(List<Article> artList)
        {
            string text = "```" + Environment.NewLine;
            text += Env.Utils.PrintLine();
            text += Env.Utils.PrintRow(new[] {"ID", "Name", "Price", "Count"});
            foreach (Article art in artList)
            {
                text += Env.Utils.PrintRow(new[] {Convert.ToString(art.id), art.name, Convert.ToString(art.price), Convert.ToString(art.count)});
            }

            if (text.Length > 1890)
            {
                text = text.Substring(0, 1890);
            }
            text += Env.Utils.PrintLine();
            text += "```";
            return text;
        }
        
        internal static string SortById(List<Article> artList)
        {
            string text = "```" + Environment.NewLine;
            artList = artList.OrderBy(x => x.id).ToList();
            text += Env.Utils.PrintLine();
            text += Env.Utils.PrintRow(new[] {"^ID^", "Name", "Price", "Count"});
            foreach (Article art in artList)
            {
                text += Env.Utils.PrintRow(new[] {Convert.ToString(art.id), art.name, Convert.ToString(art.price), Convert.ToString(art.count)});
            }

            if (text.Length > 1890)
            {
                text = text.Substring(0, 1890);
            }
            text += Env.Utils.PrintLine();
            text += "```";
            return text;
        }
        
        internal static string SortByName(List<Article> artList)
        {
            string text = "```" + Environment.NewLine;
            artList = artList.OrderBy(x => x.name).ToList();
            text += Env.Utils.PrintLine();
            text += Env.Utils.PrintRow(new[] {"ID", "^Name^", "Price", "Count"});
            foreach (Article art in artList)
            {
                text += Env.Utils.PrintRow(new[] {Convert.ToString(art.id), art.name, Convert.ToString(art.price), Convert.ToString(art.count)});
            }

            if (text.Length > 1890)
            {
                text = text.Substring(0, 1890);
            }
            text += Env.Utils.PrintLine();
            text += "```";
            return text;
        }
        
        internal static string SortByPrice(List<Article> artList)
        {
            string text = "```" + Environment.NewLine;
            artList = artList.OrderBy(x => x.price).ToList();
            text += Env.Utils.PrintLine();
            text += Env.Utils.PrintRow(new[] {"ID", "Name", "^Price^", "Count"});
            foreach (Article art in artList)
            {
                text += Env.Utils.PrintRow(new[] {Convert.ToString(art.id), art.name, Convert.ToString(art.price), Convert.ToString(art.count)});
            }

            if (text.Length > 1890)
            {
                text = text.Substring(0, 1890);
            }
            text += Env.Utils.PrintLine();
            text += "```";
            return text;
        }
        
        internal static string SortByCount(List<Article> artList)
        {
            string text = "```" + Environment.NewLine;
            artList = artList.OrderBy(x => x.count).ToList();
            text += Env.Utils.PrintLine();
            text += Env.Utils.PrintRow(new[] {"ID", "Name", "Price", "^Count^"});
            foreach (Article art in artList)
            {
                text += Env.Utils.PrintRow(new[] {Convert.ToString(art.id), art.name, Convert.ToString(art.price), Convert.ToString(art.count)});
            }

            if (text.Length > 1890)
            {
                text = text.Substring(0, 1890);
            }
            text += Env.Utils.PrintLine();
            text += "```";
            return text;
        }

        internal static string SearchById(List<Article> artList, string searchFor)
        {
            int foundCount = 0;
            string text = "```" + Environment.NewLine;

            text += Env.Utils.PrintLine();
            text += Env.Utils.PrintRow(new[] {"->ID<-", "Name", "Price", "Count"});
            foreach (Article art in artList)
            {
                if (art.id == Convert.ToInt32(searchFor))
                {
                    foundCount++;
                    text += Env.Utils.PrintRow(new[] {Convert.ToString(art.id), art.name, Convert.ToString(art.price), Convert.ToString(art.count)});
                }
            }
            
            if (text.Length > 1890)
            {
                text = text.Substring(0, 1890);
            }
            text += Env.Utils.PrintLine();
            text += "```";
            if (foundCount == 0) text = "No results found for your search quarry";
            return text;
        }
        
        internal static string SearchByName(List<Article> artList, string searchFor)
        {
            int foundCount = 0;
            string text = "```" + Environment.NewLine;

            text += Env.Utils.PrintLine();
            text += Env.Utils.PrintRow(new[] {"ID", "->Name<-", "Price", "Count"});
            foreach (Article art in artList)
            {
                if (art.name.Contains(searchFor))
                {
                    foundCount++;
                    text += Env.Utils.PrintRow(new[] {Convert.ToString(art.id), art.name, Convert.ToString(art.price), Convert.ToString(art.count)});
                }
            }
            
            if (text.Length > 1890)
            {
                text = text.Substring(0, 1890);
            }
            text += Env.Utils.PrintLine();
            text += "```";
            if (foundCount == 0) text = "No results found for your search quarry";
            return text;
        }
        
        internal static string SearchByPrice(List<Article> artList, string searchFor)
        {
            int foundCount = 0;
            string text = "```" + Environment.NewLine;

            text += Env.Utils.PrintLine();
            text += Env.Utils.PrintRow(new[] {"ID", "Name", "->Price<-", "Count"});
            foreach (Article art in artList)
            {
                if (art.price == Convert.ToDouble(searchFor))
                {
                    foundCount++;
                    text += Env.Utils.PrintRow(new[] {Convert.ToString(art.id), art.name, Convert.ToString(art.price), Convert.ToString(art.count)});
                }
            }
            
            if (text.Length > 1890)
            {
                text = text.Substring(0, 1890);
            }
            text += Env.Utils.PrintLine();
            text += "```";
            if (foundCount == 0) text = "No results found for your search quarry";
            return text;
        }
        
        internal static string SearchByCount(List<Article> artList, string searchFor)
        {
            int foundCount = 0;
            string text = "```" + Environment.NewLine;

            text += Env.Utils.PrintLine();
            text += Env.Utils.PrintRow(new[] {"ID", "Name", "Price", "->Count<-"});
            foreach (Article art in artList)
            {
                if (art.count == Convert.ToInt32(searchFor))
                {
                    foundCount++;
                    text += Env.Utils.PrintRow(new[] {Convert.ToString(art.id), art.name, Convert.ToString(art.price), Convert.ToString(art.count)});
                }
            }
            
            if (text.Length > 1890)
            {
                text = text.Substring(0, 1890);
            }
            text += Env.Utils.PrintLine();
            text += "```";
            if (foundCount == 0) text = "No results found for your search quarry";
            return text;
        }

        internal static string SetId(List<UserAdd> tempArts, string value, SocketUser user)
        {
            bool isFound = false;
            foreach (UserAdd t in tempArts)
            {
                if (t.UserId == user.Id) isFound = true;
            }
            if (isFound)
            {
                UserAdd content = tempArts.Where(x => x.UserId == user.Id).ToList().FirstOrDefault();
                content.Id = Convert.ToInt32(value);
                tempArts.Remove(content);
                tempArts.Add(content);
                Env.Vars.temporaryArticles = tempArts;
                return "Set value ID of cached content for you to " + value;
            }
            else
            {
                UserAdd content = new UserAdd();
                content.UserId = user.Id;
                content.Id = Convert.ToInt32(value);
                Env.Vars.temporaryArticles.Add(content);
                return "There was no entry in the cache found for you, so a new one has been created." + Environment.NewLine + "Set value ID for your cache to " + value;
            }
        }
        
        internal static string SetName(List<UserAdd> tempArts, string value, SocketUser user)
        {
            bool isFound = false;
            foreach (UserAdd t in tempArts)
            {
                if (t.UserId == user.Id) isFound = true;
            }
            if (isFound)
            {
                UserAdd content = tempArts.Where(x => x.UserId == user.Id).ToList().FirstOrDefault();
                content.name = value;
                tempArts.Remove(content);
                tempArts.Add(content);
                Env.Vars.temporaryArticles = tempArts;
                return "Set value Name of cached content for you to " + value;
            }
            else
            {
                UserAdd content = new UserAdd();
                content.UserId = user.Id;
                content.name = value;
                Env.Vars.temporaryArticles.Add(content);
                return "There was no entry in the cache found for you, so a new one has been created." + Environment.NewLine + "Set value Name for your cache to " + value;
            }
        }
        
        internal static string SetPrice(List<UserAdd> tempArts, string value, SocketUser user)
        {
            bool isFound = false;
            foreach (UserAdd t in tempArts)
            {
                if (t.UserId == user.Id) isFound = true;
            }
            if (isFound)
            {
                UserAdd content = tempArts.Where(x => x.UserId == user.Id).ToList().FirstOrDefault();
                content.price = Convert.ToDouble(value);
                tempArts.Remove(content);
                tempArts.Add(content);
                Env.Vars.temporaryArticles = tempArts;
                return "Set value Price of cached content for you to " + value;
            }
            else
            {
                UserAdd content = new UserAdd();
                content.UserId = user.Id;
                content.price = Convert.ToDouble(value);
                Env.Vars.temporaryArticles.Add(content);
                return "There was no entry in the cache found for you, so a new one has been created." + Environment.NewLine + "Set value Price for your cache to " + value;
            }
        }
        
        internal static string SetCount(List<UserAdd> tempArts, string value, SocketUser user)
        {
            bool isFound = false;
            foreach (UserAdd t in tempArts)
            {
                if (t.UserId == user.Id) isFound = true;
            }
            if (isFound)
            {
                UserAdd content = tempArts.Where(x => x.UserId == user.Id).ToList().FirstOrDefault();
                content.Count = Convert.ToInt32(value);
                tempArts.Remove(content);
                tempArts.Add(content);
                Env.Vars.temporaryArticles = tempArts;
                return "Set value Count of cached content for you to " + value;
            }
            else
            {
                UserAdd content = new UserAdd();
                content.UserId = user.Id;
                content.Count = Convert.ToInt32(value);
                Env.Vars.temporaryArticles.Add(content);
                return "There was no entry in the cache found for you, so a new one has been created." + Environment.NewLine + "Set value Count for your cache to " + value;
            }
        }
    }
}