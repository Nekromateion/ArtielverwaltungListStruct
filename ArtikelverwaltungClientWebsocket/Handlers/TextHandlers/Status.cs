using System;
using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Vars;

namespace ArtikelverwaltungClientWebsocket.Handlers.TextHandlers
{
    public static class Status
    {
        private static readonly Logger Logger =
            LogHandler.Logger;

        internal static void Handle(string data)
        {
            Logger.AddLine("message was status");
            var message = data.Substring(6);
            Logger.AddLine("Status message: " + message);
            var numbers = data.Split(' ');
            Vars.ConnectedUsers = Convert.ToInt32(numbers[2].Replace(" ", string.Empty));
            Console.Title = $"Article management version: {Vars.Version} | Connected users: {Vars.ConnectedUsers}";
        }
    }
}