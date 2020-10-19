using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using ArtikelverwaltungListStructServerHttp;

namespace ArtikelverwaltungListStruct
{
    internal class Program
    {
        public static string currency = String.Empty;
        public static string Ip = String.Empty;
        
        public static void Main(string[] args)
        {
            Console.Title = "Artikelverwaltungs Server";
            Console.Write("Please enter the IP you want the server to use: ");
            Ip = Console.ReadLine();
            Console.Clear();
            Console.Write("Please input the admin key: ");
            string key = Console.ReadLine();
            Console.Clear();
            Console.Write("Please input the currency you want the server to use: ");
            currency = Console.ReadLine();
            Console.WriteLine("Please input the port you want the server to use: ");
            int port = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Server server = new Server(port, key);
        }
    }
}