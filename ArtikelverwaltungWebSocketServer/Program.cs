using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp.Server;
using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

namespace ArtikelverwaltungWebSocketServer
{
    internal class Program
    {
        public class Client : WebSocketBehavior
        {
            private struct connection
            {
                public WebSocketContext context { get; set; }
                public string ID { get; set; }
            }
            
            private static int connections = 0;
            private static int activeConnections = 0;
            private static string serverId = String.Empty;
            private static bool isFirstConnection = true;
            private static List<connection> _connections = new List<connection>();

            protected override void OnMessage(MessageEventArgs e)
            {
                Console.WriteLine("New message");
                if (e.IsText)
                {
                    Console.WriteLine("Message was text");
                    Console.WriteLine(e.Data);
                    
                    if(e.Data.StartsWith(serverId)) Console.WriteLine("message was sent by server commander");

                    #region UserFunction
                    #region assemblyRequest
                    if (e.Data == "request assembly")
                    {
                        try
                        {
                            Console.WriteLine("Client requested assembly");
                            Send(System.IO.File.ReadAllBytes("ArtikelverwaltungClientWebsocket.dll"));
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
                            Console.WriteLine("Cleint Requested data");
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
                            Sessions.Broadcast("status " + connections + " " + activeConnections);
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
                                    temp.id = Data.Articles.Count;
                                    temp.name = request[1].Replace("|", string.Empty);
                                    temp.price = Convert.ToDouble(request[2].Replace("|", string.Empty));
                                    temp.count = Convert.ToInt32(request[3].Replace("|", string.Empty));
                                }
                                else
                                {
                                    temp.id = Convert.ToInt32(request[0].Replace("|", string.Empty));
                                    temp.name = request[1].Replace("|", string.Empty);
                                    temp.price = Convert.ToDouble(request[2].Replace("|", string.Empty));
                                    temp.count = Convert.ToInt32(request[3].Replace("|", string.Empty));
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
                                Article temp = new Article();
                                temp.id = Convert.ToInt32(request[0].Replace("|", string.Empty));
                                temp.name = request[1].Replace("|", string.Empty);
                                temp.price = Convert.ToDouble(request[2].Replace("|", string.Empty));
                                temp.count = Convert.ToInt32(request[3].Replace("|", string.Empty));
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
                                socket.Stop();
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
                                        toWrite += article.id + "|" + article.name + "|" + article.price + "|" + article.count;
                                    }
                                    else
                                    {
                                        toWrite += article.id + "|" + article.name + "|" + article.price + "|" + article.count + "~";
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
                    else if (e.Data.StartsWith(serverId + "open "))
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
                    else if (e.Data.StartsWith(serverId + "load "))
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

            internal void BroadcastList()
            {
                string toBroadcast = "data sync ";
                int count = 0;
                foreach (Article article in Data.Articles)
                {
                    count++;
                    if (count != Data.Articles.Count)
                    {
                        toBroadcast += article.id + "|";
                        toBroadcast += article.name + "|";
                        toBroadcast += article.price + "|";
                        toBroadcast += article.count + "~";
                    }
                    else
                    {
                        toBroadcast += article.id + "|";
                        toBroadcast += article.name + "|";
                        toBroadcast += article.price + "|";
                        toBroadcast += article.count;
                    }
                }

                Sessions.Broadcast(toBroadcast);
            }

            protected override void OnOpen()
            {
                IWebSocketSession client = Sessions.Sessions.Last();
                Console.WriteLine($"{client.ID} connected from {client.Context.UserEndPoint}");
                connections++;
                activeConnections++;
                Console.WriteLine("<======================================================================================================>");
                Console.WriteLine($"New Connection from ({Context.UserEndPoint}) Connections: [{connections}] Active: [{activeConnections}]");
                if (isFirstConnection)
                {
                    serverId = client.ID;
                    Sessions.SendTo(client.ID, client.ID);
                    Console.WriteLine("--------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(client.ID + " is now server commander");
                    isFirstConnection = false;
                }
                Console.WriteLine("--------------------------------------------------------------------------------------------------------");
                Console.WriteLine("Currently connected clients:");
                foreach (IWebSocketSession se in Sessions.Sessions)
                {
                    Console.WriteLine($"{se.ID} from {se.Context.UserEndPoint} | Started: {se.StartTime} | State: {se.State} | Time connected: {DateTime.Now - se.StartTime}");
                }
                Console.WriteLine("<======================================================================================================>");
                connection temp = new connection();
                temp.context = Context;
                temp.ID = client.ID;
                _connections.Add(temp);
                Sessions.Broadcast("status " + connections + " " + activeConnections);
            }

            protected override void OnClose(CloseEventArgs e)
            {
                try
                {
                    activeConnections--;
                    connection temp = new connection();
                    foreach (connection con in _connections)
                    {
                        if (con.context == Context)
                        {
                            temp = con;
                        }
                    }
                    Console.WriteLine("<======================================================================================================>");
                    Console.WriteLine($"Connection Closed ({temp.ID})");
                    Console.WriteLine("Currently connected clients:");
                    foreach (IWebSocketSession se in Sessions.Sessions)
                    {
                        Console.WriteLine($"{se.ID} from {se.Context.UserEndPoint} | Started: {se.StartTime} | State: {se.State} | Time connected: {DateTime.Now - se.StartTime}");
                    }
                    Console.WriteLine("<======================================================================================================>");
                    Sessions.Broadcast("status " + connections + " " + activeConnections);
                    _connections.Remove(temp);
                }
                catch (Exception) { }
            }

            protected override void OnError(ErrorEventArgs e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal static WebSocketServer socket;
        
        public static void Main(string[] args)
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
                socket = new WebSocketServer(port);
            }
            else
            {
                socket = new WebSocketServer("ws://127.0.0.1");
                port = 80;
            }
            Console.Clear();
            socket.AddWebSocketService<Client>("/artikelverwaltung");
            socket.Log.Level = LogLevel.Fatal;
            socket.Start();
            Console.WriteLine("Server started");
            #endregion
            
            #region dataReading
            if (File.Exists("data.dat"))
            {
                try
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
                catch (Exception) { }
            }
            else
            {
                Console.WriteLine("No past data found");
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
        }
    }
}