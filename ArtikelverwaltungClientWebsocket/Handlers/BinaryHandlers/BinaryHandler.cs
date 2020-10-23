namespace ArtikelverwaltungClientWebsocket.Handlers.BinaryHandlers
{
    public class BinaryHandler
    {
        internal static void Handle(byte[] data)
        {
            BinaryLoader.Loader.Load(data);
        }
    }
}