using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
}

public class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        AddComponent(this.GetEntity(new TransformUsageFlags()), new Enemy());
    }
}
