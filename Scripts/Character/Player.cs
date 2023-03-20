using System;
using Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class Player : Entity
{
    

    public PlayerInventory Inventory;
    private void Awake()
    {
        _healthBar?.SetMaxHealth(maxHealth);
        Health = maxHealth;
        _healthBar?.SetHealth(Health);

        Inventory = new PlayerInventory(8, this);
    }

    
    
    private void Update()
    {
        
    }
}



