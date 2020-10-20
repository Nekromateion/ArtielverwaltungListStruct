﻿using System;
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
            Console.WriteLine("Method setup done");
            logger.AddLine("method setup done");
            Menu();
        }

        public static void Menu()
        {
            logger.AddLine("Called");
            Console
        }

        
        // ToDo: Finsish this ( OnMessage in client )
        // imma just stop working on thsi project for now 
        // its nearly 6 am 
        // see you in 4 hours
        // i hope......
        // okay so i just got back to this and i have no clue wtf i did here or wanted to do
        private static void OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Received message from server");
            logger.AddLine("received message from server");
            if (e.IsText)
            {
                logger.AddLine("message was text");
                logger.AddLine("received data: " + e.Data);
                if (e.Data.StartsWith("data sync "))
                {
                    logger.AddLine("message was a data sync");
                    string data = e.Data.Substring(9); // i will have to wait with continueing to work on this part since i dont know yet how i will send the data
                }
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
                logger.AddLine("Server sent a invalid message");
            }
        }
    }
}