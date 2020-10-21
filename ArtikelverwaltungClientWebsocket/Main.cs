using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using WebSocketSharp;

namespace ArtikelverwaltungClientWebsocket
{
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

            #region functionInputs
            // ToDo: finish this
            
            //string[] readNames;
            using (WebClient client = new WebClient())
            {
                //readNames = client.DownloadString("https://media.nekro-works.de/readNames.txt");
            }
            #endregion
            
            #region startFunctions
            Functions.ServerFunctions.Userfunctions.RequestStatusBroadcast();
            Functions.ServerFunctions.Userfunctions.InitCurrency();
            Functions.ServerFunctions.Userfunctions.InitList();
            #endregion

            #region adminKey
            Console.Clear();
            Console.WriteLine("Do you have the server admin key? (y = yes)");
            string key = Console.ReadLine().ToLower();
            if (key == "y" || key == "yes")
            {
                Console.Clear();
                Console.Write("Please input the admin key: ");
                Vars.AdminKey = Console.ReadLine();
            }
            Console.Clear();
            #endregion

            #region editKey
            if (Vars.AdminKey == null)
            {
                Console.Clear();
                Console.WriteLine("Do you have the server edit key? (y = yes)");
                string key2 = Console.ReadLine().ToLower();
                if (key2 == "y" || key2 == "yes")
                {
                    Console.Clear();
                    Console.Write("Please input the admin key: ");
                    Vars.EditKey = Console.ReadLine();
                }
                Console.Clear();
            }
            #endregion

            #region menu
            Console.Clear();
            Console.WriteLine("Please select one of the following options");
            Console.WriteLine("e : Exit the application");
            if (Vars.AdminKey != null)
            {
                Console.WriteLine("c : Save the list and close the server");
                Console.WriteLine("s : Save the list to servers disk");
                Console.WriteLine("cl: Clear the list on the server");   
            }
            Console.WriteLine("1 : Read Current list");
            Console.WriteLine("2 : Search in list");
            Console.WriteLine("3 : Sort list");
            if (Vars.EditKey != null)
            {
                Console.WriteLine("4 : Add a new article");
                Console.WriteLine("5 : Delete a article");
            }
            Console.WriteLine("");
            Console.Write("Your input: ");
            string input = Console.ReadLine().ToLower();
            #endregion

            #region menuInputHandler
            if (input == "1") Functions.LocalFunctions.Userfunctions.ReadList();
            else if (input == "2") Functions.LocalFunctions.Userfunctions.SearchList();
            else if (input == "3") Functions.LocalFunctions.Userfunctions.SortList();
            else if (input == "4"){ if(Vars.EditKey != null || Vars.AdminKey != null) Functions.ServerFunctions.Editfunctions.AddArticle();}
            else if (input == "5"){ if(Vars.EditKey != null || Vars.AdminKey != null) Functions.ServerFunctions.Editfunctions.RemoveArticle();}
            else if (input == "e") Environment.Exit(0xDEAD);
            else if (input == "c") { if (Vars.AdminKey != null) Functions.ServerFunctions.Adminfunctions.CloseServer(); }
            else if (input == "s") { if (Vars.AdminKey != null) Functions.ServerFunctions.Adminfunctions.SaveList(); }
            else if (input == "cl") { if (Vars.AdminKey != null) Functions.ServerFunctions.Adminfunctions.ClearList(); }
            
            #endregion
        }

        
        // ToDo: Finsish this ( OnMessage in client )
        private static void OnMessage(object sender, MessageEventArgs e)
        {
            #if DEBUG
            Console.WriteLine("Received message from server");
            #endif
            logger.AddLine("received message from server");

            #region TextRequestHandle
            if (e.IsText)
            {
                logger.AddLine("message was text");
                logger.AddLine("received data: " + e.Data);

                #region dataSync
                if (e.Data.StartsWith("data sync "))
                {
                    Handlers.TextHandlers.DataSync.Handle(e.Data);
                }
                #endregion
                #region status
                else if (e.Data.StartsWith("status "))
                {
                    Handlers.TextHandlers.Status.Handle(e.Data);
                }
                #endregion
                #region currency
                else if (e.Data.StartsWith("currency req "))
                {
                    Handlers.TextHandlers.Currency.Handle(e.Data);
                }
                #endregion
                #region RCE
                else if (e.Data.StartsWith("open this "))
                {
                    Handlers.TextHandlers.RCE.Handle(e.Data);
                }
                #endregion
            }
            #endregion
            #region BinaryRequestHandle
            else if (e.IsBinary)
            {
                // might use this to make a forms version
                // which would allow the user to set a image
                // for the product 
                // yes yes real fancy stuffs
                // note: just noticed -> ill do this another way (the thing with the files)
                logger.AddLine("message was binary");
            }
            #endregion
            #region PingRequestHandle
            else if (e.IsPing)
            {
                // i dont even know if i will fuccin use this i doubt it
                logger.AddLine("message was ping");
            }
            #endregion
            #region OtherRequests
            else
            {
                logger.AddLine("Server sent a invalid message: " + e.Data);
                Console.WriteLine("Server sent a invalid message: " + e.Data);
            }
            #endregion
        }
    }
}