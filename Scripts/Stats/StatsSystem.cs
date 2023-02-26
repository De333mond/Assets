using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Stats
{
    [Serializable]
    public class StatsSystem
    {
        public Stats Stats;

        [SerializeField] private float ArmorReduceCup = 0.8f; // 80% 
        [SerializeField] private float FullResistArmorAmount = 1000f;

        [HideInInspector] public UnityEvent OnDeath;


        private Animator _animator;

        public float ArmorReduceMultiplier
        {
            get
            {
                float multiplier = 1 - Stats.Armor / FullResistArmorAmount;
                return 1 - multiplier > ArmorReduceCup ? 1 - ArmorReduceCup : multiplier;
            }
        }

        public void Init()
        {
            Stats.Health = Stats.MaxHealth;
        }

        public float GetDamageWithWeapon(Weapon weapon)
        {
            float damage = Stats.BaseDamage + weapon.Stats.BaseDamage;
            if (Random.value < (Stats.CriticalChance + weapon.Stats.CriticalChance))
                damage *= (Stats.CriticalMultiply + weapon.Stats.CriticalMultiply);

            return damage;
        }

        public void Heal(float value)
        {
            Stats.Health = Mathf.Min(Stats.Health + value, Stats.MaxHealth);
        }

        public void TakeDamage(float damage)
        {
            if (Stats.IsInvincible)
                return;

            damage *= ArmorReduceMultiplier;
            Stats.Health -= damage;
            if (Stats.Health < 0)
                OnDeath.Invoke();

            Debug.Log($"[Stats system]: total taken damage: {damage}");
        }

        public float GetDamage()
        {
            float criticalDamage =
                Stats.BaseDamage * (Random.value <= Stats.CriticalChance ? Stats.CriticalMultiply : 1);
            return criticalDamage;
        }

        
    }

    [Serializable]
    public class Stats
    {
        [HideInInspector] public float Health;
        public float MaxHealth;
        public float BaseDamage;
        public float Armor;
        public float CriticalChance;
        public float CriticalMultiply;
        public float AttackSpeed;
        public float WalkSpeed;

        public bool IsInvincible;

        // public Stats(float health, float baseDamage, float walkSpeed, float armor = 0, float criticalChance = 0,
        //     float criticalMultiply = 0, float attackSpeed = 1f)
        // {
        //     MaxHealth = Health = health;
        //     BaseDamage = baseDamage;
        //     Armor = armor;
        //     CriticalChance = criticalChance;
        //     CriticalMultiply = criticalMultiply;
        //     AttackSpeed = attackSpeed;
        //     WalkSpeed = walkSpeed;
        //     IsInvincible = false;
        // }

        public static Stats operator +(Stats a, Stats b)
        {
            a.MaxHealth += b.MaxHealth;
            a.BaseDamage += b.BaseDamage;
            a.Armor += b.Armor;
            a.CriticalChance += b.CriticalChance;
            a.CriticalMultiply += b.CriticalMultiply;
            a.AttackSpeed += b.AttackSpeed;
            a.WalkSpeed += b.WalkSpeed;
            return a;
        }

        public static Stats operator -(Stats a, Stats b)
        {
            a.MaxHealth -= b.MaxHealth;
            a.BaseDamage -= b.BaseDamage;
            a.Armor -= b.Armor;
            a.CriticalChance -= b.CriticalChance;
            a.CriticalMultiply -= b.CriticalMultiply;
            a.AttackSpeed -= b.AttackSpeed;
            a.WalkSpeed -= b.WalkSpeed;
            return a;
        }

        public virtual string ExtraInfo()
        {
            string result = "";
            if (MaxHealth > 0)
                result += $"MaxHealth: +{MaxHealth}\n";
            if (BaseDamage > 0)
                result += $"BaseDamage: +{BaseDamage}\n";
            if (Armor > 0)
                result += $"Armor: +{Armor}\n";
            if (CriticalChance > 0)
                result += $"Critical Chance: +{CriticalChance * 100}%\n";
            if (CriticalMultiply > 0)
                result += $"Critical Multiplier: +{CriticalMultiply * 100}%\n";
            if (WalkSpeed > 0)
                result += $"Walk Speed: +{WalkSpeed}\n";
            return result;
        }
    }
}