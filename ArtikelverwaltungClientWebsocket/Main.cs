using System;
using WebSocketSharp;

namespace ArtikelverwaltungClientWebsocket
{
    public class ConnectionManager
    {
        public static WebSocket socket;
    }
    
    public class Main
    {
        public static void OnApplicationStart()
        {
            Console.WriteLine("Getting webhook from loader");
            ConnectionManager.socket = ArtikelverwaltungClientWebsocketLoader.SocketManager.Socket;
            Console.WriteLine("Got socket");
            Console.WriteLine("Setting up methods");
        }
        
            
    }
}