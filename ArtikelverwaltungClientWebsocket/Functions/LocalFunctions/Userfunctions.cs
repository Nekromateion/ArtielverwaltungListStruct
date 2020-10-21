using System;
using System.Diagnostics.Eventing.Reader;
using System.Threading;

namespace ArtikelverwaltungClientWebsocket.Functions.LocalFunctions
{
    public class Userfunctions
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        
        internal static void ReadList()
        {
            try
            {
                long startTimeReadPrint = DateTime.Now.Ticks;
                logger.AddLine("called");
                Console.Clear();
                Utils.PrintLine();
                Utils.PrintRow(ConsoleColor.White, new string[]{"ID", "Name", $"Price ({Vars.Currency})", "Count"});
                foreach (Article article in Data.Articles)
                {
                    Utils.PrintRow(ConsoleColor.White, new string[]{article.id.ToString(), article.name, article.price.ToString(), article.count.ToString()});
                }
                Utils.PrintLine();
                long endTimeReadPrint = DateTime.Now.Ticks;
                logger.AddLine($"reading and printing {Data.Articles.Count} took: Action took: {(endTimeReadPrint / TimeSpan.TicksPerMillisecond) - (startTimeReadPrint / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeReadPrint-startTimeReadPrint} ticks)");
                Console.Write("Press any key to return to the menu...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                logger.AddLine("An error occured");
                Console.WriteLine("An error occured");
                logger.AddLine(e.Message);
                Thread.Sleep(2500);
            }
        }

        internal static void SearchList()
        {
            try
            {
                Console.WriteLine("By what do you want to search?");
                Console.WriteLine("1: Name");
                Console.WriteLine("2: Price");
                long startTime = DateTime.Now.Ticks;
                long endTime = DateTime.Now.Ticks;
                logger.AddLine($"reading and printing {Data.Articles.Count} took: Action took: {(endTime / TimeSpan.TicksPerMillisecond) - (startTime / TimeSpan.TicksPerMillisecond)} milliseconds ({endTime-startTime} ticks)");
                Console.Write("Press any key to return to the menu...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                logger.AddLine("An error occured");
                Console.WriteLine("An error occured");
                logger.AddLine(e.Message);
                Thread.Sleep(2500);
            }
        }

        internal static void SortList()
        {
            
        }
    }
}