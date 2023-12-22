using Unity.Entities;
using UnityEngine;

public partial struct PowerPill : IComponentData
{
    public float invincibilityTimer;
}

public class PowerPillAuthoring : MonoBehaviour
{
    public float invincibilityTimer;
}

public class PowerPillBaker : Baker<PowerPillAuthoring>
{
    public override void Bake(PowerPillAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new PowerPill
        {
            invincibilityTimer = authoring.invincibilityTimer,
        });
    }
}