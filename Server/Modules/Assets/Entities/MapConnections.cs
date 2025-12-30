namespace Server.Modules.Assets.Entities;

public class MapConnections(Dictionary<int, List<int>> connections)
{
    public Dictionary<int, List<int>> AsDict()
    {
        return connections;
    }

    public bool IsNeighborOf(int commanderyId, int neighborId)
    {
        return connections.TryGetValue(commanderyId, out var neighbors)
            && neighbors.FindIndex(nid => nid == neighborId) != -1;
    }
}