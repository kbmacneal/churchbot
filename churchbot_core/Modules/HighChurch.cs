using System;
using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;

namespace churchbot.Modules.HighChurch
{

    public class commands : ModuleBase<SocketCommandContext>
    {

        public async Task SendPMAsync(string message, SocketUser user)
        {
            await user.SendMessageAsync(message);
        }

        public async Task PingAsync(SocketUser user)
        {
            await user.SendMessageAsync("Pong!");
        }


        public async Task PrayAsync(SocketUser user)
        {
            await user.SendMessageAsync(":keycap_ten: :pray:");
        }


        public async Task VirtuesAsync(SocketUser user)
        {

            await Virtue1Async(user);
            await Virtue2Async(user);
            await Virtue3Async(user);
            await Virtue4Async(user);
            await Virtue5Async(user);
            await Virtue6Async(user);
            await Virtue7Async(user);
            await Virtue8Async(user);
            await Virtue9Async(user);
            await Virtue10Async(user);
        }


        public async Task Virtue1Async(SocketUser user)
        {

            await user.SendMessageAsync("The First Virtue is Faith. Recitation: “Faith above all. We must trust God and their chosen Emperor to guide us.” Faith is exemplified by daily prayer, regular attendance of Church ceremonies, dutiful tithing, and pilgrimage to holy sites.");
        }


        public async Task Virtue2Async(SocketUser user)
        {

            await user.SendMessageAsync("The Second Virtue is Propriety, which flows from Faith. Recitation: “We must be obedient to tradition, ceremony, courtesy and station.” Propriety is exemplified by respectful loyalty to righteous authority and cultural norms, along with unfailing intolerance for all heretics and heathens. Custom, dress, and technology all must adhere to Propriety.");
        }


        public async Task Virtue3Async(SocketUser user)
        {

            await user.SendMessageAsync("The Third Virtue is Justice, which flows from Propriety. Recitation: “We must reward those who behave rightly and punish those who do not.” Justice is exemplified by its unflinching enforcement, and we must correct our own failings before looking to those of others.");
        }


        public async Task Virtue4Async(SocketUser user)
        {

            await user.SendMessageAsync("The Fourth Virtue is Fortitude, which reinforces Justice. Recitation: “We must patiently endure the challenges laid upon us and follow the rightful path despite them.” Fortitude is exemplified by steadfast courage and endurance in the face of adversity, particularly in avoiding all cringing or complaint when upon holy ground or fulfilling holy duties.");
        }


        public async Task Virtue5Async(SocketUser user)
        {

            await user.SendMessageAsync("The Fifth Virtue is Wisdom, which accompanies Fortitude. Recitation: “We must strive to see the world in its truth and shape it according to God’s will.” Wisdom is exemplified by perceived the flawed world as it is, but never losing sight of what it should be.  Daily reflection upon the sacred texts and their application to our lives is essential to Wisdom.");
        }


        public async Task Virtue6Async(SocketUser user)
        {

            await user.SendMessageAsync("The Sixth Virtue is Temperance, which flows from Wisdom. Recitation: “We must show prudent moderation and diligent control over our desires.” Temperance is exemplified by self-restraint in all facets, particularly regular fasting and avoidance of intoxicants.");
        }


        public async Task Virtue7Async(SocketUser user)
        {

            await user.SendMessageAsync("The Seventh Virtue is Diligence, which reinforces Temperance.  Recitation: “We must be ever persistent and expend all effort and attention in keeping ourselves and others to the true path.” Diligence is exemplified by constant, tireless vigilance against temptation and treachery in all aspects of life.");
        }


        public async Task Virtue8Async(SocketUser user)
        {

            await user.SendMessageAsync("The Eighth Virtue is Charity, which echoes Justice. Recitation: “We must show compassion to those worthy of God’s mercy.” Charity is exemplified by philanthropic acts and outreach to faithful sufferers.");
        }


        public async Task Virtue9Async(SocketUser user)
        {

            await user.SendMessageAsync("The Ninth Virtue is Integrity, which echoes Propriety. Recitation: “We must honor our oaths and uphold the truth.” Integrity is exemplified by unfailing honesty and the exposure of deviants, as well as regular confession of our failings.");
        }


        public async Task Virtue10Async(SocketUser user)
        {

            await user.SendMessageAsync("The Tenth Virtue is Hope, which echoes Faith. Recitation: “We must never despair, no matter how dark the hour, as God shines their light upon us.”  Hope is exemplified by the conquest of despair, symbolized most prominently by the restriction of mourning to a designated period, as well as the teaching of the Virtues to the ignorant.");
        }


        public async Task TwitterAsync(SocketUser user)
        {

            await user.SendMessageAsync("Follow the church on twitter at https://twitter.com/ExarchTatiana");
        }


        public async Task WebsiteAsync(SocketUser user)
        {

            await user.SendMessageAsync("The latest news and information on Acheron Rho can be found on the Church's official website at http://highchurch.space");
        }


        public async Task WelcomeAsync(SocketUser user)
        {

            await user.SendMessageAsync("Welcome to the Church! Please review the pinned messages on the RP:FV discord to get caught up on the work that's been done so far. The link for the startup guide is https://docs.google.com/document/d/1exepWYJTPdWKNuYKTUOesQbZ-ByRX5vf8RxjCGNEkgo/edit?usp=sharing . You can use that as a one-stop-shop for all the other content generated so far. Another good resource is the website, http://highchurch.space. Use cb!commands to find out what else I can do!");
        }


        public async Task CommandsAsync(SocketUser user)
        {

            await user.SendMessageAsync(String.Concat("```Here are the commands available to everyone" + System.Environment.NewLine +
                "cb!ping : Make sure the bot is alive" + System.Environment.NewLine +
                "cb!virtues : List all 10 of the Virtues" + System.Environment.NewLine +
                "cb!virtue[1..10] : List a particular virtue" + System.Environment.NewLine +
                "cb!welcome : Welcome a newbie" + System.Environment.NewLine +
                "cb!commands : You're using it right now" + System.Environment.NewLine +
                "cb!website : Link to the website" + System.Environment.NewLine +
                "cb!twitter : Link to the official twitter" + System.Environment.NewLine +
                "cb!pray : :keycap_ten: :pray:" + System.Environment.NewLine +
                "cb!donate: pass the donation plate around" + System.Environment.NewLine +
                "cb!votefor#:# vote for an issue. The first number is the vote id, the second is your choice" + System.Environment.NewLine +
                "cb!votetally# tally the votes for an added issue, where # is the vote you want to tally" + System.Environment.NewLine +
                "cb!addquestion adds a question for voting, where # is the new id." + System.Environment.NewLine +
                "cb!listquestions lists the available votes```"));
        }


        public async Task DonateAsync(SocketUser user)
        {

            await user.SendMessageAsync("_Passes the donation plate around_ http://highchurch.space/support");
        }

        //template
        // [Command ("")]
        // public async Task NameAsync () {
        //     await user.SendMessageAsync ("Text");
        // }

    }

}