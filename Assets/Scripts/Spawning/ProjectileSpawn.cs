using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileSpawn : IComponentData
{
	public Entity Prefab;
	public int PoolCount;
	public float Speed;
    public float CollisionSize;
	public float3 PoolLocation;
}