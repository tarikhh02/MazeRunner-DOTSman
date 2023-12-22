using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public partial struct ManagePlayerInputSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    { 
        new ManagePlayerInputJob
        {
            direction = new float3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")),
        }.ScheduleParallel();
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
public partial struct ManagePlayerInputJob : IJobEntity
{
    public float3 direction;
    [BurstCompile]
    public void Execute(ref MoveComponent moveComponent, in Player tag)
    {
        moveComponent.direction = direction;
    }
}