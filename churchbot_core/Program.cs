using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace churchbot {
    internal class Program
    {

        private static string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        private static void Main(string[] args) => new Program().RunBotAsync(args[0]).GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        public static readonly string[] prefixes = {
            "cr!",
            "fr!",
            "vl!",
            "aq!",
            "er!",
            "ly!",
            "py!",
            "rt!",
            "sr!",
            "tr!",
            "ac!",
            "pr!",
            "dt!",
            "cb!",
            "rp!",
            "14!",
            "up!",
            "vg!",
            "!"
        };

        private async Task<SortedDictionary<string, string[]>> GenPrefixMapping()
        {
            SortedDictionary<string, string[]> rtn = new SortedDictionary<string, string[]>();

            rtn.Add("cr!", new string[] {"House Crux"});
            rtn.Add("fr!", new string[] {"House Fornax"});
            rtn.Add("vl!", new string[] {"House Vela"});
            rtn.Add("aq!", new string[] {"House Aquila"});
            rtn.Add("er!", new string[] {"House Eridanus"});
            rtn.Add("ly!", new string[] {"House Lyra"});
            rtn.Add("py!", new string[] {"House Pyxis"});
            rtn.Add("rt!", new string[] {"House Reticulum"});
            rtn.Add("sr!", new string[] {"House Serpens"});
            rtn.Add("tg!", new string[] {"House Triangulum"});
            rtn.Add("tr!", new string[] {"The Trilliant Ring"});
            rtn.Add("ac!", new string[] {"ACRE"});
            rtn.Add("pr!", new string[] {"The Prism Network"});
            rtn.Add("dt!", new string[] {"The Deathless"});
            rtn.Add("cb!", new string[] {"The High Church of the Messiah-as-Emperor","church_member"});
            rtn.Add("rp!", new string[] {"The Church of Humanity, Repentant"});
            rtn.Add("14!", new string[] {" 14 Red Dogs Triad"});
            rtn.Add("up!", new string[] {"The Unified People's Collective"});
            rtn.Add("vg!", new string[] {"\"House\" Vagrant"});
            rtn.Add("!", new string[] {"Default"});

            return rtn;
        }

        public async Task RunBotAsync(string botToken)
        {
            //this directory has to exist for voting to work, make sure it exists.
            if (!(System.IO.Directory.Exists("votes"))) System.IO.Directory.CreateDirectory("votes");

            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();

            //event subscriptions
            _client.Log += Log;

            await RegisterCommandAsync();

            await _client.LoginAsync(TokenType.Bot, botToken);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
            {
                return;
            }

            string msg_prefix = "";
                if(message.Content.ToString().ToCharArray()[0] == '!'){
                        msg_prefix = "!";
                }
                else{
                    msg_prefix = message.Content.ToString().Substring(0, 3);
                }

            if (prefixes.Any(msg_prefix.Contains))
            {
                // var context = new SocketCommandContext(_client, message);

                string logmessage = String.Concat(message.Author, " sent command ", message.Content);

                await Log(new LogMessage(LogSeverity.Info, "VERBOSE", logmessage));

                string fullcommand = message.Content;

                string command = fullcommand.Replace(msg_prefix, "");

                SocketGuildUser user = (message.Author as SocketGuildUser);

                bool isAuthorized = CheckAuthorization(user, msg_prefix);

                if (!isAuthorized)
                {

                    await SendPMAsync("You are not authorized to send messages for this Faction.", message.Author);

                    return;
                }

                if (fullcommand.ToString().Contains("votefor") || fullcommand.ToString().Contains("votetally") || fullcommand.ToString().Contains("listquestions"))
                {

                    voting.voting vt = new churchbot.voting.voting();
                    List<string> returns = await vt.ProcessVote(message, msg_prefix);
                    foreach (string rtn in returns)
                    {
                        await SendPMAsync(rtn, message.Author);
                    }
                }
                else if (fullcommand.ToString().Contains("addquestion"))
                {
                    churchbot.voting.voting vt = new churchbot.voting.voting();
                    List<int> ids = new List<int>();
                    string path = String.Concat("votes/" + msg_prefix.Replace("!", ""), "/");
                    //make sure the directory exists before we call any other methods
                    if (!(Directory.Exists(path))) Directory.CreateDirectory(path);

                    string[] files = System.IO.Directory.GetFiles(path);
                    int id = 0;

                    foreach (string file in files)
                    {
                        ids.Add(Convert.ToInt32(file.Replace(path, "").Replace(".json", "")));
                    }

                    if (ids.Count == 0)
                    {
                        id = 1;
                    }
                    else
                    {
                        id = ids.Max() + 1;
                    }

                    List<string> returns = await vt.AddQuestion(id, msg_prefix);

                    await SendPMAsync(returns[0], message.Author);

                }
                else
                {

                    switch (msg_prefix)
                    {
                        case "cb!":
                            Modules.HighChurch.commands cmd = new Modules.HighChurch.commands();
                            Type type = cmd.GetType();
                            MethodInfo methodInfo = type.GetMethod(FirstCharToUpper(command) + "Async");
                            List<object> list = new List<object>();
                            list.Add(message.Author);
                            methodInfo.Invoke(cmd, list.ToArray());
                            break;
                        case "!":
                            Modules.Default.commands cmd2 = new Modules.Default.commands();
                            Type type2 = cmd2.GetType();
                            MethodInfo methodInfo2 = type2.GetMethod(FirstCharToUpper(command)+"Async");
                            List<object> list2 = new List<object>();
                            list2.Add(message.Author);
                            methodInfo2.Invoke(cmd2, list2.ToArray());
                            break;
                        default:
                            await SendPMAsync("There are no commands associated with this faction. Please consult an admin", message.Author);
                            break;
                    }
                }
            }
        }

        public async Task SendPMAsync(string message, SocketUser user)
        {
            await user.SendMessageAsync(message);
        }

        private bool CheckAuthorization(SocketGuildUser user, string prefix)
        {
            bool isAuthorized = false;

            string[] user_faction = ((GenPrefixMapping()).Result)[prefix];

            foreach(string faction in user_faction)
            {
                if(user.Roles.Select(s => s.Name).Contains(faction)){
                    isAuthorized=true;
                    break;
                }
            }

            if(prefix == "!")
            {
                isAuthorized=true;
            }

            return isAuthorized;
        }

        public static string FirstCharToUpper(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }


        

    }
}