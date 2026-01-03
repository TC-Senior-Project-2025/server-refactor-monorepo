using System.Text.Json.Serialization;

namespace Server.Modules.Assets.Entities;

public class Army
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("game_id")]
    public int GameId { get; set; }

    [JsonPropertyName("country_id")]
    public int CountryId { get; set; }

    [JsonPropertyName("commander_id")]
    public int? CommanderId { get; set; }

    [JsonPropertyName("location_id")]
    public int LocationId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("morale")]
    public int Morale { get; set; }

    [JsonPropertyName("supply")]
    public int Supply { get; set; }

    [JsonPropertyName("action_left")]
    public int ActionLeft { get; set; }

    [JsonPropertyName("history")]
    public string History { get; set; }
}

public class Commandery
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("game_id")]
    public int GameId { get; set; }

    [JsonPropertyName("country_id")]
    public int CountryId { get; set; }

    [JsonPropertyName("commander_id")]
    public int CommanderId { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("population")]
    public int Population { get; set; }

    [JsonPropertyName("wealth")]
    public int Wealth { get; set; }

    [JsonPropertyName("unrest")]
    public int Unrest { get; set; }

    [JsonPropertyName("defensiveness")]
    public int Defensiveness { get; set; }

    [JsonPropertyName("garrisons")]
    public int Garrisons { get; set; }

    [JsonPropertyName("neighbors")]
    public List<int> Neighbors { get; set; }

    [JsonPropertyName("history")]
    public string History { get; set; }
}

public class Country
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("game_id")]
    public int GameId { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("is_player")]
    public bool IsPlayer { get; set; }

    [JsonPropertyName("efficiency")]
    public int Efficiency { get; set; }

    [JsonPropertyName("treasury")]
    public int Treasury { get; set; }

    [JsonPropertyName("stability")]
    public int Stability { get; set; }

    [JsonPropertyName("manpower")]
    public int Manpower { get; set; }

    [JsonPropertyName("prestige")]
    public int Prestige { get; set; }

    [JsonPropertyName("mission_point")]
    public int MissionPoint { get; set; }

    [JsonPropertyName("last_turn_gold_income")]
    public int LastTurnGoldIncome { get; set; }

    [JsonPropertyName("last_turn_manpower_income")]
    public int LastTurnManpowerIncome { get; set; }

    [JsonPropertyName("history")]
    public string History { get; set; }
}

public class Game
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("turn")]
    public int Turn { get; set; }

    [JsonPropertyName("current_year")]
    public int CurrentYear { get; set; }

    [JsonPropertyName("current_month")]
    public int CurrentMonth { get; set; }

    [JsonPropertyName("months_per_turn")]
    public int MonthsPerTurn { get; set; }

    [JsonPropertyName("history")]
    public string History { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; }
}

public class Person
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("country_id")]
    public int CountryId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("is_alive")]
    public bool IsAlive { get; set; }

    [JsonPropertyName("loyalty")]
    public int Loyalty { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("stats")]
    public Stats Stats { get; set; }

    [JsonPropertyName("history")]
    public string History { get; set; }

    [JsonPropertyName("game_id")]
    public int GameId { get; set; }
}

public class Relation
{
    [JsonPropertyName("src_country")]
    public int SrcCountry { get; set; }

    [JsonPropertyName("dst_country")]
    public int DstCountry { get; set; }

    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("is_allied")]
    public bool IsAllied { get; set; }

    [JsonPropertyName("is_at_war")]
    public bool IsAtWar { get; set; }

    [JsonPropertyName("history")]
    public string History { get; set; }

    [JsonPropertyName("game_id")]
    public int GameId { get; set; }
}

public class Scenario
{
    [JsonPropertyName("game")]
    public List<Game> Game { get; set; }

    [JsonPropertyName("country")]
    public List<Country> Country { get; set; }

    [JsonPropertyName("relation")]
    public List<Relation> Relation { get; set; }

    [JsonPropertyName("person")]
    public List<Person> Person { get; set; }

    [JsonPropertyName("commandery")]
    public List<Commandery> Commandery { get; set; }

    [JsonPropertyName("army")]
    public List<Army> Army { get; set; }

    [JsonPropertyName("battle")]
    public List<object> Battle { get; set; }
}

public class Stats
{
    [JsonPropertyName("leadership")]
    public int? Leadership { get; set; }

    [JsonPropertyName("politics")]
    public int? Politics { get; set; }

    [JsonPropertyName("morale")]
    public int Morale { get; set; }

    [JsonPropertyName("field_offense")]
    public int FieldOffense { get; set; }

    [JsonPropertyName("field_defense")]
    public int FieldDefense { get; set; }

    [JsonPropertyName("siege_offense")]
    public int SiegeOffense { get; set; }

    [JsonPropertyName("siege_defense")]
    public int SiegeDefense { get; set; }

    [JsonPropertyName("strategy")]
    public int Strategy { get; set; }

    [JsonPropertyName("ruthlessness")]
    public int Ruthlessness { get; set; }

    [JsonPropertyName("influence")]
    public int? Influence { get; set; }

    [JsonPropertyName("wealth")]
    public int? Wealth { get; set; }

    [JsonPropertyName("networking")]
    public int? Networking { get; set; }

    [JsonPropertyName("cunning")]
    public int? Cunning { get; set; }

    [JsonPropertyName("pressured")]
    public int? Pressured { get; set; }

    [JsonPropertyName("writing")]
    public int? Writing { get; set; }

    [JsonPropertyName("legalism")]
    public int? Legalism { get; set; }

    [JsonPropertyName("administration")]
    public int? Administration { get; set; }

    [JsonPropertyName("patience")]
    public int? Patience { get; set; }

    [JsonPropertyName("boldness")]
    public int? Boldness { get; set; }

    [JsonPropertyName("initiative")]
    public int? Initiative { get; set; }

    [JsonPropertyName("discipline")]
    public int? Discipline { get; set; }

    [JsonPropertyName("engineering")]
    public int? Engineering { get; set; }

    [JsonPropertyName("diplomacy")]
    public int? Diplomacy { get; set; }

    [JsonPropertyName("resilience")]
    public int? Resilience { get; set; }

    [JsonPropertyName("theory")]
    public int? Theory { get; set; }

    [JsonPropertyName("stress")]
    public int? Stress { get; set; }

    [JsonPropertyName("authority")]
    public int? Authority { get; set; }

    [JsonPropertyName("persistence")]
    public int? Persistence { get; set; }

    [JsonPropertyName("charisma")]
    public int? Charisma { get; set; }

    [JsonPropertyName("campaigning")]
    public int? Campaigning { get; set; }

    [JsonPropertyName("resolve")]
    public int? Resolve { get; set; }

    [JsonPropertyName("desperation")]
    public int? Desperation { get; set; }

    [JsonPropertyName("bravery")]
    public int? Bravery { get; set; }

    [JsonPropertyName("stealth")]
    public int? Stealth { get; set; }

    [JsonPropertyName("stability")]
    public int? Stability { get; set; }

    [JsonPropertyName("planning")]
    public int? Planning { get; set; }

    [JsonPropertyName("mobility")]
    public int? Mobility { get; set; }

    [JsonPropertyName("experience")]
    public int? Experience { get; set; }

    [JsonPropertyName("intrigue")]
    public int? Intrigue { get; set; }

    [JsonPropertyName("corruption")]
    public int? Corruption { get; set; }

    [JsonPropertyName("courage")]
    public int? Courage { get; set; }

    [JsonPropertyName("ethics")]
    public int? Ethics { get; set; }

    [JsonPropertyName("persuasion")]
    public int? Persuasion { get; set; }

    [JsonPropertyName("greed")]
    public int? Greed { get; set; }

    [JsonPropertyName("short_sighted")]
    public int? ShortSighted { get; set; }

    [JsonPropertyName("prestige")]
    public int? Prestige { get; set; }

    [JsonPropertyName("ingenuity")]
    public int? Ingenuity { get; set; }

    [JsonPropertyName("caution")]
    public int? Caution { get; set; }

    [JsonPropertyName("gluttony")]
    public int? Gluttony { get; set; }

    [JsonPropertyName("culture")]
    public int? Culture { get; set; }

    [JsonPropertyName("fortune")]
    public int? Fortune { get; set; }

    [JsonPropertyName("ambition")]
    public int? Ambition { get; set; }
}

