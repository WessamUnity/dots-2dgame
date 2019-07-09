using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

public class InputTrackingSpawnAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
	public GameObject Prefab;

	public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
	{
		gameObjects.Add(Prefab);
	}

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		var spawnerData = new InputTrackingSpawn
		{
			// The referenced prefab will be converted due to DeclareReferencedPrefabs.
			// So here we simply map the game object to an entity reference to that prefab.
			Prefab = conversionSystem.GetPrimaryEntity(Prefab)
		};
		dstManager.AddComponentData(entity, spawnerData);
	}
}