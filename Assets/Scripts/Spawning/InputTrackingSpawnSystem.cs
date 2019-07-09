using Unity.Collections;
using Unity.Entities;

public class InputTrackingSpawnSystem : ComponentSystem
{
	EntityQuery m_SpawnerQuery;

	protected override void OnCreate()
	{
		m_SpawnerQuery = GetEntityQuery
		(
			ComponentType.ReadOnly<InputTrackingSpawn>()
		);
	}

	protected override void OnUpdate()
	{
		var spawnerFromEntities = m_SpawnerQuery.ToComponentDataArray<InputTrackingSpawn>(Allocator.TempJob);
		var entities = m_SpawnerQuery.ToEntityArray(Allocator.TempJob);

		if (entities.Length > 0)
		{
			var instance = EntityManager.Instantiate(spawnerFromEntities[0].Prefab);
			EntityManager.AddComponent(instance, typeof(InputLocations));
			EntityManager.AddComponent(instance, typeof(InputTimes));
			EntityManager.AddComponent(instance, typeof(InputEvents));
			EntityManager.AddComponent(instance, typeof(ProjectedInputs));

			EntityManager.DestroyEntity(entities[0]);
		}

		entities.Dispose();
		spawnerFromEntities.Dispose();

		Enabled = false;
	}
}
