using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerInputSystem : ComponentSystem
{
    EntityQuery m_playerQuery;
    EntityQuery m_inputQuery;
    EntityQuery m_projectileQuery;

    float3 Up = new float3(0, 1, 0);

    protected override void OnCreate()
    {
        m_inputQuery = GetEntityQuery
        (
            ComponentType.ReadOnly<InputEvents>(),
            ComponentType.ReadOnly<ProjectedInputs>()
        );

        m_playerQuery = GetEntityQuery
        (
            ComponentType.ReadOnly<PlayerTag>(),
            ComponentType.ReadOnly<Translation>()
        );

        m_projectileQuery = GetEntityQuery
        (
            ComponentType.ReadOnly<ProjectileInPoolTag>(),
            ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadOnly<Rotation>()
        );
    }

    protected override void OnUpdate()
    {
        var events = m_inputQuery.ToComponentDataArray<InputEvents>(Allocator.TempJob);
        var projections = m_inputQuery.ToComponentDataArray<ProjectedInputs>(Allocator.TempJob);
        var playerTranslations = m_playerQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        var projectiles = m_projectileQuery.ToEntityArray(Allocator.TempJob);

        if (projectiles.Length > 0 && events.Length > 0 && events[0].PrimaryDown)
        {
            var projectile = projectiles[0];
            // remove projectile from pool and mark as moving
            EntityManager.RemoveComponent<ProjectileInPoolTag>(projectile);

            // spawn at player location
            EntityManager.SetComponentData(projectile, playerTranslations[0]);

            // set the projectile heading correctly
            float3 playerLoc = playerTranslations[0].Value;
            float2 mouseDown = projections[0].LastPrimaryDown;
            float3 playerToClick = new float3(mouseDown.x, playerLoc.y, mouseDown.y) - playerLoc;
            playerToClick = math.normalize(playerToClick);
            Rotation rotation = new Rotation();
            rotation.Value = quaternion.LookRotation(playerToClick, Up);
            EntityManager.SetComponentData(projectile, rotation);
        }

        events.Dispose();
        projections.Dispose();
        playerTranslations.Dispose();
        projectiles.Dispose();
    }
}