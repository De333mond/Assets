using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _attackPoint;
    
    public int AttackAnimationsCount = 4;
    private Player _player;
    
    void Start()
    {
        _player = GetComponent<Player>();
    }

    public void Attack()
    {
        if (_player.Inventory.ActiveWeapon is null)
            return;
        
        Weapon weapon = _player.Inventory.ActiveWeapon;
        
        if (weapon is Melee)
            AttackWithMelee(weapon as Melee);
        else 
            Debug.Log("Unknown type of weapon!");
    }

    private void AttackWithMelee(Melee weapon)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(_attackPoint.position, weapon.AttackRange);
        foreach (var target in targets)
        {
            if (target.CompareTag("Enemy"))
            {
                Enemy enemy = target.GetComponent<Enemy>();
                enemy.TakeDamage(_player.Stats.GetDamageWithWeapon(weapon));
            }
        }
    }

    public float GetAttackDuration()
    { 
        return _player.Inventory.ActiveWeapon?.Stats.Cooldown / _player.Stats.AttackSpeed ?? 0f;
    }
}
