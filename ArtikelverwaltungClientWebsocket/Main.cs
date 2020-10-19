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
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        public static void OnApplicationStart()
        {
            logger.AddLine("Called");
            logger.AddLine("Getting webhook");
            Console.WriteLine("Getting webhook from loader");
            ConnectionManager.socket = ArtikelverwaltungClientWebsocketLoader.SocketManager.Socket;
            Console.WriteLine("Got webhook");
            logger.AddLine("Got webhook");
            logger.AddLine("setting up methods");
            Console.WriteLine("Setting up methods");
            ConnectionManager.socket.OnMessage += OnMessage;
            logger.AddLine("method setup done");
        }

        private static void OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Received message from server");
            logger.AddLine("received message from server");
            if (e.IsText)
            {
                logger.AddLine("message was text");
                logger.AddLine("received data: " + e.Data);
            }
            else if (e.IsBinary)
            {
                logger.AddLine("message was binary");
            }
            else if (e.IsPing)
            {
                logger.AddLine("message was ping");
            }
            else
            {
                logger.AddLine("Server sent a invalid messageq");
            }
        }
    }
}