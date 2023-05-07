using System;
using System.Collections;
using Character;
using UnityEngine;
using UniversalStatsSystem;
 

[RequireComponent(typeof(ItemDropper))]
public class Enemy : CharacterBase
{
    //TODO fix attack mechanic because too hard to kill without take damage 
    
    [Header("Enemy")]
    [Space]
    [Header("Moving")]
    [SerializeField] private Vector2 _patrolRange;
    [SerializeField] private float _stayCooldown;
    [Header("Attack")]
    
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _target;
    
    [SerializeField] private AudioClip walk, attack;
    [SerializeField] private Animator _animator;
    
        

    private AudioSource _source;
    private Vector3 _startPosition;
    private ItemDropper _itemDropper;
    private bool _isAlive, _canMove;    
    private Rigidbody2D _rigidbody;
    private bool _stay;
    private bool _following, _attacking;
    private float direction = 1f;
    
    protected override void OnStart()
    {
        base.OnStart();
        
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
    }

    private bool _frozen = false;
    private void FrozenStatus()
    {
        _frozen = true;
        _animator.SetFloat("velocity", 0);
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
        if(_target == null) return;
        
        float distanceToTarget = Vector2.Distance(_target.position, transform.position);
        if (distanceToTarget < _patrolRange.x / 2 || _following)
        {
            _following = true;
            if (!_attacking)
            {
                Follow();
                var distance = Vector2.Distance(_target.position, _attackPoint.position);
                if (distance < _attackRange)
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

        
        _animator.SetFloat("velocity", Math.Abs(_rigidbody.velocity.x / StatsSystem.MainStats.WalkSpeed));
    }

    private void Patrol()
    {
        bool outOfBounds = transform.position.x > _startPosition.x + _patrolRange.x / 2 ||
                           transform.position.x < _startPosition.x - _patrolRange.x / 2;
        
        if (outOfBounds && !_stay)
        {
            direction *= -1;
            _stay = true;
            StartCoroutine(ResetStayState(_stayCooldown));
        }

        _rigidbody.velocity = new Vector2(StatsSystem.MainStats.WalkSpeed * direction, _rigidbody.velocity.y);
    }

    
    private void Follow()
    {
        if (transform.position.x > _target.position.x)
            _rigidbody.velocity = new Vector2(-StatsSystem.MainStats.WalkSpeed, _rigidbody.velocity.y);
        else
            _rigidbody.velocity = new Vector2(StatsSystem.MainStats.WalkSpeed, _rigidbody.velocity.y);
    }

    private void Attack()
    {
        
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _attacking = true;
        _animator.SetTrigger("Attack");
        StartCoroutine(ResetAttackFlag(_attackCooldown / StatsSystem.AttackStats.attackCooldown));
        
    }

    public override void TakeDamage(AttackStats attackStats)
    {
        base.TakeDamage(attackStats);
        
        StatsSystem.TakeDamage(attackStats);
        Debug.Log(StatsSystem.MainStats.Health);
        _animator.SetTrigger("Hurt");
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
        _animator.SetTrigger("Dead");
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
        _itemDropper.DropItems();
    }

    private IEnumerator ResetAttackFlag(float attackCooldown)
    {
        yield return new WaitForSeconds(.2f);

        Collider2D[] targets = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange);
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
        Gizmos.DrawWireCube(transform.position, _patrolRange);
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
        Gizmos.DrawWireSphere(Position, 5f);//electricity status effect radius
    }
}