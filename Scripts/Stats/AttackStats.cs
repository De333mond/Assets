using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalStatsSystem
{
    [System.Serializable]
    public struct AttackStats
    {
        public float physicalDamage;
        public float fireDamage;
        public float waterDamage;
        public float airDamage;
        public float earthDamage;
        public float electricityDamage;
        public float poisonDamage;

        [Space] public float criticalChance;
        public float criticalMultiply;
        [Space] public float attackCooldown;
        [Space] public float attackRange;

        [Space] public StatusEffects statusEffects;

        public float Magnitude => physicalDamage + fireDamage + waterDamage + airDamage + earthDamage +
                                  electricityDamage + poisonDamage;

        public AttackStats(float damage)
        {
            physicalDamage = damage;
            fireDamage = damage;
            waterDamage = damage;
            airDamage = damage;
            earthDamage = damage;
            electricityDamage = damage;
            poisonDamage = damage;
            criticalChance = 0;
            criticalMultiply = 0;
            attackCooldown = 1;
            attackRange = 1;

            statusEffects = new StatusEffects();
        }

        public AttackStats(float physicalDamage, float fireDamage, float waterDamage, float airDamage,
            float earthDamage, float electricityDamage, float poisonDamage)
        {
            this.physicalDamage = physicalDamage;
            this.fireDamage = fireDamage;
            this.waterDamage = waterDamage;
            this.airDamage = airDamage;
            this.earthDamage = earthDamage;
            this.electricityDamage = electricityDamage;
            this.poisonDamage = poisonDamage;
            criticalChance = 0;
            criticalMultiply = 0;
            attackCooldown = 1;
            attackRange = 1;

            statusEffects = new StatusEffects();
        }

        public static AttackStats operator +(AttackStats attackStatsA, AttackStats attackStatsB)
        {
            attackStatsA.physicalDamage += attackStatsB.physicalDamage;
            attackStatsA.fireDamage += attackStatsB.fireDamage;
            attackStatsA.waterDamage += attackStatsB.waterDamage;
            attackStatsA.airDamage += attackStatsB.airDamage;
            attackStatsA.earthDamage += attackStatsB.earthDamage;
            attackStatsA.electricityDamage += attackStatsB.electricityDamage;
            attackStatsA.poisonDamage += attackStatsB.poisonDamage;

            attackStatsA.criticalChance += attackStatsB.criticalChance;
            attackStatsA.criticalMultiply += attackStatsB.criticalMultiply;
            attackStatsA.attackCooldown += attackStatsB.attackCooldown;
            attackStatsA.attackRange += attackStatsB.attackRange;
            attackStatsA.statusEffects += attackStatsB.statusEffects;

            return attackStatsA;
        }

        public static AttackStats operator -(AttackStats attackStatsA, AttackStats attackStatsB)
        {
            attackStatsA.physicalDamage -= attackStatsB.physicalDamage;
            attackStatsA.fireDamage -= attackStatsB.fireDamage;
            attackStatsA.waterDamage -= attackStatsB.waterDamage;
            attackStatsA.airDamage -= attackStatsB.airDamage;
            attackStatsA.earthDamage -= attackStatsB.earthDamage;
            attackStatsA.electricityDamage -= attackStatsB.electricityDamage;
            attackStatsA.poisonDamage -= attackStatsB.poisonDamage;

            attackStatsA.criticalChance -= attackStatsB.criticalChance;
            attackStatsA.criticalMultiply -= attackStatsB.criticalMultiply;
            attackStatsA.attackCooldown -= attackStatsB.attackCooldown;
            attackStatsA.attackRange -= attackStatsB.attackRange;
            attackStatsA.statusEffects -= attackStatsB.statusEffects;


            return attackStatsA;
        }

        public static AttackStats operator *(AttackStats attackStats, float scale)
        {
            attackStats.physicalDamage *= scale;
            attackStats.fireDamage *= scale;
            attackStats.waterDamage *= scale;
            attackStats.airDamage *= scale;
            attackStats.earthDamage *= scale;
            attackStats.electricityDamage *= scale;
            attackStats.poisonDamage *= scale;

            return attackStats;
        }
    }
}