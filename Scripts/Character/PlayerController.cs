using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : PhysicsObject {

    [SerializeField] private Animator animator;
    // [SerializeField] private float maxSpeed = 7;
    [SerializeField] private float jumpTakeOffSpeed = 7;
    [SerializeField] private PlayerAttack _playerAttack;

    private Playeraudio _playeraudio;
    private Vector2 move = Vector2.zero;
    private Player _player;

    public bool Grounded => grounded;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    protected override void ComputeVelocity()
    {
        var localScale = transform.localScale;
        if ((move.x < 0 && localScale.x > 0) || (move.x > 0 && localScale.x < 0))
        {
            transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        }
        animator.SetBool ("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / _player.Stats.WalkSpeed);

        targetVelocity = move * _player.Stats.WalkSpeed;
    }

    public void Attack()
    {
        float attackDuration = _playerAttack.GetAttackDuration();
        if (!grounded || isAttacking || (attackDuration < 0.1f))
        {
            return;
        }
        
        _playerAttack.Attack();

        if (!_playeraudio)
            _playeraudio = GetComponent<Playeraudio>();
        
        _playeraudio.PlayAttack();
        
        velocity.x = 0;
        isAttacking = true;
        StartCoroutine(WaitForAttack(attackDuration));
        
        int attackNum = Random.Range(0, _playerAttack.AttackAnimationsCount);
        animator.SetInteger("numAttack", attackNum);
        animator.SetTrigger("Attack");
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

    private IEnumerator WaitForAttack(float attackDuration)
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }
}