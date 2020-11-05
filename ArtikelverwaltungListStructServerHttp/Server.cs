using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ArtikelverwaltungListStruct;

namespace ArtikelverwaltungListStructServerHttp
{
    internal class Server
    {
        private List<Artikel> _artikels = new List<Artikel>();
        private string _key;
        private HttpListener _listener;
        private int _port;

        private Thread _serverThread;

        public int RequestCount;
        private string waerung = string.Empty;

        public Server(int port, string key)
        {
            Initialize(port, key);
        }

        public Server(string key)
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            var port = ((IPEndPoint) l.LocalEndpoint).Port;
            l.Stop();
            Initialize(port, key);
        }

        public int Port
        {
            get => _port;
            private set { }
        }

        public string Key
        {
            get => _key;
            private set { }
        }

        public void Stop()
        {
            Console.WriteLine($"[{Port}] Server on port {Port} is stopping...");
            Console.WriteLine($"[{Port}] Saving contents...");
            File.WriteAllText("save", "");
            foreach (var artikel in _artikels)
                File.AppendAllText("save",
                    $"{artikel.name}|{artikel.nummer}|{artikel.preis}|{artikel.bestand}" + Environment.NewLine);
            Console.WriteLine($"[{Port}] Contents saved.");
            Console.WriteLine($"[{Port}] Aborting server thread...");
            _serverThread.Abort();
            Console.WriteLine($"[{Port}] Server thread aborted");
            _listener.Stop();
            Console.WriteLine($"[{Port}] listener stopped");
        }

        public void Save()
        {
            Console.WriteLine($"[{Port}] Saving contents...");
            File.WriteAllText("save", "");
            foreach (var artikel in _artikels)
                File.AppendAllText("save",
                    $"{artikel.name}|{artikel.nummer}|{artikel.preis}|{artikel.bestand}" + Environment.NewLine);
            Console.WriteLine($"[{Port}] Contents saved.");
        }

        private void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://" + Program.Ip + ":" + _port + "/");
            _listener.Start();
            while (true)
                try
                {
                    var context = _listener.GetContext();
                    Process(context);
                }
                catch (Exception)
                {
                }
        }

        private void Process(HttpListenerContext context)
        {
            RequestCount++;
            Console.WriteLine(
                $"[{Port}] New request({context.Request.RemoteEndPoint}) with ID {RequestCount} to: {context.Request.Url}");


            WorkWithRequest(context);


            context.Response.OutputStream.Close();
        }

        private void Initialize(int port, string key)
        {
            if (File.Exists("save"))
            {
                Console.WriteLine($"[{Port}] Reading past content...");
                foreach (var line in File.ReadAllLines("save"))
                {
                    var content = line.Split('|');
                    var temp = new Artikel();
                    temp.name = content[0].Replace("|", string.Empty);
                    temp.nummer = Convert.ToInt32(content[1].Replace("|", string.Empty));
                    temp.preis = Convert.ToDouble(content[2].Replace("|", string.Empty));
                    temp.bestand = Convert.ToInt32(content[3].Replace("|", string.Empty));
                    _artikels.Add(temp);
                }

                Console.WriteLine($"[{Port}] Read past content.");
            }
            else
            {
                Console.WriteLine($"[{Port}] no past content to read");
            }

            Console.WriteLine($"[{Port}] Setting variables...");
            _port = port;
            _key = key;
            Console.WriteLine($"[{Port}] Variables set.");
            Console.WriteLine($"[{Port}] Creating server thread...");
            _serverThread = new Thread(Listen);
            Console.WriteLine($"[{Port}] Server thread created proceeding to start it...");
            _serverThread.Start();
            Console.WriteLine($"[{Port}] Server thread started");
            Console.WriteLine($"[{Port}] Server is running on port {Port}");
        }

        private void SendResponse(HttpListenerContext context, string response)
        {
            var bytes = Encoding.ASCII.GetBytes(response);
            Stream input = new MemoryStream(bytes);

            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = input.Length;
            context.Response.AddHeader("Date", DateTime.Now.ToString("r"));

            var buffer = new byte[1024 * 16];
            int nbytes;
            while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                context.Response.OutputStream.Write(buffer, 0, nbytes);
            input.Close();

            context.Response.StatusCode = (int) HttpStatusCode.OK;
            context.Response.OutputStream.Flush();
        }

        private void AddReqest(HttpListenerContext context)
        {
            var content = context.Request.Url.AbsolutePath.Replace("%20", " ").Split('/');
            var temp = new Artikel();
            temp.name = content[2];
            temp.nummer = Convert.ToInt32(content[3]);
            temp.preis = Convert.ToDouble(content[4]);
            temp.bestand = Convert.ToInt32(content[5]);
            _artikels.Add(temp);
            SendResponse(context, "0");
        }

        private void RemoveReqest(HttpListenerContext context)
        {
            var content = context.Request.Url.AbsolutePath.Replace("%20", " ").Split('/');

            if (content[2] == "name")
            {
                var toremove = new Artikel();
                var done = false;
                var count = 0;
                foreach (var artikel in _artikels)
                {
                    if (artikel.name.ToLower().Contains(content[3].ToLower()) && !done)
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
                else
                {
                    SendResponse(context, "2: No item with that name was found in the list");
                }
            }
            else if (content[2] == "id")
            {
                var toremove = new Artikel();
                var done = false;
                var count = 0;
                foreach (var artikel in _artikels)
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
                else
                {
                    SendResponse(context, "2: No item with that number was found in the list");
                }
            }
            else
            {
                SendResponse(context, "1: That is not a valid remove term");
            }
        }

        private void ReadReqest(HttpListenerContext context)
        {
            var toReturn = string.Empty;
            if (_artikels.Count != 0)
            {
                var count = 0;
                foreach (var artikel in _artikels)
                {
                    count++;
                    if (count != _artikels.Count)
                        toReturn += $"{artikel.name}|{artikel.nummer}|{artikel.preis}|{artikel.bestand}" + "~";
                    else
                        toReturn += $"{artikel.name}|{artikel.nummer}|{artikel.preis}|{artikel.bestand}";
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
            var content = context.Request.Url.AbsolutePath.Replace("%20", " ").Split('/');
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

        private void SaveRequest(HttpListenerContext context)
        {
            var content = context.Request.Url.AbsolutePath.Replace("%20", " ").Split('/');
            if (content[2] == _key)
            {
                Save();
                SendResponse(context, "0");
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

        private void CurrencyRequest(HttpListenerContext context)
        {
            SendResponse(context, Program.currency);
        }

        private void ClearRequest(HttpListenerContext context)
        {
            var content = context.Request.Url.AbsolutePath.Replace("%20", " ").Split('/');
            if (content[2] == _key)
            {
                _artikels = new List<Artikel>();
                Save();
                SendResponse(context, "0");
            }
            else
            {
                SendResponse(context, "Unauthorized: Wrong key...");
            }
        }

        private void WorkWithRequest(HttpListenerContext context)
        {
            var endpoint = context.Request.Url.AbsolutePath.Split('/')[1];
            Console.WriteLine(
                $"[{Port}] Request({context.Request.RemoteEndPoint}) with ID {RequestCount} called endpoint: {endpoint}");
            if (endpoint == "add")
            {
                try
                {
                    AddReqest(context);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{Port}] error while processing request ID {RequestCount}");
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
                    Console.WriteLine($"[{Port}] error while processing request ID {RequestCount}");
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
                    Console.WriteLine($"[{Port}] error while processing request ID {RequestCount}");
                    Console.WriteLine(ex);
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (endpoint == "save")
            {
                try
                {
                    SaveRequest(context);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{Port}] error while processing request ID {RequestCount}");
                    Console.WriteLine(ex);
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (endpoint == "clear")
            {
                try
                {
                    ClearRequest(context);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{Port}] error while processing request ID {RequestCount}");
                    Console.WriteLine(ex);
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (endpoint == "close")
            {
                CloseRequest(context);
            }
            else if (endpoint == "status")
            {
                StatusRequest(context);
            }
            else if (endpoint == "curr")
            {
                CurrencyRequest(context);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(
                    $"[{Port}] Request({context.Request.RemoteEndPoint}) with ID {RequestCount} called a not implemented endpoint...");
                Console.ForegroundColor = ConsoleColor.White;
                SendResponse(context, "The requested endpoint is not implemented...");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}