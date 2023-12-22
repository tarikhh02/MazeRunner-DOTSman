using Unity.Entities;
using UnityEngine;

public class CollisionBufferAuthoring : MonoBehaviour
{
}

public class CollisionBufferBaker : Baker<CollisionBufferAuthoring>
{
    public override void Bake(CollisionBufferAuthoring authoring)
    {
        AddBuffer<CollisionBuffer>(GetEntity(new TransformUsageFlags()));
    }
}