﻿using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Utils;

namespace ArtikelverwaltungClientWebsocket.Handlers.TextHandlers
{
    public static class Rce
    {
        private static readonly Logger Logger =
            LogHandler.Logger;

        internal static void Handle(string data)
        {
            Logger.AddLine("called");
            string toOpen = data.Substring(10);
            Logger.AddLine("got told to open: " + toOpen);
            Utils.OpenBrowser(toOpen);
        }
    }
}