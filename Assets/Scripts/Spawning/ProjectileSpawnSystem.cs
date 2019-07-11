using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

public class ProjectileSpawnSystem : ComponentSystem
{
	EntityQuery m_spawnerQuery;

	protected override void OnCreate()
	{
		m_spawnerQuery = GetEntityQuery
		(
			ComponentType.ReadOnly<ProjectileSpawn>(),
			ComponentType.ReadWrite<Translation>()
		);
	}

	protected override void OnUpdate()
	{
		var spawners = m_spawnerQuery.ToComponentDataArray<ProjectileSpawn>(Allocator.TempJob);
		var entities = m_spawnerQuery.ToEntityArray(Allocator.TempJob);

		if (entities.Length > 0)
		{
			var spawner = spawners[0];
			for (int i = 0; i < spawner.PoolCount; i++)
			{
				var instance = EntityManager.Instantiate(spawner.Prefab);
				EntityManager.AddComponentData(instance, new ProjectileSpeed { MetersPerSecond = spawner.Speed });
				EntityManager.AddComponentData(instance, new CollisionSize { Value = spawner.CollisionSize });
				EntityManager.AddComponent(instance, typeof(ProjectileInPoolTag));
				EntityManager.AddComponent(instance, typeof(ProjectileTag));
			}

			EntityManager.DestroyEntity(entities[0]);
			EntityManager.DestroyEntity(spawner.Prefab);
        }

		spawners.Dispose();
		entities.Dispose();
	}
}