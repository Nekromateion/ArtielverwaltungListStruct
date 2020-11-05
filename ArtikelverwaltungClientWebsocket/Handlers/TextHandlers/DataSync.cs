using System;
using System.Collections.Generic;
using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Structs;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Vars;

namespace ArtikelverwaltungClientWebsocket.Handlers.TextHandlers
{
    public static class DataSync
    {
        private static readonly Logger Logger =
            LogHandler.Logger;
        internal static void Handle(string data)
        {
            Logger.AddLine("message was a data sync");
            string content = data.Substring(10);
            string[] splitArticles = content.Split('~');
            Data.Articles = new List<Article>();
            foreach (string article in splitArticles)
            {
                string[] splitData = article.Split('|');
                Article temp = new Article
                {
                    Id = Convert.ToInt32(splitData[0].Replace("|", string.Empty).Replace("~", string.Empty)),
                    Name = splitData[1].Replace("|", string.Empty).Replace("~", string.Empty),
                    Price = Convert.ToDouble(splitData[2].Replace("|", string.Empty).Replace("~", string.Empty)),
                    Count = Convert.ToInt32(splitData[3].Replace("|", string.Empty).Replace("~", string.Empty))
                };
                Data.Articles.Add(temp);
            }
        }
    }
}