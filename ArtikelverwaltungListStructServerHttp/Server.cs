﻿﻿using System;
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
         private List<Artikel> _artikels = new List<Artikel>();
         private string waerung = String.Empty;

         public int Port
         {
             get { return _port; }
             private set { }
         }

         public int RequestCount = 0;

         public Server(int port)
         {
             this.Initialize(port);
         }

         public Server()
         {
             TcpListener l = new TcpListener(IPAddress.Loopback, 0);
             l.Start();
             int port = ((IPEndPoint) l.LocalEndpoint).Port;
             l.Stop();
             this.Initialize(port);
         }

         public void Stop()
         {
             Console.WriteLine($"[{this.Port}] Server on port {this.Port} is stopping...");
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
                 catch (Exception ex)
                 {

                 }
             }
         }

         private void Process(HttpListenerContext context)
         {
             this.RequestCount++;
             int requestId = this.RequestCount;
             Console.WriteLine($"[{this.Port}] New request({context.Request.RemoteEndPoint}) with ID {requestId} to: {context.Request.Url}");


             try
             {
                 string test = "test";
                 byte[] bytes = Encoding.ASCII.GetBytes(test);
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
             catch (Exception ex)
             {
                 Console.ForegroundColor = ConsoleColor.Red;
                 Console.WriteLine($"[{this.Port}] error while processing request ID {requestId}");
                 Console.WriteLine(ex);
                 context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                 Console.ForegroundColor = ConsoleColor.White;
             }


             context.Response.OutputStream.Close();
         }

         private void Initialize(int port)
         {
             this._port = port;
             Console.WriteLine($"[{this.Port}] Creating server thread...");
             _serverThread = new Thread(this.Listen);
             Console.WriteLine($"[{this.Port}] Server thread created proceeding to start it...");
             _serverThread.Start();
             Console.WriteLine($"[{this.Port}] Server thread started");
             Console.WriteLine($"[{this.Port}] Server is running on port {this.Port}");
         }


     }
 }