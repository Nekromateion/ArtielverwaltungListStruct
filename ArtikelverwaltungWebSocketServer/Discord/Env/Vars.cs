using System.Collections.Generic;

namespace ArtikelverwaltungWebSocketServer.Discord.Env
{
    public class Vars
    {
        internal static string Token { get; set; }
        internal static ulong ownerId { get; set; }
        internal static List<ulong> editors { get; set; }
        internal static List<ulong> channelIds  = new List<ulong>();
        internal static List<LogChannel> locChannels = new List<LogChannel>();
        internal static List<string> LogMessages = new List<string>();
    }
}