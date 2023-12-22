using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;

[BurstCompile]
public partial struct DamageHandlingSystem : ISystem
{
    public ComponentLookup<Damage> damageComponentLookUp;
    public ComponentLookup<Kill> killComponentLookUp;
    public ComponentLookup<Audio> audioComponentLookup;
    public BufferLookup<Child> childLookup;

    [BurstCompile]
    public void OnUpdate(ref SystemState systemState)
    {
        RefRW<SpawnManager> spawnManager;
        if (!SystemAPI.TryGetSingletonRW<SpawnManager>(out spawnManager))
            return;

        damageComponentLookUp.Update(ref systemState);
        killComponentLookUp.Update(ref systemState);
        childLookup.Update(ref systemState);
        audioComponentLookup.Update(ref systemState);

        new TakeDamage
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            damageComponent = damageComponentLookUp,
            killComponent = killComponentLookUp,
            audioComponentLookup = audioComponentLookup,
            spawnManager = spawnManager,
            ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(systemState.WorldUnmanaged),
        }.Schedule();

        new KillJob
        {
            ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(systemState.WorldUnmanaged),
            deltaTime = SystemAPI.Time.DeltaTime,
            childrenLookUp = childLookup,
        }.Schedule();
    }
    [BurstCompile]
    public void OnCreate(ref SystemState systemState)
    {
        damageComponentLookUp = systemState.GetComponentLookup<Damage>();
        killComponentLookUp = systemState.GetComponentLookup<Kill>();
        childLookup = systemState.GetBufferLookup<Child>();
        audioComponentLookup = systemState.GetComponentLookup<Audio>();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState systemState)
    {

    }
}

[BurstCompile]
partial struct TakeDamage : IJobEntity
{
    public EntityCommandBuffer ecb;
    public ComponentLookup<Damage> damageComponent;
    public ComponentLookup<Kill> killComponent;
    public ComponentLookup<Audio> audioComponentLookup;
    [NativeDisableUnsafePtrRestriction]
    public RefRW<SpawnManager> spawnManager;
    public float deltaTime;

    [BurstCompile]
    public void Execute(DynamicBuffer<CollisionBuffer> collisions, DynamicBuffer<AudioBuffer> audioPlays, 
                        DynamicBuffer<PlayDamageAnimBuffer> playDamageAnimBuffer, ref Health health, ref Points points, 
                        ref LocalTransform playerTransform, Entity entity)
    {
        for (int i = 0; i < collisions.Length; i++)
        {
            if (!damageComponent.HasComponent(collisions[i].entity))
                continue;

            if (!health.isCollidingWithEnemy)
            {
                playDamageAnimBuffer.Add(new PlayDamageAnimBuffer { timer = 0.2f });
                health.value -= damageComponent.GetRefRO(collisions[i].entity).ValueRO.value;
                health.isCollidingWithEnemy = true;
                health.invincibilityTimer = 1;

                if (health.value <= 0)
                {
                    if (!audioComponentLookup.HasComponent(entity))
                    {
                        audioPlays.Add(new AudioBuffer { name = "PlayerDeath", isMusic = false });
                        ecb.AddComponent(entity, new Audio());
                    }
                    ecb.AddComponent(entity, new Kill { killTimer = health.killTimer });
                }
                else
                    playerTransform.Position = new float3(-6,0,0);

                break;
            }
            else if (health.canKillEnemy && !killComponent.HasComponent(collisions[i].entity))
            {
                if (!audioComponentLookup.HasComponent(collisions[i].entity))
                {
                    audioPlays.Add(new AudioBuffer { name = "GhostDeath", isMusic = false });
                    ecb.AddComponent(collisions[i].entity, new Audio());
                }
                ecb.AddComponent(collisions[i].entity, new Kill { killTimer = 0f });
                spawnManager.ValueRW.enemyNum--;
                points.points += (spawnManager.ValueRW.maxEnemyNum * 100) / spawnManager.ValueRW.enemyNum;
            }
        }
        if (!killComponent.HasComponent(entity))
        {
            if (health.isCollidingWithEnemy && health.invincibilityTimer > 0)
                health.invincibilityTimer -= deltaTime;
            else if (health.invincibilityTimer <= 0)
            {
                health.canKillEnemy = false;
                health.isCollidingWithEnemy = false;
            }
        }
    }
}

[BurstCompile]
partial struct KillJob : IJobEntity
{
    public BufferLookup<Child> childrenLookUp;
    public EntityCommandBuffer ecb;
    public float deltaTime;

    [BurstCompile]
    public void Execute(ref Kill killComponent, Entity entity)
    {
        if (killComponent.killTimer > 0)
            killComponent.killTimer -= deltaTime;
        else
        {
            if (childrenLookUp.HasBuffer(entity))
            { 
                DynamicBuffer<Child> children;
                childrenLookUp.TryGetBuffer(entity, out children);

                for (int i = 0; i < children.Length; i++)
                    ecb.DestroyEntity(children[i].Value);
            }

            ecb.DestroyEntity(entity);
        }
    }
}