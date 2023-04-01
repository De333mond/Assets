using System;
using PlayerInventory.Scriptable;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Stats
{
    [Serializable]
    public class StatsSystem
    {
        public Stats Stats;
        [HideInInspector] public UnityEvent OnDeath;
        [HideInInspector] public UnityEvent OnStatsChanged;

        private Animator _animator;
        
        public void Init()
        {
            Stats.Health = Stats.MaxHealth;
            OnStatsChanged = new UnityEvent();
        }

        public AttackStats GetDamageWithWeapon(Weapon weapon)
        {
            AttackStats attackStats = Stats.attackStats + weapon.MainAttackStats;
            if (Random.value < (Stats.attackStats.criticalChance + weapon.MainAttackStats.criticalChance))
                attackStats *= Stats.attackStats.criticalMultiply + weapon.MainAttackStats.criticalMultiply;

            return attackStats;
        }

        public void Heal(float value)
        {
            Stats.Health = Mathf.Min(Stats.Health + value, Stats.MaxHealth);
        }

        public void TakeDamage(AttackStats attackStats)
        {
            if (Stats.IsInvincible)
                return;

            float damageMagnitude = attackStats * Stats.resistStats;
            Stats.Health -= damageMagnitude;
            if (Stats.Health < 0)
                OnDeath.Invoke();

            Debug.Log($"[Stats system]: total taken damage: {damageMagnitude}");
        }

        public AttackStats GetDamage()
        {
            AttackStats criticalAttackStats =
                Stats.attackStats * (Random.value <= Stats.attackStats.criticalChance ? Stats.attackStats.criticalMultiply : 1);
            return criticalAttackStats;
        }
    }

    [Serializable]
    public class Stats
    {
        [HideInInspector] public float Health;
        public float MaxHealth;
        [HideInInspector] public float Mana;
        public float MaxMana;
        
        public AttackStats attackStats;
        public ResistStats resistStats;
        [Space]
        
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
            a.MaxMana += b.MaxMana;
            a.attackStats += b.attackStats;
            a.resistStats += b.resistStats;
            a.WalkSpeed += b.WalkSpeed;
            return a;
        }

        public static Stats operator -(Stats a, Stats b)
        {
            a.MaxHealth -= b.MaxHealth;
            a.MaxMana -= b.MaxMana;
            a.attackStats -= b.attackStats;
            a.resistStats -= b.resistStats;
            a.WalkSpeed -= b.WalkSpeed;
            return a;
        }

        public virtual string ExtraInfo()
        {
            string result = "";
            if (MaxHealth > 0)
                result += $"MaxHealth: +{MaxHealth}\n"; 
            if (MaxMana > 0)
                result += $"MaxMana: +{MaxMana}\n";
            if (attackStats.Magnitude > 0)
                result += $"BaseDamage: +{attackStats.Magnitude}\n";
            if (resistStats.Magnitude > 0)
                result += $"Resistance magnitude: +{resistStats.Magnitude}\n";
            if (attackStats.criticalChance > 0)
                result += $"Critical Chance: +{attackStats.criticalChance * 100}%\n";
            if (attackStats.criticalMultiply > 0)
                result += $"Critical Multiplier: +{attackStats.criticalMultiply * 100}%\n";
            if (WalkSpeed > 0)
                result += $"Walk Speed: +{WalkSpeed}\n";
            return result;
        }
    }
}