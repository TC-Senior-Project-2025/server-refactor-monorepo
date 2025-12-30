using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedCommanderies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "commanderies",
                columns: new[]
                {
                    "id",
                    "country_id",
                    "code",
                    "name",
                    "population",
                    "wealth",
                    "unrest",
                    "defense",
                    "history"
                },
                values: new object[,]
                {
                    { 2, 1, "SANCHUAN", "Sanchuan", 1218928L, 105, 2, 17000, "Rich agricultural heartland with strategic city Luoyang; economically strong but politically sensitive due to recent conquest." },
                    { 3, 1, "HEDONG", "Hedong", 1116505L, 100, 1, 22000, "Mature Qin agricultural and logistics region; critical staging ground for eastern expansion." },
                    { 4, 1, "LONGXI", "Longxi", 224177L, 50, 6, 15000, "Strategic western frontier focused on cavalry supply and tribal defense rather than economic output." },
                    { 5, 1, "BEIDI", "Beidi", 42988L, 40, 8, 11000, "Militarized frontier zone with minimal civilian economy; strategic depth against northern threats." },
                    { 6, 1, "SHANG", "Shang", 212330L, 40, 7, 15000, "Defensive hill frontier; limited agriculture, sustained mainly for military control." },
                    { 7, 1, "HANZHONG", "Hanzhong", 165338L, 70, 5, 19000, "Fertile Han River corridor; strategic basin controlling access between Guanzhong and Sichuan." },
                    { 8, 1, "BA", "Ba", 389481L, 70, 6, 16000, "Resource-rich frontier with persistent local identity; Recently integrated." },
                    { 9, 1, "SHU", "Shu", 1318715L, 100, 8, 23000, "One of China’s richest agrarian regions; economically decisive but still stabilizing after conquest." },

                    { 10, 4, "HANDAN", "Handan", 1554872L, 95, 2, 18000, "Symbolic and political capital of Zhao; weakened yet resistant under Qin pressure." },
                    { 11, 4, "YUNZHONG", "Yunzhong", 84104L, 35, 10, 12000, "Harsh northern frontier; functionally a military outpost against Xiongnu tribal frictions." },
                    { 12, 4, "YANMEN", "Yanmen", 102709L, 45, 8, 17000, "Strategic Yanmen mountain pass." },

                    { 13, 1, "TAIYUAN", "Taiyuan", 827594L, 90, 3, 17000, "Fresh Qin foothold in Zhao heartland; economically valuable but politically unstable." },
                    { 14, 4, "DAI", "Dai", 83626L, 45, 10, 17000, "A heavily fortified buffer region against Yan and northern nomads." },
                    { 15, 4, "JULU", "Julu", 2014847L, 85, 3, 7000, "Important agricultural area in southern Zhao. Under pressure from Qin's expansion in the region." },
                    { 16, 4, "SHANGDANG", "Shangdang", 168883L, 65, 5, 18000, "Several recent Qin invasion left it as severely contested strategic plateau; politically fragmented, militarily exhausted." },
                    { 17, 4, "JIUYUAN", "Jiuyuan", 57832L, 25, 10, 8000, "Remote northern frontier. Control was likely nominal and constantly disputed with the Xiongnu." },

                    { 18, 6, "SHANGGU", "Shanggu", 679829L, 55, 6, 19000, "Yan's northwestern frontier. A defensive line against the Xiongnu and a potential avenue for conflict with Zhao." },
                    { 19, 6, "YUYANG", "Yuyang", 198087L, 55, 5, 17000, "Defensive agrarian zone shielding Yan core territories." },
                    { 20, 6, "YOUBEIPING", "Youbeiping", 224546L, 45, 6, 11000, "Peripheral coastal frontier; weak control, exposed to Donghu tribal pressure." },
                    { 21, 6, "LIAOXI", "Liaoxi", 229011L, 45, 5, 10000, "Peninsula hinterland; contested between Yan and steppe, moderate farming." },
                    { 22, 6, "LIAODONG", "Liaodong", 204404L, 55, 5, 11000, "Eastern peninsula base; Yan’s eastern naval and fishing projection zone; Culturaly mixed of Yan colonists and other groups like Gojoseon." },

                    { 23, 5, "DONG", "Dong", 2768473L, 95, 5, 15000, "Remnant heartland of Wei in eastern Henan; still productive but squeezed by surrounding powers." },

                    { 24, 3, "QI", "Qi", 2645021L, 110, 2, 17000, "The wealthy, populous core of Qi. Had avoided major war for decades but was diplomatically isolated. The capital Linzi was a major economic center." },
                    { 25, 3, "LANGYE", "Langye", 1882792L, 75, 2, 7000, "Coastal region of Qi. Known for trade and salt production." },
                    { 26, 3, "XUE", "Xue", 789982L, 95, 3, 7000, "Southwestern Shandong plains; steady agrarian hinterland feeding Qi’s supply." },

                    { 27, 1, "NANYANG", "Nanyang", 1359436L, 100, 3, 19000, "Major agrarian heartland formerly of Han Qin conquered from Han in 263 BCE." },

                    { 28, 7, "DANG", "Dang", 1329399L, 85, 5, 12000, "Eastern Henan small but moderate wealthy farmlands; under extreme and constant military pressure." },
                    { 29, 7, "YINGCHUAN", "Yingchuan", 1547695L, 100, 3, 18000, "Core Han territory in central Henan; high population but surrounded." },

                    { 30, 2, "JIUJIANG", "Jiujiang", 3509495L, 75, 2, 9000, "Productive riverine region with partial cultural integration." },
                    { 31, 2, "ZHANG", "Zhang", 303878L, 60, 7, 9000, "A mountainous area carved from former Yue territory, with mixed Chu/Yue populace." },
                    { 32, 2, "SISHUI", "Sishui", 1516970L, 65, 5, 9000, "Huai River plain; tributary agrarian zone, contested by Chu, Zhao, and Qi influence; medium productivity." },
                    { 33, 2, "NAN", "Nan", 609543L, 80, 2, 14000, "Chu's heartland in the middle Yangtze. Good rice lands, substantial peasant base." },
                    { 34, 2, "KUAIJI", "Kuaiji", 774453L, 55, 4, 13000, "Former heartland of Yue, annexed by Chu. Productive rice and waterways under Chu influence but still dominated by Yue culture and prone to unrest." },
                    { 35, 2, "DONGHAI", "Donghai", 1987022L, 60, 5, 16000, "Chu's coastal frontier.  economy based on agriculture and fishing. Incorporated former Yue territories with local autonomy;" },
                    { 36, 2, "CHANGSHA", "Changsha", 398768L, 65, 2, 8000, "Hunan plain; rice and subtropical crops; Southern frontier of Chu. Sparse population with significant indigenous \"Hundred Yue\" peoples; loose Chu control." },

                    { 1, 1, "NEISHI", "Neishi", 1747452L, 120, 0, 33000, "Capital Qin core centered on Xianyang; most secure, populous, and administratively mature region; supreme logistics and command hub." },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 1);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 2);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 3);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 4);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 5);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 6);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 7);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 8);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 9);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 10);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 11);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 12);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 13);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 14);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 15);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 16);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 17);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 18);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 19);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 20);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 21);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 22);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 23);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 24);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 25);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 26);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 27);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 28);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 29);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 30);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 31);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 32);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 33);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 34);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 35);
            migrationBuilder.DeleteData(table: "commandery", keyColumn: "id", keyValue: 36);
        }
    }
}
