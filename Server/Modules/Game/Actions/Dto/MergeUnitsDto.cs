namespace Server.Modules.Game.Actions.Dto;

public class MergeUnitsDto
{
    public required int SourceUnitId { get; set; }
    public required int TargetUnitId { get; set; }
}