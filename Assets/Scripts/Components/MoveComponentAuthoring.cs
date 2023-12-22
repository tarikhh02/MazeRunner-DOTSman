using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial struct MoveComponent : IComponentData
{
    public float3 direction;
    public float speed;

}

public class MoveComponentAuthoring : MonoBehaviour
{
    public float3 direction;
    public float speed;
}

public class MoveComponentBaker : Baker<MoveComponentAuthoring>
{
    public override void Bake(MoveComponentAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new MoveComponent
        {
            direction = authoring.direction,
            speed = authoring.speed,
        });
    }
}