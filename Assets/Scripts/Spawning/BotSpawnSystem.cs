using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(LevelSpawnSystem))]
public class BotSpawnSystem : ComponentSystem
{
    EntityQuery m_SpawnerQuery;
	EntityQuery m_LevelSizeQuery;

	protected override void OnCreate()
	{
		m_SpawnerQuery = GetEntityQuery
		(
			ComponentType.ReadOnly<BotSpawn>(),
			ComponentType.ReadWrite<LocalToWorld>()
		);

		m_LevelSizeQuery = GetEntityQuery
		(
			ComponentType.ReadOnly<LevelSize>()
		);
	}

    protected override void OnUpdate()
    {
        var spawnerFromEntities = m_SpawnerQuery.ToComponentDataArray<BotSpawn>(Allocator.TempJob);
        var entities = m_SpawnerQuery.ToEntityArray(Allocator.TempJob);
		var levelSizes = m_LevelSizeQuery.ToComponentDataArray<LevelSize>(Allocator.TempJob);

		if (entities.Length > 0 && levelSizes.Length > 0)
		{
			Spawn(entities[0], spawnerFromEntities[0], levelSizes[0]);
		}

		levelSizes.Dispose();
		entities.Dispose();
        spawnerFromEntities.Dispose();

        Enabled = false;
    }

    void Spawn(Entity entity, BotSpawn botSpawnFromEntity, LevelSize levelSize)
    {
        Random rand = new Random((uint)System.DateTime.Now.ToBinary());
		float maxX = levelSize.X / 2;
		float minX = -maxX;
		float maxY = levelSize.Y / 2;
		float minY = -maxY;

		for (var i = 0; i < botSpawnFromEntity.Count; ++i)
        {
            var position = new float3(rand.NextFloat(minX, maxX), 0, rand.NextFloat(minY, maxY));
            // Create our new Bot entity
            var instance = EntityManager.Instantiate(botSpawnFromEntity.Prefab);
            // Set correct bot location
            EntityManager.SetComponentData(instance, new Translation { Value = position });
        }

        EntityManager.DestroyEntity(entity);
    }
}
