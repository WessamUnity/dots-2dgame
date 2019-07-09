using Unity.Entities;
using Unity.Mathematics;

public struct LevelSpawn : IComponentData
{
    public Entity Prefab;
	public float2 Size;
}
