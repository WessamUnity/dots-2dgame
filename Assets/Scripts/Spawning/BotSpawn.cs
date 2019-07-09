using Unity.Entities;

public struct BotSpawn : IComponentData
{
    public Entity Prefab;
    public int Count;
}
