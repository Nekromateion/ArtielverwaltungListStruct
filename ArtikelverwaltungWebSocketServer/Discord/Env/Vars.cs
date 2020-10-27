using System.Collections.Generic;

namespace ArtikelverwaltungWebSocketServer.Discord.Env
{
    public class Vars
    {
        internal static string Token { get; set; }
        internal static ulong ownerId { get; set; }
        
        internal static List<ulong> channelIds { get; set; }
    }
}