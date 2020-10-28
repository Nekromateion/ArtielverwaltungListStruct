using System;
using System.Linq;
using System.Collections.Generic;

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
    }
}