using System;
using System.Configuration;
using System.Net;
using System.Threading;

namespace ArtikelverwaltungListStructClientConsole
{
    internal class Program
    {
        private static string _serverIp = string.Empty;
        private static string _serverPort = string.Empty;
        private static string _currency = String.Empty;
        private static bool serverAvailable = false;
        private static bool doRun = true;
        public static void Main(string[] args)
        {
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
            _currency = new WebClient().DownloadString($"http://{_serverIp}:{_serverPort}/curr");

            while (true)
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

        private static void ReadList()
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
        
        private static void AddArticle()
        {
            Console.Clear();
            
        }
        
        private static void DelArticle()
        {
            Console.Clear();
            
        }
        
        private static void CloseServer()
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
            }
        }
    }
}