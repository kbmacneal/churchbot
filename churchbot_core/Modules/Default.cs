using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace churchbot.Modules.Default {

    public class commands : ModuleBase<SocketCommandContext> {

        public async Task SendPMAsync (string message, SocketUser user) {
            await user.SendMessageAsync (message);
        }

        public async Task PingAsync (SocketUser user) {
            await user.SendMessageAsync ("Pong!");
        }

        public async Task PrayAsync (SocketUser user) {
            await user.SendMessageAsync (":keycap_ten: :pray:");
        }

        public async Task VirtuesAsync (SocketUser user) {

            await Virtue1Async (user);
            await Virtue2Async (user);
            await Virtue3Async (user);
            await Virtue4Async (user);
            await Virtue5Async (user);
            await Virtue6Async (user);
            await Virtue7Async (user);
            await Virtue8Async (user);
            await Virtue9Async (user);
            await Virtue10Async (user);
        }

        public async Task Virtue1Async (SocketUser user) {

            await user.SendMessageAsync ("The First Virtue is Faith. Recitation: “Faith above all. We must trust God and their chosen Emperor to guide us.” Faith is exemplified by daily prayer, regular attendance of Church ceremonies, dutiful tithing, and pilgrimage to holy sites.");
        }

        public async Task Virtue2Async (SocketUser user) {

            await user.SendMessageAsync ("The Second Virtue is Propriety, which flows from Faith. Recitation: “We must be obedient to tradition, ceremony, courtesy and station.” Propriety is exemplified by respectful loyalty to righteous authority and cultural norms, along with unfailing intolerance for all heretics and heathens. Custom, dress, and technology all must adhere to Propriety.");
        }

        public async Task Virtue3Async (SocketUser user) {

            await user.SendMessageAsync ("The Third Virtue is Justice, which flows from Propriety. Recitation: “We must reward those who behave rightly and punish those who do not.” Justice is exemplified by its unflinching enforcement, and we must correct our own failings before looking to those of others.");
        }

        public async Task Virtue4Async (SocketUser user) {

            await user.SendMessageAsync ("The Fourth Virtue is Fortitude, which reinforces Justice. Recitation: “We must patiently endure the challenges laid upon us and follow the rightful path despite them.” Fortitude is exemplified by steadfast courage and endurance in the face of adversity, particularly in avoiding all cringing or complaint when upon holy ground or fulfilling holy duties.");
        }

        public async Task Virtue5Async (SocketUser user) {

            await user.SendMessageAsync ("The Fifth Virtue is Wisdom, which accompanies Fortitude. Recitation: “We must strive to see the world in its truth and shape it according to God’s will.” Wisdom is exemplified by perceived the flawed world as it is, but never losing sight of what it should be.  Daily reflection upon the sacred texts and their application to our lives is essential to Wisdom.");
        }

        public async Task Virtue6Async (SocketUser user) {

            await user.SendMessageAsync ("The Sixth Virtue is Temperance, which flows from Wisdom. Recitation: “We must show prudent moderation and diligent control over our desires.” Temperance is exemplified by self-restraint in all facets, particularly regular fasting and avoidance of intoxicants.");
        }

        public async Task Virtue7Async (SocketUser user) {

            await user.SendMessageAsync ("The Seventh Virtue is Diligence, which reinforces Temperance.  Recitation: “We must be ever persistent and expend all effort and attention in keeping ourselves and others to the true path.” Diligence is exemplified by constant, tireless vigilance against temptation and treachery in all aspects of life.");
        }

        public async Task Virtue8Async (SocketUser user) {

            await user.SendMessageAsync ("The Eighth Virtue is Charity, which echoes Justice. Recitation: “We must show compassion to those worthy of God’s mercy.” Charity is exemplified by philanthropic acts and outreach to faithful sufferers.");
        }

        public async Task Virtue9Async (SocketUser user) {

            await user.SendMessageAsync ("The Ninth Virtue is Integrity, which echoes Propriety. Recitation: “We must honor our oaths and uphold the truth.” Integrity is exemplified by unfailing honesty and the exposure of deviants, as well as regular confession of our failings.");
        }

        public async Task Virtue10Async (SocketUser user) {

            await user.SendMessageAsync ("The Tenth Virtue is Hope, which echoes Faith. Recitation: “We must never despair, no matter how dark the hour, as God shines their light upon us.”  Hope is exemplified by the conquest of despair, symbolized most prominently by the restriction of mourning to a designated period, as well as the teaching of the Virtues to the ignorant.");
        }

        public async Task TwitterAsync (SocketUser user) {

            await user.SendMessageAsync ("Follow the church on twitter at https://twitter.com/ExarchTatiana");
        }

        public async Task WebsiteAsync (SocketUser user) {

            await user.SendMessageAsync ("The latest news and information on Acheron Rho can be found on the Church's official website at http://highchurch.space");
        }

        public async Task CommandsAsync (SocketUser user) {

            await user.SendMessageAsync (String.Concat ("```Here are the commands available to everyone" + System.Environment.NewLine +
                "!ping : Make sure the bot is alive" + System.Environment.NewLine +
                "!virtues : List all 10 of the Virtues" + System.Environment.NewLine +
                "!virtue[1..10] : List a particular virtue" + System.Environment.NewLine +
                "!commands : You're using it right now" + System.Environment.NewLine +
                "!website : Link to the website" + System.Environment.NewLine +
                "!twitter : Link to the official twitter" + System.Environment.NewLine +
                "!pray : :keycap_ten: :pray:" + System.Environment.NewLine +
                "!donate: pass the donation plate around" + System.Environment.NewLine +
                "!listquestions lists the available votes" + System.Environment.NewLine +
                "!contribute: if you want to contribute to the churchbot's development, a link to the git repo" + System.Environment.NewLine +
                "!factioncommands: instructions on how to add your own faction commands to the bot.```"));
        }

        public async Task DonateAsync (SocketUser user) {

            await user.SendMessageAsync ("Passes the donation plate around_ http://highchurch.space/support");
        }

        public async Task FactioncommandsAsync (SocketUser user) {
            await user.SendMessageAsync ("If your faction wishes to extend churchbot to have commands on this and other Church servers, contact a server admin and we can discuss.");
        }

        public async Task ContributeAsync (SocketUser user) {
            await user.SendMessageAsync ("https://github.com/kbmacneal/churchbot");
        }

        //template
        // [Command ("")]
        // public async Task NameAsync (SocketUser user) {
        //     await user.SendMessageAsync ("Text");
        // }

    }

}