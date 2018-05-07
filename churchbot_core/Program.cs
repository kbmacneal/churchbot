using System;
using System.Collections.Generic;
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
    internal class Program {
        private static void Main (string[] args) => new Program ().RunBotAsync (args[0]).GetAwaiter ().GetResult ();

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
            "vg!"
        };

        private async Task<SortedDictionary<string, string>> GenPrefixMapping () {
            SortedDictionary<string, string> rtn = new SortedDictionary<string, string> ();

            rtn.Add ("cr!", "House Crux");
            rtn.Add ("fr!", "House Fornax");
            rtn.Add ("vl!", "House Vela");
            rtn.Add ("aq!", "House Aquila");
            rtn.Add ("er!", "House Eridanus");
            rtn.Add ("ly!", "House Lyra");
            rtn.Add ("py!", "House Pyxis");
            rtn.Add ("rt!", "House Reticulum");
            rtn.Add ("sr!", "House Serpens");
            rtn.Add ("tg!", "House Triangulum");
            rtn.Add ("tr!", "The Trilliant Ring");
            rtn.Add ("ac!", "ACRE");
            rtn.Add ("pr!", "The Prism Network");
            rtn.Add ("dt!", "The Deathless");
            rtn.Add ("cb!", "The High Church of the Messiah-as-Emperor");
            rtn.Add ("rp!", "The Church of Humanity, Repentant");
            rtn.Add ("14!", " 14 Red Dogs Triad");
            rtn.Add ("up!", "The Unified People's Collective");
            rtn.Add ("vg!", "\"House\" Vagrant");

            return rtn;
        }

        public async Task RunBotAsync (string botToken) {
            //this directory has to exist for voting to work, make sure it exists.
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

            if (prefixes.Any (message.Content.ToString ().Substring (0, 3).Contains)) {
                string msg_prefix = message.Content.ToString ().Substring (0, 3);
                var context = new SocketCommandContext (_client, message);

                string logmessage = String.Concat (message.Author, " sent command ", message.Content);

                await Log (new LogMessage (LogSeverity.Info, "VERBOSE", logmessage));

                string fullcommand = message.Content;

                string command = fullcommand.Replace (msg_prefix, "");

                string prefix = fullcommand.ToString ().Substring (0, 3);

                SocketGuildUser user = (message.Author as SocketGuildUser);

                string user_faction = ((GenPrefixMapping ()).Result) [prefix];

                bool isAuthorized = CheckAuthorization (user, msg_prefix);

                if (!isAuthorized) {

                    await SendPMAsync ("You are not authorized to send messages for this Faction.", message.Author);

                    return;
                }

                if (fullcommand.ToString ().Contains ("votefor") || fullcommand.ToString ().Contains ("votetally") || fullcommand.ToString ().Contains ("listquestions")) {

                    voting.voting vt = new churchbot.voting.voting ();
                    List<string> returns = await vt.ProcessVote (message, msg_prefix);
                    foreach (string rtn in returns) {
                        await SendPMAsync (rtn, message.Author);
                    }
                } else if (fullcommand.ToString ().Contains ("addquestion")) {
                    churchbot.voting.voting vt = new churchbot.voting.voting ();
                    List<int> ids = new List<int> ();
                    string path = String.Concat ("votes\\" + prefix.Replace ("!", ""), "\\");
                    //make sure the directory exists before we call any other methods
                    if (!(Directory.Exists (path))) Directory.CreateDirectory (path);
                    string[] files = System.IO.Directory.GetFiles (path);
                    int id = 0;

                    foreach (string file in files) {
                        ids.Add (Convert.ToInt32 (file.Replace (path + "\\", "").Replace (".json", "")));
                    }

                    if (ids.Count == 0) {
                        id = 1;
                    } else {
                        id = ids.Max () + 1;
                    }

                    List<string> returns = await vt.AddQuestion (id, msg_prefix);

                    await SendPMAsync (returns[0], message.Author);

                } else {

                    switch (msg_prefix) {
                        case "cb!":
                            Modules.HighChurch.commands cmd = new Modules.HighChurch.commands ();
                            Type type = cmd.GetType ();
                            MethodInfo methodInfo = type.GetMethod (FirstCharToUpper (command) + "Async");
                            List<object> list = new List<object> ();
                            list.Add (message.Author);
                            methodInfo.Invoke (cmd, list.ToArray ());
                            break;
                        default:
                            await SendPMAsync ("There are no commands associated with this faction. Please consult an admin", message.Author);
                            break;
                    }
                }
            }
        }

        public async Task SendPMAsync (string message, SocketUser user) {
            await user.SendMessageAsync (message);
        }

        private bool CheckAuthorization (SocketGuildUser user, string prefix) {
            bool isAuthorized = false;

            switch (prefix) {
                case "cr!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Crux");
                    break;
                case "fr!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Fornax");
                    break;
                case "vl!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Vela");
                    break;
                case "aq!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Aquila");
                    break;
                case "er!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Eridanus");
                    break;
                case "ly!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Lyra");
                    break;
                case "py!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Pyxis");
                    break;
                case "rt!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Reticulum");
                    break;
                case "sr!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Serpens");
                    break;
                case "tg!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Triangulum");
                    break;
                case "tr!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("The Trilliant Ring");
                    break;
                case "ac!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("ACRE");
                    break;
                case "pr!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("The Prism Network");
                    break;
                case "dt!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("The Deathless");
                    break;
                case "cb!":
                    if ((user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("The High Church of the Messiah-as-Emperor") || (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("church_member")) {
                        isAuthorized = true;
                    }
                    break;
                case "rp!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("The Church of Humanity, Repentant");
                    break;
                case "14!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("14 Red Dogs Triad");
                    break;
                case "up!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("The Unified People's Collective");
                    break;
                case "vg!":
                    isAuthorized = (user as IGuildUser).Guild.Roles.Select (s => s.Name).Contains ("House Vagrant");
                    break;
                default:

                    break;
            }

            return isAuthorized;
        }

        public static string FirstCharToUpper (string s) {
            // Check for empty string.
            if (string.IsNullOrEmpty (s)) {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper (s[0]) + s.Substring (1);
        }

    }
}