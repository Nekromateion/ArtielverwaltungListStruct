namespace ArtikelverwaltungWebSocketServer
{
    public struct Article
    {
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public int count { get; set; }
        //public bool hasFile { get; set; } // this is used to store a file on the server (maybe a .pdf with extra infos) -> temporary not in use
    }
}