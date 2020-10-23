using System;

namespace ArtikelverwaltungClientWebsocket.Handlers.TextHandlers
{
    public class RCE
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;

        internal static void Handle(string data)
        {
            logger.AddLine("called");
            string toOpen = data.Substring(10);
            logger.AddLine("got told to open: " + toOpen);
            Utils.OpenBrowser(toOpen);
        }
    }
}