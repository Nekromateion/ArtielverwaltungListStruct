using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using WebSocketSharp;

namespace ArtikelverwalktungClientWebsocket
{
    public class SocketManager
    {
        public static WebSocket Socket = new WebSocket("ws://localhost/artikelverwaltung");
    }
    
    internal class Program
    {
        public static ApplicationController controller { get; set; }
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Setting methods");
            SocketManager.Socket.OnMessage += OnMessage;
            Console.WriteLine("Set methods\nConnecting to server...");
            SocketManager.Socket.Connect();
            Console.WriteLine("Connected to servers");
            Console.WriteLine("Sending request to server");
            SocketManager.Socket.Send("yo server gib assembly plz");
            Console.WriteLine("Sent request waiting for assembly to be loaded");
            while (assembly == null) { Thread.Sleep(10);}
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
            if (e.IsText)
            {
                if (e.Data.StartsWith("oki here you go buddy  "))
                {
                    Console.WriteLine("Server sent assembly");
                    assembly = Convert.FromBase64String(new StreamReader(e.Data.Substring(22)).ReadToEnd());
                }
            }
        }

        internal static byte[] assembly;
    }
}