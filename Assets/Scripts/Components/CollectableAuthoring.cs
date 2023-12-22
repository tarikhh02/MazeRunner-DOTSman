using Unity.Entities;
using UnityEngine;

public class CollectableAuthoring : MonoBehaviour
{
    public float points;
}

public partial struct Collectable : IComponentData
{
    public float points;
}

public class CollectableBaker : Baker<CollectableAuthoring>
{
    public override void Bake(CollectableAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new Collectable
        {
            points = authoring.points
        });
    }
}