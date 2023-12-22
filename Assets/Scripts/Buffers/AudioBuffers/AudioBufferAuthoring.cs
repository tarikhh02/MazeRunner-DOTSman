using Unity.Entities;
using UnityEngine;

public class AudioBufferAuthoring : MonoBehaviour
{
}

public class AudioBufferBaker : Baker<AudioBufferAuthoring>
{
    public override void Bake(AudioBufferAuthoring authoring)
    {
        AddBuffer<AudioBuffer>(GetEntity(new TransformUsageFlags()));
    }
}