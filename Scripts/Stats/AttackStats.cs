using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackStats
{
    public float physicalDamage;
    public float fireDamage;
    public float waterDamage;
    public float airDamage;
    public float earthDamage;
    [Space]
    public float criticalChance;
    public float criticalMultiply;
    [Space]
    public float attackSpeed;
    [Space]
    public float attackRange;
    
    public float Magnitude => physicalDamage + fireDamage + waterDamage + airDamage + earthDamage;

    public AttackStats(float damage)
    {
        physicalDamage = damage;
        fireDamage = damage;
        waterDamage = damage;
        airDamage = damage;
        earthDamage = damage;
        criticalChance = 0;
        criticalMultiply = 0;
        attackSpeed = 1;
        attackRange = 1;
    }
    
    public static AttackStats operator +(AttackStats attackStatsA, AttackStats attackStatsB)
    {
        attackStatsA.physicalDamage += attackStatsB.physicalDamage;
        attackStatsA.fireDamage += attackStatsB.fireDamage;
        attackStatsA.waterDamage += attackStatsB.waterDamage;
        attackStatsA.airDamage += attackStatsB.airDamage;
        attackStatsA.earthDamage += attackStatsB.earthDamage;
        attackStatsA.criticalChance += attackStatsB.criticalChance;
        attackStatsA.criticalMultiply += attackStatsB.criticalMultiply;
        attackStatsA.attackSpeed += attackStatsB.attackSpeed;
        attackStatsA.attackRange += attackStatsB.attackRange;
        
        return attackStatsA;
    }
    
    public static AttackStats operator -(AttackStats attackStatsA, AttackStats attackStatsB)
    {
        attackStatsA.physicalDamage -= attackStatsB.physicalDamage;
        attackStatsA.fireDamage -= attackStatsB.fireDamage;
        attackStatsA.waterDamage -= attackStatsB.waterDamage;
        attackStatsA.airDamage -= attackStatsB.airDamage;
        attackStatsA.earthDamage -= attackStatsB.earthDamage;
        attackStatsA.criticalChance -= attackStatsB.criticalChance;
        attackStatsA.criticalMultiply -= attackStatsB.criticalMultiply;
        attackStatsA.attackSpeed -= attackStatsB.attackSpeed;
        attackStatsA.attackRange -= attackStatsB.attackRange;

        return attackStatsA;
    }
    
    public static AttackStats operator *(AttackStats attackStats, float scale)
    {
        attackStats.physicalDamage *= scale;
        attackStats.fireDamage *= scale;
        attackStats.waterDamage *= scale;
        attackStats.airDamage *= scale;
        attackStats.earthDamage *= scale;
        
        return attackStats;
    }
}