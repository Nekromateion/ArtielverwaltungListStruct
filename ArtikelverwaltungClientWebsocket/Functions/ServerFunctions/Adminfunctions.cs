namespace ArtikelverwaltungClientWebsocket.Functions.ServerFunctions
{
    public class Adminfunctions
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        
        internal static void CloseServer()
        {
            logger.AddLine("called");
            ConnectionManager.socket.Send("close server " + Vars.AdminKey);
        }

        internal static void SaveList()
        {
            logger.AddLine("called");
            ConnectionManager.socket.Send("save server list " + Vars.AdminKey);
        }

        internal static void ClearList()
        {
            logger.AddLine("called");
            ConnectionManager.socket.Send("clear server list " + Vars.AdminKey);
        }
    }
}