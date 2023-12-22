using Unity.Entities;
using UnityEngine;

public class SpawnPointBufferAuthoring : MonoBehaviour
{
    public GameObject[] spawnPoints;
}

public class SpawnPointBufferBaker : Baker<SpawnPointBufferAuthoring>
{
    public override void Bake(SpawnPointBufferAuthoring authoring)
    {
        DynamicBuffer<SpawnPointBuffer> spawnBuffer = AddBuffer<SpawnPointBuffer>(GetEntity(new TransformUsageFlags()));

        foreach(GameObject spawnPoint in authoring.spawnPoints)
        {
            spawnBuffer.Add(new SpawnPointBuffer { spawnPointPosition = spawnPoint.transform.position });
        }
    }
}