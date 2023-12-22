using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

[BurstCompile]
public partial struct MoveEnemyInputSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState systemState)
    {
        RefRW<RandomComponentSingleton> randomComponent;
        if (!SystemAPI.TryGetSingletonRW<RandomComponentSingleton>(out randomComponent))
            return;

        new MoveEnemyInputJob
        {
            physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>(),
            randomComponent = randomComponent,
        }.Schedule();
    }
    [BurstCompile]
    public void OnCreate(ref SystemState systemState)
    {

    }
    [BurstCompile]
    public void OnDestroy(ref SystemState systemState)
    {

    }
}

[BurstCompile]
public partial struct MoveEnemyInputJob : IJobEntity
{
    public PhysicsWorldSingleton physicsWorld;
    [NativeDisableUnsafePtrRestriction]
    public RefRW<RandomComponentSingleton> randomComponent;

    [BurstCompile]
    public void Execute(ref MoveComponent moveComponent, in LocalTransform transform, in Enemy tag)
    {
        NativeArray<float3> directions = new NativeArray<float3>(3, Allocator.Temp);
        directions[0] = moveComponent.direction;
        directions[1] = new float3(moveComponent.direction.x * 0 + moveComponent.direction.z, 0, moveComponent.direction.z * 0 + moveComponent.direction.x);
        directions[2] = new float3(moveComponent.direction.x * 0 - moveComponent.direction.z, 0, moveComponent.direction.z * 0 - moveComponent.direction.x);

        for (int i = 0; i < directions.Length; i++)
        {
            RaycastInput ray = new RaycastInput()
            {
                Start = transform.Position,
                End = transform.Position + (directions[i] * 0.7f),
                Filter = new CollisionFilter()
                {
                    BelongsTo = 3u,
                    CollidesWith = 4u
                }
            };
            if (!physicsWorld.CastRay(ray) && randomComponent.ValueRW.random.NextInt(1, 150) < 140)
            {
                moveComponent.direction = directions[i];
                break;
            }
        }

        directions.Dispose();
    }
}