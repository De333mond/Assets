using System;
using System.Collections;
using New_Folder.Healthbar;
using Stats_system;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour
{
    [Header("Entity")] 
    [SerializeField] protected Stats entityStats;
    [SerializeField] protected HealthBar _healthBar;
    [SerializeField] protected Animator _animator;
    private float _deathCooldown = 0.5f;


    protected bool isAlive = true;

    public bool IsAlive => isAlive;
    public string _name;
    public UnityEvent DeathEvent;

    private void Awake()
    {
        _healthBar?.SetMaxHealth(entityStats.MaxHealth);
        _healthBar?.SetHealth(entityStats.Health);
        DeathEvent = new UnityEvent();
    }

    public void TakeDamage(float damage)
    {
        if (isAlive && damage >= 0)
        {
            damage *= entityStats.ArmorReduceMultiplier;
            
            _animator.SetTrigger("Hurt");
            entityStats.Health -= damage;
            Debug.Log($"{name} recieved {damage} damage reduced by *{entityStats.ArmorReduceMultiplier}! Health: {entityStats.Health}");

            _healthBar?.SetHealth(entityStats.Health);

            if (entityStats.Health <= 0)
            {
                Die();
            }
        }
    }

    public void AddHealth(float addedHealth)
    {
        entityStats.Health += addedHealth;
        
        if (entityStats.Health > entityStats.MaxHealth)
            entityStats.Health = entityStats.MaxHealth;
        
        _healthBar?.SetHealth(entityStats.Health);
    }

    private void Die()
    {
        isAlive = false;
        DeathEvent.Invoke();
        _animator.SetTrigger("Dead");
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(_deathCooldown);
        Destroy(gameObject);
    }
}