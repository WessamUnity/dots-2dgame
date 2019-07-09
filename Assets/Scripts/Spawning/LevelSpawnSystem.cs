using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class LevelSpawnSystem : ComponentSystem
{
    EntityQuery m_SpawnerQuery;

    protected override void OnCreate()
    {
        m_SpawnerQuery = GetEntityQuery
             (
                 ComponentType.ReadOnly<LevelSpawn>(),
                 ComponentType.ReadWrite<LocalToWorld>()
             );
    }

    protected override void OnUpdate()
    {
        var spawnerFromEntities = m_SpawnerQuery.ToComponentDataArray<LevelSpawn>(Allocator.TempJob);
        var entities = m_SpawnerQuery.ToEntityArray(Allocator.TempJob);

		if (entities.Length > 0)
		{
			Spawn(entities[0], spawnerFromEntities[0]);
		}

        entities.Dispose();
        spawnerFromEntities.Dispose();

        Enabled = false;
    }

    void Spawn(Entity entity, LevelSpawn levelSpawnFromEntity)
    {
        // Create our new Bot entity
        var instance = EntityManager.Instantiate(levelSpawnFromEntity.Prefab);
        EntityManager.SetComponentData(instance, new Translation { Value = new float3() });

        EntityManager.DestroyEntity(entity);
    }
}
