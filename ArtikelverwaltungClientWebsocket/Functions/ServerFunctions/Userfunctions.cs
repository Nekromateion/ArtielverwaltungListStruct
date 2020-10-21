namespace ArtikelverwaltungClientWebsocket.Functions.ServerFunctions
{
    public class Userfunctions
    {
        internal static void InitList()
        {
            ConnectionManager.socket.Send("request data");
        }

        internal static void InitCurrency()
        {
            ConnectionManager.socket.Send("get currency");
        }

        internal static void RequestStatusBroadcast()
        {
            
        }
    }
}