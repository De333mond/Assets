using System;
using UnityEngine;

namespace PlayerInventory.Scriptable
{
    [CreateAssetMenu(menuName = "Items/Specials/Weapon", fileName = "New item")]
    public class Weapon : SpecialItem
    {
        [SerializeField] private float _attackRange;
        public float AttackRange => _attackRange;
        
        public override string Info()
        {
            return base.Info() + $"Attack range: {_attackRange}\n";
        }
    }
}