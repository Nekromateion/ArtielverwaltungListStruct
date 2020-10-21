using System;
using System.Collections.Generic;

namespace ArtikelverwaltungClientWebsocket.Handlers.TextHandlers
{
    public class DataSync
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        internal static void Handle(string data)
        {
            logger.AddLine("message was a data sync");
            string content = data.Substring(9); // i will have to wait with continueing to work on this part since i dont know yet how i will send the data
            string[] splitArticles = content.Split('~');
            Data.Articles = new List<Article>();
            foreach (string article in splitArticles)
            {
                string[] splitData = article.Split('|');
                Article temp = new Article();
                temp.id = Convert.ToInt32(splitData[0].Replace("|", string.Empty).Replace("~", string.Empty));
                temp.name = splitData[1].Replace("|", string.Empty).Replace("~", string.Empty);
                temp.price = Convert.ToDouble(splitData[2].Replace("|", string.Empty).Replace("~", string.Empty));
                temp.count = Convert.ToInt32(splitData[3].Replace("|", string.Empty).Replace("~", string.Empty));
                Data.Articles.Add(temp);
            }
        }
    }
}