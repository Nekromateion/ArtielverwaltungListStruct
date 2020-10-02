﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
 using ArtikelverwaltungListStruct;

 namespace ArtikelverwaltungListStructServerHttp
 {
     class Server
     {

         private Thread _serverThread;
         private string _rootDirectory;
         private HttpListener _listener;
         private int _port;
         private string _key;
         private List<Artikel> _artikels = new List<Artikel>();
         private string waerung = String.Empty;

         public int Port
         {
             get { return _port; }
             private set { }
         }
         
         public string Key
         {
             get { return _key; }
             private set { }
         }

         public int RequestCount = 0;

         public Server(int port, string key)
         {
             this.Initialize(port, key);
         }

         public Server(string key)
         {
             TcpListener l = new TcpListener(IPAddress.Loopback, 0);
             l.Start();
             int port = ((IPEndPoint) l.LocalEndpoint).Port;
             l.Stop();
             this.Initialize(port, key);
         }

         public void Stop()
         {
             Console.WriteLine($"[{this.Port}] Server on port {this.Port} is stopping...");
             Console.WriteLine($"[{this.Port}] Saving contents...");
             File.WriteAllText("save", "");
             foreach (Artikel artikel in _artikels)
             {
                 File.AppendAllText("save", $"{artikel.name}|{artikel.nummer}|{artikel.preis}|{artikel.bestand}" + Environment.NewLine);
             }
             Console.WriteLine($"[{this.Port}] Contents saved.");
             Console.WriteLine($"[{this.Port}] Aborting server thread...");
             _serverThread.Abort();
             Console.WriteLine($"[{this.Port}] Server thread aborted");
             _listener.Stop();
             Console.WriteLine($"[{this.Port}] listener stopped");
         }

         private void Listen()
         {
             _listener = new HttpListener();
             _listener.Prefixes.Add("http://127.0.0.1:" + _port + "/");
             _listener.Start();
             while (true)
             {
                 try
                 {
                     HttpListenerContext context = _listener.GetContext();
                     Process(context);
                 }
                 catch (Exception) { }
             }
         }

         private void Process(HttpListenerContext context)
         {
             this.RequestCount++;
             Console.WriteLine($"[{this.Port}] New request({context.Request.RemoteEndPoint}) with ID {RequestCount} to: {context.Request.Url}");

             
             
             WorkWithRequest(context);


             context.Response.OutputStream.Close();
         }

         private void Initialize(int port, string key)
         {
             if (File.Exists("save"))
             {
                 Console.WriteLine($"[{this.Port}] Reading past content...");
                 foreach (string line in File.ReadAllLines("save"))
                 {
                     string[] content = line.Split('|');
                     Artikel temp = new Artikel();
                     temp.name = content[0].Replace("|", string.Empty);
                     temp.nummer = Convert.ToInt32(content[1].Replace("|", string.Empty));
                     temp.preis = Convert.ToDouble(content[2].Replace("|", String.Empty));
                     temp.bestand = Convert.ToInt32(content[3].Replace("|", String.Empty));
                     _artikels.Add(temp);
                 }
                 Console.WriteLine($"[{this.Port}] Read past content.");
             }
             else
             {
                 Console.WriteLine($"[{this.Port}] no past content to read");
             }
             Console.WriteLine($"[{this.Port}] Setting variables...");
             this._port = port;
             this._key = key;
             Console.WriteLine($"[{this.Port}] Variables set.");
             Console.WriteLine($"[{this.Port}] Creating server thread...");
             _serverThread = new Thread(this.Listen);
             Console.WriteLine($"[{this.Port}] Server thread created proceeding to start it...");
             _serverThread.Start();
             Console.WriteLine($"[{this.Port}] Server thread started");
             Console.WriteLine($"[{this.Port}] Server is running on port {this.Port}");
         }

         private void SendResponse(HttpListenerContext context, string response)
         {
             byte[] bytes = Encoding.ASCII.GetBytes(response);
             Stream input = new MemoryStream(bytes);
                 
             context.Response.ContentType = "text/plain";
             context.Response.ContentLength64 = input.Length;
             context.Response.AddHeader("Date", DateTime.Now.ToString("r"));

             byte[] buffer = new byte[1024 * 16];
             int nbytes;
             while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                 context.Response.OutputStream.Write(buffer, 0, nbytes);
             input.Close();

             context.Response.StatusCode = (int) HttpStatusCode.OK;
             context.Response.OutputStream.Flush();
         }

         private void AddReqest(HttpListenerContext context)
         {
             string[] content = context.Request.Url.AbsolutePath.Replace("%20", " ").Split('/');
             Artikel temp = new Artikel();
             temp.name = content[2];
             temp.nummer = Convert.ToInt32(content[3]);
             temp.preis = Convert.ToDouble(content[4]);
             temp.bestand = Convert.ToInt32(content[5]);
             _artikels.Add(temp);
             SendResponse(context, "0");
         }

         private void RemoveReqest(HttpListenerContext context)
         {
             string[] content = context.Request.Url.AbsolutePath.Replace("%20", " ").Split('/');

             if (content[2] == "name")
             {
                 Artikel toremove = new Artikel();
                 bool done = false;
                 int count = 0;
                 foreach (Artikel artikel in _artikels)
                 {
                     if (artikel.name.Contains(content[3]) && !done)
                     {
                         toremove = _artikels[count];
                         done = true;
                     }
                     count++;
                 }

                 if (done)
                 {
                     _artikels.Remove(toremove);
                     SendResponse(context, "0");
                 }
                 else SendResponse(context, "2: No item with that name was found in the list");
             }
             else if(content[2] == "id")
             {
                 Artikel toremove = new Artikel();
                 bool done = false;
                 int count = 0;
                 foreach (Artikel artikel in _artikels)
                 {
                     if (artikel.nummer == Convert.ToInt32(content[3]) && !done)
                     {
                         toremove = _artikels[count];
                         done = true;
                     }
                     count++;
                 }

                 if (done)
                 {
                     _artikels.Remove(toremove);
                     SendResponse(context, "0");
                 }
                 else SendResponse(context, "2: No item with that number was found in the list");
             }
             else SendResponse(context, "1: That is not a valid remove term");
         }

         private void ReadReqest(HttpListenerContext context)
         {
             string toReturn = string.Empty;
             if (_artikels.Count != 0)
             {
                 int count = 0;
                 foreach (Artikel artikel in _artikels)
                 {
                     count++;
                     if (count != _artikels.Count)
                     {
                         toReturn += $"{artikel.name}|{artikel.nummer}|{artikel.preis}|{artikel.bestand}" + "-.-";
                     }
                     else
                     {
                         toReturn += $"{artikel.name}|{artikel.nummer}|{artikel.preis}|{artikel.bestand}";
                     }
                 }
                 SendResponse(context, toReturn);
             }
             else
             {
                 SendResponse(context, "1: List is empty nothing to return");
             }
         }
         
         private void CloseRequest(HttpListenerContext context)
         {
             string[] content = context.Request.Url.AbsolutePath.Replace("%20", " ").Split('/');
             if (content[2] == _key)
             {
                 SendResponse(context, "0");
                 Stop();
             }
             else
             {
                 SendResponse(context, "Unauthorized: Wrong key...");
             }
         }
         
         private void StatusRequest(HttpListenerContext context)
         {
             SendResponse(context, "0");
         }

         private void WorkWithRequest(HttpListenerContext context)
         {
             string endpoint = context.Request.Url.AbsolutePath.Split('/')[1];
             Console.WriteLine($"[{this.Port}] Request({context.Request.RemoteEndPoint}) with ID {RequestCount} called endpoint: {endpoint}");
             if (endpoint == "add")
             {
                 try
                 {
                     AddReqest(context);
                 }
                 catch (Exception ex)
                 {
                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.WriteLine($"[{this.Port}] error while processing request ID {RequestCount}");
                     Console.WriteLine(ex);
                     context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                     Console.ForegroundColor = ConsoleColor.White;
                 }
             }
             else if (endpoint == "remove")
             {
                 try
                 {
                     RemoveReqest(context);
                 }
                 catch (Exception ex)
                 {
                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.WriteLine($"[{this.Port}] error while processing request ID {RequestCount}");
                     Console.WriteLine(ex);
                     context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                     Console.ForegroundColor = ConsoleColor.White;
                 }
             }
             else if (endpoint == "read")
             {
                 try
                 {
                     ReadReqest(context);
                 }
                 catch (Exception ex)
                 {
                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.WriteLine($"[{this.Port}] error while processing request ID {RequestCount}");
                     Console.WriteLine(ex);
                     context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                     Console.ForegroundColor = ConsoleColor.White;
                 }
             }
             else if(endpoint == "close") CloseRequest(context);
             else if (endpoint == "status") StatusRequest(context);
             else
             {
                 Console.ForegroundColor = ConsoleColor.DarkRed;
                 Console.WriteLine($"[{this.Port}] Request({context.Request.RemoteEndPoint}) with ID {RequestCount} called a not implemented endpoint...");
                 Console.ForegroundColor = ConsoleColor.White;
                 SendResponse(context, "The requested endpoint is not implemented...");
             }
         }
     }
 }