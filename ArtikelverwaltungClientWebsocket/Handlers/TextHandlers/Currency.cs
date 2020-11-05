using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Vars;

namespace ArtikelverwaltungClientWebsocket.Handlers.TextHandlers
{
    public static class Currency
    {
        private static readonly Logger Logger =
            LogHandler.Logger;

        internal static void Handle(string data)
        {
            Logger.AddLine("message was currency info");
            var currency = data.Substring(13);
            if (Vars.Currency == null)
            {
                Logger.AddLine("server uses currency: " + currency);
                Vars.Currency = currency;
            }
            else
            {
                Logger.AddLine("server switched currency to: " + currency);
                Vars.Currency = currency;
            }
        }
    }
}