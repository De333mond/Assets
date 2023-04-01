using System;
using UnityEngine;

namespace PlayerInventory.Scriptable
{
    [CreateAssetMenu(menuName = "Items/Specials/Weapon", fileName = "New item")]
    public class Weapon : SpecialItem
    {
        [field : SerializeField] public AttackStats MainAttackStats { get; private set; }
        
        public override string Info()
        {
            return base.Info() + $"Attack range: {MainAttackStats.attackRange}\n";
        }
    }
}