using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct AnimationSystem : ISystem
{
    public float timer;

    public void OnUpdate(ref SystemState systemState)
    {
        if (MonoBehaviour.FindAnyObjectByType<AnimationManager>() == null)
            return;

        if (timer >= 0)
            timer -= SystemAPI.Time.DeltaTime;
        else
            AnimationManager.instance.SetBoolAnimationTriggers("Hit", false);

        foreach((LocalTransform transform, PhysicsVelocity physicsVelocity, Player playerTag) in SystemAPI.Query<LocalTransform, PhysicsVelocity, Player>())
        {
            AnimationManager.instance.UpdateCharacterTransform(transform.Position);
            AnimationManager.instance.SetAnimationBySpeed(physicsVelocity.Linear);
        }
        foreach(DynamicBuffer<PlayDamageAnimBuffer> playDmgBuff in SystemAPI.Query<DynamicBuffer<PlayDamageAnimBuffer>>())
        {
            if (timer <= 0)
            {
                foreach (var buffer in playDmgBuff)
                {
                    timer = buffer.timer;
                    AnimationManager.instance.SetBoolAnimationTriggers("Hit", true);
                }
            }
            playDmgBuff.Clear();
        }
    }
    public void OnCreate(ref SystemState systemState)
    {
        timer = 0;
    }
    public void OnDestroy(ref SystemState systemState)
    {
    }
}