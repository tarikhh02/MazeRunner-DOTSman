using Unity.Entities;

public partial struct MeshSpawnSystem : ISystem
{
    public EntityQuery playerQuery;
    public void OnUpdate(ref SystemState state)
    {
        if (playerQuery.IsEmpty || MeshSpawn.instance.hasMeshSpawned)
            return;

        MeshSpawn.instance.SpawnMesh();
        MeshSpawn.instance.hasMeshSpawned = true;
    }
    public void OnCreate(ref SystemState state)
    {
        playerQuery = state.GetEntityQuery(ComponentType.ReadOnly<Player>());
    }
    public void OnDestroy(ref SystemState state)
    {

    }
}