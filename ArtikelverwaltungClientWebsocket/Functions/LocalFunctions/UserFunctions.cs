using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Structs;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Utils;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Vars;

namespace ArtikelverwaltungClientWebsocket.Functions.LocalFunctions
{
    public static class UserFunctions
    {
        private static readonly Logger Logger =
            LogHandler.Logger;
        
        internal static void ReadList()
        {
            try
            {
                long startTimeReadPrint = DateTime.Now.Ticks;
                Logger.AddLine("called");
                Console.Clear();
                Utils.PrintLine();
                Utils.PrintRow(ConsoleColor.White, "ID", "Name", $"Price ({Vars.Currency})", "Count");
                foreach (Article article in Data.Articles)
                {
                    Utils.PrintRow(ConsoleColor.White, article.Id.ToString(), article.Name, article.Price.ToString(CultureInfo.InvariantCulture), article.Count.ToString());
                }
                Utils.PrintLine();
                long endTimeReadPrint = DateTime.Now.Ticks;
                Logger.AddLine($"reading and printing {Data.Articles.Count} took: Action took: {(endTimeReadPrint / TimeSpan.TicksPerMillisecond) - (startTimeReadPrint / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeReadPrint-startTimeReadPrint} ticks)");
                Console.Write("Press any key to return to the menu...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Logger.AddLine("An error occured");
                Console.WriteLine("An error occured");
                Logger.AddLine(e.Message);
                Thread.Sleep(2500);
            }
        }

        internal static void SearchList()
        {
            Logger.AddLine("called");
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
                    string toSearch = Console.ReadLine()?.ToLower();
                    startTime = DateTime.Now.Ticks;
                    Console.Clear();
                    Utils.PrintLine();
                    Utils.PrintRow(ConsoleColor.White, "ID", "->Name<-", $"Price({Vars.Currency})", "Count");
                    foreach (Article article in Data.Articles)
                    {
                        if (article.Name.ToLower().Contains(toSearch ?? "no input given"))
                        {
                            Utils.PrintRow(ConsoleColor.White, article.Id.ToString(), article.Name, article.Price.ToString(CultureInfo.InvariantCulture), article.Count.ToString());
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
                    Utils.PrintRow(ConsoleColor.White, "ID", "Name", $"->Price({Vars.Currency})<-", "Count");
                    foreach (Article article in Data.Articles)
                    {
                        if (Math.Abs(article.Price - Convert.ToDouble(toSearch ?? "no input given")) < 0.25)
                        {
                            Utils.PrintRow(ConsoleColor.White, article.Id.ToString(), article.Name, article.Price.ToString(CultureInfo.InvariantCulture), article.Count.ToString());
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
                    Utils.PrintRow(ConsoleColor.White, "ID", "Name", $"Price({Vars.Currency})", "->Count<-");
                    foreach (Article article in Data.Articles)
                    {
                        if (Math.Abs(article.Count - Convert.ToDouble(toSearch)) < 0)
                        {
                            Utils.PrintRow(ConsoleColor.White, article.Id.ToString(), article.Name, article.Price.ToString(CultureInfo.InvariantCulture), article.Count.ToString());
                        }
                    }
                    Utils.PrintLine();
                }
                long endTime = DateTime.Now.Ticks;
                Logger.AddLine($"reading and printing {Data.Articles.Count} took: Action took: {(endTime / TimeSpan.TicksPerMillisecond) - (startTime / TimeSpan.TicksPerMillisecond)} milliseconds ({endTime-startTime} ticks)");
                Console.Write("Press any key to return to the menu...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Logger.AddLine("An error occured");
                Console.WriteLine("An error occured");
                Logger.AddLine(e.Message);
                Thread.Sleep(2500);
            }
        }

        internal static void SortList()
        {
            Logger.AddLine("called");
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
                    string input = Console.ReadLine()?.ToLower();
                    Logger.AddLine("Sorting list");
                    List<Article> artList = Data.Articles;
                    if (input == "1" || input == "id")
                    {
                        artList = artList.OrderBy(x => x.Id).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "^ID^", "Name", "Price", "Count");
                        foreach (Article art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, art.Id.ToString(), art.Name, art.Price.ToString(CultureInfo.InvariantCulture), art.Count.ToString());
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "2" || input == "name")
                    {
                        artList = artList.OrderBy(x => x.Name).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "ID", "^Name^", "Price", "Count");
                        foreach (Article art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, art.Id.ToString(), art.Name, art.Price.ToString(CultureInfo.InvariantCulture), art.Count.ToString());
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "3" || input == "price" || input == "preis")
                    {
                        artList = artList.OrderBy(x => x.Price).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "ID", "Name", "^Price^", "Count");
                        foreach (Article art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, art.Id.ToString(), art.Name, art.Price.ToString(CultureInfo.InvariantCulture), art.Count.ToString());
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "4" || input == "count" || input == "bestand")
                    {
                        artList = artList.OrderBy(x => x.Count).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "ID", "Name", "Price", "^Count^");
                        foreach (Article art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, art.Id.ToString(), art.Name, art.Price.ToString(CultureInfo.InvariantCulture), art.Count.ToString());
                        }
                        Utils.PrintLine();
                    }
            }
            catch (Exception e)
            {
                Logger.AddLine("An error occured");
                Console.WriteLine("An error occured");
                Logger.AddLine(e.Message);
                Thread.Sleep(2500);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}