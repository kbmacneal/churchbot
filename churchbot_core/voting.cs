using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace churchbot.voting
{
    public class Question
    {
        public int QuestionID
        {
            get;
            set;
        }

        public string QuestionTitle
        {
            get;
            set;
        }

        public string QuestionText
        {
            get;
            set;
        }

        public SortedDictionary<int, string> Options
        {
            get;
            set;
        }
    }

    public class Vote
    {
        public User user = new User();

        public int QuestionID
        {
            get;
            set;
        }

        public int Choice
        {
            get;
            set;
        }
    }

    public class User
    {
        public string UserName
        {
            get;
            set;
        }

        public int ID
        {
            get;
            set;
        }

        public string NickName
        {
            get;
            set;
        }
    }

    public class Votes
    {
        public List<Vote> votes
        {
            get;
            set;
        }
    }

    public class VoteMessage
    {
        public string Message
        {
            get;
            set;
        }
    }

    public class voting
    {
        public async Task<List<string>> ProcessVote(SocketUserMessage message, string prefix)
        {
            string fullcommand = message.Content;

            List<string> rtn_messages = new List<string>();



            if (fullcommand.ToString().Contains("votefor"))
            {
                //cb!votefor1:1
                User user = new User();
                Vote vote = new Vote();
                int test = 0;

                user.UserName = message.Author.ToString().Split('#')[0];
                user.NickName = (message.Author as SocketGuildUser).Nickname;

                if (Int32.TryParse(message.Author.ToString().Split('#')[1], out test))
                {
                    user.ID = test;
                }
                else
                {
                    user.ID = 0;
                }

                if (Int32.TryParse(fullcommand.ToString().Split("votefor")[1].Split(":")[0], out test))
                {
                    vote.QuestionID = test;
                }
                else
                {
                    vote.QuestionID = 0;
                }

                if (Int32.TryParse(fullcommand.ToString().Split("votefor")[1].Split(":")[1], out test))
                {
                    vote.Choice = test;
                }
                else
                {
                    vote.Choice = 0;
                }

                vote.user = user;

                rtn_messages.Add(await CastVote(vote, message.Author, prefix));
            }
            else if (fullcommand.ToString().Contains("votetally"))
            {
                //cb!votetally1
                int test = 0;

                if (Int32.TryParse(fullcommand.ToString().Split("votetally")[1], out test))
                {
                    List<string> rtns = ReturnTally(test, message.Author, prefix).Result;

                    foreach (string msg in rtns)
                    {
                        rtn_messages.Add(msg);
                    }
                }
                else
                {
                    rtn_messages.Add("Invalid request");
                }
            }
            else if (fullcommand.ToString().Contains("listquestions"))
            {

                List<string> rtns = await ListVotes(prefix);

                foreach (string msg in rtns)
                {
                    rtn_messages.Add(msg);
                }
            }
            else
            {
                rtn_messages.Add("Invalid request");
            }

            return rtn_messages;
        }

        private async Task<string> CastVote(Vote vote, SocketUser user, string prefix)
        {
            string path = GetGuildDir(prefix);

            if (System.IO.File.Exists(path + vote.QuestionID.ToString() + ".json"))
            {
                string filecontents = System.IO.File.ReadAllText(path + vote.QuestionID.ToString() + ".json");

                Votes votes = JsonConvert.DeserializeObject<Votes>(filecontents);

                if (votes is null)
                {
                    votes = new Votes();
                    votes.votes = new List<Vote>();
                    votes.votes.Add(vote);

                    string serialized = JsonConvert.SerializeObject(votes);

                    System.IO.File.WriteAllText(path + vote.QuestionID.ToString() + ".json", serialized);

                    return (String.Concat(vote.user.UserName, " has successfully cast their vote."));
                }
                else
                {
                    if (votes.votes.Where(s => s.user.UserName == vote.user.UserName).Count() > 0)
                    {
                        foreach (Vote item in votes.votes)
                        {
                            if (item.user.UserName == vote.user.UserName && item.user.ID == vote.user.ID)
                            {
                                item.Choice = vote.Choice;
                            }
                        }
                        string serialized = JsonConvert.SerializeObject(votes);

                        System.IO.File.WriteAllText(path + vote.QuestionID.ToString() + ".json", serialized);

                        return (String.Concat(vote.user.UserName, " has successfully changed their vote."));
                    }
                    else
                    {
                        votes.votes.Add(vote);

                        string serialized = JsonConvert.SerializeObject(votes);

                        System.IO.File.WriteAllText(path + vote.QuestionID.ToString() + ".json", serialized);

                        return (String.Concat(vote.user.UserName, " has successfully cast their vote."));
                    }
                }
            }
            else
            {
                return (String.Concat("There is no vote with the provided ID. Please enter a valid vote id."));
            }
        }

        private async Task<List<string>> ReturnTally(int votenum, SocketUser user, string prefix)
        {
            List<string> tallies = new List<string>();

            string prepath = GetGuildDir(prefix);

            if (System.IO.File.Exists(prepath + votenum.ToString() + ".json"))
            {
                string path = string.Concat(prepath + votenum.ToString() + ".json");

                string value = System.IO.File.ReadAllText(path);

                Votes tally = JsonConvert.DeserializeObject<Votes>(value);

                List<int> Options = tally.votes.Select(s => s.Choice).Distinct().ToList();

                foreach (int opt in Options)
                {
                    tallies.Add(String.Concat("Tally for option ", votenum, " is ", tally.votes.Where(s => s.Choice == opt).Count()));
                }
                tallies.Add("The following users voted:");
                foreach (Vote vote in tally.votes)
                {
                    tallies.Add(vote.user.NickName);
                }
            }
            else
            {
                tallies.Add("There is no vote with that ID. Please provide a valid vote ID.");
            }

            return tallies;
        }

        public async Task<List<string>> AddQuestion(int votenum, string prefix)
        {
            List<string> rtn = new List<string>();
            string prepath = GetGuildDir(prefix);
            string path = string.Concat(prepath, votenum, ".json");
            if (!(System.IO.File.Exists(path)))
            {
                System.IO.File.Create(path);
                rtn.Add(string.Concat("Successfully created vote at ", path));
            }
            else
            {
                rtn.Add("There is already a vote with this ID. Please choose another.");
            }

            return rtn;
        }

        public async Task<List<string>> ListVotes(string prefix)
        {
            string path = GetGuildDir(prefix);

            return System.IO.Directory.GetFiles(path).Select(s => s.Split("/votes/")[1].Replace(".json", "")).ToList();
        }

        private string GetGuildDir(string prefix)
        {
            //make sure that the directory exists

            if(!(System.IO.Directory.Exists("votes\\" + prefix.Replace("!", ""))))
            {
                System.IO.Directory.CreateDirectory("votes\\" + prefix.Replace("!", ""));
            }

            prefix = "votes\\" + prefix.Replace("!", "") + "\\";


            return prefix;
        }
    }
}