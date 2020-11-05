using System;
using System.Threading;
using System.Threading.Tasks;
using ArtikelverwaltungWebSocketServer.DataVars;
using ArtikelverwaltungWebSocketServer.Discord.Env;
using ArtikelverwaltungWebSocketServer.Structs;
using Discord;
using Discord.WebSocket;

namespace ArtikelverwaltungWebSocketServer.Discord
{
    public class DiscordManager
    {
        private DiscordSocketClient _client;

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
            Env.Vars.OwnerId = Convert.ToUInt64(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("[Discord] Starting bot setup");
            new DiscordManager().Start().GetAwaiter().GetResult();
        }

        private async Task Start()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += OnMessage;
            _client.Ready += OnReady;
            
            await _client.LoginAsync(TokenType.Bot, Env.Vars.Token);
            await _client.StartAsync();
            new Thread(() =>
            {
                while (true)
                {
                    if (Env.Vars.LocChannels.Count > 0)
                    {
                        if (Env.Vars.LogMessages.Count > 0)
                        {
                            foreach (LogChannel logChannel in Env.Vars.LocChannels)
                            {
                                if (Env.Vars.LogMessages[0].Length > 1900) _client.GetGuild(logChannel.ServerId).GetTextChannel(logChannel.ChannelId).SendMessageAsync(Env.Vars.LogMessages[0].Substring(1890) + " ...");
                                else _client.GetGuild(logChannel.ServerId).GetTextChannel(logChannel.ChannelId).SendMessageAsync(Env.Vars.LogMessages[0]);
                                if(Env.Vars.LocChannels.Count > 1) Thread.Sleep(200);
                            }

                            Env.Vars.LogMessages.Remove(Env.Vars.LogMessages[0]);
                        }
                    }
                    Thread.Sleep(1000);
                }
                // ReSharper disable once FunctionNeverReturns
            }).Start();
        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[Discord] LOG: {msg.ToString()}");
            Env.Vars.LogMessages.Add($"[Discord] [{DateTime.Now:HH.mm.ss.ffffff}] " + msg.ToString());
            return Task.CompletedTask;
        }


        private async Task OnMessage(SocketMessage message)
        {
            try
            {
                if (!message.Author.IsBot)
                {
                    // ReSharper disable once StringLiteralTypo
                    if (message.Content == "art!addchannel")
                    {
                        if (message.Author.Id == Env.Vars.OwnerId)
                        {
                            Env.Vars.ChannelIds.Add(message.Channel.Id);
                            Env.Vars.LogMessages.Add(
                                $"[Discord] [{DateTime.Now:HH.mm.ss.ffffff}] <@!{message.Author.Id}> added <#{message.Channel.Id}> to the command channel list");
                            await message.Channel.SendMessageAsync(
                                $"Added <#{message.Channel.Id}>({message.Channel.Id}) to the list of permitted channels.");
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("https://media.nekro-works.de/rump-image.jpg");
                        }
                    }
                    // ReSharper disable once StringLiteralTypo
                    else if (message.Content == "art!logchannel")
                    {
                        if (message.Author.Id == Env.Vars.OwnerId)
                        {
                            LogChannel tmp = new LogChannel();
                            if (message.Channel is SocketGuildChannel t) tmp.ServerId = t.Guild.Id;
                            tmp.ChannelId = message.Channel.Id;
                            Env.Vars.LocChannels.Add(tmp);
                            Env.Vars.LogMessages.Add(
                                $"[Discord] [{DateTime.Now:HH.mm.ss.ffffff}] <@!{message.Author.Id}> added <#{message.Channel.Id}> to the log channel list");
                            await message.Channel.SendMessageAsync(
                                $"Added <#{message.Channel.Id}>({message.Channel.Id}) to the list of log channels.");
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("https://media.nekro-works.de/rump-image.jpg");
                        }
                    }
                    else if (message.Content.StartsWith("art!editor "))
                    {
                        if (message.Author.Id == Env.Vars.OwnerId)
                        {
                            string toReturn = "Added users ";
                            foreach (SocketUser user in message.MentionedUsers)
                            {
                                Env.Vars.Editors.Add(user.Id);
                                toReturn += " <@!" + user.Id + "> ";
                            }

                            if (toReturn.Length > 1890) toReturn = toReturn.Substring(0, 1950);
                            // ReSharper disable once RedundantAssignment
                            toReturn += "to list of editors";
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("https://media.nekro-works.de/rump-image.jpg");
                        }
                    }


                    if (Env.Vars.ChannelIds.Contains(message.Channel.Id))
                    {
                        Env.Vars.LogMessages.Add($"[Discord] [{DateTime.Now:HH.mm.ss.ffffff}] Message from a command channel in <#{message.Channel.Id}> by <@!{message.Author.Id}> saying: {message.Content}");
                        if (message.Content.ToLower() == "art!read" || message.Content.ToLower() == "art!list")
                        {
                            Console.WriteLine(
                                $"[Discord] User {message.Author.Discriminator}({message.Author.Id}) requested list.");
                            string toSend = Tools.ReadList(Data.Articles);
                            await message.Channel.SendMessageAsync(toSend);
                        }
                        else if (message.Content.ToLower().StartsWith("art!sort "))
                        {
                            string sortBy = message.Content.ToLower().Split(' ')[1];
                            if (sortBy == "id")
                            {
                                string toSend = Tools.SortById(Data.Articles);
                                await message.Channel.SendMessageAsync(toSend);
                            }
                            else if (sortBy == "name")
                            {
                                string toSend = Tools.SortByName(Data.Articles);
                                await message.Channel.SendMessageAsync(toSend);
                            }
                            else if (sortBy == "price")
                            {
                                string toSend = Tools.SortByPrice(Data.Articles);
                                await message.Channel.SendMessageAsync(toSend);
                            }
                            else if (sortBy == "count")
                            {
                                string toSend = Tools.SortByCount(Data.Articles);
                                await message.Channel.SendMessageAsync(toSend);
                            }
                            else
                            {
                                await message.Channel.SendMessageAsync($"`{sortBy}` is not a valid sorting term");
                            }
                        }
                        else if (message.Content.ToLower().StartsWith("art!search "))
                        {
                            try
                            {
                                string searchBy = message.Content.ToLower().Split(' ')[1];
                                string searchFor = message.Content.ToLower().Substring(message.Content.ToLower().Split(' ')[0].Length + 1 + message.Content.ToLower().Split(' ')[1].Length + 1);
                                if (searchBy == "id")
                                {
                                    string toSend = Tools.SearchById(Data.Articles, searchFor);
                                    await message.Channel.SendMessageAsync(toSend);
                                }
                                else if (searchBy == "name")
                                {
                                    string toSend = Tools.SearchByName(Data.Articles, searchFor);
                                    await message.Channel.SendMessageAsync(toSend);
                                }
                                else if (searchBy == "price")
                                {
                                    string toSend = Tools.SearchByPrice(Data.Articles, searchFor);
                                    await message.Channel.SendMessageAsync(toSend);
                                }
                                else if (searchBy == "count")
                                {
                                    string toSend = Tools.SearchByCount(Data.Articles, searchFor);
                                    await message.Channel.SendMessageAsync(toSend);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        else if (message.Content.ToLower().StartsWith("art!set "))
                        {
                            if (Env.Vars.Editors.Contains(message.Author.Id))
                            {
                                string setBy = message.Content.ToLower().Split(' ')[1];
                                string value = message.Content.ToLower().Substring(message.Content.ToLower().Split(' ')[0].Length + 1 + message.Content.ToLower().Split(' ')[1].Length + 1);
                                if (setBy == "id")
                                {
                                    string toReturn = Tools.SetId(Env.Vars.TemporaryArticles, value, message.Author);
                                    await message.Channel.SendMessageAsync(toReturn);
                                }
                                else if (setBy == "name")
                                {
                                    string toReturn = Tools.SetName(Env.Vars.TemporaryArticles, value, message.Author);
                                    await message.Channel.SendMessageAsync(toReturn);
                                }
                                else if (setBy == "price")
                                {
                                    string toReturn = Tools.SetPrice(Env.Vars.TemporaryArticles, value, message.Author);
                                    await message.Channel.SendMessageAsync(toReturn);
                                }
                                else if (setBy == "count")
                                {
                                    string toReturn = Tools.SetCount(Env.Vars.TemporaryArticles, value, message.Author);
                                    await message.Channel.SendMessageAsync(toReturn);
                                }
                            }
                            else
                            {
                                await message.Channel.SendMessageAsync("https://media.nekro-works.de/rump-image.jpg");
                            }
                        }
                        else if (message.Content.ToLower().StartsWith("art!submit"))
                        {
                            if (Env.Vars.Editors.Contains(message.Author.Id))
                            {
                                bool isFound = false;
                                UserAdd art = new UserAdd();
                                foreach (UserAdd t in Env.Vars.TemporaryArticles)
                                {
                                    if (t.UserId == message.Author.Id)
                                    {
                                        isFound = true;
                                        art = t;
                                    }
                                }

                                if (isFound)
                                {
                                    if (art.Name != null)
                                    {
                                        string toSend = "Added article:" + Environment.NewLine;
                                        toSend += "```" + Environment.NewLine;
                                        toSend += $"Id: {art.Id}" + Environment.NewLine;
                                        toSend += $"Name: {art.Name}" + Environment.NewLine;
                                        toSend += $"Price: {art.Price}" + Environment.NewLine;
                                        toSend += $"Count: {art.Count}" + "```" + Environment.NewLine;
                                        toSend += $"Article by <@!{message.Author.Id}>";
                                        Article temp = new Article
                                        {
                                            Id = art.Id, Name = art.Name, Price = art.Price, Count = art.Count
                                        };
                                        Data.Articles.Add(temp);
                                        Env.Vars.LogMessages.Add(
                                            $"[Discord] [{DateTime.Now:HH.mm.ss.ffffff}] <@!{message.Author.Id}> added article {art.Id}");
                                        await message.Channel.SendMessageAsync(toSend);
                                        Env.Vars.TemporaryArticles.Remove(art);
                                    }
                                    else
                                    {
                                        await message.Channel.SendMessageAsync(
                                            "Your cached article is incomplete please make sure you filled out all fields before submitting");
                                    }
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync(
                                        "There was no cached article found for you please start creating one.");
                                }
                            }
                            else
                            {
                                await message.Channel.SendMessageAsync("https://media.nekro-works.de/rump-image.jpg");
                            }
                        }
                        // ReSharper disable once StringLiteralTypo
                        else if (message.Content.ToLower().StartsWith("art!myarticle"))
                        {
                            if (Env.Vars.Editors.Contains(message.Author.Id))
                            {
                                bool isFound = false;
                                UserAdd art = new UserAdd();
                                foreach (UserAdd t in Env.Vars.TemporaryArticles)
                                {
                                    if (t.UserId == message.Author.Id)
                                    {
                                        isFound = true;
                                        art = t;
                                    }
                                }

                                if (isFound)
                                {
                                    string toSend = "Cached article:" + Environment.NewLine;
                                    toSend += "```" + Environment.NewLine;
                                    toSend += $"Id: {art.Id}" + Environment.NewLine;
                                    toSend += $"Name: {art.Name}" + Environment.NewLine;
                                    toSend += $"Price: {art.Price}" + Environment.NewLine;
                                    toSend += $"Count: {art.Count}" + "```" + Environment.NewLine;
                                    toSend += $"Article by <@!{message.Author.Id}>";
                                    await message.Channel.SendMessageAsync(toSend);
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync("You have no cached article please create one.");
                                }
                            }
                            else
                            {
                                await message.Channel.SendMessageAsync("https://media.nekro-works.de/rump-image.jpg");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Discord] ERROR");
                Console.WriteLine(e);
                await message.Channel.SendMessageAsync("An error occured, if you are the administrator please look at the log channel(s) for more information");
                Env.Vars.LogMessages.Add($"[Discord] [{DateTime.Now:HH.mm.ss.ffffff}] ERROR OCCURED: {e.Message}");
            }
        }

#pragma warning disable 1998
        private async Task OnReady()
#pragma warning restore 1998
        {
            Program.IsReady = true;
        }
    }
}