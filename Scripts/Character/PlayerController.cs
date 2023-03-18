using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : PhysicsObject {

    [SerializeField] private Animator animator;
    [SerializeField] private float maxSpeed = 7;
    [SerializeField] private float jumpTakeOffSpeed = 7;
    [SerializeField] private Player player;

    private Playeraudio _playeraudio;
    private Vector2 move = Vector2.zero;

    public bool Grounded => grounded;

    protected override void OnUpdate()
    {
        isAttacking = player.IsAttacking;
        base.OnUpdate();
    }

    protected override void ComputeVelocity()
    {
        var localScale = transform.localScale;
        if ((move.x < 0 && localScale.x > 0) || (move.x > 0 && localScale.x < 0))
        {
            transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        }
        animator.SetBool ("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
    
    public void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && grounded && !isAttacking)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y *=  0.5f;
            }
        }
    }

    public void SetVelocity(float direction)
    {
        if (!isAttacking)
            move.x = direction;
    }
}