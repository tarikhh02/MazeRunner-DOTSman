using Unity.Entities;
using UnityEngine;

public partial struct Kill : IComponentData
{
    public float killTimer;
}

public class KillAuthoring : MonoBehaviour
{
    public float killTimer;
}

public class KillBaker : Baker<KillAuthoring>
{
    public override void Bake(KillAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new Kill
        {
            killTimer = authoring.killTimer
        });
    }
}