using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class AudioAuthoring : MonoBehaviour
{

}

public class AudioBaker : Baker<AudioAuthoring>
{
    public override void Bake(AudioAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new Audio
        {
        });
    }
}