using ArtikelverwaltungClientWebsocket.BinaryLoader;

namespace ArtikelverwaltungClientWebsocket.Handlers.BinaryHandlers
{
    public static class BinaryHandler
    {
        internal static void Handle(byte[] data)
        {
            Loader.Load(data);
        }
    }
}