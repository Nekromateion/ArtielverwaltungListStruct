using System;
using System.Net;
using System.Reflection;
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

            #region functionInputs
            // ToDo: finish this
            
            //string[] readNames;
            using (WebClient client = new WebClient())
            {
                //readNames = client.DownloadString("https://media.nekro-works.de/readNames.txt");
            }
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
            
            #endregion
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
                else if (e.Data.StartsWith("status "))
                {
                    logger.AddLine("message was status");
                    string message = e.Data.Substring(6);
                    logger.AddLine("Status message: " + message);
                    string[] numbers = e.Data.Split(' ');
                    Vars.ConnectedUsers = Convert.ToInt32(numbers[2].Replace(" ", string.Empty));
                    Console.Title = $"Article management v{Vars.Version} | Connected users: {Vars.ConnectedUsers}";
                }
            }
            else if (e.IsBinary)
            {
                // might use this to make a forms version
                // which would allow the user to set a image
                // for the product 
                // yes yes real fancy stuffs
                // note: just noticed -> ill do this another way (the thing with the files)
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