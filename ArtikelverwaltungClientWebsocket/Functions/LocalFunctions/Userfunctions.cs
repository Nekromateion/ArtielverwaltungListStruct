using System;
using System.Diagnostics.Eventing.Reader;

namespace ArtikelverwaltungClientWebsocket.Functions.LocalFunctions
{
    public class Userfunctions
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        
        internal static void ReadList()
        {
            logger.AddLine("called");
            Utils.PrintLine();
            Utils.PrintRow(ConsoleColor.White, new string[]{"ID", "Name", $"Price ({Vars.Currency})", "Count"});
            foreach (Article article in Data.Articles)
            {
                Utils.PrintRow(ConsoleColor.White, new string[]{article.id.ToString(), article.name, article.price.ToString(), article.count.ToString()});
            }
            Utils.PrintLine();
        }

        internal static void SearchList()
        {
            
        }

        internal static void SortList()
        {
            
        }
    }
}