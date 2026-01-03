using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server.Modules.Assets.Entities;

public class Scenario
{
    [JsonPropertyName("country")] public List<CountryData> Country { get; set; } = [];
    [JsonPropertyName("relation")] public List<RelationData> Relation { get; set; } = [];
    [JsonPropertyName("person")] public List<PersonData> Person { get; set; } = [];
    [JsonPropertyName("commandery")] public List<CommanderyData> Commandery { get; set; } = [];
    [JsonPropertyName("army")] public List<ArmyData> Army { get; set; } = [];
    [JsonPropertyName("battle")] public List<BattleData> Battle { get; set; } = [];
    [JsonExtensionData] public Dictionary<string, JsonElement> ExtensionData { get; set; }
}

public class CountryData
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("game_id")] public int GameId { get; set; }
    [JsonPropertyName("code")] public string Code { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("is_player")] public bool IsPlayer { get; set; }
    [JsonPropertyName("efficiency")] public int Efficiency { get; set; }
    [JsonPropertyName("treasury")] public int Treasury { get; set; }
    [JsonPropertyName("stability")] public int Stability { get; set; }
    [JsonPropertyName("manpower")] public int Manpower { get; set; }
    [JsonPropertyName("prestige")] public int Prestige { get; set; }
    [JsonPropertyName("mission_point")] public int MissionPoint { get; set; }
    [JsonPropertyName("last_turn_gold_income")] public int LastTurnGoldIncome { get; set; }
    [JsonPropertyName("last_turn_manpower_income")] public int LastTurnManpowerIncome { get; set; }
    [JsonPropertyName("history")] public string History { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement> ExtensionData { get; set; }
}

public class RelationData
{
    [JsonPropertyName("src_country")] public int SrcCountryId { get; set; }
    [JsonPropertyName("dst_country")] public int DstCountryId { get; set; }
    [JsonPropertyName("value")] public int Value { get; set; }
    [JsonPropertyName("is_allied")] public bool IsAllied { get; set; }
    [JsonPropertyName("is_at_war")] public bool IsAtWar { get; set; }
    [JsonPropertyName("history")] public string History { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement> ExtensionData { get; set; }
}

public class PersonData
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("country_id")] public int CountryId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("age")] public int Age { get; set; }
    [JsonPropertyName("is_alive")] public bool IsAlive { get; set; }
    [JsonPropertyName("loyalty")] public int Loyalty { get; set; }
    [JsonPropertyName("role")] public string Role { get; set; }
    [JsonPropertyName("stats")] public PersonStats Stats { get; set; }
    [JsonPropertyName("history")] public string History { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement> ExtensionData { get; set; }
}

public class PersonStats
{
    [JsonPropertyName("leadership")] public int? Leadership { get; set; }
    [JsonPropertyName("politics")] public int? Politics { get; set; }

    [JsonPropertyName("morale")] public int? Morale { get; set; }
    [JsonPropertyName("field_offense")] public int? FieldOffense { get; set; }
    [JsonPropertyName("field_defense")] public int? FieldDefense { get; set; }
    [JsonPropertyName("siege_offense")] public int? SiegeOffense { get; set; }
    [JsonPropertyName("siege_defense")] public int? SiegeDefense { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement> ExtensionData { get; set; }
}

public class CommanderyData
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("game_id")] public int GameId { get; set; }
    [JsonPropertyName("country_id")] public int CountryId { get; set; }
    [JsonPropertyName("commander_id")] public int? CommanderId { get; set; }
    [JsonPropertyName("code")] public string Code { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("population")] public int Population { get; set; }
    [JsonPropertyName("wealth")] public int Wealth { get; set; }
    [JsonPropertyName("unrest")] public int Unrest { get; set; }
    [JsonPropertyName("defensiveness")] public int Defensiveness { get; set; }
    [JsonPropertyName("garrisons")] public int Garrisons { get; set; }
    [JsonPropertyName("neighbors")] public List<int> Neighbors { get; set; } = new List<int>();
    [JsonPropertyName("history")] public string History { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement>? ExtensionData { get; set; }
}

public class ArmyData
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("game_id")] public int GameId { get; set; }
    [JsonPropertyName("country_id")] public int CountryId { get; set; }
    [JsonPropertyName("commander_id")] public int? CommanderId { get; set; }
    [JsonPropertyName("location_id")] public int LocationId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("morale")] public int Morale { get; set; }

    [JsonPropertyName("supply")] public int Supply { get; set; }
    [JsonPropertyName("action_left")] public int ActionLeft { get; set; }
    [JsonPropertyName("history")] public string History { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement> ExtensionData { get; set; }
}


public class BattleData
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("phase")] public string Phase { get; set; }
    [JsonPropertyName("attacker_army")] public List<int> AttackerArmyIds { get; set; } = [];
    [JsonPropertyName("defender_army")] public List<int> DefenderArmyIds { get; set; } = [];
    [JsonPropertyName("location_id")] public int LocationId { get; set; }
    [JsonPropertyName("starting_month")] public int StartingMonth { get; set; }
    [JsonPropertyName("starting_year")] public int StartingYear { get; set; }
    [JsonPropertyName("attacker_losses")] public int AttackerLosses { get; set; }
    [JsonPropertyName("defender_losses")] public int DefenderLosses { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement> ExtensionData { get; set; }
}