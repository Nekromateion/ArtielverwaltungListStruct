using System;
using System.IO;
using System.Linq;
using WebSocketSharp;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

namespace ArtikelverwaltungClientWebsocketLoader
{
    public class SocketManager
    {
        public static WebSocket Socket = new WebSocket($"ws://{Program.ipOrPort}/artikelverwaltung");
    }

    public class LogHandler
    {
        public static Logger logger = new Logger();
    }
    
    internal class Program
    {
        public static ApplicationController controller { get; set; }
        internal static string ipOrPort;
        
        public static void Main(string[] args)
        {
            Logger logger = LogHandler.logger;
            logger.Init();
            Console.Write("Please enter the server ip/hostname: ");
            ipOrPort = Console.ReadLine();
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
            while (assembly == null) { Thread.Sleep(10);}
            logger.AddLine("assembly created");
            logger.AddLine("loading assembly");
            Console.WriteLine("Loading assembly");
            foreach (Type type in GetLoadableTypes(Assembly.Load(assembly)))
            {
                if ("Main".Equals(type.Name))
                {
                    try
                    {
                        controller = new ApplicationController();
                        controller.Create(type);
                        Console.WriteLine("Loaded Assembly");
                        logger.AddLine("loaded assembly");
                        controller.OnApplicationStart();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
        
        public static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            IEnumerable<Type> result;
            try
            {
                result = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                string str = "An error occured while getting types from assembly ";
                string name = assembly.GetName().Name;
                string str2 = ". Returning types from error.\n";
                ReflectionTypeLoadException ex2 = ex;
                Console.WriteLine(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
                result = from t in ex.Types
                    where t != null
                    select t;
            }
            return result;
        }

        private static void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsBinary)
            {
                if (!isLoaded)
                {
                    Console.WriteLine("Received assembly");
                    assembly = e.RawData;
                    isLoaded = true;
                    Console.WriteLine("loaded assembly into ram");
                }
            }
        }

        internal static byte[] assembly;
        private static bool isLoaded = false;
    }
}