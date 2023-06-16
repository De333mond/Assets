using System;
using System.Collections;
using Character;
using UnityEngine;
using UniversalStatsSystem;
using Random = UnityEngine.Random;


[RequireComponent(typeof(ItemDropper))]
public class Enemy : CharacterBase
{
    //TODO fix attack mechanic because too hard to kill without take damage 
    
    [Header("Enemy")]
    [Space]
    [Header("Moving")]
    [SerializeField] private Vector2 patrolRange;
    [SerializeField] private float stayCooldown;
    [Header("Attack")]
    
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform target;
    
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private AudioClip walk, attack;
    [SerializeField] private Animator animator;
    
        

    private AudioSource _source;
    private Vector3 _startPosition;
    private ItemDropper _itemDropper;
    private bool _isAlive, _canMove;    
    private Rigidbody2D _rigidbody;
    private bool _stay;
    private bool _following, _attacking;
    private float _direction = 1f;
    private bool _frozen = false;

    protected override void OnAwake()
    {
        base.OnAwake();
        
        _startPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
        _itemDropper = GetComponent<ItemDropper>();
        // DeathEvent.AddListener(DropItems);
        _isAlive = _canMove = true;
        _source = GetComponent<AudioSource>();
        _source.playOnAwake = false;
        
        StatsSystem.Init();
        
        StatusEffectSystem.OnFrozenStatusStart += FrozenStatus;
        StatusEffectSystem.OnFrozenStatusEnd += UnFrozenStatus;

        if(target == null)
            target = Player.Instance.CharacterCenter;

        _direction = Random.value >= 0.5f ? 1 : -1;
    }
    
    private void FrozenStatus()
    {
        _frozen = true;
        animator.SetFloat("velocity", 0);
    }
    private void UnFrozenStatus()
    {
        _frozen = false;
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        
        if (_isAlive && _canMove && !_frozen)
            Move();
    }

    private void Move()
    {
        if(target == null) return;
        
        float distanceToTarget = Vector2.Distance(target.position, transform.position);
        if (distanceToTarget < patrolRange.x / 2 || _following)
        {
            _following = true;
            if (!_attacking)
            {
                Follow();
                var distance = Vector2.Distance(target.position, attackPoint.position);
                if (distance < attackRange)
                {
                    _source.clip = attack;
                    _source.loop = false;
                    _source.Play();
                    Attack();
                }
            }
        }
        else
        {
            if (!_stay)
            {
                Patrol();
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
            }
        }

        var localScale = transform.localScale;
        if (((_rigidbody.velocity.x < 0 && localScale.x > 0) || (_rigidbody.velocity.x > 0 && localScale.x < 0))
            && Mathf.Abs(_rigidbody.velocity.x) > 0.001f)
        {
            transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        }

        
        animator.SetFloat("velocity", Math.Abs(_rigidbody.velocity.x / StatsSystem.MainStats.WalkSpeed));
    }

    private void Patrol()
    {
        bool outOfBounds = transform.position.x > _startPosition.x + patrolRange.x / 2 ||
                           transform.position.x < _startPosition.x - patrolRange.x / 2;
        
        if (outOfBounds && !_stay)
        {
            _direction *= -1;
            _stay = true;
            StartCoroutine(ResetStayState(stayCooldown));
        }

        _rigidbody.velocity = new Vector2(StatsSystem.MainStats.WalkSpeed * _direction, _rigidbody.velocity.y);
    }

    
    private void Follow()
    {
        if (transform.position.x > target.position.x)
            _rigidbody.velocity = new Vector2(-StatsSystem.MainStats.WalkSpeed, _rigidbody.velocity.y);
        else
            _rigidbody.velocity = new Vector2(StatsSystem.MainStats.WalkSpeed, _rigidbody.velocity.y);
    }

    private void Attack()
    {
        
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _attacking = true;
        animator.SetTrigger("Attack");
        StartCoroutine(ResetAttackFlag(attackCooldown / StatsSystem.AttackStats.attackCooldown));
        
    }

    public override void TakeDamage(AttackStats attackStats)
    {
        base.TakeDamage(attackStats);
        
        StatsSystem.TakeDamage(attackStats);
        Debug.Log(StatsSystem.MainStats.Health);
        animator.SetTrigger("Hurt");
        StartCoroutine(Stun());

        if (StatsSystem.MainStats.Health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Stun()
    {
        _canMove = false;
        yield return new WaitForSeconds(.2f);
        _canMove = true;
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
        _itemDropper.DropItems();
    }

    private IEnumerator ResetAttackFlag(float attackCooldown)
    {
        yield return new WaitForSeconds(.2f);

        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (var target in targets)
        {
            if (target.CompareTag("Player"))
            {
                var player = target.GetComponent<Player>();
                player.TakeDamage(StatsSystem.GetDamage(), transform.position);
            }
        }
  
        yield return new WaitForSeconds(attackCooldown);
        
        _attacking = false;
    }

    private IEnumerator ResetStayState(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        _stay = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, patrolRange);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        if(groundCheckPoint) Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        Gizmos.DrawWireSphere(Position, 5f);//electricity status effect radius
    }
}