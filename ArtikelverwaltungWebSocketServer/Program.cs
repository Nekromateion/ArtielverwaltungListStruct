using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ArtikelverwaltungWebSocketServer
{
    internal class Program
    {

        public class Client : WebSocketBehavior
        {
            private static int connections = 0;
            private static int activeConnections = 0;

            protected override void OnMessage(MessageEventArgs e)
            {
                Console.WriteLine("New message");
                if (e.IsText)
                {
                    Console.WriteLine("Message was text");
                    Console.WriteLine(e.Data);
                    if (e.Data == "request assembly")
                    {
                        Console.WriteLine("Client requested assembly");
                        Send(System.IO.File.ReadAllBytes("ArtikelverwaltungClientWebsocket.dll"));
                    }
                    else if (e.Data == "request data")
                    {
                        Console.WriteLine("Cleint Requested data");
                        string data = "data req ";
                        if (Data.Articles.Count > 0)
                        {
                            Console.WriteLine("1: List is empty");
                            data = "1: Nothing in list";
                            Send(data);
                        }
                        else
                        {
                            int count = 0;
                            foreach (Article article in Data.Articles)
                            {
                                count++;
                                if (count != Data.Articles.Count)
                                {
                                    data += article.id + "|";
                                    data += article.name + "|";
                                    data += article.price + "|";
                                    data += article.count + "~";
                                }
                                else
                                {
                                    data += article.id + "|";
                                    data += article.name + "|";
                                    data += article.price + "|";
                                    data += article.count;
                                }
                            }
                            Send(data);
                        }
                    }
                }
                else
                {
                    Send("Invalid request");
                }
            }

            protected override void OnOpen()
            {
                connections++;
                activeConnections++;
                Console.WriteLine($"New Connection Connections: [{connections}] Active: [{activeConnections}]");
                Sessions.Broadcast("status " + connections + " " + activeConnections);
            }

            protected override void OnClose(CloseEventArgs e)
            {
                activeConnections--;
                Console.WriteLine("Connection Closed");
                Sessions.Broadcast("status " + connections + " " + activeConnections);
            }

            protected override void OnError(ErrorEventArgs e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void Main(string[] args)
        {
            WebSocketServer socket;
            Console.WriteLine("Do you want to start the server global or only local? (g = global | l = local)");
            string input = Console.ReadLine();
            if (input == "g")
            {
                socket = new WebSocketServer(80);   
            }
            else
            {
                socket = new WebSocketServer("ws://127.0.0.1");
            }
            socket.AddWebSocketService<Client>("/artikelverwaltung");
            socket.Start();
            Console.WriteLine("Server started");
            Console.ReadLine();
        }
    }
}