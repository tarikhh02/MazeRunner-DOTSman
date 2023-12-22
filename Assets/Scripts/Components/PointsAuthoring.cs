using Unity.Entities;
using UnityEngine;

public partial struct Points : IComponentData
{
    public float points;
}

public class PointsAuthoring : MonoBehaviour
{
    public float points;
}

public class PointsBaker : Baker<PointsAuthoring>
{
    public override void Bake(PointsAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new Points
        {
            points = authoring.points
        });
    }
}