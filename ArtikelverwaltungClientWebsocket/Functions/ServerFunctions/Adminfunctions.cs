namespace ArtikelverwaltungClientWebsocket.Functions.ServerFunctions
{
    public class Adminfunctions
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        
        internal static void CloseServer()
        {
            logger.AddLine("called");
        }

        internal static void SaveList()
        {
            logger.AddLine("called");
        }

        internal static void ClearList()
        {
            logger.AddLine("called");
        }
    }
}