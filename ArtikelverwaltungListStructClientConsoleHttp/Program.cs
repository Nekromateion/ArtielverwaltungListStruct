using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using ArtikelverwaltungListStructClientConsole;

namespace ArtikelverwaltungListStructClientConsoleHttp
{
    internal class Program
    {
        private static readonly Thread upChecker = new Thread(() =>
        {
            _logger.AddLine("upchecker thread init");
            var unreachablecount = 0;
            while (true)
            {
                try
                {
                    var startTime = DateTime.Now.Ticks;
                    _logger.AddLine("upchecker : starting check");
                    new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/status");
                    var endTime = DateTime.Now.Ticks;
                    _logger.AddLine(
                        $"upchecker : server is reachable Action took: {endTime / TimeSpan.TicksPerMillisecond - startTime / TimeSpan.TicksPerMillisecond} milliseconds ({endTime - startTime} ticks)");
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
        private static bool serverAvailable;
        private static bool doRun = true;
        private static readonly Logger _logger = new Logger();

        public static void Main(string[] args)
        {
            #region b4init

            var startTimeInit = DateTime.Now.Ticks;
            _logger.Init();

            #endregion

            while (true)
            {
                #region init

                Console.Clear();
                doRun = true;
                while (!serverAvailable)
                {
                    var endTimeInit = DateTime.Now.Ticks;
                    _logger.AddLine(
                        $"Init took {endTimeInit / TimeSpan.TicksPerMillisecond - startTimeInit / TimeSpan.TicksPerMillisecond} milliseconds ({endTimeInit - startTimeInit} ticks)");
                    Console.Write("Please input the Servers IP: ");
                    var serverip = Console.ReadLine();
                    _logger.AddLine($"User input for server ip was {serverip}");
                    Console.Write("Please input the Servers Port: ");
                    var serverport = Console.ReadLine();
                    _logger.AddLine($"User input for server port was {serverport}");
                    Console.Clear();
                    var writeThread = new Thread(() =>
                    {
                        var delay = 50;
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
                    var startTimeServerCheck = DateTime.Now.Ticks;
                    try
                    {
                        var response = new WebClient().DownloadString($"http://{serverip}:{serverport}/status");
                        var endTimeServerCheck = DateTime.Now.Ticks;
                        _logger.AddLine(
                            $"Server was reachable... Action took: {endTimeServerCheck / TimeSpan.TicksPerMillisecond - startTimeServerCheck / TimeSpan.TicksPerMillisecond} milliseconds ({endTimeServerCheck - startTimeServerCheck} ticks)");
                        writeThread.Abort();
                        _logger.AddLine("Stopped write thread");
                        if (response != "0")
                        {
                            _logger.AddLine("Server was reachable but is not compatible with this application");
                            Console.Clear();
                            Console.WriteLine(
                                "The given server was reachable but is not a valid server type in a valid ip and port...");
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
                        var endTimeServerCheck = DateTime.Now.Ticks;
                        _logger.AddLine(
                            $"Server was not reachable... Action took: {endTimeServerCheck / TimeSpan.TicksPerMillisecond - startTimeServerCheck / TimeSpan.TicksPerMillisecond} milliseconds ({endTimeServerCheck - startTimeServerCheck} ticks)");
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
                var startTimeCurrency = DateTime.Now.Ticks;
                _currency = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/curr");
                var endTimeCurrency = DateTime.Now.Ticks;
                _logger.AddLine(
                    $"currency request took: {endTimeCurrency / TimeSpan.TicksPerMillisecond - startTimeCurrency / TimeSpan.TicksPerMillisecond} milliseconds ({endTimeCurrency - startTimeCurrency} ticks)");

                #endregion

                #region adminKey

                Console.Clear();
                Console.WriteLine("Do you have the server admin key? (y = yes)");
                var inp = Console.ReadLine().ToLower();
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
                    var startTimeMenuPrint = DateTime.Now.Ticks;
                    Console.Clear();
                    Console.WriteLine("Please select one of the following options");
                    Console.WriteLine("e : Exit the application");
                    if (_key != string.Empty) Console.WriteLine("c : Close the server");
                    if (_key != string.Empty) Console.WriteLine("s : Save the list to servers disk");
                    if (_key != string.Empty) Console.WriteLine("cl: Clear the list on the server");
                    Console.WriteLine("1 : Read Current list");
                    Console.WriteLine("2 : Add a new article");
                    Console.WriteLine("3 : Delete a article");
                    Console.WriteLine("4 : Search in list");
                    Console.WriteLine("5 : Sort list");
                    Console.WriteLine("");
                    Console.Write("Your input: ");
                    var endTimeMenuPrint = DateTime.Now.Ticks;
                    _logger.AddLine(
                        $"printing menu took: {endTimeMenuPrint / TimeSpan.TicksPerMillisecond - startTimeMenuPrint / TimeSpan.TicksPerMillisecond} milliseconds ({endTimeMenuPrint - startTimeMenuPrint} ticks)");
                    var input = Console.ReadLine().ToLower();

                    if (input == "1" || input == "read" || input == "list")
                    {
                        ReadList();
                    }
                    else if (input == "2" || input == "add" || input == "put")
                    {
                        AddArticle();
                    }
                    else if (input == "3" || input == "remove" || input == "delete" || input == "del")
                    {
                        DelArticle();
                    }
                    else if (input == "4" || input == "search" || input == "seek")
                    {
                        SearchList();
                    }
                    else if (input == "5" || input == "sort" || input == "group")
                    {
                        SortList();
                    }
                    else if (input == "e" || input == "exit")
                    {
                        Environment.Exit(0xDEAD);
                    }
                    else if (input == "c" || input == "close" || input == "abort")
                    {
                        if (_key != string.Empty) CloseServer();
                    }
                    else if (input == "s" || input == "save" || input == "speichern")
                    {
                        if (_key != string.Empty) Save();
                    }
                    else if (input == "cl" || input == "clear" || input == "clean" || input == "empty")
                    {
                        if (_key != string.Empty) Clear();
                    }
                    else
                    {
                        Console.WriteLine("That is not a valid input");
                        Thread.Sleep(2500);
                    }
                }

                #endregion
            }
        }

        #region user

        private static void ReadList()
        {
            _logger.AddLine("Called");
            try
            {
                Console.Clear();
                var startTimeReq = DateTime.Now.Ticks;
                var response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/read");
                var endTimeReq = DateTime.Now.Ticks;
                _logger.AddLine(
                    $"read request was successful took: {endTimeReq / TimeSpan.TicksPerMillisecond - startTimeReq / TimeSpan.TicksPerMillisecond} milliseconds ({endTimeReq - startTimeReq} ticks)");
                if (!response.StartsWith("1"))
                {
                    var startTimeReadPrint = DateTime.Now.Ticks;
                    var artikelList = response.Split('~');
                    Utils.PrintLine();
                    Utils.PrintRow(ConsoleColor.White, "ID", "Name", $"Price ({_currency})", "Count");
                    foreach (var artikel in artikelList)
                    {
                        var list = artikel.Split('|');
                        var id = list[1].Replace("~", string.Empty).Replace("|", string.Empty);
                        var name = list[0].Replace("~", string.Empty).Replace("|", string.Empty);
                        var price = list[2].Replace("~", string.Empty).Replace("|", string.Empty);
                        var count = list[3].Replace("~", string.Empty).Replace("|", string.Empty);
                        Utils.PrintRow(ConsoleColor.White, id, name, price, count);
                    }

                    Utils.PrintLine();
                    var endTimeReadPrint = DateTime.Now.Ticks;
                    _logger.AddLine(
                        $"reading and printing {artikelList.Length} took: Action took: {endTimeReadPrint / TimeSpan.TicksPerMillisecond - startTimeReadPrint / TimeSpan.TicksPerMillisecond} milliseconds ({endTimeReadPrint - startTimeReadPrint} ticks)");
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
            _logger.AddLine("Called");
            try
            {
                Console.Clear();
                Console.WriteLine("Please input the details about the article:");
                Console.Write("ID: ");
                var id = Console.ReadLine();
                Console.Write("Name: ");
                var name = Console.ReadLine();
                Console.Write("Price: ");
                var price = Console.ReadLine();
                Console.Write("Count: ");
                var count = Console.ReadLine();
                Console.Clear();
                var response =
                    new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/add/{name}/{id}/{price}/{count}");
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
            _logger.AddLine("Called");
            try
            {
                Console.Clear();
                Console.WriteLine("Please select by what criteria you want to delete:");
                Console.WriteLine("1: Name");
                Console.WriteLine("2: ID");
                Console.WriteLine("");
                Console.Write("Your input: ");
                var input = Console.ReadLine();
                Console.Clear();
                if (input == "1")
                {
                    Console.Write("Name: ");
                    var name = Console.ReadLine();
                    var response =
                        new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/remove/name/{name}");
                    if (response != "0")
                    {
                        Console.WriteLine(response);
                        Thread.Sleep(2500);
                    }
                }
                else if (input == "2")
                {
                    Console.Write("ID: ");
                    var Id = Console.ReadLine();
                    var response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/remove/name/{Id}");
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
                var startTimeReq = DateTime.Now.Ticks;
                var response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/read");
                var endTimeReq = DateTime.Now.Ticks;
                _logger.AddLine(
                    $"read request was successful took: {endTimeReq / TimeSpan.TicksPerMillisecond - startTimeReq / TimeSpan.TicksPerMillisecond} milliseconds ({endTimeReq - startTimeReq} ticks)");
                if (!response.StartsWith("1"))
                {
                    _logger.AddLine("Starting to process request");
                    var artikelList = response.Split('~');
                    _logger.AddLine("converted to array");
                    var artList = new List<Artikel>();
                    _logger.AddLine("starting to add articles to the list");
                    foreach (var artikel in artikelList)
                    {
                        var list = artikel.Split('|');
                        var temp = new Artikel();
                        temp.Nummer = Convert.ToInt32(list[1].Replace("~", string.Empty).Replace("|", string.Empty));
                        temp.Name = list[0].Replace("~", string.Empty).Replace("|", string.Empty);
                        temp.Preis = Convert.ToDouble(list[2].Replace("~", string.Empty).Replace("|", string.Empty));
                        temp.Bestand = Convert.ToInt32(list[3].Replace("~", string.Empty).Replace("|", string.Empty));
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
                    var input = Console.ReadLine().ToLower();
                    _logger.AddLine("Sorting list");
                    if (input == "1" || input == "id")
                    {
                        artList = artList.OrderBy(x => x.Nummer).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "^ID^", "Name", "Price", "Count");
                        foreach (var art in artList)
                            Utils.PrintRow(ConsoleColor.White, art.Nummer.ToString(), art.Name, art.Preis.ToString(),
                                art.Bestand.ToString());
                        Utils.PrintLine();
                    }
                    else if (input == "2" || input == "name")
                    {
                        artList = artList.OrderBy(x => x.Name).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "ID", "^Name^", "Price", "Count");
                        foreach (var art in artList)
                            Utils.PrintRow(ConsoleColor.White, art.Nummer.ToString(), art.Name, art.Preis.ToString(),
                                art.Bestand.ToString());
                        Utils.PrintLine();
                    }
                    else if (input == "3" || input == "price" || input == "preis")
                    {
                        artList = artList.OrderBy(x => x.Preis).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "ID", "Name", "^Price^", "Count");
                        foreach (var art in artList)
                            Utils.PrintRow(ConsoleColor.White, art.Nummer.ToString(), art.Name, art.Preis.ToString(),
                                art.Bestand.ToString());
                        Utils.PrintLine();
                    }
                    else if (input == "4" || input == "count" || input == "bestand")
                    {
                        artList = artList.OrderBy(x => x.Bestand).ToList();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "ID", "Name", "Price", "^Count^");
                        foreach (var art in artList)
                            Utils.PrintRow(ConsoleColor.White, art.Nummer.ToString(), art.Name, art.Preis.ToString(),
                                art.Bestand.ToString());
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

        private static void SearchList()
        {
            try
            {
                Console.Clear();
                var response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/read");
                if (!response.StartsWith("1"))
                {
                    var all = new List<Artikel>();
                    var artikelList = response.Split('~');
                    foreach (var artikel in artikelList)
                    {
                        var temp = new Artikel();
                        var list = artikel.Split('|');
                        temp.Nummer = Convert.ToInt32(list[1].Replace("~", string.Empty).Replace("|", string.Empty));
                        temp.Name = list[0].Replace("~", string.Empty).Replace("|", string.Empty);
                        temp.Preis = Convert.ToDouble(list[2].Replace("~", string.Empty).Replace("|", string.Empty));
                        temp.Bestand = Convert.ToInt32(list[3].Replace("~", string.Empty).Replace("|", string.Empty));
                        all.Add(temp);
                    }

                    Console.WriteLine("By what do you want to search?");
                    Console.WriteLine("1: Name");
                    Console.WriteLine("2: Price");
                    Console.WriteLine("3: Count");
                    Console.WriteLine("");
                    Console.Write("Your input: ");
                    var input = Console.ReadLine();
                    Console.Clear();
                    if (input == "1")
                    {
                        Console.Write("Name: ");
                        var toSearch = Console.ReadLine().ToLower();
                        Console.Clear();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "ID", "Name", $"Price({_currency})", "Count");
                        foreach (var artikel in all)
                            if (artikel.Name.ToLower().Contains(toSearch))
                                Utils.PrintRow(ConsoleColor.White, artikel.Nummer.ToString(), artikel.Name,
                                    artikel.Preis.ToString(), artikel.Bestand.ToString());
                        Utils.PrintLine();
                    }
                    else if (input == "2")
                    {
                        Console.Write("Price: ");
                        var toSearch = Console.ReadLine();
                        Console.Clear();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "ID", "Name", $"Price({_currency})", "Count");
                        foreach (var artikel in all)
                            if (artikel.Preis == Convert.ToDouble(toSearch))
                                Utils.PrintRow(ConsoleColor.White, artikel.Nummer.ToString(), artikel.Name,
                                    artikel.Preis.ToString(), artikel.Bestand.ToString());
                        Utils.PrintLine();
                    }
                    else if (input == "3")
                    {
                        Console.Write("Count: ");
                        var toSearch = Console.ReadLine();
                        Console.Clear();
                        Utils.PrintLine();
                        Utils.PrintRow(ConsoleColor.White, "ID", "Name", $"Price({_currency})", "Count");
                        foreach (var artikel in all)
                            if (artikel.Bestand == Convert.ToInt32(toSearch))
                                Utils.PrintRow(ConsoleColor.White, artikel.Nummer.ToString(), artikel.Name,
                                    artikel.Preis.ToString(), artikel.Bestand.ToString());
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

        #endregion

        #region administrative

        private static void CloseServer()
        {
            try
            {
                Console.Clear();
                var response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/close/{_key}");
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
                var response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/save/{_key}");
                if (response == "0") Console.Clear();
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
                var response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/clear/{_key}");
                if (response == "0") Console.Clear();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("An error occured");
                Console.WriteLine(e);
                Thread.Sleep(2500);
            }
        }

        #endregion
    }
}