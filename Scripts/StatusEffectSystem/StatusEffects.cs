using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffects
{
    [Serializable]
    private struct StatusEffect
    {
        [SerializeField] [Range(0, 1)] public float Chance;
        [SerializeField] public float Duration;

        public StatusEffect(float chance, float duration)
        {
            Chance = Mathf.Clamp(chance, 0, 1);
            Duration = Mathf.Clamp(duration, 0, Mathf.Infinity);
        }
        
        public static StatusEffect operator +(StatusEffect statusEffectA, StatusEffect statusEffectB)
        {
            statusEffectA.Chance += statusEffectB.Chance;
            statusEffectA.Duration += statusEffectB.Duration;

            return statusEffectA;
        }
        
        public static StatusEffect operator -(StatusEffect statusEffectA, StatusEffect statusEffectB)
        {
            statusEffectA.Chance -= statusEffectB.Chance;
            statusEffectA.Duration -= statusEffectB.Duration;

            return statusEffectA;
        }
    }

    [SerializeField] private StatusEffect fireStatusEffect;
    public float FireEffectChance => fireStatusEffect.Chance;
    public float FireEffectDuration => fireStatusEffect.Duration;
    
    
    [SerializeField] private StatusEffect frozenStatusEffect;
    public float FrozenEffectChance => frozenStatusEffect.Chance;
    public float FrozenEffectDuration => frozenStatusEffect.Duration;
    
    [SerializeField] private StatusEffect electricityStatusEffect;
    public float ElectricityEffectChance => electricityStatusEffect.Chance;
    public float ElectricityEffectDuration => electricityStatusEffect.Duration;
    
    [SerializeField] private StatusEffect poisonStatusEffect;
    public float PoisonEffectChance => poisonStatusEffect.Chance;
    public float PoisonEffectDuration => poisonStatusEffect.Duration;


    public StatusEffects()
    {
        fireStatusEffect = new StatusEffect(0f, 0f);
        frozenStatusEffect = new StatusEffect(0f, 0f);
        electricityStatusEffect = new StatusEffect(0f, 0f);
        poisonStatusEffect = new StatusEffect(0f, 0f);
    }

    public StatusEffects(float fireEffectChance, float fireEffectDuration, float frozenEffectChance, float frozenEffectDuration, float electricityEffectChance, float electricityEffectDuration, float poisonEffectChance, float poisonEffectDuration)
    {
        fireStatusEffect = new StatusEffect(fireEffectChance, fireEffectDuration);
        frozenStatusEffect = new StatusEffect(frozenEffectChance, frozenEffectDuration);
        electricityStatusEffect = new StatusEffect(electricityEffectChance, electricityEffectDuration);
        poisonStatusEffect = new StatusEffect(poisonEffectChance, poisonEffectDuration);
    }
    
    public static StatusEffects operator +(StatusEffects statusEffectsA, StatusEffects statusEffectsB)
    {
        statusEffectsA.fireStatusEffect += statusEffectsB.fireStatusEffect;
        statusEffectsA.frozenStatusEffect += statusEffectsB.frozenStatusEffect;
        statusEffectsA.electricityStatusEffect += statusEffectsB.electricityStatusEffect;
        statusEffectsA.poisonStatusEffect += statusEffectsB.poisonStatusEffect;

        return statusEffectsA;
    }
    
    public static StatusEffects operator -(StatusEffects statusEffectsA, StatusEffects statusEffectsB)
    {
        statusEffectsA.fireStatusEffect -= statusEffectsB.fireStatusEffect;
        statusEffectsA.frozenStatusEffect -= statusEffectsB.frozenStatusEffect;
        statusEffectsA.electricityStatusEffect -= statusEffectsB.electricityStatusEffect;
        statusEffectsA.poisonStatusEffect -= statusEffectsB.poisonStatusEffect;
        
        return statusEffectsA;
    }
}