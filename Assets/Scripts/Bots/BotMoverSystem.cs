using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class BotMoverSystem : JobComponentSystem
{
    [RequireComponentTag(typeof(BotTag))]
    struct BotMoverJob : IJobForEach<BotSpeed, Translation>
    {
        public float DeltaTime;
        public float2 LevelSize;

        public void Execute(ref BotSpeed botSpeed, ref Translation translation)
        {
            translation.Value.x += botSpeed.Value.x * DeltaTime;
            translation.Value.z += botSpeed.Value.y * DeltaTime;

            if ((translation.Value.x >= LevelSize.x / 2 && botSpeed.Value.x > 0) ||
                (translation.Value.x <= -LevelSize.x / 2 && botSpeed.Value.x < 0))
            {
                botSpeed.Value.x = -botSpeed.Value.x;
            }

            if ((translation.Value.z >= LevelSize.y / 2 && botSpeed.Value.y > 0) ||
                (translation.Value.z <= -LevelSize.y / 2 && botSpeed.Value.y < 0))
            {
                botSpeed.Value.y = -botSpeed.Value.y;
            }
        }
    }

    EntityQuery m_LevelSizeQuery;

    protected override void OnCreate()
    {
        m_LevelSizeQuery = GetEntityQuery
        (
            ComponentType.ReadOnly<LevelSize>()
        );
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var levelSizes = m_LevelSizeQuery.ToComponentDataArray<LevelSize>(Allocator.TempJob);

        if (levelSizes.Length > 0)
        {
            var job = new BotMoverJob
            {
                DeltaTime = Time.deltaTime,
                LevelSize = new float2(levelSizes[0].X, levelSizes[0].Y)
            };

            inputDeps = job.Schedule(this, inputDeps);
        }

        levelSizes.Dispose();

        return inputDeps;
    }
}
