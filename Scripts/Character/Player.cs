using System;
using Ability_system;
using Inventory;
using Stats_system;
using Stats_System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class Player : Entity
{ 
    public float Damage => _abilitySystem.GetDamage();
    public Stats Stats => _abilitySystem.Stats;
    public PlayerInventory Inventory;
    
    protected override void Awake()
    {
        base.Awake();
        Inventory = new PlayerInventory(8, this);
        Inventory.OnWeaponChanged.AddListener(item => _abilitySystem.SetWeapon(item));
    }
    
    
}



