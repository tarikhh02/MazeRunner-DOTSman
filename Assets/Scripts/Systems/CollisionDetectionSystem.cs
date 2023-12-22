using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Jobs;

public partial struct CollisionDetectionSystem : ISystem
{
    BufferLookup<CollisionBuffer> collisions;

    [BurstCompile]
    public void OnUpdate(ref SystemState systemState)
    {
        new CollisionBufferDeletion().Schedule();

        collisions.Update(ref systemState);

        JobHandle jobHandle = new CollisionDetectionJob
        {
            collisions = collisions,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), systemState.Dependency);
        jobHandle.Complete();
    }
    [BurstCompile]
    public void OnCreate(ref SystemState systemState)
    {
        collisions = systemState.GetBufferLookup<CollisionBuffer>();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState systemState)
    {

    }
}
[BurstCompile]
public partial struct CollisionDetectionJob : ICollisionEventsJob
{
    public BufferLookup<CollisionBuffer> collisions;

    [BurstCompile]
    public void Execute(CollisionEvent collisionEvent)
    {
        if (collisions.HasBuffer(collisionEvent.EntityA))
            collisions[collisionEvent.EntityA].Add(new CollisionBuffer { entity = collisionEvent.EntityB });
        if (collisions.HasBuffer(collisionEvent.EntityB))
            collisions[collisionEvent.EntityB].Add(new CollisionBuffer { entity = collisionEvent.EntityA });
    }
}

[BurstCompile]
public partial struct CollisionBufferDeletion : IJobEntity
{
    public void Execute(DynamicBuffer<CollisionBuffer> collisionBuffer)
    {
        collisionBuffer.Clear();
    }
}