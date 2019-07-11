using Unity.Entities;

public struct BotSpawn : IComponentData
{
    public Entity Prefab;
    public int Count;
    public float MinSpeed;
    public float MaxSpeed;
	public float CollisionSize;
}
