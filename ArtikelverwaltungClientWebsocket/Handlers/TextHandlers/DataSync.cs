using System;
using System.Collections.Generic;
using ArtikelverwalktungClientWebsocket;
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
            var content = data.Substring(10);
            var splitArticles = content.Split('~');
            Data.Articles = new List<Article>();
            foreach (var article in splitArticles)
            {
                var splitData = article.Split('|');
                var temp = new Article
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