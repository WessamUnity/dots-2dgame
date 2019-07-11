using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

public class ProjectileSpawnAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
	public GameObject Prefab;
	public int PoolCount;
	public float Speed;
    public float CollisionSize;

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
            CollisionSize = CollisionSize
        };
		dstManager.AddComponentData(entity, spawnData);
	}
}