using System;

namespace ArtikelverwaltungClientWebsocket.Functions.ServerFunctions
{
    public class Editfunctions
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        
        internal static void AddArticle()
        {
            bool didWork = false;
            logger.AddLine("called");
            string id;
            string name;
            double price;
            int count;
            while (!didWork)
            {
                Console.Clear();
                try
                {
                    Console.WriteLine("Please enter some information about the article you want to add");
                    Console.Write("ID(A = Auto): ");
                    id = Console.ReadLine();
                    Console.Write("Name: ");
                    name = Console.ReadLine();
                    Console.Write("Price: ");
                    price = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Count: ");
                    count = Convert.ToInt32(Console.ReadLine());
                    didWork = true;
                }
                catch (Exception) { }
                Console.Clear();
            }
            ConnectionManager.socket.Send("");
        }

        internal static void RemoveArticle()
        {
            logger.AddLine("called");
        }
    }
}