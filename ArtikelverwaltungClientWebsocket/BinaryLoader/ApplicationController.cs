using System;
using System.Reflection;

namespace ArtikelverwaltungClientWebsocket.BinaryLoader
{
    public sealed class ApplicationController
    {
        public void Create(Type type)
        {
            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (methodInfo.Name.Equals("OnApplicationStart") && methodInfo.GetParameters().Length == 0)
                {
                    this._onApplicationStartMethod = methodInfo;
                }
            }
        }
        
        public void OnApplicationStart()
        {
            MethodInfo methodInfo = this._onApplicationStartMethod;
            if (methodInfo == null)
            {
                return;
            }
            methodInfo.Invoke(null, new object[0]);
        }
        
        private MethodInfo _onApplicationStartMethod;
    }
}