namespace ArtikelverwaltungWebSocketServer.Discord
{
    public struct UserAdd
    {
        public ulong UserId { get; set; }
        public int Id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public int Count { get; set; }
    }
}