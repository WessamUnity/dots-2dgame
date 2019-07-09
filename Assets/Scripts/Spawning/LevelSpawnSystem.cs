using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class LevelSpawnSystem : ComponentSystem
{
    EntityQuery m_LevelSpawnerQuery;

	protected override void OnCreate()
    {
		m_LevelSpawnerQuery = GetEntityQuery
		(
			ComponentType.ReadOnly<LevelSpawn>(),
			ComponentType.ReadWrite<LocalToWorld>()
		);
	}

    protected override void OnUpdate()
    {
        var levelSpawners = m_LevelSpawnerQuery.ToComponentDataArray<LevelSpawn>(Allocator.TempJob);
        var entities = m_LevelSpawnerQuery.ToEntityArray(Allocator.TempJob);

		if (entities.Length > 0)
		{
			Spawn(entities[0], levelSpawners[0]);
		}

        entities.Dispose();
		levelSpawners.Dispose();

		Enabled = false;
    }

    void Spawn(Entity entity, LevelSpawn levelSpawnFromEntity)
    {
        // Create our new Bot entity
        var instance = EntityManager.Instantiate(levelSpawnFromEntity.Prefab);
        EntityManager.SetComponentData(instance, new Translation { Value = new float3() });
		EntityManager.AddComponentData(instance, new LevelSize { X = levelSpawnFromEntity.Size.x, Y = levelSpawnFromEntity.Size.y });

        EntityManager.DestroyEntity(entity);
    }
}
