using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Mathematics;

public class ProjectileSpawnAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
	public GameObject Prefab;
	public int PoolCount;
	public float Speed;
    public float CollisionSize;
	public float3 PoolLocation;

	public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
	{
		gameObjects.Add(Prefab);
	}

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		var spawnData = new ProjectileSpawn
		{
			Prefab = conversionSystem.GetPrimaryEntity(Prefab),
			PoolCount = PoolCount,
			Speed = Speed,
            CollisionSize = CollisionSize,
			PoolLocation = PoolLocation
		};
		dstManager.AddComponentData(entity, spawnData);
	}
}