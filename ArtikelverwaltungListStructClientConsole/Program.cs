using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading;
using ArtikelverwaltungListStruct;

namespace ArtikelverwaltungListStructClientConsole
{
    internal class Program
    {
        private static Thread upChecker = new Thread(() =>
        {
            int unreachablecount = 0;
            while (true)
            {
                try
                {
                    new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/status");
                    Thread.Sleep(5000);
                    unreachablecount = 0;
                }
                catch (Exception)
                {
                    unreachablecount++;
                }

                if (unreachablecount == 5)
                {
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
        private static string _currency = String.Empty;
        private static bool serverAvailable = false;
        private static bool doRun = true;
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                doRun = true;
                while (!serverAvailable)
                {
                    Console.Write("Please input the Servers IP: ");
                    string serverip = Console.ReadLine();
                    Console.Write("Please input the Servers Port: ");
                    string serverport = Console.ReadLine();
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
                    try
                    {
                        string response = new WebClient().DownloadString($"http://{serverip}:{serverport}/status");
                        writeThread.Abort();
                        if (response != "0")
                        {
                            Console.Clear();
                            Console.WriteLine("The given server was reachable but is not a valid server type in a valid ip and port...");
                            Thread.Sleep(2500);
                            Console.Clear();
                        }
                        else
                        {
                            Console.Clear();
                            serverAvailable = true;
                            _serverIp = serverip;
                            _serverPort = serverport;
                        }
                    }
                    catch (Exception)
                    {
                        writeThread.Abort();
                        Console.Clear();
                        Console.WriteLine("The given server is unreachable please type in a valid ip and port...");
                        Thread.Sleep(2500);
                        Console.Clear();
                    }
                }
                upChecker.Start();
                _currency = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/curr");

                while (doRun)
                {
                    Console.Clear();
                    Console.WriteLine("Please select one of the following options");
                    Console.WriteLine("e: Exit the application");
                    Console.WriteLine("c: Close the server");
                    Console.WriteLine("1: Read Current list");
                    Console.WriteLine("2: Add a new article");
                    Console.WriteLine("3: Delete a article");
                    Console.WriteLine("");
                    Console.Write("Your input: ");
                    string input = Console.ReadLine();
            
                    if(input == "1" || input == "read" || input == "list") ReadList();
                    else if (input == "2" || input == "add" || input == "put") AddArticle();
                    else if (input == "3" || input == "remove" || input == "delete" || input == "del") DelArticle();
                    else if (input == "e" || input == "exit") Environment.Exit(0xDEAD);
                    else if (input == "c" || input == "close" || input == "abort") CloseServer();
                    else
                    {
                        Console.WriteLine("That is not a valid input");
                        Thread.Sleep(2500);
                    }
                }
            }
        }

        private static void ReadList()
        {
            try
            {
                Console.Clear();
                string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/read");
                if (!response.StartsWith("1"))
                {
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
        
        private static void CloseServer()
        {
            try
            {
                Console.Clear();
                Console.Write("Please input the close key: ");
                string key = Console.ReadLine();
                Console.Clear();
                string response = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/close/{key}");
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