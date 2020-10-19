using System;
using System.Reflection;

namespace ArtikelverwaltungClientWebsocketLoader
{
    public class ApplicationController
    {
        public void Create(Type type)
        {
            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (methodInfo.Name.Equals("OnApplicationStart") && methodInfo.GetParameters().Length == 0)
                {
                    this.onApplicationStartMethod = methodInfo;
                }
            }
        }
        
        public virtual void OnApplicationStart()
        {
            MethodInfo methodInfo = this.onApplicationStartMethod;
            if (methodInfo == null)
            {
                return;
            }
            methodInfo.Invoke(null, new object[0]);
        }
        
        private MethodInfo onApplicationStartMethod;
    }
}