using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rpc;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace churchbot {
    internal class Program {
        private static void Main (string[] args) => new Program ().RunBotAsync ().GetAwaiter ().GetResult ();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync () {
            _client = new DiscordSocketClient ();
            _commands = new CommandService ();
            _services = new ServiceCollection ().AddSingleton (_client).AddSingleton (_commands).BuildServiceProvider ();

            //event subscriptions
            _client.Log += Log;

            //before publish, fill this out
            string botToken = "";

            await RegisterCommandAsync ();

            await _client.LoginAsync (TokenType.Bot, botToken);

            await _client.StartAsync ();

            SocketChannel bot_channel = _client.GetChannel (440690259752386560);

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

                await Log (new LogMessage (LogSeverity.Info, "VERBOSE", logmessage));

                var result = await _commands.ExecuteAsync (context, argPosition, _services);

                if (!result.IsSuccess) {
                    Console.WriteLine (result.ErrorReason);
                }
            }
        }
    }
}