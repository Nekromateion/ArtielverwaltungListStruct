using System;

namespace ArtikelverwaltungClientWebsocket.Handlers.TextHandlers
{
    public class Status
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        internal static void Handle(string data)
        {
            logger.AddLine("message was status");
            string message = data.Substring(6);
            logger.AddLine("Status message: " + message);
            string[] numbers = data.Split(' ');
            Vars.ConnectedUsers = Convert.ToInt32(numbers[2].Replace(" ", string.Empty));
            Console.Title = $"Article management v{Vars.Version} | Connected users: {Vars.ConnectedUsers}";
        }
    }
}