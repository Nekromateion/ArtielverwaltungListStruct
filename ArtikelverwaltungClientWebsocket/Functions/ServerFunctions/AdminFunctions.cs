using ArtikelverwalktungClientWebsocket;
using ArtikelverwaltungClientWebsocket;
using ArtikelverwaltungClientWebsocket.UtilsVarsStructs.Vars;

namespace ArtikelverwaltungClientWebsocket.Functions.ServerFunctions
{
    public static class AdminFunctions
    {
        private static readonly Logger Logger =
            LogHandler.Logger;
        
        internal static void CloseServer()
        {
            Logger.AddLine("called");
            ConnectionManager.Socket.Send("close server " + Vars.AdminKey);
        }

        internal static void SaveList()
        {
            Logger.AddLine("called");
            ConnectionManager.Socket.Send("save server list " + Vars.AdminKey);
        }

        internal static void ClearList()
        {
            Logger.AddLine("called");
            ConnectionManager.Socket.Send("clear server list " + Vars.AdminKey);
        }
    }
}