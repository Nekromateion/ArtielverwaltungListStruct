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

        
        // ToDo: Finsish this ( OnMessage in client )
        // imma just stop working on thsi project for now 
        // its nearly 6 am 
        // see you in 4 hours
        // i hope......
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
                // might use this to make a forms version
                // which would allow the user to set a image
                // for the product 
                // yes yes real fancy stuffs
                logger.AddLine("message was binary");
            }
            else if (e.IsPing)
            {
                // i dont even know if i will fuccin use this i doubt it
                logger.AddLine("message was ping");
            }
            else
            {
                logger.AddLine("Server sent a invalid messageq");
            }
        }
    }
}