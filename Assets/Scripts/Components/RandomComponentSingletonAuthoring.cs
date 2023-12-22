using System;
using Unity.Entities;
using UnityEngine;

public partial struct RandomComponentSingleton : IComponentData
{
    public Unity.Mathematics.Random random;
}

public class RandomComponentSingletonAuthoring : MonoBehaviour
{
}

public class RandomComponentSingletonBaker : Baker<RandomComponentSingletonAuthoring>
{
    public override void Bake(RandomComponentSingletonAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new RandomComponentSingleton
        {
            random = new Unity.Mathematics.Random((uint)(DateTime.Now.Second + 1))
        });
    }
}