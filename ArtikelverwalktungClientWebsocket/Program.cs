using WebSocketSharp;

namespace ArtikelverwalktungClientWebsocket
{
    public class SocketManager
    {
        public static WebSocket Socket = new WebSocket("ws://localhost/artikelverwaltung");
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            
        }

        internal static byte[] assembly;
    }
}