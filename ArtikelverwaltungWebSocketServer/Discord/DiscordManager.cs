using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace ArtikelverwaltungWebSocketServer.Discord
{
    public class DiscordManager
    {
        private DiscordSocketClient _Client;
        
        private static Program.Client server = new Program.Client();

        internal static void Init()
        {
            Console.Clear();
            Console.Write("Please input your Discord ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("BOT");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" token: ");
            Env.Vars.Token = Console.ReadLine();
            Console.Clear();
            Console.Write("Please input your Discord User ID to set yourself as the bot owner: ");
            Env.Vars.ownerId = Convert.ToUInt64(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("[Discord] Starting bot setup"); 
            new DiscordManager().Start().GetAwaiter().GetResult();
        }
        
        public async Task Start()
        {
            _Client = new DiscordSocketClient();

            _Client.Log += Log;
            _Client.MessageReceived += OnMessage;
            _Client.Ready += OnReady;
            
            await _Client.LoginAsync(TokenType.Bot, Env.Vars.Token);
            await _Client.StartAsync();

            await Task.Delay(-1);
        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[Discord] LOG: {msg.ToString()}");
            return Task.CompletedTask;
        }

        
        private async Task OnMessage(SocketMessage message)
        {
            if (message.Content == "art!addchannel")
            {
                if (message.Author.Id == Env.Vars.ownerId)
                {
                    Env.Vars.channelIds.Add(message.Channel.Id);
                    await message.Channel.SendMessageAsync($"Added <#{message.Channel.Id}>({message.Channel.Id}) to the list of permitted channels.");
                }
                else
                {
                    await message.Channel.SendMessageAsync("https://media.nekro-works.de/rump-image.jpg");
                }
            }
            

            if (Env.Vars.channelIds.Contains(message.Channel.Id))
            {
                if (message.Content.ToLower() == "art!read")
                {Console.WriteLine($"[Discord] User {message.Author.Discriminator}({message.Author.Id}) requested list.");
                    string toSend =
                        
                        await message.Channel.SendMessageAsync(toSend);
                }

                if (message.Content.ToLower().StartsWith("art!sort "))
                {
                    string sortBy = message.Content.ToLower().Substring(9);
                    if (sortBy == "id")
                    {
                        
                    }
                    else if (sortBy == "name")
                    {
                        
                    }
                    else if (sortBy == "price")
                    {
                        
                    }
                    else if (sortBy == "count")
                    {
                        
                    }
                }
            }
        }

        private async Task OnReady()
        {
            Program.isReady = true;
        }
    }
}