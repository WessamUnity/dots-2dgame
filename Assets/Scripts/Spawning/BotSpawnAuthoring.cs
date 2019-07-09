using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
public class BotSpawnAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject Prefab;
    public int Count;
    public float MinSpeed;
    public float MaxSpeed;

    // Referenced prefabs have to be declared so that the conversion system knows about them ahead of time
    public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
    {
        gameObjects.Add(Prefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new BotSpawn
        {
            // The referenced prefab will be converted due to DeclareReferencedPrefabs.
            // So here we simply map the game object to an entity reference to that prefab.
            Prefab = conversionSystem.GetPrimaryEntity(Prefab),
            Count = Count,
            MinSpeed = MinSpeed,
            MaxSpeed = MaxSpeed
        };
        dstManager.AddComponentData(entity, spawnerData);
    }
}
