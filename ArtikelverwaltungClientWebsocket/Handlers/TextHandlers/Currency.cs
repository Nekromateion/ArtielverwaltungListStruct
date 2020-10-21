namespace ArtikelverwaltungClientWebsocket.Handlers.TextHandlers
{
    public class Currency
    {
        private static ArtikelverwaltungClientWebsocketLoader.Logger logger =
            ArtikelverwaltungClientWebsocketLoader.LogHandler.logger;
        
        internal static void Handle(string data)
        {
            logger.AddLine("message was currency info");
            string currency = data.Substring(12);
            if (Vars.Currency == null)
            {
                logger.AddLine("server uses currency: " + currency);
                Vars.Currency = currency;
            }
            else
            {
                logger.AddLine("server switched currency to: " + currency);
                Vars.Currency = currency;
            }
        }
    }
}