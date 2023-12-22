using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnManagerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState systemState)
    {
        RefRW<RandomComponentSingleton> randomComponent;
        if (!SystemAPI.TryGetSingletonRW<RandomComponentSingleton>(out randomComponent))
            return;

        new SpawmManagerJob
        {
            randomComponent = randomComponent,
            ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(systemState.WorldUnmanaged),
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
public partial struct SpawmManagerJob : IJobEntity 
{
    public EntityCommandBuffer ecb;
    [NativeDisableUnsafePtrRestriction]
    public RefRW<RandomComponentSingleton> randomComponent;

    [BurstCompile]
    public void Execute(ref SpawnManager spawnManager, in DynamicBuffer<SpawnPointBuffer> spawnPointBuffer)
    {
        if (spawnManager.enemyNum >= spawnManager.maxEnemyNum)
            return;

        foreach (SpawnPointBuffer spawnPoint in spawnPointBuffer)
        {
            if (randomComponent.ValueRW.random.NextInt(0, 100) > 95)
            {
                spawnManager.enemyNum++;
                Entity enemtInstance = ecb.Instantiate(spawnManager.enemyPrefab);
                ecb.SetComponent<LocalTransform>(enemtInstance, new LocalTransform 
                { 
                    Position = spawnPoint.spawnPointPosition,
                    Rotation = quaternion.identity,
                    Scale = 1,
                });
            }
        }
    }
}