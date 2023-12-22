using Unity.Entities;
using UnityEngine;

public partial struct SpawnManager : IComponentData
{
    public int enemyNum;
    public int maxEnemyNum;
    public Entity enemyPrefab;
}

public class SpawnManagerAuthoring : MonoBehaviour
{
    public int enemyNum;
    public int maxEnemyNum;
    public GameObject enemyPrefab;
}

public class SpawnManagerBaker : Baker<SpawnManagerAuthoring>
{
    public override void Bake(SpawnManagerAuthoring authoring)
    {
        AddComponent(GetEntity(new TransformUsageFlags()), new SpawnManager
        {
            enemyNum = authoring.enemyNum,
            maxEnemyNum = authoring.maxEnemyNum,
            enemyPrefab = GetEntity(authoring.enemyPrefab, new TransformUsageFlags()),
});
    }
}