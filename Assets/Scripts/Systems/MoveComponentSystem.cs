using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

[BurstCompile]
public partial struct MoveComponentSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new MoveComponentJob().ScheduleParallel();
    }
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}

[BurstCompile]
public partial struct MoveComponentJob : IJobEntity
{ 
    [BurstCompile]
    public void Execute(ref PhysicsVelocity physicsVelocity, in MoveComponent moveComponent)
    {
        physicsVelocity.Linear = moveComponent.speed * moveComponent.direction;
    }
}
