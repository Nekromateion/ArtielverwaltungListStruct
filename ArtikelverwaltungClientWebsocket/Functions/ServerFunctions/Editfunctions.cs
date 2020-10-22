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
            string id = String.Empty;
            string name = string.Empty;
            double price = 0;
            int count = 0;
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
            ConnectionManager.socket.Send("add " + Vars.EditKey + "~" + id + "|" + name + "|" + price + "|" + count);
        }

        internal static void RemoveArticle()
        {
            logger.AddLine("called");
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
                
            }
            else if (input == "2")
            {
                
            }
        }
    }
}