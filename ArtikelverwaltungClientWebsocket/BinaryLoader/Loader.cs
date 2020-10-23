using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArtikelverwaltungClientWebsocket.BinaryLoader
{
    public class Loader
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        
        public static ApplicationController controller { get; set; }
        
        internal static void Load(byte[] assembly)
        {
            foreach (Type type in GetLoadableTypes(Assembly.Load(assembly)))
            {
                if ("Main".Equals(type.Name))
                {
                    try
                    {
                        controller = new ApplicationController();
                        controller.Create(type);
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
    }
}