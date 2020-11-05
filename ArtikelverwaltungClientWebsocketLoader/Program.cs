using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using WebSocketSharp;

namespace ArtikelverwalktungClientWebsocket
{
    public static class SocketManager
    {
        public static WebSocket Socket;
    }

    public static class LogHandler
    {
        public static readonly Logger Logger = new Logger();
    }

    public static class Program
    {
        private static byte[] _assembly;
        public static bool IsLoaded;
        private static ApplicationController Controller { get; set; }

        public static void Main(string[] args)
        {
            var logger = LogHandler.Logger;
            logger.Init();
            Console.Write("Please enter the server ip/hostname: ");
            var urlInput = Console.ReadLine();
            var url = "ws://" + urlInput + "/artikelverwaltung";
            SocketManager.Socket = new WebSocket(url) {Log = {Level = LogLevel.Fatal}};
            // ToDo: add the option for a centralized update server
            Console.WriteLine("Setting methods");
            logger.AddEmpty();
            logger.AddLine("Setting up methods");
            SocketManager.Socket.OnMessage += OnMessage;
            logger.AddLine("Set up methods");
            Console.WriteLine("Set methods\nConnecting to server...");
            logger.AddLine("Connecting to server");
            SocketManager.Socket.Connect();
            Console.WriteLine("Connected to servers");
            logger.AddLine("Connected to the server");
            logger.AddLine("Sending request");
            Console.WriteLine("Sending request to server");
            SocketManager.Socket.Send("request assembly");
            Console.WriteLine("Sent request waiting for assembly to be loaded");
            logger.AddLine("Sent request");
            logger.AddLine("waiting for assembly to be created");
            while (_assembly == null) Thread.Sleep(10);
            logger.AddLine("assembly created");
            logger.AddLine("loading assembly");
            Console.WriteLine("Loading assembly");
            foreach (var type in GetLoadableTypes(Assembly.Load(_assembly)))
                if ("Main".Equals(type.Name))
                    try
                    {
                        Controller = new ApplicationController();
                        Controller.Create(type);
                        Console.WriteLine("Loaded Assembly");
                        logger.AddLine("loaded assembly");
                        Controller.OnApplicationStart();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            IEnumerable<Type> result;
            try
            {
                result = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var str = "An error occured while getting types from assembly ";
                var name = assembly.GetName().Name;
                var str2 = ". Returning types from error.\n";
                var ex2 = ex;
                Console.WriteLine(str + name + str2 + ex2);
                result = from t in ex.Types
                    where t != null
                    select t;
            }

            return result;
        }

        private static void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsBinary)
                if (!IsLoaded)
                {
                    LogHandler.Logger.AddLine("received assembly");
                    Console.WriteLine("Received assembly");
                    _assembly = e.RawData;
                    IsLoaded = true;
                    Console.WriteLine("loaded assembly into ram");
                    LogHandler.Logger.AddLine("loaded assembly");
                }
        }
    }
}