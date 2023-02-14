using System;
using Inventory;
using Stats_system;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class Player : Entity
{
    public PlayerInventory Inventory;
    public Stats Stats => entityStats;

    private void Awake()
    {
        _healthBar?.SetMaxHealth(entityStats.MaxHealth);
        entityStats.Health = entityStats.MaxHealth;
        _healthBar?.SetHealth(entityStats.Health);
        Inventory = new PlayerInventory(8, this);
    }
}



