using System;
using Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Player : Entity
{
    private Playeraudio _playerAudio;
    private PlayerController _playerController;
    
    [SerializeField] private PlayerAttack _playerAttack;
    private bool _isAttacking;
    public bool IsAttacking => _isAttacking;
    
    public PlayerInventory Inventory;
    private void Awake()
    {
        _playerAudio = GetComponent<Playeraudio>();
        _playerController = GetComponent<PlayerController>();
        _playerAttack.OnAwake(this);
        
        _healthBar?.SetMaxHealth(maxHealth);
        Health = maxHealth;
        _healthBar?.SetHealth(Health);

        Inventory = new PlayerInventory(8, this);
    }
    
    private void Update()
    {
        
    }
    
    public void Attack()
    {
        float attackDuration = _playerAttack.GetAttackDuration();
        if (!_playerController.Grounded || _isAttacking || (attackDuration < 0.1f))
        {
            return;
        }
        
        _playerAttack.Attack();

        if (!_playerAudio)
            _playerAudio = GetComponent<Playeraudio>();
        
        _playerAudio.PlayAttack();
        
        _isAttacking = true;
        StartCoroutine(WaitForAttack(attackDuration));
        
        int attackNum = UnityEngine.Random.Range(0, _playerAttack.AttackAnimationsCount);
        _animator.SetInteger("numAttack", attackNum);
        _animator.SetTrigger("Attack");
    }
    
    private IEnumerator WaitForAttack(float attackDuration)
    {
        yield return new WaitForSeconds(attackDuration);
        _isAttacking = false;
    }
    
}