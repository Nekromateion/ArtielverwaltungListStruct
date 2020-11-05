using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket;

namespace ArtikelverwaltungClientWebsocket.BinaryLoader
{
    public static class Loader
    {
        private static readonly Logger Logger =
            LogHandler.Logger;

        private static ApplicationController Controller { get; set; }
        
        internal static void Load(byte[] assembly)
        {
            foreach (Type type in GetLoadableTypes(Assembly.Load(assembly)))
            {
                if ("Main".Equals(type.Name))
                {
                    try
                    {
                        Controller = new ApplicationController();
                        Controller.Create(type);
                        Logger.AddLine("loaded assembly");
                        Controller.OnApplicationStart();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
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
                Console.WriteLine(str + name + str2 + ex2);
                result = from t in ex.Types
                    where t != null
                    select t;
            }
            return result;
        }
    }
}