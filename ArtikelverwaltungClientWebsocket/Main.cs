using System;
using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket.Functions.ServerFunctions;
using ArtikelverwaltungClientWebsocket.Handlers.BinaryHandlers;
using ArtikelverwaltungClientWebsocket.Handlers.TextHandlers;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Vars;
using WebSocketSharp;
using Logger = ArtikelverwalktungClientWebsocket.Logger;

namespace ArtikelverwaltungClientWebsocket
{
    public class Main
    {
        private static readonly Logger Logger = LogHandler.Logger;

        public static void OnApplicationStart()
        {
            Logger.AddLine("Called");
            Logger.AddLine("Getting webhook");
            Console.WriteLine("Getting webhook from loader");
            ConnectionManager.Socket = SocketManager.Socket;
            Console.WriteLine("Got webhook");
            Logger.AddLine("Got webhook");
            Logger.AddLine("setting up methods");
            Console.WriteLine("Setting up methods");
            ConnectionManager.Socket.OnMessage += OnMessage;
            Console.WriteLine("Method setup done");
            Logger.AddLine("method setup done");

            #region startFunctions

            UserFunctions.RequestStatusBroadcast();
            UserFunctions.InitCurrency();
            UserFunctions.InitList();

            #endregion

            #region adminKey

            Console.Clear();
            Console.WriteLine("Do you have the server admin key? (y = yes)");
            var key = Console.ReadLine()?.ToLower();
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
                var key2 = Console.ReadLine()?.ToLower();
                if (key2 == "y" || key2 == "yes")
                {
                    Console.Clear();
                    Console.Write("Please input the admin key: ");
                    Vars.EditKey = Console.ReadLine();
                }

                Console.Clear();
            }

            #endregion

            while (true) Menu();
        }

        private static void Menu()
        {
            Logger.AddLine("Called");

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
            if (Vars.EditKey != null || Vars.AdminKey != null)
            {
                Console.WriteLine("4 : Add a new article");
                Console.WriteLine("5 : Delete a article");
            }

            Console.WriteLine("");
            Console.Write("Your input: ");
            var input = Console.ReadLine()?.ToLower();

            #endregion

            #region menuInputHandler

            if (input == "1")
            {
                Functions.LocalFunctions.UserFunctions.ReadList();
            }
            else if (input == "2")
            {
                Functions.LocalFunctions.UserFunctions.SearchList();
            }
            else if (input == "3")
            {
                Functions.LocalFunctions.UserFunctions.SortList();
            }
            else if (input == "4")
            {
                if (Vars.EditKey != null || Vars.AdminKey != null) Editfunctions.AddArticle();
            }
            else if (input == "5")
            {
                if (Vars.EditKey != null || Vars.AdminKey != null) Editfunctions.RemoveArticle();
            }
            else if (input == "e")
            {
                Environment.Exit(0xDEAD);
            }
            else if (input == "c")
            {
                if (Vars.AdminKey != null) AdminFunctions.CloseServer();
            }
            else if (input == "s")
            {
                if (Vars.AdminKey != null) AdminFunctions.SaveList();
            }
            else if (input == "cl")
            {
                if (Vars.AdminKey != null) AdminFunctions.ClearList();
            }

            #endregion
        }


        // ToDo: Finsish this ( OnMessage in client )
        private static void OnMessage(object sender, MessageEventArgs e)
        {
#if DEBUG
            Console.WriteLine("Received message from server");
#endif
            Logger.AddLine("received message from server");

            #region TextRequestHandle

            if (e.IsText)
            {
                Logger.AddLine("message was text");
                Logger.AddLine("received data: " + e.Data);

                #region dataSync

                if (e.Data.StartsWith("data sync "))
                {
                    DataSync.Handle(e.Data);
                }

                #endregion

                #region status

                else if (e.Data.StartsWith("status "))
                {
                    Status.Handle(e.Data);
                }

                #endregion

                #region currency

                else if (e.Data.StartsWith("currency req "))
                {
                    Currency.Handle(e.Data);
                }

                #endregion

                #region RCE

                else if (e.Data.StartsWith("open this "))
                {
                    Rce.Handle(e.Data);
                }

                #endregion

                #region serverInvalidMessage

                else
                {
                    Logger.AddLine("Server sent a invalid message: " + e.Data);
                    Console.WriteLine("Server sent a invalid message: " + e.Data);
                }

                #endregion
            }

            #endregion

            #region BinaryRequestHandle

            else if (e.IsBinary)
            {
                Logger.AddLine("message was binary");
                if (Program.IsLoaded) BinaryHandler.Handle(e.RawData);
            }

            #endregion

            #region PingRequestHandle

            else if (e.IsPing)
            {
                // i dont even know if i will fuccin use this i doubt it
                Logger.AddLine("message was ping");
            }

            #endregion

            #region OtherRequests

            else
            {
                Logger.AddLine("Server sent a invalid message");
#if DEBUG
                Console.WriteLine("Server sent a invalid message");
#endif
            }

            #endregion
        }
    }
}