using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerAttack
{
    private Player _player;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private int attackAnimationsCount = 4;
    public int AttackAnimationsCount => attackAnimationsCount;
    
    public void OnAwake(Player player)
    {
        _player = player;
    }

    public void Attack()
    {
        if (_player == null)
        {
            Debug.LogError("Not set Player script");
            return;
        }
            
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
        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPoint.position, weapon.AttackRange);
        foreach (var target in targets)
        {
            if (target.CompareTag("Enemy"))
            {
                Enemy enemy = target.GetComponent<Enemy>();
                enemy.TakeDamage(weapon.Damage);
            }
        }
    }

    public float GetAttackDuration()
    {
        return _player.Inventory.ActiveWeapon?.AttackTime ?? 0f;
    }
}
