using Unity.Entities;
using UnityEngine;

public class PlayDamageAnimComponentAuthoring : MonoBehaviour
{
}

public class PlayDamageAnimBaker : Baker<PlayDamageAnimComponentAuthoring>
{
    public override void Bake(PlayDamageAnimComponentAuthoring authoring)
    {
        AddBuffer<PlayDamageAnimBuffer>(GetEntity(new TransformUsageFlags()));
    }
}