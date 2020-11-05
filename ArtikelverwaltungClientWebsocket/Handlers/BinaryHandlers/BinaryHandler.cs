namespace ArtikelverwaltungClientWebsocket.Handlers.BinaryHandlers
{
    public static class BinaryHandler
    {
        internal static void Handle(byte[] data)
        {
            BinaryLoader.Loader.Load(data);
        }
    }
}