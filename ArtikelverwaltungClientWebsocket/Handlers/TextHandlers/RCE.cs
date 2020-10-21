﻿namespace ArtikelverwaltungClientWebsocket.Handlers.TextHandlers
{
    public class RCE
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;

        internal static void Handle(string data)
        {
            string toOpen = data.Substring(9);
            logger.AddLine("got told to open: " + toOpen);
            Utils.OpenBrowser(data);
        }
    }
}