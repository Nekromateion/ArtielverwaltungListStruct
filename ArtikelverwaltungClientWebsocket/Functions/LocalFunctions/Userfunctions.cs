using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
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
            logger.AddLine("called");
            try
            {
                Console.WriteLine("By what do you want to search?");
                Console.WriteLine("1: Name");
                Console.WriteLine("2: Price");
                Console.WriteLine("3: Count");
                Console.WriteLine();
                Console.Write("Your input: ");
                string input = Console.ReadLine();
                Console.Clear();
                long startTime = Int64.MaxValue;
                if (input == "1")
                {
                    Console.Write("Name: ");
                    string toSearch = Console.ReadLine().ToLower();
                    startTime = DateTime.Now.Ticks;
                    Console.Clear();
                    Utils.PrintLine();
                    Utils.PrintRow(ConsoleColor.White, new string[]{"ID","->Name<-",$"Price({Vars.Currency})","Count"});
                    foreach (Article article in Data.Articles)
                    {
                        if (article.name.ToLower().Contains(toSearch))
                        {
                            Utils.PrintRow(ConsoleColor.White, new string[]{article.id.ToString(), article.name, article.price.ToString(), article.count.ToString()});
                        }
                    }
                    Utils.PrintLine();
                }
                else if (input == "2")
                {
                    Console.Write("Price: ");
                    string toSearch = Console.ReadLine();
                    startTime = DateTime.Now.Ticks;
                    Console.Clear();
                    Utils.PrintLine();
                    Utils.PrintRow(ConsoleColor.White, new string[]{"ID","Name",$"->Price({Vars.Currency})<-","Count"});
                    foreach (Article article in Data.Articles)
                    {
                        if (article.price == Convert.ToDouble(toSearch))
                        {
                            Utils.PrintRow(ConsoleColor.White, new string[]{article.id.ToString(), article.name, article.price.ToString(), article.count.ToString()});
                        }
                    }
                    Utils.PrintLine();
                }
                else if (input == "3")
                {
                    Console.Write("Count: ");
                    string toSearch = Console.ReadLine();
                    startTime = DateTime.Now.Ticks;
                    Console.Clear();
                    Utils.PrintLine();
                    Utils.PrintRow(ConsoleColor.White, new string[]{"ID","Name",$"Price({Vars.Currency})","->Count<-"});
                    foreach (Article article in Data.Articles)
                    {
                        if (article.count == Convert.ToDouble(toSearch))
                        {
                            Utils.PrintRow(ConsoleColor.White, new string[]{article.id.ToString(), article.name, article.price.ToString(), article.count.ToString()});
                        }
                    }
                    Utils.PrintLine();
                }
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
            logger.AddLine("called");
            try
            {
                Console.Clear();
                Console.WriteLine("What do you want to sort by?");
                    Console.WriteLine("1: ID");
                    Console.WriteLine("2: Name");
                    Console.WriteLine("3: Price");
                    Console.WriteLine("4: Count");
                    Console.WriteLine();
                    Console.Write("Your input: ");
                    string input = Console.ReadLine().ToLower();
                    logger.AddLine("Sorting list");
                    List<Article> artList = Data.Articles;
                    if (input == "1" || input == "id")
                    {
                        artList = artList.OrderBy(x => x.id).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new []{"^ID^", "Name", "Price", "Count"});
                        foreach (Article art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, new []{art.id.ToString(), art.name, art.price.ToString(), art.count.ToString()});
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "2" || input == "name")
                    {
                        artList = artList.OrderBy(x => x.name).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new []{"ID", "^Name^", "Price", "Count"});
                        foreach (Article art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, new []{art.id.ToString(), art.name, art.price.ToString(), art.count.ToString()});
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "3" || input == "price" || input == "preis")
                    {
                        artList = artList.OrderBy(x => x.price).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new []{"ID", "Name", "^Price^", "Count"});
                        foreach (Article art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, new []{art.id.ToString(), art.name, art.price.ToString(), art.count.ToString()});
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "4" || input == "count" || input == "bestand")
                    {
                        artList = artList.OrderBy(x => x.count).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new []{"ID", "Name", "Price", "^Count^"});
                        foreach (Article art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, new []{art.id.ToString(), art.name, art.price.ToString(), art.count.ToString()});
                        }
                        Utils.PrintLine();
                    }
            }
            catch (Exception e)
            {
                logger.AddLine("An error occured");
                Console.WriteLine("An error occured");
                logger.AddLine(e.Message);
                Thread.Sleep(2500);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}