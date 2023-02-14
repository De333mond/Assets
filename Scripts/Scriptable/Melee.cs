using System.Collections;
using System.Collections.Generic;
using Scriptable;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Items/Weapon/Melee")]
public class Melee : Weapon
{
    [SerializeField] private float _attackRange;
    public float AttackRange => _attackRange;

    public override string ExtraInfo => $"Damge: {Stats.Damage}\n" +
                                        $"Attack time: {Stats.Cooldown}s\n" +
                                        $"Attack Range: {_attackRange}\n";
}

