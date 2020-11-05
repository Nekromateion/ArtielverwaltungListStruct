using ArtikelverwalktungClientWebsocket;

namespace ArtikelverwaltungClientWebsocket.Functions.ServerFunctions
{
    public static class UserFunctions
    {
        private static readonly Logger Logger =
            LogHandler.Logger;

        internal static void InitList()
        {
            Logger.AddLine("called");
            ConnectionManager.Socket.Send("request data");
            Logger.AddLine("sent data request");
        }

        internal static void InitCurrency()
        {
            Logger.AddLine("called");
            ConnectionManager.Socket.Send("get currency");
            Logger.AddLine("sent currency request");
        }

        internal static void RequestStatusBroadcast()
        {
            Logger.AddLine("called");
            ConnectionManager.Socket.Send("broadcast status");
            Logger.AddLine("sent status request");
        }
    }
}