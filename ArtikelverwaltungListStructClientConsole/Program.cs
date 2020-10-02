using System;
using System.Net;
using System.Threading;

namespace ArtikelverwaltungListStructClientConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            bool serverAvailable = false;
            while (!serverAvailable)
            {
                Console.Write("Please input the Servers IP: ");
                string serverip = Console.ReadLine();
                Console.Write("Please input the Servers Port: ");
                string serverport = Console.ReadLine();
                Console.Clear();
                Thread writeThread = new Thread(() =>
                {
                    int delay = 20;
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
            Console.WriteLine("Please select one of the following options");
            Console.WriteLine("1: Read Current list");
        }
    }
}