using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;

[BurstCompile]
public partial struct TriggerDetectionSystem : ISystem
{
    BufferLookup<TriggerBuffer> triggers;

    [BurstCompile]
    public void OnUpdate(ref SystemState systemState)
    {
        new TriggerBufferDeletion().Schedule();

        triggers.Update(ref systemState);

        JobHandle jobHandle = new TriggerDetectionJob
        {
            triggers = triggers,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), systemState.Dependency);
        jobHandle.Complete();
    }
    [BurstCompile]
    public void OnCreate(ref SystemState systemState)
    {
        triggers = systemState.GetBufferLookup<TriggerBuffer>();
    }
    [BurstCompile]
    public void OnDestory(ref SystemState systemState)
    {

    }
}

[BurstCompile]
public partial struct TriggerDetectionJob : ITriggerEventsJob
{
    public BufferLookup<TriggerBuffer> triggers;

    [BurstCompile]
    public void Execute(TriggerEvent triggerEvent)
    {

        if (triggers.HasBuffer(triggerEvent.EntityA))
            triggers[triggerEvent.EntityA].Add(new TriggerBuffer { entity = triggerEvent.EntityB });
        if (triggers.HasBuffer(triggerEvent.EntityB))
            triggers[triggerEvent.EntityB].Add(new TriggerBuffer { entity = triggerEvent.EntityA });
    }
}

[BurstCompile]
public partial struct TriggerBufferDeletion : IJobEntity
{
    public void Execute(DynamicBuffer<TriggerBuffer> triggerBuffer)
    {
        triggerBuffer.Clear();
    }
}