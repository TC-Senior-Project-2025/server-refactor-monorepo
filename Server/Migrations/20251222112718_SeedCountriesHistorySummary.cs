using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedCountriesHistorySummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string chuSummary = 
                "Chu under King Kaolie is stable but slow-moving, guided by the influential Lord Chunshen amid court factionalism. Vast territory and population support a huge army with strong infantry and capable generals, recently proven by annexing Lu. Diplomatically, Chu leads southern coalitions, trades with Qi, and balances wary ties with Wei and Zhao, lacking firm alliances. Qin remains the primary threat, while Chu’s scale offers opportunities for regional dominance and alliance leadership.";
            const string qiSummary =
                "Qi under King Jian is wealthy and cultured but politically complacent, with real power resting in the suspect premier Hou Sheng and lingering court corruption. Its economy is the richest in the east, supporting a large, well‑trained infantry army and strong coastal navy, though unused in major wars since Changping. Diplomatically, Qi remains neutral, trading widely and maintaining a wary non‑aggression stance with Qin. The chief danger is Qin’s infiltration and Qi’s own inertia, while its wealth, fortifications, and isolation offer long‑term survival or coalition leverage.";
            const string zhaoSummary =
                "Under King Xiaocheng, Zhao rules from Handan but remains shaken after Changping, with cautious leadership and low morale amid slow economic recovery. Its once-great cavalry army is reduced yet disciplined, relying on veteran archers, rebuilt fortifications, and the defensive skill of generals like Li Mu. Diplomatically isolated, Zhao leans weakly on Wei and courts distant Chu. Qin is the overriding threat, but Zhao’s fortresses, cavalry resources, and defensive talent offer a chance to endure and delay conquest.";
            const string weiSummary =
                "Under King Anxi, Wei is ruled from Daliang through a cultured but noble‑heavy court, dominated by Lord Xinling and other powerful ministers, trading royal tolerance for military loyalty. The state is economically sound and internally stable. Wei fields a large, modernized army with strong cavalry, chariots, and Yellow River fortifications, proven capable of checking Qin. Diplomatically, Wei seeks coalitions with Zhao, Han, and Chu while courting Qi. Qin remains the chief threat, but Wei’s central position and resources offer chances for coordinated counterattacks.";
            const string yanSummary =
                "Under King Xi, Yan is an energetic but remote frontier kingdom ruled from Ji, guided by the aggressive Crown Prince Dan. Its economy relies on northern trade and migrant-settled commanderies, protected by expanded Great Walls. Yan’s army is hardened by frontier warfare, with recent northern conquests under Qin Kai. Diplomatically, Yan courts Zhao and Wei against Qin while trading warily with Qi. Distance offers time, but nomads and southern rivals remain constant threats.";
            const string hanSummary =
                "Han is the weakest Warring State, ruled by King Huanhui but constrained by cautious ministers and weak central authority. Its modest Central Plains economy and low manpower limit reform and ambition. Militarily defensive, Han relies on fortifications and cannot withstand sustained war, especially against Qin. Diplomacy is its primary weapon, balancing Qin while courting Wei and Zhao. Though threatened by encirclement, Han’s control of key transit corridors offers leverage as a broker and logistical linchpin.";
            const string qinSummary =
                "Qin is ruled by the boy king Ying Zheng under the dominant regency of Lü Buwei, with a stable but intrigue‑prone court and a fully developed Legalist state. Its economy and conscription system support the largest, best equipped army, led by veteran generals and pressing offensively from newly formed commanderies. Diplomatically isolated yet feared, Qin relies on deterrence and temporary truces. Youthful succession invites palace plots, but unrivaled strength offers the chance to unify China.";

            migrationBuilder.Sql($"""UPDATE "countries" SET "history_summary" = '{chuSummary}' WHERE "code" = 'CHU';""");
            migrationBuilder.Sql($"""UPDATE "countries" SET "history_summary" = '{qiSummary}' WHERE "code" = 'QI';""");
            migrationBuilder.Sql($"""UPDATE "countries" SET "history_summary" = '{zhaoSummary}' WHERE "code" = 'ZHAO';""");
            migrationBuilder.Sql($"""UPDATE "countries" SET "history_summary" = '{weiSummary}' WHERE "code" = 'WEI';""");
            migrationBuilder.Sql($"""UPDATE "countries" SET "history_summary" = '{yanSummary}' WHERE "code" = 'YAN';""");
            migrationBuilder.Sql($"""UPDATE "countries" SET "history_summary" = '{hanSummary}' WHERE "code" = 'HAN';""");
            migrationBuilder.Sql($"""UPDATE "countries" SET "history_summary" = '{qinSummary}' WHERE "code" = 'QIN';""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""UPDATE "countries" SET "history_summary" = '' WHERE "code" = 'CHU';""");
            migrationBuilder.Sql("""UPDATE "countries" SET "history_summary" = '' WHERE "code" = 'QI';""");
            migrationBuilder.Sql("""UPDATE "countries" SET "history_summary" = '' WHERE "code" = 'ZHAO';""");
            migrationBuilder.Sql("""UPDATE "countries" SET "history_summary" = '' WHERE "code" = 'WEI';""");
            migrationBuilder.Sql("""UPDATE "countries" SET "history_summary" = '' WHERE "code" = 'YAN';""");
            migrationBuilder.Sql("""UPDATE "countries" SET "history_summary" = '' WHERE "code" = 'HAN';""");
            migrationBuilder.Sql("""UPDATE "countries" SET "history_summary" = '' WHERE "code" = 'QIN';""");
        }
    }
}
