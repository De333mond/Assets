using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    
    [Header("Speeds")] public float WalkSpeed = 3;
    public float JumpForce = 10;

    private MoveState _moveState = MoveState.Idle;
    private DirectionState _directionState = DirectionState.Right;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private Animator _animatorController;
    private float _walkTime;
    private readonly float _walkCooldown = 0.2f;
    [SerializeField] private float _attackCooldown = 0.3f;
    [SerializeField] private Collider2D _meleeAttackCollider;
    [SerializeField] private ContactFilter2D attackFilter;


    public void MoveRight()
    {
        if (_moveState != MoveState.Jump && _moveState != MoveState.Attack)
        {
            _moveState = MoveState.Walk;
            if (_directionState == DirectionState.Left)
            {
                _transform.localScale = new Vector3(-_transform.localScale.x, _transform.localScale.y,
                    _transform.localScale.z);
                _directionState = DirectionState.Right;
            }

            _walkTime = _walkCooldown;
            _animatorController.Play("Run");
        }
    }

    public void MoveLeft()
    {
        if (_moveState != MoveState.Jump && _moveState != MoveState.Attack)
        {
            _moveState = MoveState.Walk;
            if (_directionState == DirectionState.Right)
            {
                _transform.localScale = new Vector3(-_transform.localScale.x, _transform.localScale.y,
                    _transform.localScale.z);
                _directionState = DirectionState.Left;
            }

            _walkTime = _walkCooldown;
            _animatorController.Play("Run");
        }
    }

    public void Jump()
    {
        if (_moveState != MoveState.Jump && _moveState != MoveState.Attack)
        {
            _rigidbody.velocity = Vector3.up * JumpForce;
            _moveState = MoveState.Jump;
            _animatorController.Play("CheckJump");
        }
    }

    private void Idle()
    {
        _moveState = MoveState.Idle;
        _animatorController.Play("Idle");
    }

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animatorController = GetComponent<Animator>();
        _directionState = transform.localScale.x > 0 ? DirectionState.Right : DirectionState.Left;
    }

    private void Update()
    {
        if (_moveState != MoveState.Attack)
        {
            if (_moveState == MoveState.Jump)
            {
                if (_rigidbody.velocity == Vector2.zero)
                {
                    _rigidbody.velocity = Vector2.zero;
                    Idle();
                }
            }
            else if (_moveState == MoveState.Walk)
            {
                _rigidbody.velocity = (_directionState == DirectionState.Right ? Vector2.right : -Vector2.right) *
                                      WalkSpeed;
                _walkTime -= Time.deltaTime;
                if (_walkTime <= 0)
                {
                    _rigidbody.velocity = Vector2.zero;
                    Idle();
                }
            }
        }
    }

    private enum DirectionState
    {
        Right,
        Left
    }

    private enum MoveState
    {
        Idle,
        Walk,
        Jump,
        Attack
    }

    public void Attack()
    {
        if (_moveState != MoveState.Attack && _moveState != MoveState.Jump)
        {
            _rigidbody.velocity = Vector2.zero;
            _moveState = MoveState.Attack;
            _animatorController.Play("Attack");
            StartCoroutine(AttackCoroutine());


            var targets = new Collider2D[10];

            _meleeAttackCollider.OverlapCollider(attackFilter, targets);
            foreach (var target in targets)
                if (target)
                {
                    var entity = target.gameObject.GetComponent<Entity>();
                    if (entity)
                    {
                        entity.TakeDamage(1f);

                        if (entity._name != "")
                            Debug.Log($"{entity._name} received 1 damage");
                    }
                }
                else
                {
                    break;
                }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(_attackCooldown);
        Idle();
    }
}