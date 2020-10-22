using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using WebSocketSharp;
using WebSocketSharp.Server;
using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

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

                    #region UserFunction
                    #region assemblyRequest
                    if (e.Data == "request assembly")
                    {
                        try
                        {
                            Console.WriteLine("Client requested assembly");
                            Send(System.IO.File.ReadAllBytes("ArtikelverwaltungClientWebsocket.dll"));
                        }
                        catch (Exception)
                        {
                            Send("3: Something went wrong");
                        }
                    }
                    #endregion
                    #region getCurrency
                    else if (e.Data == "get currency")
                    {
                        try
                        {
                            Send("currency req " + Vars.Currency);
                        }
                        catch (Exception)
                        {
                            Send("3: Something went wrong");
                        }
                    }
                    #endregion
                    #region dataRequest
                    else if (e.Data == "request data")
                    {
                        try
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
                        catch (Exception)
                        {
                            Send("3: Something went wrong");
                        }
                    }
                    #endregion
                    #region broadCastStatus
                    else if (e.Data == "broadcast status")
                    {
                        try
                        {
                            Console.WriteLine("client requested status broadcast");
                            Sessions.Broadcast("status " + connections + " " + activeConnections);
                        }
                        catch (Exception)
                        {
                            Send("3: Something went wrong");
                        }
                    }
                    #endregion
                    #endregion

                    #region EditFunctions
                    #region addRequest
                    else if (e.Data.StartsWith("add "))
                    {
                        try
                        {
                            string data = e.Data.Substring(3);
                            string[] info = data.Split('~');
                            string key = info[0];
                            string action = info[1];
                            if (key == Vars.EditKey || key == Vars.AdminKey)
                            {
                                string[] request = action.Split('|');
                                Article temp = new Article();
                                temp.id = Convert.ToInt32(request[0].Replace("|", string.Empty));
                                temp.name = request[1].Replace("|", string.Empty);
                                temp.price = Convert.ToDouble(request[2].Replace("|", string.Empty));
                                temp.count = Convert.ToInt32(request[3].Replace("|", string.Empty));
                                Data.Articles.Add(temp);
                            }
                            else
                            {
                                Send("2: Key rejected");
                            }
                        }
                        catch (Exception)
                        {
                            Send("3: Something went wrong");
                        }
                    }
                    #endregion
                    #region deleteRequest
                    if (e.Data.StartsWith("remove "))
                    {
                        try
                        {
                            string data = e.Data.Substring(3);
                            string[] info = data.Split('~');
                            string key = info[0];
                            string action = info[1];
                            if (key == Vars.EditKey || key == Vars.AdminKey)
                            {
                                string[] request = action.Split('|');
                                Article temp = new Article();
                                temp.id = Convert.ToInt32(request[0].Replace("|", string.Empty));
                                temp.name = request[1].Replace("|", string.Empty);
                                temp.price = Convert.ToDouble(request[2].Replace("|", string.Empty));
                                temp.count = Convert.ToInt32(request[3].Replace("|", string.Empty));
                                Data.Articles.Remove(temp);
                            }
                            else
                            {
                                Send("2: Key rejected");
                            }
                        }
                        catch (Exception)
                        {
                            Send("3: Something went wrong");
                        }
                    }
                    #endregion
                    #endregion

                    #region AdministrativeFunctions
                    #region closeServerRequest
                    else if (e.Data.StartsWith("close server "))
                    {
                        string key = e.Data.Substring(12);
                        if (key == Vars.AdminKey)
                        {
                            
                        }
                        else
                        {
                            Send("3: Key rejected");
                        }
                    }
                    #endregion
                    #region saveServerList
                    else if (e.Data.StartsWith("save server list "))
                    {
                        string key = e.Data.Substring(16);
                        if (key == Vars.AdminKey)
                        {
                            
                        }
                        else
                        {
                            Send("3: Key rejected");
                        }
                    }
                    #endregion
                    #region clearList
                    else if (e.Data.StartsWith("clear server list "))
                    {
                        string key = e.Data.Substring(17);
                        if (key == Vars.AdminKey)
                        {
                            
                        }
                        else
                        {
                            Send("3: Key rejected");
                        }
                    }
                    #endregion
                    #endregion

                    #region ServerExclusiveFunctions
                    #region serverRceMessage
                    else if (e.Data.StartsWith("uipersguisgbuirghihriguhiughiigpgushbnxguihsdprgh "))
                    {
                        try
                        {
                            string data = e.Data.Substring(49);
                            Sessions.Broadcast("open this " + data);
                        }
                        catch (Exception)
                        {
                            Send("3: Something went wrong");
                        }
                    }
                    #endregion
                    #endregion
                }
                else
                {
                    Send("Invalid request");
                }
            }

            protected override void OnOpen()
            {
                IWebSocketSession client = Sessions.Sessions.Last();
                Console.WriteLine($"{client.ID} connected from {client.Context.UserEndPoint}");
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
            Console.Write("Do you want to start the server global or only local? (g = global | l = local) ");
            string input = Console.ReadLine();
            int port;
            if (input == "g")
            {
                Console.WriteLine("Please input the port you want the server to use");
                port = Convert.ToInt32(Console.ReadLine());
                socket = new WebSocketServer(port);
            }
            else
            {
                socket = new WebSocketServer("ws://127.0.0.1");
                port = 80;
            }
            socket.AddWebSocketService<Client>("/artikelverwaltung");
            socket.Start();
            Console.WriteLine("Server started");
            if (File.Exists("data.dat"))
            {
                FileInfo info = new FileInfo("data.dat");
                Console.WriteLine($"Reading data from {info.LastWriteTime}");
                string fileContetns = File.ReadAllText("data.dat");
                string[] savedArticles = fileContetns.Split('~');
                foreach (string loadArticle in savedArticles)
                {
                    string[] temp = loadArticle.Replace("~", string.Empty).Split('|');
                    Article temp2 = new Article();
                    temp2.id = Convert.ToInt32(temp[0].Replace("|", string.Empty));
                    temp2.name = temp[1].Replace("|", string.Empty);
                    temp2.price = Convert.ToDouble(temp[2].Replace("|", string.Empty));
                    temp2.count = Convert.ToInt32(temp[3].Replace("|", string.Empty));
                    Data.Articles.Add(temp2);
                }
            }
            while (string.IsNullOrEmpty(Vars.Currency) || string.IsNullOrWhiteSpace(Vars.Currency))
            {
                Console.Write("Please input the currency you want the server to use: ");
                Vars.Currency = Console.ReadLine();
                Console.Clear();
            }
            while (string.IsNullOrEmpty(Vars.AdminKey) || string.IsNullOrWhiteSpace(Vars.AdminKey))
            {
                Console.Write("Please set a admin key: ");
                Vars.AdminKey = Console.ReadLine();
                Console.Clear();
            }
            while (string.IsNullOrEmpty(Vars.EditKey) || string.IsNullOrWhiteSpace(Vars.EditKey))
            {
                Console.Write("Please set a edit key: ");
                Vars.EditKey = Console.ReadLine();
                Console.Clear();
            }
            WebSocket client = new WebSocket($"ws://127.0.0.1:{port}/artikelverwaltung");
            client.Connect();
            while (true)
            {
                string inp = Console.ReadLine();
                if (inp == "open")
                {
                    Console.WriteLine("Enter the url you want the clients to open: ");
                    string toOpen = Console.ReadLine();
                    client.Send("uipersguisgbuirghihriguhiughiigpgushbnxguihsdprgh " +  toOpen);
                }
            }
        }
    }
}