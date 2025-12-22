using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedCountries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .Sql("""
                     INSERT INTO "countries" ("code", "name", "efficiency", "treasury", "stability", "manpower", "prestige")
                     VALUES 
                         ('CHU', 'Chu', 60, 200, 75, 1000, 75),
                         ('QI',	'Qi', 70, 250, 80, 600, 80),
                         ('ZHAO', 'Zhao', 70, 150, 65, 500,	65),
                         ('WEI', 'Wei', 65,	120, 60, 300, 60),
                         ('YAN', 'Yan', 50, 100, 55, 300, 55),
                         ('HAN', 'Han', 55, 80, 60, 300, 60),
                         ('QIN', 'Qin', 95, 300, 80, 1000,	80);
                     """);
            
            // CHU History
            migrationBuilder
                .Sql("""
                     UPDATE "countries"
                     SET "history" = 'Leadership: King Kaolie (Xiong Wan/Yuan, r. 262–238 BC) sits on the Chu throne . His chief
                 minister is Lord Chunshen (Huang Xie), an astute veteran who has been Chu’s Prime Minister and
                 one of the famed “Four Lords” of the period . Queen Dowager and court aristocrats also influence
                 policies.
                 Internal: Chu is the largest state by land and population. Its economy is based on rice farming in the
                 south and loess agriculture up north. Kaolie’s government is stable, absorbing the recent annexation
                 of Lu (249 BC) . Chunshen has invited many scholars and retainers to court, boosting Chu’s
                 culture. However, internal factionalism (nobles vs. central court) and a large bureaucracy mean
                 reforms are slow.
                 Military: Chu’s army is vast, drawing on loyal peasant conscripts and nobles’ cavalry. It boasts elite
                 troops (e.g. Lord Chunshen himself is a capable general). Recently Chu successfully absorbed Lu ,
                 extending its border east. Though once repeatedly checked by Qin, Chu still fields the largest
                 infantry. Generals like Zhuang Qiao and Li You stand ready at frontier garrisons.
                 Diplomacy: Chu often leads southern coalitions. It maintains ties with Wei and Zhao against Qin and
                 has friendly trade with Qi. However, it remains suspicious of northern neighbors. There are no formal
                 alliances now (some rivals fear Chu’s expansion) but Chu’s reputation causes others to court or avoid
                 it.
                 Territory: Chu rules all lands between the Huai and Yangtze rivers and south to the Han River basin.
                 Its effective capital is in Shouchun (moved from Yinglong/Shou in past decades to stay away from
                 Qin). Chu holds rich rice fields and strategic southern passes (e.g. at Shouxian). It also commands
                 frontier corridors into the Yangtze valleys.
                 Threats/Opportunities: Qin’s looming power in the west is Chu’s chief concern. Chu’s leaders must
                 guard against Qin’s next move. Internally, however, Chu’s size is its strength – it can field armies large
                 enough to strike at weaker neighbors (an opportunity Chu has used to expand). With careful
                 diplomacy, Chu could be pivot in any anti‑Qin alliance while retaining its dominion over southern
                 China.'
                     WHERE "code" = 'CHU';
                 """);
            
            // QI History
            migrationBuilder.Sql("""
                UPDATE "countries"
                SET "history" = 'Leadership: King Jian of Qi (Tian Jian, r. 264–221 BC) rules from Linzi. He succeeded his father King
            Xiang in 264 BC. For years Jian’s mother served as regent, and after her death the minister Hou
            Sheng became Premier . (Hou Sheng is suspected of corruption – some say he is secretly paid by
            Qin .)
            Internal: Qi is the richest state: irrigated farmlands and bustling coastwise trade fill its coffers. Its
            agriculture is efficient and its markets busy with fish, salt and crafts. Linzi is a scholarly center
            (former Lord Mengchang’s legacy, though he died years ago). The court is cultured but complacent;
            King Jian enjoys luxury and has largely left affairs to ministers. Hou Sheng’s intrigues, however, have
            sapped confidence – rumors persist he obstructed aid to Zhao after Changping.
            Military: Qi fields a powerful army (especially infantry) but has been cautious. After the Battle of
            Changping (260 BC), Qi did nothing to help Zhao, preferring to conserve forces . Today Qi’s
            troops remain on high alert along its western border, with great fortresses guarding the Shouzhang
            Pass. Its navy on the Yellow Sea is also strong, defending trade routes. Qi’s generals (like Tian Dan’s
            heirs) keep the army professionally drilled, but the state has no recent major battle record.
            Diplomacy: Qi has kept to itself. It maintains a non‑aggression understanding with Qin (though
            Qin’s bribery of Hou Sheng has poisoned trust ). Qi trades freely with Yan and Zhao, but others suspect its neutrality. The kingdom has avoided joining recent wars; it once allied with Chu and Zhao
            long ago, but now stays out of others’ conflicts.
            Territory: Qi encompasses the Shandong peninsula and adjacent plains up to the Yellow River
            estuary. Its commanderies (Jinan, Langya, etc.) produce surplus grain. Key cities are Linzi (capital),
            Linqu, and coastal ports like Qing and Guang. Qi controls the vital Wangwu Pass into central Henan
            and coast guard forts guarding the east.
            Threats/Opportunities: Qi’s main danger is complacency. Its greatest risk is Qin’s reach extending
            eastward (already Qin employs spies in Linzi). Internally, corruption at court could weaken Qi’s
            defenses (as when the Qin‑paid Hou Sheng was premier). On the bright side, Qi’s wealth and
            distance give it room to hire mercenaries or fortify great walls. If upheaval hits the other states, Qi’s
            isolation may let it survive longer. A savvy player might use Qi’s resources to outlast rivals or to
            sponsor allies against common foes.'
                WHERE "code" = 'QI';
            """);
            
            // ZHAO History
            migrationBuilder.Sql("""
                UPDATE "countries"
                SET "history" = 'Leadership: King Xiaocheng of Zhao (Zhao Dan, r. 265–245 BC) reigns from Handan . His
            ministers include the venerable statesman Lin Xiangru (recently retired) and the formidable general
            Li Mu (often in distant commands). Zhao’s leadership is anxious: they inherited a kingdom shattered
            by defeat.
            Internal: Zhao is still recovering from the bloodiest battle (Changping, 260 BC) which annihilated its
            main army . The economy (especially in southern Shanxi) has been devastated by war and Qin
            raiding. King Xiaocheng has encouraged resettlement of border farms and demobilization reforms,
            but morale is low. Nonetheless, Zhao’s nobility maintains the cavalry traditions of past reformers
            (Wuling’s legacy), and Handan’s artisans rebuild weapons and walls.
            Military: Once famous for horse-archer cavalry, Zhao’s forces are now lean. They still have great
            horse breeders in Shangdang and Xiongnu war captives supplying mounts. Zhao’s generals (like Lian
            Po, now retired, and Lin Xiangru) have taught the troops defensive tactics. However, Qin’s Qin’s
            earlier cavalry raids stole grain and livestock, leaving Zhao’s army under-strength. Their archers and
            crossbowmen are veteran, but there simply aren’t enough men to field the vast armies of decades
            past.
            Diplomacy: Zhao is nearly isolated. After Changping, ally Wei is weakened by its own wars; Yan and
            Han are considered “too weak to offer support,” and Qi won’t risk its riches for Zhao . Chu might
            help, but for now Wang between Qin and Wei feel immediate threat from Qin too. Zhao still leans on
            Wei for supplies and occasionally courts Chu as a distant ally. But its diplomats know that only
            self‑reliance (and Li Mu’s eventual recall) can save them.
            Territory: Zhao’s core is southern Shanxi and parts of Hebei (Handan, Changzi, Julu). Northern
            nomadic tribes border it but are held at bay by Great Walls. Zhao holds the vital Shangdang area
            (ceded by Han) which provides iron and cavalry, as well as the Hedong plains. Key fortresses like Fan
            in Hebei and Fen in Shanxi block western invasion routes.
            Threats/Opportunities: The primary threat is Qin. Zhao was its target last campaign, and Qin
            armies eye Handan. Internally, unrest may flare from conscripted poor men and displaced farmers.
            But a great opportunity exists in Zhao’s legendary defensive talent: under Li Mu Zhao can still resist
            invasions. If Zhao can restore an alliance (Chu or Wei) or even strike at Qin’s rear, it may slow Qin’s
            advance and buy time for resurgence. For now, Zhao holds its ground, banking on attrition and its
            famed generals to guard the Central Plains.'
                WHERE "code" = 'ZHAO';
            """);
            
            // WEI History
            migrationBuilder.Sql("""
                UPDATE "countries"
                SET "history" = 'Leadership: King Anxi of Wei (Wei Yu, r. 276–243 BC) rules from Daliang (near modern Kaifeng). He is
            advised and defended by his charismatic half-brother, Lord Xinling (Wei Wuji) , a famed general
            and patron of 3,000 retainers. Other ministers (e.g. the Lords of Pingyuan and Juke) also wield great
            influence.
            Internal: Wei’s land is rich loess plain, with busy markets in capitals (Daliang, Anyi). The court is
            cultured and open (notably, Wei maintains “house guests” as scholars and tacticians). Despite old
            wars, it remains moderately stable. King Anxi tolerates his powerful nobles in exchange for military
            support. Wei has recovered some prestige after earlier defeats by Qin; its people are industrious but
            cautious.
            Military: Wei’s army is large by manpower. It retains skilled cavalry and heavy chariots. Wei generals
            (like Wei Qingwu) have modernized tactics and erected fortifications along the Yellow River. Wei once
            halted Qin at the Taigang River (273 BC), showing it can stand firm. Its famed defenders (e.g. Lord
            Xinling) are veterans of wars. Wei’s strategic position means it often fights on Qin’s eastern frontiers.
            Diplomacy: Wei sits between Qin and the Central Plains. It has allied with Zhao and Chu (as at
            Guiling) to check Qin historically. Now, with Qin still powerful, Wei seeks partners: a loose coalition
            with Zhao, Han and Chu against Qin is in the air. Wei also trades with Qi, hoping to keep it neutral or
            friendly. However, Lord Xinling’s loyalty is primarily to Zhao (he leads rescue of Handan if needed)
            rather than to Qin.
            Territory: The heartland is in modern Henan: Daliang, Anyi, Dongjun commanderies. Wei also
            controls fringe garrisons north of the Yellow River (Qin territory has pushed them to Yunzhong). Key
            passes on the Taihang frontier (like Linjin) guard Shanxi approaches. Wei’s Yellow River keeps its
            flank protected, and it fields garrisons at Chenliu and Yingchuan to secure supplies.
            Threats/Opportunities: The immediate threat is Qin’s armies; Wei was sacked in 257 BC when Qin
            took Wei’s Changping spoils. King Anxi suspects betrayal, so Wei moves troops to guard passes. But
            Wei’s advantage is its strong coffers and strategic location: it can coordinate with Chu and Zhao to
            form a counter‑coalition. If Wei plays its cards well (for example, by sending reinforcements when
            Qin strikes elsewhere), it could preserve its lands and even strike at Qin’s rear. The famous Lord
            Xinling himself may soon lead Wei’s forces as commander‑in‑chief.'
                WHERE "code" = 'WEI';
            """);
            
            // YAN History
            migrationBuilder.Sql("""
                UPDATE "countries"
                SET "history" = 'Leadership: King Xi of Yan (Yān Xǐ, r. 255–222 BC) holds court at Ji (near modern Beijing) . His chief
            advisor is Crown Prince Dan, a bold and hot-headed planner. The king’s generals include Qin Kai, who
            has pushed Yan’s frontiers northward (see below). (King Xi inherited the throne from his father Xiao.)
            Internal: Yan is extensive but remote. Its economy benefits from trade with peoples of the north
            (Yan’s “knife” coins even mimic nomadic styles). The state has invested in Great Walls along the
            northern border for defense. Its court is energetic; King Xi is younger and more adventurous than
            some other monarchs. However, Yan remains uneasy on the inside – border fortresses vie for
            attention versus the central plain. Yan has improved frontier commanderies (Liaodong, Liaoxi) to
            settle migrants and soldiers.
            Military: Yan fields a hardy army experienced in frontier skirmishes. General Qin Kai has led
            campaigns far to the north and east – Yan’s forces have chased the Donghu and conquered areas of
            Manchuria and even parts of what would become Korea . These successes extended the Great
            Wall up to five new commanderies. Meanwhile, along the Yellow River, Zhao and Qi to Yan’s south
            have been perennial rivals – their mutual border wars (often stalemates) have kept Yan on alert . Yan’s generals (and Yue Yi, the former Qin defector, is a household name) are aggressive and often
            outnumber opponents on these frontiers.
            Diplomacy: Distant Yan is sometimes overlooked by southern kings, but it has been an active ally or
            enemy. In past decades Yan even joined Zhao, Han, Wei and Qin to seize Qi’s cities (under the hero
            Yue Yi). Now Yan trades (mostly grain and furs) with Qi, Zhao and Wei, and keeps careful watch on
            Qin. Crown Prince Dan openly courts Zhao and Wei as brothers-in-arms against Qin. Relations with
            the Central Plains are cordial but tense; Qi remains suspicious, and Han is too far to help.
            Territory: Yan’s land stretches from the Yi and Ying Rivers eastward to the Gulf of Bohai. Its main
            capitals are Ji and sometimes Xiadu (moved west in emergencies). The key commanderies are
            Liaodong, Liaoxi, Shanggu, Yuyang, Youbeiping – all created for defense after northward expansion
            . Yan’s control of these gives it a buffer of mountains and steppes. To the west, Yan holds
            Xuanquan and Changgu (along the Zhao border) – mountain passes that cut off hostile Zhao
            incursions.
            Threats/Opportunities: Yan’s greatest threats are its two frontiers. Steppe nomads (Donghu/
            Xiongnu) still raid its northern reaches, and Qi (to the south) looms as a strong rival. Importantly, the
            easternmost position buys Yan time: when Qin eventually turns east, Yan could muster its strength
            or retreat to peninsular strongholds. For now Yan’s opportunity is expansion – it has secured
            Liaodong and built walls to hold nomads . A clever Yan ruler can exploit distance: by trading fur
            and gold and hiring any spare forces, Yan might survive longer than most, emerging stronger if the
            central states exhaust themselves against Qin.'
                WHERE "code" = 'YAN';
            """);
            
            // HAN History
            migrationBuilder.Sql("""
                UPDATE "countries"
                SET "history" = 'Han stands as the smallest and most vulnerable of the seven Warring States. Sandwiched between the expansionist Qin to the west, powerful Wei to the north, and ambitious Chu to the south, Han’s survival depends on diplomacy, strategic positioning, and caution. Once a central Zhou territory, Han now acts more as a buffer state than a contender.
            
            Leadership: King Huanhui rules in name, but real power is diffused among ministers and regional elites. Han Fei, a brilliant Legalist thinker, influences policy circles but lacks formal authority. The royal court is cautious, with little appetite for reform that might anger nobles or Qin.
            
            Internal Affairs: The economy is modest. Han’s heartland in the Central Plains provides grain and access to trade, but manpower is low and recent territorial losses — such as Shangdang to Qin — have hurt morale. Central authority is weak, and local lords often act autonomously. Reforms are possible but risky.
            
            Military: Han maintains a small, largely defensive army. Fortified passes and river crossings offer natural advantages, but prolonged warfare would be ruinous. Without strong allies or hired troops, Han cannot survive direct assault by Qin.
            
            Diplomacy: Han’s best weapon is negotiation. It has ties to Wei and Zhao, and must avoid provoking Qin while quietly supporting any anti-Qin coalition. Bribery, flattery, and promises of access or supplies can buy Han time and security.
            
            Territory: Han controls a narrow corridor of the Central Plains, including Yingchuan and Sanchuan. Though small, this land is critical for movement between east and west — a fact that can be leveraged in diplomacy.
            
            Threats & Opportunities: Qin’s dominance threatens Han’s survival. However, by acting as a diplomatic broker and logistical keystone, Han can delay conquest and shape the battlefield indirectly. Survive the early game, and you may yet turn irrelevance into influence.'
                WHERE "code" = 'HAN';
            """);
            
            // QIN History
            migrationBuilder.Sql("""
                UPDATE "countries"
                SET "history" = 'Leadership: The new monarch is King Ying Zheng (age 13), under regent Lü Buwei (Prime Minister)
            . Lü dominates court and army; he even assembled scholars at Xianyang to compile the Lüshi
            Chunqiu , signaling Qin’s intellectual and political ambition. (Ying Zheng’s mother, Queen Dowager
            Zhao, also holds influence.)
            Internal: Qin’s Legalist system is mature. Heavy reforms (tax, conscription, rewards for merit) have
            created a robust economy and disciplined society. Lü Buwei’s regency has been stable but intrigues
            loom – powerful eunuchs and court factions may jockey for power in this regency.
            Military: Qin fields the largest, best-equipped army. Under Lü Buwei, Qin has been on the offensive
            – he “oversaw Qin’s military campaigns against neighboring states” . Veteran generals (e.g.
            successors to Bai Qi and Wang Jian) hold command, and Qin has recently conquered border lands
            (now organized into new commanderies). Its cavalry and chariot forces are unmatched in China.
            Diplomacy: Few states dare challenge Qin directly. Qin has made no formal allies; most others are
            wary. Perhaps Qin maintains uneasy truces (e.g. with Wei) to isolate opponents. Neighbors gossip
            about a coming Qin push, but for now none has united to confront it.
            Territory: Qin controls Guanzhong (heartland) and lands west of the Taihang. Its capital Xianyang
            (near modern Xi’an) anchors broad frontier commanderies. Key passes (Hangu, Tongguan) are in Qin
            hands. To the north Qin commands Loess Plateau; to the south it reaches the rich Cheng plains.
            Threats/Opportunities: Ying Zheng’s youth is a vulnerability – palace coups or the Lao Ai affair (in
            the future) could erupt. Externally, Qin’s main threat is a future multi‑state alliance (already forming
            in whispers) that could challenge its advances. But opportunity is immense: under deft leadership
            Qin could march east to reunify China under Legalist rule.'
                WHERE "code" = 'QIN';
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .Sql("""
                     DELETE FROM "countries"
                     WHERE "code" IN ('CHU', 'QI', 'ZHAO', 'WEI', 'YAN', 'HAN', 'QIN');
                     """);
        }
    }
}
