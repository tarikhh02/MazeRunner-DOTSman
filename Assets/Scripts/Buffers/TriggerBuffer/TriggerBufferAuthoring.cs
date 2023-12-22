using Unity.Entities;
using UnityEngine;

public class TriggerBufferAuthoring : MonoBehaviour
{
}

public class TriggerBufferBaker : Baker<TriggerBufferAuthoring>
{
    public override void Bake(TriggerBufferAuthoring authoring)
    {
        AddBuffer<TriggerBuffer>(GetEntity(new TransformUsageFlags()));
    }
}
