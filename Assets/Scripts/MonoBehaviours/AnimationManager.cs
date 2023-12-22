using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;
    Animator animator;

    public void Awake()
    {
        instance = this;
        animator = this.GetComponent<Animator>();
    }

    public void SetAnimationBySpeed(Unity.Mathematics.float3 speedDir)
    {
        if (animator == null)
            return;

        float speed = 0;

        if (speedDir.x > 0.5f || speedDir.x < -0.5f)
            speed = speedDir.x;
        else if (speedDir.z > 0.5f || speedDir.z < -0.5f)
            speed = speedDir.z;

        if (speed < 0)
            speed *= -1;

        animator.SetFloat("Speed", speed);
    }

    public void SetBoolAnimationTriggers(string name, bool hasWon)
    {
        if (animator == null)
            return;

        animator.SetBool(name, hasWon);
    }

    public void UpdateCharacterTransform(Unity.Mathematics.float3 position)
    {
        if (animator == null)
            return;

        this.transform.position = new Vector3(position.x, 0, position.z);
        float rotationValue = 90;

        if (Input.GetKey(KeyCode.A))
            rotationValue = 0;
        else if (Input.GetKey(KeyCode.D))
            rotationValue = 180;
        else if (Input.GetKey(KeyCode.W))
            rotationValue = 90;
        else if (Input.GetKey(KeyCode.S))
            rotationValue = -90;

        this.transform.rotation = Quaternion.Euler(0, rotationValue, 0);
    }
}
