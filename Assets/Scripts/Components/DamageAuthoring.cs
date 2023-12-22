using Unity.Entities;
using UnityEngine;

public partial struct Damage : IComponentData
{
    public float value;
}

public class DamageAuthoring : MonoBehaviour
{
    public float value;
}

public class DamageBaker : Baker<DamageAuthoring>
{
    public override void Bake(DamageAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new Damage
        {
            value = authoring.value
        });
    }
}