using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace churchbot {
    internal class Program {
        private static void Main (string[] args) => new Program ().RunBotAsync (args[0]).GetAwaiter ().GetResult ();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync (string botToken) {
            //this directory has to exist
            if (!(System.IO.Directory.Exists ("votes"))) System.IO.Directory.CreateDirectory ("votes");

            _client = new DiscordSocketClient ();
            _commands = new CommandService ();
            _services = new ServiceCollection ().AddSingleton (_client).AddSingleton (_commands).BuildServiceProvider ();

            //event subscriptions
            _client.Log += Log;

            await RegisterCommandAsync ();

            await _client.LoginAsync (TokenType.Bot, botToken);

            await _client.StartAsync ();

            await Task.Delay (-1);
        }

        private Task Log (LogMessage arg) {
            Console.WriteLine (arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandAsync () {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync (Assembly.GetEntryAssembly ());
        }

        private async Task HandleCommandAsync (SocketMessage arg) {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot) {
                return;
            }

            int argPosition = 0;

            if (message.HasStringPrefix ("cb!", ref argPosition) || message.HasMentionPrefix (_client.CurrentUser, ref argPosition)) {
                var context = new SocketCommandContext (_client, message);

                string logmessage = String.Concat (message.Author, " sent command ", message.Content);
                int test = 0;
                await Log (new LogMessage (LogSeverity.Info, "VERBOSE", logmessage));

                string fullcommand = message.Content;
                SocketGuildUser user = (message.Author as SocketGuildUser);
                bool isChurchUser = false;
                IRole role = (message.Author as IGuildUser).Guild.Roles.FirstOrDefault (s => s.Name == "church_member");
                if (user.Roles.Contains (role)) {
                    isChurchUser = true;
                }

                if (fullcommand.ToString ().Contains ("votefor") || fullcommand.ToString ().Contains ("votetally") || fullcommand.ToString ().Contains ("listquestions")) {
                    if (!isChurchUser) {
                        await SendPMAsync ("You must be a church member to vote.", message.Author);
                        return;
                    }

                    churchbot.voting.voting vt = new churchbot.voting.voting ();
                    List<string> returns = await vt.ProcessVote (message);
                    foreach (string rtn in returns) {
                        await SendPMAsync (rtn, message.Author);
                    }
                } else if (fullcommand.ToString ().Contains ("addquestion")) {
                    churchbot.voting.voting vt = new churchbot.voting.voting ();
                    if (Int32.TryParse (fullcommand.Split ("addquestion") [1], out test)) {
                        int id = test;

                        List<string> returns = await vt.AddQuestion (id);

                        await SendPMAsync (returns[0], message.Author);
                    } else {
                        await SendPMAsync ("Invalid request.", message.Author);
                    }

                } else {
                    var result = await _commands.ExecuteAsync (context, argPosition, _services);

                    if (!result.IsSuccess) {
                        Console.WriteLine (result.ErrorReason);
                    }
                }
            }
        }

        public async Task SendPMAsync (string message, SocketUser user) {
            await user.SendMessageAsync (message);
        }

    }
}