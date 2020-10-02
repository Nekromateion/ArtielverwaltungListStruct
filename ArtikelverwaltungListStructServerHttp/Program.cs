using System;
using System.Collections.Generic;
using ArtikelverwaltungListStructServerHttp;

namespace ArtikelverwaltungListStruct
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Artikelverwaltungs Server";
            Console.Write("Please input the server close key: ");
            string key = Console.ReadLine();
            Console.Clear();
            Server server = new Server(8080, key);
        }
    }
}