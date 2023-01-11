using System;
using System.Collections;
using New_Folder.Healthbar;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] protected float maxHealth = 10;
    [SerializeField] protected HealthBar _healthBar;
    [SerializeField] protected Animator _animator;
    private float _deathCooldown = 0.5f;


    protected float Health;
    protected bool isAlive = true;

    public bool IsAlive => isAlive;
    public string _name;
    public UnityEvent DeathEvent;

    private void Awake()
    {
        _healthBar?.SetMaxHealth(maxHealth);
        Health = maxHealth;
        _healthBar?.SetHealth(Health);
        DeathEvent = new UnityEvent();
    }

    public void TakeDamage(float damage)
    {
        if (isAlive)
        {
            _animator.SetTrigger("Hurt");
            Health -= damage;
            Debug.Log($"{name} recieved {damage} damage! Health: {Health}");

            _healthBar?.SetHealth(Health);

            if (Health <= 0)
            {
                Die();
            }
        }
    }

    public void AddHealth(float addedHealth)
    {
        Health += addedHealth;
        if (Health > maxHealth)
            Health = maxHealth;
        _healthBar?.SetHealth(Health);
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