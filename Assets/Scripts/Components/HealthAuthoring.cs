using Unity.Entities;
using UnityEngine;

public partial struct Health : IComponentData
{
    public float value;
    public float invincibilityTimer;
    public float killTimer;
    public bool isCollidingWithEnemy;
    public bool canKillEnemy;
}

public class HealthAuthoring : MonoBehaviour
{
    public float value;
    public float invincibilityTimer;
    public float killTimer;
    public bool isCollidingWithEnemy;
    public bool canKillEnemy;
}

public class HealthBaker : Baker<HealthAuthoring>
{
    public override void Bake(HealthAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new Health
        {
            value = authoring.value,
            invincibilityTimer = authoring.invincibilityTimer,
            killTimer = authoring.killTimer,
            canKillEnemy = authoring.canKillEnemy,
            isCollidingWithEnemy = authoring.isCollidingWithEnemy
        });
    }
}