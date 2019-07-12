using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ProjectileCollisionSystem : JobComponentSystem
{
    [RequireComponentTag(typeof(BotTag))]
    struct ProjectileCollisionJob : IJobForEachWithEntity<Translation, CollisionSize>
    {
        [DeallocateOnJobCompletion]
        public NativeArray<Translation> ProjectileTranslations;

        [DeallocateOnJobCompletion]
        [ReadOnly]
        public NativeArray<CollisionSize> ProjectileCollisions;

        [DeallocateOnJobCompletion]
        public NativeArray<Entity> Projectiles;

        public EntityCommandBuffer.Concurrent ECB;

		public float3 PoolLocation;

        public void Execute(Entity entity, int jobIndex, ref Translation translation, ref CollisionSize collision)
        {
            float2 posA = new float2(translation.Value.x, translation.Value.z);
            float sizeA = collision.Value;
             
            for (int i = 0; i < ProjectileTranslations.Length; i++)
            {
                float2 posB = new float2(ProjectileTranslations[i].Value.x, ProjectileTranslations[i].Value.z);
                float sizeB = ProjectileCollisions[i].Value;

                if (RectOverlap(posA, sizeA, posB, sizeB))
                {
					// destroy bot
                    ECB.DestroyEntity(jobIndex, entity);

					// return projectile to pool
                    ECB.AddComponent(jobIndex, Projectiles[i], new ProjectileInPoolTag());
					ECB.SetComponent(jobIndex, Projectiles[i], new Translation { Value = PoolLocation });

					// only one collision is enough
                    return;
                }
            }
        }

        bool ValueInRange(float value, float min, float max)
        { return (value >= min) && (value <= max); }

        bool RectOverlap(float2 A, float sizeA, float2 B, float sizeB)
        {
            bool xOverlap = ValueInRange(A.x, B.x, B.x + sizeB) ||
                            ValueInRange(B.x, A.x, A.x + sizeA);

            bool yOverlap = ValueInRange(A.y, B.y, B.y + sizeB) ||
                            ValueInRange(B.y, A.y, A.y + sizeA);

            return xOverlap && yOverlap;
        }
    }

    EntityQuery m_projectilesQuery;
	List<ProjectilePoolLocation> poolLocations;

	BeginSimulationEntityCommandBufferSystem m_beginSimEcbSystem;

    protected override void OnCreate()
    {
        m_projectilesQuery = GetEntityQuery
        (
            ComponentType.ReadOnly<ProjectileTag>(),
            ComponentType.ReadOnly<CollisionSize>(),
            ComponentType.Exclude<ProjectileInPoolTag>(),
            ComponentType.ReadWrite<Translation>()
        );

        m_beginSimEcbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

		poolLocations = new List<ProjectilePoolLocation>();
	}

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var projectileTranslations = m_projectilesQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        var projectileCollisions = m_projectilesQuery.ToComponentDataArray<CollisionSize>(Allocator.TempJob);
        var projectiles = m_projectilesQuery.ToEntityArray(Allocator.TempJob);

		poolLocations.Clear();
		EntityManager.GetAllUniqueSharedComponentData<ProjectilePoolLocation>(poolLocations);

        if (projectileCollisions.Length > 0 && poolLocations.Count > 1)
        {
            var job = new ProjectileCollisionJob
            {
                ProjectileTranslations = projectileTranslations,
                ProjectileCollisions = projectileCollisions,
                Projectiles = projectiles,
                ECB = m_beginSimEcbSystem.CreateCommandBuffer().ToConcurrent(),
				PoolLocation = poolLocations[1].Value
			};

            inputDeps = job.Schedule(this, inputDeps);
            m_projectilesQuery.AddDependency(inputDeps);
            m_beginSimEcbSystem.AddJobHandleForProducer(inputDeps);
        }
        else
        {
            projectileTranslations.Dispose();
            projectileCollisions.Dispose();
            projectiles.Dispose();
        }

        return inputDeps;
    }
}