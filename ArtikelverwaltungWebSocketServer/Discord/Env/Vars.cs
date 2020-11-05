using System.Collections.Generic;

namespace ArtikelverwaltungWebSocketServer.Discord.Env
{
    public static class Vars
    {
        internal static readonly List<ulong> ChannelIds = new List<ulong>();
        internal static readonly List<LogChannel> LocChannels = new List<LogChannel>();
        internal static readonly List<string> LogMessages = new List<string>();
        internal static List<UserAdd> TemporaryArticles = new List<UserAdd>();
        internal static string Token { get; set; }
        internal static ulong OwnerId { get; set; }
        internal static List<ulong> Editors => null;
    }
}