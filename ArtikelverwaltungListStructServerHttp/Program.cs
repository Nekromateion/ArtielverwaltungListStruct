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
            Server server = new Server(8080);
        }
    }
}