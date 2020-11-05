namespace ArtikelverwaltungWebSocketServer.Structs
{
    public struct Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        //public bool hasFile { get; set; } // this is used to store a file on the server (maybe a .pdf with extra infos) -> temporary not in use
    }
}