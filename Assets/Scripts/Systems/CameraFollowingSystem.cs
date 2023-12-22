using Unity.Entities;
using Unity.Transforms;

public partial struct CameraFollowingSystem : ISystem
{
    public void OnUpdate(ref SystemState systemState)
    {
        foreach((LocalTransform playerTransform, Player player) in SystemAPI.Query<LocalTransform, Player>())
        {
            FollowPlayer.instance.FollowPlayerMovement(playerTransform.Position);
        }
    }
    public void OnCreate(ref SystemState systemState)
    {
    }
    public void OnDestroy(ref SystemState systemState)
    {
    }
}