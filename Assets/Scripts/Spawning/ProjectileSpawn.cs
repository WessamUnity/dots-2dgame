using Unity.Entities;

public struct ProjectileSpawn : IComponentData
{
	public Entity Prefab;
	public int PoolCount;
	public float Speed;
    public float CollisionSize;
}