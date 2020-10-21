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
            logger.AddLine("sent data request");
        }

        internal static void InitCurrency()
        {
            logger.AddLine("called");
            ConnectionManager.socket.Send("get currency");
            logger.AddLine("sent currency request");
        }

        internal static void RequestStatusBroadcast()
        {
            logger.AddLine("called");
            ConnectionManager.socket.Send("broadcast status");
            logger.AddLine("sent status request");
        }
    }
}