using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Stats_system
{
    [Serializable]
    public struct Stats
    {
        [HideInInspector] public float Health;
        public float MaxHealth;
        public float BaseDamage;
        public float Armor;
        public float CriticalChance;
        public float CriticalMultiply;
        public float AttackSpeed;
        public float WalkSpeed;


        private const float ArmorReduceCup = 0.8f; // 80% 
        private const float FullResistArmorAmount = 1000f;

        public float ArmorReduceMultiplier
        {
            get
            {
                float multiplier = 1 - Armor / FullResistArmorAmount;
                return 1 - multiplier > ArmorReduceCup ? 1 - ArmorReduceCup : multiplier;
            }
        }

        public Stats(float health, float baseDamage, float walkSpeed, float armor = 0, float criticalChance = 0,
            float criticalMultiply = 0, float attackSpeed = 1f)
        {
            MaxHealth = Health = health;
            BaseDamage = baseDamage;
            Armor = armor;
            CriticalChance = criticalChance;
            CriticalMultiply = criticalMultiply;
            AttackSpeed = attackSpeed;
            WalkSpeed = walkSpeed;
        }

        public static Stats operator +(Stats a, Stats b)
        {
            return new Stats(
                a.MaxHealth + b.MaxHealth,
                a.BaseDamage + b.BaseDamage,
                a.Armor + b.Armor,
                a.CriticalChance + b.CriticalChance,
                a.CriticalMultiply + b.CriticalMultiply);
        }

        public static Stats operator -(Stats a, Stats b)
        {
            return new Stats(
                ToZero(a.MaxHealth - b.MaxHealth),
                ToZero(a.BaseDamage - b.BaseDamage),
                ToZero(a.Armor - b.Armor),
                ToZero(a.CriticalChance - b.CriticalChance),
                ToZero(a.CriticalMultiply - b.CriticalMultiply));
        }

        public float GetDamageWithWeapon(Weapon weapon)
        {
            float damage = BaseDamage + weapon.Stats.Damage;
            if (Random.value < (CriticalChance + weapon.Stats.CriticalChance))
                damage *= (CriticalMultiply + weapon.Stats.CriticalMultiplier);

            return damage;
        }

        private static float ToZero(float value)
        {
            return value < 0 ? 0 : value;
        }
        
        
    }
}