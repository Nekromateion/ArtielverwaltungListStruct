using System;
using System.Threading;
using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Structs;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Vars;

namespace ArtikelverwaltungClientWebsocket.Functions.ServerFunctions
{
    public static class Editfunctions
    {
        private static readonly Logger Logger =
            LogHandler.Logger;
        
        internal static void AddArticle()
        {
            bool didWork = false;
            Logger.AddLine("called");
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
                catch (Exception)
                {
                    // ignored
                }

                Console.Clear();
            }

            if (Vars.AdminKey != null)
            {
                ConnectionManager.Socket.Send("add " + Vars.AdminKey + "~" + id + "|" + name + "|" + price + "|" + count);
            }
            else
            {
                ConnectionManager.Socket.Send("add " + Vars.EditKey + "~" + id + "|" + name + "|" + price + "|" + count);
            }
        }

        internal static void RemoveArticle()
        {
            Logger.AddLine("called");
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
                try
                {
                    Console.Write("Name: ");
                    string name = Console.ReadLine();
                    Article toremove = new Article();
                    bool done = false;
                    int count = 0;
                    foreach (Article article in Data.Articles)
                    {
                        if (name != null && article.Name.ToLower().Contains(name.ToLower()) && !done)
                        {
                            toremove = Data.Articles[count];
                            done = true;
                        }
                        count++;
                    }

                    if (done)
                    {
                        if (Vars.AdminKey != null)
                        {
                            ConnectionManager.Socket.Send("remove " + Vars.AdminKey + "~" + toremove.Id + "|" + toremove.Name + "|" + toremove.Price + "|" + toremove.Count);
                        }
                        else
                        {
                            ConnectionManager.Socket.Send("remove " + Vars.EditKey + "~" + toremove.Id + "|" + toremove.Name + "|" + toremove.Price + "|" + toremove.Count);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No item with that name was found in the list");
                        Thread.Sleep(2500);
                        Console.Clear();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                    Thread.Sleep(2500);
                }
            }
            else if (input == "2")
            {
                try
                {
                    Console.Write("ID: ");
                    string id = Console.ReadLine();
                    Article toremove = new Article();
                    bool done = false;
                    int count = 0;
                    foreach (Article article in Data.Articles)
                    {
                        if (article.Id == Convert.ToInt32(id) && !done)
                        {
                            toremove = Data.Articles[count];
                            done = true;
                        }
                        count++;
                    }

                    if (done)
                    {
                        if (Vars.AdminKey != null)
                        {
                            ConnectionManager.Socket.Send("remove " + Vars.AdminKey + "~" + toremove.Id + "|" + toremove.Name + "|" + toremove.Price + "|" + toremove.Count);
                        }
                        else
                        {
                            ConnectionManager.Socket.Send("remove " + Vars.EditKey + "~" + toremove.Id + "|" + toremove.Name + "|" + toremove.Price + "|" + toremove.Count);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No item with that id was found in the list");
                        Thread.Sleep(2500);
                        Console.Clear();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                    Thread.Sleep(2500);
                }
            }
        }
    }
}