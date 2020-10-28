﻿using System;
using System.CodeDom.Compiler;
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
            new Thread(() =>
            {
                while (true)
                {
                    if (Env.Vars.locChannels.Count > 0)
                    {
                        if (Env.Vars.LogMessages.Count > 0)
                        {
                            foreach (LogChannel logChannel in Env.Vars.locChannels)
                            {
                                if (Env.Vars.LogMessages[0].Length > 1900) _Client.GetGuild(logChannel.ServerId).GetTextChannel(logChannel.ChannelId).SendMessageAsync(Env.Vars.LogMessages[0].Substring(1890) + " ...");
                                else _Client.GetGuild(logChannel.ServerId).GetTextChannel(logChannel.ChannelId).SendMessageAsync(Env.Vars.LogMessages[0]);
                                if(Env.Vars.locChannels.Count > 1) Thread.Sleep(200);
                            }

                            Env.Vars.LogMessages.Remove(Env.Vars.LogMessages[0]);
                        }
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[Discord] LOG: {msg.ToString()}");
            Env.Vars.LogMessages.Add($"[Discord] [{DateTime.Now.ToString("HH.mm.ss.ffffff")}] " + msg.ToString());
            return Task.CompletedTask;
        }

        
        private async Task OnMessage(SocketMessage message)
        {
            if (!message.Author.IsBot)
            {
                if (message.Content == "art!addchannel")
                {
                    if (message.Author.Id == Env.Vars.ownerId)
                    {
                        Env.Vars.channelIds.Add(message.Channel.Id);
                        Env.Vars.LogMessages.Add($"[Discord] [{DateTime.Now.ToString("HH.mm.ss.ffffff")}] <@!{message.Author.Id}> added <#{message.Channel.Id}> to the command channel list");
                        await message.Channel.SendMessageAsync($"Added <#{message.Channel.Id}>({message.Channel.Id}) to the list of permitted channels.");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("https://media.nekro-works.de/rump-image.jpg");
                    }
                }
                else if (message.Content == "art!logchannel")
                {
                    if (message.Author.Id == Env.Vars.ownerId)
                    {
                        LogChannel tmp = new LogChannel();
                        SocketGuildChannel t = message.Channel as SocketGuildChannel;
                        tmp.ServerId = t.Guild.Id;
                        tmp.ChannelId = message.Channel.Id;
                        Env.Vars.locChannels.Add(tmp);
                        Env.Vars.LogMessages.Add($"[Discord] [{DateTime.Now.ToString("HH.mm.ss.ffffff")}] <@!{message.Author.Id}> added <#{message.Channel.Id}> to the log channel list");
                        await message.Channel.SendMessageAsync($"Added <#{message.Channel.Id}>({message.Channel.Id}) to the list of log channels.");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("https://media.nekro-works.de/rump-image.jpg");
                    }
                }


                if (Env.Vars.channelIds.Contains(message.Channel.Id))
                {
                    Env.Vars.LogMessages.Add($"[Discord] [{DateTime.Now.ToString("HH.mm.ss.ffffff")}] Message from a command channel in <#{message.Channel.Id}> by <@!{message.Author.Id}> saying: {message.Content}");
                    if (message.Content.ToLower() == "art!read")
                    {
                        Console.WriteLine($"[Discord] User {message.Author.Discriminator}({message.Author.Id}) requested list.");
                        Env.Vars.LogMessages.Add();
                        string toSend = Tools.ReadList(Data.Articles);

                        await message.Channel.SendMessageAsync(toSend);
                    }

                    if (message.Content.ToLower().StartsWith("art!sort "))
                    {
                        string sortBy = message.Content.ToLower().Substring(9);
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
                            await message.Channel.SendMessageAsync($"`{sortBy}` is not valid");
                        }
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