using System.Threading.Tasks;
using Discord.Commands;

namespace churchbot.Modules {
    public class ping : ModuleBase<SocketCommandContext> {
        [Command ("ping")]
        public async Task PingAsync () {
            await ReplyAsync ("Pong!");
        }

        [Command ("start")]
        public async Task StartAsync () {
            await ReplyAsync ("Churchbot has started!");
        }
    }

    public class virtues : ModuleBase<SocketCommandContext> {
        [Command ("virtues")]
        public async Task VirtuesAsync () {
            await Virtue1Async ();
            await Virtue2Async ();
            await Virtue3Async ();
            await Virtue4Async ();
            await Virtue5Async ();
            await Virtue6Async ();
            await Virtue7Async ();
            await Virtue8Async ();
            await Virtue9Async ();
            await Virtue10Async ();
        }

        [Command ("virtue1")]
        public async Task Virtue1Async () {
            await ReplyAsync ("The First Virtue is Faith. Recitation: “Faith above all. We must trust God and their chosen Emperor to guide us.” Faith is exemplified by daily prayer, regular attendance of Church ceremonies, dutiful tithing, and pilgrimage to holy sites.");
        }

        [Command ("virtue2")]
        public async Task Virtue2Async () {
            await ReplyAsync ("The Second Virtue is Propriety, which flows from Faith. Recitation: “We must be obedient to tradition, ceremony, courtesy and station.” Propriety is exemplified by respectful loyalty to righteous authority and cultural norms, along with unfailing intolerance for all heretics and heathens. Custom, dress, and technology all must adhere to Propriety.");
        }

        [Command ("virtue3")]
        public async Task Virtue3Async () {
            await ReplyAsync ("The Third Virtue is Justice, which flows from Propriety. Recitation: “We must reward those who behave rightly and punish those who do not.” Justice is exemplified by its unflinching enforcement, and we must correct our own failings before looking to those of others.");
        }

        [Command ("virtue4")]
        public async Task Virtue4Async () {
            await ReplyAsync ("The Fourth Virtue is Fortitude, which reinforces Justice. Recitation: “We must patiently endure the challenges laid upon us and follow the rightful path despite them.” Fortitude is exemplified by steadfast courage and endurance in the face of adversity, particularly in avoiding all cringing or complaint when upon holy ground or fulfilling holy duties.");
        }

        [Command ("virtue5")]
        public async Task Virtue5Async () {
            await ReplyAsync ("The Fifth Virtue is Wisdom, which accompanies Fortitude. Recitation: “We must strive to see the world in its truth and shape it according to God’s will.” Wisdom is exemplified by perceived the flawed world as it is, but never losing sight of what it should be.  Daily reflection upon the sacred texts and their application to our lives is essential to Wisdom.");
        }

        [Command ("virtue6")]
        public async Task Virtue6Async () {
            await ReplyAsync ("The Sixth Virtue is Temperance, which flows from Wisdom. Recitation: “We must show prudent moderation and diligent control over our desires.” Temperance is exemplified by self-restraint in all facets, particularly regular fasting and avoidance of intoxicants.");
        }

        [Command ("virtue7")]
        public async Task Virtue7Async () {
            await ReplyAsync ("The Seventh Virtue is Diligence, which reinforces Temperance.  Recitation: “We must be ever persistent and expend all effort and attention in keeping ourselves and others to the true path.” Diligence is exemplified by constant, tireless vigilance against temptation and treachery in all aspects of life.");
        }

        [Command ("virtue8")]
        public async Task Virtue8Async () {
            await ReplyAsync ("The Eighth Virtue is Charity, which echoes Justice. Recitation: “We must show compassion to those worthy of God’s mercy.” Charity is exemplified by philanthropic acts and outreach to faithful sufferers.");
        }

        [Command ("virtue9")]
        public async Task Virtue9Async () {
            await ReplyAsync ("The Ninth Virtue is Integrity, which echoes Propriety. Recitation: “We must honor our oaths and uphold the truth.” Integrity is exemplified by unfailing honesty and the exposure of deviants, as well as regular confession of our failings.");
        }

        [Command ("virtue10")]
        public async Task Virtue10Async () {
            await ReplyAsync ("The Tenth Virtue is Hope, which echoes Faith. Recitation: “We must never despair, no matter how dark the hour, as God shines their light upon us.”  Hope is exemplified by the conquest of despair, symbolized most prominently by the restriction of mourning to a designated period, as well as the teaching of the Virtues to the ignorant.");
        }
    }
}