using UnityEngine;

public class MeshSpawn : MonoBehaviour
{
    public static MeshSpawn instance;

    public GameObject playerMesh;
    public bool hasMeshSpawned = false;

    private void Awake()
    {
        instance = this;
        MeshSpawn.instance.hasMeshSpawned = false;
    }

    public void SpawnMesh()
    {
        var mesh = Instantiate(playerMesh);
        mesh.transform.position = new Vector3(-6, 0, 0);
    }
}