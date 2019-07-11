using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ProjectileMoverSystem : JobComponentSystem
{
	[BurstCompile]
	[RequireComponentTag(typeof(ProjectileMovingTag))]
	struct ProjectileMoverJob : IJobForEach<ProjectileSpeed, Translation, Rotation>
	{
		public float DeltaTime;

		public void Execute(ref ProjectileSpeed speed, ref Translation translation, ref Rotation rotation)
		{
			translation.Value += math.forward(rotation.Value) * speed.MetersPerSecond * DeltaTime;
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var job = new ProjectileMoverJob
		{
			DeltaTime = Time.deltaTime
		};

		inputDeps = job.Schedule(this, inputDeps);
		return inputDeps;
	}
}