using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class BotSpawnSystem : ComponentSystem
{
    EntityQuery m_SpawnerQuery;

    protected override void OnCreate()
    {
        m_SpawnerQuery = GetEntityQuery
             (
                 ComponentType.ReadOnly<BotSpawn>(),
                 ComponentType.ReadWrite<LocalToWorld>()
             );
    }

    protected override void OnUpdate()
    {
        var spawnerFromEntities = m_SpawnerQuery.ToComponentDataArray<BotSpawn>(Allocator.TempJob);
        var entities = m_SpawnerQuery.ToEntityArray(Allocator.TempJob);

		if (entities.Length > 0)
		{
			Spawn(entities[0], spawnerFromEntities[0]);
		}

        entities.Dispose();
        spawnerFromEntities.Dispose();

        Enabled = false;
    }

    void Spawn(Entity entity, BotSpawn botSpawnFromEntity)
    {
        Random rand = new Random((uint)System.DateTime.Now.ToBinary());
        for (var i = 0; i < botSpawnFromEntity.Count; ++i)
        {
            var position = new float3(rand.NextFloat(), rand.NextFloat(), rand.NextFloat());
            // Create our new Bot entity
            var instance = EntityManager.Instantiate(botSpawnFromEntity.Prefab);
            // Set correct bot location
            EntityManager.SetComponentData(instance, new Translation { Value = position });
        }

        EntityManager.DestroyEntity(entity);
    }
}
