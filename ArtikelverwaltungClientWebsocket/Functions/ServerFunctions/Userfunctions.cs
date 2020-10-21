using System;

namespace ArtikelverwaltungClientWebsocket.Functions.ServerFunctions
{
    public class Userfunctions
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        internal static void InitList()
        {
            logger.AddLine("called");
            ConnectionManager.socket.Send("request data");
        }

        internal static void InitCurrency()
        {
            logger.AddLine("called");
            ConnectionManager.socket.Send("get currency");
        }

        internal static void RequestStatusBroadcast()
        {
            logger.AddLine("called");
            ConnectionManager.socket.Send("broadcast status");
        }
    }
}