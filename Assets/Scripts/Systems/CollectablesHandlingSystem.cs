using Unity.Burst;
using Unity.Entities;
using UnityEngine.Assertions.Must;

[BurstCompile]
public partial struct CollectablesHandlingSystem : ISystem
{
    public ComponentLookup<Collectable> collectableComponentLookUp;
    public ComponentLookup<PowerPill> powerPillComponentLookUp;
    public ComponentLookup<Kill> killComponentLookup;
    public ComponentLookup<Audio> audioComponentLookup;

    [BurstCompile]
    public void OnUpdate(ref SystemState systemState)
    {
        collectableComponentLookUp.Update(ref systemState);
        powerPillComponentLookUp.Update(ref systemState);
        killComponentLookup.Update(ref systemState);
        audioComponentLookup.Update(ref systemState);

        new CollectablesHandlingJob
        {
            collectableComponentLookUp = collectableComponentLookUp,
            powerPillComponentLookUp = powerPillComponentLookUp,
            killComponentLookup = killComponentLookup,
            audioComponentLookup = audioComponentLookup,
            ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(systemState.WorldUnmanaged),
        }.Schedule();
    }
    [BurstCompile]
    public void OnCreate(ref SystemState systemState)
    {
        collectableComponentLookUp = systemState.GetComponentLookup<Collectable>();
        powerPillComponentLookUp = systemState.GetComponentLookup<PowerPill>();
        killComponentLookup = systemState.GetComponentLookup<Kill>();
        audioComponentLookup = systemState.GetComponentLookup<Audio>();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState systemState)
    {

    }
}

public partial struct CollectablesHandlingJob : IJobEntity
{
    public EntityCommandBuffer ecb;
    public ComponentLookup<Collectable> collectableComponentLookUp;
    public ComponentLookup<PowerPill> powerPillComponentLookUp;
    public ComponentLookup<Kill> killComponentLookup;
    public ComponentLookup<Audio> audioComponentLookup;
    public void Execute(DynamicBuffer<TriggerBuffer> triggers, DynamicBuffer<AudioBuffer> audioPlays, ref Health health, ref Points points)
    {
        for (int i = 0; i < triggers.Length; i++) 
        {
            if (killComponentLookup.HasComponent(triggers[i].entity))
                continue;

            if (collectableComponentLookUp.HasComponent(triggers[i].entity))
            {
                if (!audioComponentLookup.HasComponent(triggers[i].entity))
                {
                    audioPlays.Add(new AudioBuffer { name = "PelletCollect", isMusic = false });
                    ecb.AddComponent(triggers[i].entity, new Audio());
                }
                ecb.AddComponent(triggers[i].entity, new Kill { killTimer = 0f });
                points.points += collectableComponentLookUp.GetRefRO(triggers[i].entity).ValueRO.points;
            }
            else if (powerPillComponentLookUp.HasComponent(triggers[i].entity) && !health.canKillEnemy)
            {
                if (!audioComponentLookup.HasComponent(triggers[i].entity))
                {
                    audioPlays.Add(new AudioBuffer { name = "PillCollect", isMusic = false });
                    audioPlays.Add(new AudioBuffer { name = "powerup", isMusic = true });
                    ecb.AddComponent(triggers[i].entity, new Audio());
                }
                ecb.AddComponent(triggers[i].entity, new Kill { killTimer = 0f });
                health.canKillEnemy = true;
                health.isCollidingWithEnemy = true;
                health.invincibilityTimer = powerPillComponentLookUp.GetRefRO(triggers[i].entity).ValueRO.invincibilityTimer;
            }
        }
    }
}