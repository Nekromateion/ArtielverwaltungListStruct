using System;
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
            }
            Console.WriteLine("Please select one of the following options");
            Console.WriteLine("1: Read Current list");
        }
    }
}