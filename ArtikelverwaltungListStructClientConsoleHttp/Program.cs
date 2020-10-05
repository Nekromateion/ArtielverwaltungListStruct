using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using ArtikelverwaltungListStruct;
using ArtikelverwaltungListStructClientConsole;

namespace ArtikelverwaltungListStructClientConsoleHttp
{
    internal class Program
    {
        private static Thread upChecker = new Thread(() =>
        {
            _logger.AddLine("upchecker thread init");
            int unreachablecount = 0;
            while (true)
            {
                try
                {
                    long startTime = DateTime.Now.Ticks;
                    _logger.AddLine("upchecker : starting check");
                    new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/status");
                    long endTime = DateTime.Now.Ticks;
                    _logger.AddLine($"upchecker : server is reachable Action took: {(endTime / TimeSpan.TicksPerMillisecond) - (startTime / TimeSpan.TicksPerMillisecond)} milliseconds ({endTime-startTime} ticks)");
                    Thread.Sleep(5000);
                    unreachablecount = 0;
                }
                catch (Exception)
                {
                    unreachablecount++;
                    _logger.AddLine($"upchecker : !!! SERVER NOT REACHABLE !!! ({unreachablecount})");
                }

                if (unreachablecount == 5)
                {
                    _logger.AddLine("upchecker : !!! SERVER WAS NOT REACHABLE FOR 5 TRIES ABORTING CONNECTION !!!");
                    Console.Clear();
                    Console.WriteLine("The connection the server unexpectedly closed");
                    Thread.Sleep(2500);
                    doRun = false;
                    serverAvailable = false;
                }
            }
        });
        
        private static string _serverIp = string.Empty;
        private static string _serverPort = string.Empty;
        private static string _currency = string.Empty;
        private static string _key = string.Empty;
        private static bool serverAvailable = false;
        private static bool doRun = true;
        private static Logger _logger = new Logger();
        public static void Main(string[] args)
        {
            #region b4init
            long startTimeInit = DateTime.Now.Ticks;
            Directory.CreateDirectory("Logs");
            _logger.Init();
            #endregion
            while (true)
            {
                #region init
                Console.Clear();
                doRun = true;
                while (!serverAvailable)
                {
                    long endTimeInit = DateTime.Now.Ticks;
                    _logger.AddLine($"Init took {(endTimeInit / TimeSpan.TicksPerMillisecond) - (startTimeInit / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeInit-startTimeInit} ticks)");
                    Console.Write("Please input the Servers IP: ");
                    string serverip = Console.ReadLine();
                    _logger.AddLine($"User input for server ip was {serverip}");
                    Console.Write("Please input the Servers Port: ");
                    string serverport = Console.ReadLine();
                    _logger.AddLine($"User input for server port was {serverport}");
                    Console.Clear();
                    Thread writeThread = new Thread(() =>
                    {
                        int delay = 50;
                        while (true)
                        {
                            Console.WriteLine("Checking if server is reachable -");
                            Thread.Sleep(delay);
                            Console.Clear();
                            Console.WriteLine("Checking if server is reachable \\");
                            Thread.Sleep(delay);
                            Console.Clear();
                            Console.WriteLine("Checking if server is reachable |");
                            Thread.Sleep(delay);
                            Console.Clear();
                            Console.WriteLine("Checking if server is reachable /");
                            Thread.Sleep(delay);
                            Console.Clear();
                            Console.WriteLine("Checking if server is reachable -");
                            Thread.Sleep(delay);
                            Console.Clear();
                            Console.WriteLine("Checking if server is reachable \\");
                            Thread.Sleep(delay);
                            Console.Clear();
                            Console.WriteLine("Checking if server is reachable |");
                            Thread.Sleep(delay);
                            Console.Clear();
                            Console.WriteLine("Checking if server is reachable /");
                            Thread.Sleep(delay);
                            Console.Clear();
                        }
                    });
                    writeThread.Start();
                    _logger.AddLine("Started write thread for waiting animation");
                    long startTimeServerCheck = DateTime.Now.Ticks;
                    try
                    {
                        string response = new WebClient().DownloadString($"http://{serverip}:{serverport}/status");
                        long endTimeServerCheck = DateTime.Now.Ticks;
                        _logger.AddLine($"Server was reachable... Action took: {(endTimeServerCheck / TimeSpan.TicksPerMillisecond) - (startTimeServerCheck / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeServerCheck-startTimeServerCheck} ticks)");
                        writeThread.Abort();
                        _logger.AddLine("Stopped write thread");
                        if (response != "0")
                        {
                            _logger.AddLine("Server was reachable but is not compatible with this application");
                            Console.Clear();
                            Console.WriteLine("The given server was reachable but is not a valid server type in a valid ip and port...");
                            Thread.Sleep(2500);
                            Console.Clear();
                        }
                        else
                        {
                            _logger.AddLine("Server was reachable and is a valid server");
                            Console.Clear();
                            serverAvailable = true;
                            _serverIp = serverip;
                            _serverPort = serverport;
                        }
                    }
                    catch (Exception)
                    {
                        long endTimeServerCheck = DateTime.Now.Ticks;
                        _logger.AddLine($"Server was not reachable... Action took: {(endTimeServerCheck / TimeSpan.TicksPerMillisecond) - (startTimeServerCheck / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeServerCheck-startTimeServerCheck} ticks)");
                        writeThread.Abort();
                        _logger.AddLine("Stopped write thread");
                        Console.Clear();
                        Console.WriteLine("The given server is unreachable please type in a valid ip and port...");
                        Thread.Sleep(2500);
                        Console.Clear();
                    }
                }
                upChecker.Start();
                _logger.AddLine("Started up checker thread");
                long startTimeCurrency = DateTime.Now.Ticks;
                _currency = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/curr");
                long endTimeCurrency = DateTime.Now.Ticks;
                _logger.AddLine($"currency request took: {(endTimeCurrency / TimeSpan.TicksPerMillisecond) - (startTimeCurrency / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeCurrency-startTimeCurrency} ticks)");
                #endregion
                #region adminKey
                Console.Clear();
                Console.WriteLine("Do you have the server admin key? (y = yes)");
                string inp = Console.ReadLine().ToLower();
                if (inp == "y" || inp == "yes")
                {
                    Console.Clear();
                    Console.Write("Please input the admin key: ");
                    _key = Console.ReadLine();
                }
                Console.Clear();
                #endregion
                #region menu
                while (doRun)
                {
                    long startTimeMenuPrint = DateTime.Now.Ticks;
                    Console.Clear();
                    Console.WriteLine("Please select one of the following options");
                    Console.WriteLine("e : Exit the application");
                    if(_key != string.Empty) Console.WriteLine("c : Close the server");
                    if(_key != string.Empty) Console.WriteLine("s : Save the list to servers disk");
                    if(_key != string.Empty) Console.WriteLine("cl: Clear the list on the server");
                    Console.WriteLine("1 : Read Current list");
                    Console.WriteLine("2 : Add a new article");
                    Console.WriteLine("3 : Delete a article");
                    Console.WriteLine("4 : Search in list");
                    Console.WriteLine("5 : Sort list");
                    Console.WriteLine("");
                    Console.Write("Your input: ");
                    long endTimeMenuPrint = DateTime.Now.Ticks;
                    _logger.AddLine($"printing menu took: {(endTimeMenuPrint / TimeSpan.TicksPerMillisecond) - (startTimeMenuPrint / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeMenuPrint-startTimeMenuPrint} ticks)");
                    string input = Console.ReadLine().ToLower();

                    if(input == "1" || input == "read" || input == "list") ReadList();
                    else if (input == "2" || input == "add" || input == "put") AddArticle();
                    else if (input == "3" || input == "remove" || input == "delete" || input == "del") DelArticle();
                    else if (input == "4" || input == "search" || input == "seek") SearchList();
                    else if (input == "5" || input == "sort" || input == "group") SortList();
                    else if (input == "e" || input == "exit") Environment.Exit(0xDEAD);
                    else if (input == "c" || input == "close" || input == "abort"){ if(_key != string.Empty) CloseServer(); }
                    else if (input == "s" || input == "save" || input == "speichern"){if(_key != string.Empty) Save(); }
                    else if (input == "cl" || input == "clear" || input == "clean" || input == "empty"){if(_key != string.Empty) Clear(); }
                    else
                    {
                        Console.WriteLine("That is not a valid input");
                        Thread.Sleep(2500);
                    }
                }
                #endregion
            }
        }

        private static void ReadList()
        {
            _logger.AddLine("Called");
            try
            {
                Console.Clear();
                long startTimeReq = DateTime.Now.Ticks;
                string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/read");
                long endTimeReq = DateTime.Now.Ticks;
                _logger.AddLine($"read request was successful took: {(endTimeReq / TimeSpan.TicksPerMillisecond) - (startTimeReq / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeReq-startTimeReq} ticks)");
                if (!response.StartsWith("1"))
                {
                    long startTimeReadPrint = DateTime.Now.Ticks;
                    string[] artikelList = response.Split('~');
                    Utils.PrintLine();
                    Utils.PrintRow(ConsoleColor.White, new string[]{"ID", "Name", $"Price ({_currency})", "Count"});
                    foreach (string artikel in artikelList)
                    {
                        string[] list = artikel.Split('|');
                        string id = list[1].Replace("~", string.Empty).Replace("|", String.Empty);
                        string name = list[0].Replace("~", string.Empty).Replace("|", String.Empty);
                        string price = list[2].Replace("~", string.Empty).Replace("|", String.Empty);
                        string count = list[3].Replace("~", string.Empty).Replace("|", String.Empty);
                        Utils.PrintRow(ConsoleColor.White, new string[]{id, name, price, count});
                    }
                    Utils.PrintLine();
                    long endTimeReadPrint = DateTime.Now.Ticks;
                    _logger.AddLine($"reading and printing {artikelList.Length} took: Action took: {(endTimeReadPrint / TimeSpan.TicksPerMillisecond) - (startTimeReadPrint / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeReadPrint-startTimeReadPrint} ticks)");
                }
                else
                {
                    _logger.AddLine("Server returned a unprocessable string " + response);
                    Console.WriteLine(response);
                }

                Console.ReadKey();
            }
            catch (Exception e)
            {
                _logger.AddLine("An error occured");
                Console.WriteLine("An error occured");
                _logger.AddLine(e.Message);
                Console.WriteLine(e);
                Thread.Sleep(2500);
            }
        }
        
        private static void AddArticle()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Please input the details about the article:");
                Console.Write("ID: ");
                string id = Console.ReadLine();
                Console.Write("Name: ");
                string name = Console.ReadLine();
                Console.Write("Price: ");
                string price = Console.ReadLine();
                Console.Write("Count: ");
                string count = Console.ReadLine();
                Console.Clear();
                string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/add/{name}/{id}/{price}/{count}");
                if (response != "0")
                {
                    Console.WriteLine("IDK how you got here but you did so dafuq?!");
                    Thread.Sleep(2500);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured");
                Console.WriteLine(e);
                Thread.Sleep(2500);
            }
        }
        
        private static void DelArticle()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Please select by what criteria you want to delete:");
                Console.WriteLine("1: Name");
                Console.WriteLine("2: ID");
                Console.WriteLine("");
                Console.Write("Your input: ");
                string input = Console.ReadLine();
                Console.Clear();
                if (input == "1")
                {
                    Console.Write("Name: ");
                    string name = Console.ReadLine();
                    string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/remove/name/{name}");
                    if (response != "0")
                    {
                        Console.WriteLine(response);
                        Thread.Sleep(2500);
                    }
                }
                else if (input == "2")
                {
                    Console.Write("ID: ");
                    string Id = Console.ReadLine();
                    string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/remove/name/{Id}");
                    if (response != "0")
                    {
                        Console.WriteLine(response);
                        Thread.Sleep(2500);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured");
                Console.WriteLine(e);
                Thread.Sleep(2500);
            }
        }
        
        private static void SortList()
        {
            _logger.AddLine("Called");
            try
            {
                Console.Clear();
                long startTimeReq = DateTime.Now.Ticks;
                string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/read");
                long endTimeReq = DateTime.Now.Ticks;
                _logger.AddLine($"read request was successful took: {(endTimeReq / TimeSpan.TicksPerMillisecond) - (startTimeReq / TimeSpan.TicksPerMillisecond)} milliseconds ({endTimeReq-startTimeReq} ticks)");
                if (!response.StartsWith("1"))
                {
                    _logger.AddLine("Starting to process request");
                    string[] artikelList = response.Split('~');
                    _logger.AddLine("converted to array");
                    List<Artikel> artList = new List<Artikel>();
                    _logger.AddLine("starting to add articles to the list");
                    foreach (string artikel in artikelList)
                    {
                        string[] list = artikel.Split('|');
                        Artikel temp = new Artikel();
                        temp.nummer = Convert.ToInt32(list[1].Replace("~", string.Empty).Replace("|", String.Empty));
                        temp.name = list[0].Replace("~", string.Empty).Replace("|", String.Empty);
                        temp.preis = Convert.ToDouble(list[2].Replace("~", string.Empty).Replace("|", String.Empty));
                        temp.bestand = Convert.ToInt32(list[3].Replace("~", string.Empty).Replace("|", String.Empty));
                        artList.Add(temp);
                    }
                    _logger.AddLine("added all articles to the list");
                    Console.WriteLine("What do you want to sort by?");
                    Console.WriteLine("1: ID");
                    Console.WriteLine("2: Name");
                    Console.WriteLine("3: Price");
                    Console.WriteLine("4: Count");
                    Console.WriteLine();
                    Console.Write("Your input: ");
                    string input = Console.ReadLine().ToLower();
                    _logger.AddLine("Sorting list");
                    if (input == "1" || input == "id")
                    {
                        artList = artList.OrderBy(x => x.nummer).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new []{"^ID^", "Name", "Price", "Count"});
                        foreach (Artikel art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, new []{art.nummer.ToString(), art.name, art.preis.ToString(), art.bestand.ToString()});
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "2" || input == "name")
                    {
                        artList = artList.OrderBy(x => x.name).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new []{"ID", "^Name^", "Price", "Count"});
                        foreach (Artikel art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, new []{art.nummer.ToString(), art.name, art.preis.ToString(), art.bestand.ToString()});
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "3" || input == "price" || input == "preis")
                    {
                        artList = artList.OrderBy(x => x.preis).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new []{"ID", "Name", "^Price^", "Count"});
                        foreach (Artikel art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, new []{art.nummer.ToString(), art.name, art.preis.ToString(), art.bestand.ToString()});
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "4" || input == "count" || input == "bestand")
                    {
                        artList = artList.OrderBy(x => x.bestand).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new []{"ID", "Name", "Price", "^Count^"});
                        foreach (Artikel art in artList)
                        {
                            Utils.PrintRow(ConsoleColor.White, new []{art.nummer.ToString(), art.name, art.preis.ToString(), art.bestand.ToString()});
                        }
                        Utils.PrintLine();
                    }
                    
                    
                    _logger.AddLine($"reading, sorting and printing {artikelList.Length} done");
                }
                else
                {
                    _logger.AddLine("Server returned a unprocessable string " + response);
                    Console.WriteLine(response);
                }

                Console.ReadKey();
            }
            catch (Exception e)
            {
                _logger.AddLine("An error occured");
                Console.WriteLine("An error occured");
                _logger.AddLine(e.Message);
                Console.WriteLine(e);
                Thread.Sleep(2500);
            }
        }
        
        private static void CloseServer()
        {
            try
            {
                Console.Clear();
                string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/close/{_key}");
                if (response == "0")
                {
                    Console.WriteLine("Server is closing...");
                    Thread.Sleep(2500);
                    doRun = false;
                    serverAvailable = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured");
                Console.WriteLine(e);
                Thread.Sleep(2500);
            }
        }
        
        private static void Save()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Server is saving all data...");
                string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/save/{_key}");
                if (response == "0")
                {
                    Console.Clear();
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("An error occured");
                Console.WriteLine(e);
                Thread.Sleep(2500);
            }
        }
        
        private static void Clear()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Server clearing all data...");
                string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/clear/{_key}");
                if (response == "0")
                {
                    Console.Clear();
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("An error occured");
                Console.WriteLine(e);
                Thread.Sleep(2500);
            }
        }
        
        private static void SearchList()
        {
            try
            {
                Console.Clear();
                string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/read");
                if (!response.StartsWith("1"))
                {
                    List<Artikel> all = new List<Artikel>();
                    string[] artikelList = response.Split('~');
                    foreach (string artikel in artikelList)
                    {
                        Artikel temp = new Artikel();
                        string[] list = artikel.Split('|');
                        temp.nummer = Convert.ToInt32(list[1].Replace("~", string.Empty).Replace("|", String.Empty));
                        temp.name = list[0].Replace("~", string.Empty).Replace("|", String.Empty);
                        temp.preis = Convert.ToDouble(list[2].Replace("~", string.Empty).Replace("|", String.Empty));
                        temp.bestand = Convert.ToInt32(list[3].Replace("~", string.Empty).Replace("|", String.Empty));
                        all.Add(temp);
                    }
                    Console.WriteLine("By what do you want to search?");
                    Console.WriteLine("1: Name");
                    Console.WriteLine("2: Price");
                    Console.WriteLine("3: Count");
                    Console.WriteLine("");
                    Console.Write("Your input: ");
                    string input = Console.ReadLine();
                    Console.Clear();
                    if (input == "1")
                    {
                        Console.Write("Name: ");
                        string toSearch = Console.ReadLine();
                        Console.Clear();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new string[]{"ID","Name",$"Price({_currency})","Count"});
                        foreach (Artikel artikel in all)
                        {
                            if (artikel.name.Contains(toSearch))
                            {
                                Utils.PrintRow(ConsoleColor.White, new string[]{artikel.nummer.ToString(), artikel.name, artikel.preis.ToString(), artikel.bestand.ToString()});
                            }
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "2")
                    {
                        Console.Write("Price: ");
                        string toSearch = Console.ReadLine();
                        Console.Clear();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new string[]{"ID","Name",$"Price({_currency})","Count"});
                        foreach (Artikel artikel in all)
                        {
                            if (artikel.preis == Convert.ToDouble(toSearch))
                            {
                                Utils.PrintRow(ConsoleColor.White, new string[]{artikel.nummer.ToString(), artikel.name, artikel.preis.ToString(), artikel.bestand.ToString()});
                            }
                        }
                        Utils.PrintLine();
                    }
                    else if (input == "3")
                    {
                        Console.Write("Count: ");
                        string toSearch = Console.ReadLine();
                        Console.Clear();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, new string[]{"ID","Name",$"Price({_currency})","Count"});
                        foreach (Artikel artikel in all)
                        {
                            if (artikel.bestand == Convert.ToInt32(toSearch))
                            {
                                Utils.PrintRow(ConsoleColor.White, new string[]{artikel.nummer.ToString(), artikel.name, artikel.preis.ToString(), artikel.bestand.ToString()});
                            }
                        }
                        Utils.PrintLine();
                    }
                }
                else
                {
                    Console.WriteLine(response);
                }

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured");
                Console.WriteLine(e);
                Thread.Sleep(2500);
            }
        }
    }
}