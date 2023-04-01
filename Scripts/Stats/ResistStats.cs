using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResistStats
{
    [SerializeField] private float resistReduceCup; // 80% 
    [SerializeField] private float fullResistAmount; // 1000
    [Space]
    public float physicalResistance;
    public float fireResistance;
    public float waterResistance;
    public float airResistance;
    public float earthResistance;

    public float Magnitude => physicalResistance + fireResistance + waterResistance + airResistance + earthResistance;

    public ResistStats(float resistance)
    {
        resistReduceCup = 0.8f;
        fullResistAmount = 1000f;
        
        physicalResistance = resistance;
        fireResistance = resistance;
        waterResistance = resistance;
        airResistance = resistance;
        earthResistance = resistance;
    }
    
    public static ResistStats operator +(ResistStats resistStatsA, ResistStats resistStatsB)
    {
        resistStatsA.physicalResistance += resistStatsB.physicalResistance;
        resistStatsA.fireResistance += resistStatsB.fireResistance;
        resistStatsA.waterResistance += resistStatsB.waterResistance;
        resistStatsA.airResistance += resistStatsB.airResistance;
        resistStatsA.earthResistance += resistStatsB.earthResistance;
        
        return resistStatsA;
    }
    
    public static ResistStats operator -(ResistStats resistStatsA, ResistStats resistStatsB)
    {
        resistStatsA.physicalResistance -= resistStatsB.physicalResistance;
        resistStatsA.fireResistance -= resistStatsB.fireResistance;
        resistStatsA.waterResistance -= resistStatsB.waterResistance;
        resistStatsA.airResistance -= resistStatsB.airResistance;
        resistStatsA.earthResistance -= resistStatsB.earthResistance;
        
        return resistStatsA;
    }
    
    public static ResistStats operator *(ResistStats resistStats, float scale)
    {
        resistStats.physicalResistance *= scale;
        resistStats.fireResistance *= scale;
        resistStats.waterResistance *= scale;
        resistStats.airResistance *= scale;
        resistStats.earthResistance *= scale;
        
        return resistStats;
    }
    
    public static float operator *(AttackStats attackStats, ResistStats resistStats)
    {
        float multiplier = 1;
        multiplier = 1 - resistStats.physicalResistance / resistStats.fullResistAmount;
        multiplier = ((1 - multiplier) > resistStats.resistReduceCup) ? (1 - resistStats.resistReduceCup) : multiplier;
        float physicalDamage = attackStats.physicalDamage* multiplier;
        
        multiplier = 1 - resistStats.fireResistance / resistStats.fullResistAmount;
        multiplier =  ((1 - multiplier) > resistStats.resistReduceCup) ? (1 - resistStats.resistReduceCup) : multiplier;
        float fireDamage = attackStats.fireDamage* multiplier;
        
        multiplier = 1 - resistStats.waterResistance / resistStats.fullResistAmount;
        multiplier =  ((1 - multiplier) > resistStats.resistReduceCup) ? (1 - resistStats.resistReduceCup) : multiplier;
        float waterDamage = attackStats.waterDamage* multiplier;
        
        multiplier = 1 - resistStats.airResistance / resistStats.fullResistAmount;
        multiplier =  ((1 - multiplier) > resistStats.resistReduceCup) ? (1 - resistStats.resistReduceCup) : multiplier;
        float airDamage = attackStats.airDamage* multiplier;
        
        multiplier = 1 - resistStats.earthResistance / resistStats.fullResistAmount;
        multiplier =  ((1 - multiplier) > resistStats.resistReduceCup) ? (1 - resistStats.resistReduceCup) : multiplier;
        float earthDamage = attackStats.earthDamage* multiplier;
        
        return physicalDamage + fireDamage + waterDamage + airDamage + earthDamage;
    }
}