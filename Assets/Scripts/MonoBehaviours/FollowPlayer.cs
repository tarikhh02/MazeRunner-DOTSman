using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static FollowPlayer instance;

    private void Awake()
    {
        instance = this;
    }

    public void FollowPlayerMovement(Vector3 playerPosition)
    {
        this.transform.position = new Vector3(playerPosition.x, this.transform.position.y, playerPosition.z);
    }
}
