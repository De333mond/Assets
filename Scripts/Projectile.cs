using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage;
    [SerializeField] private float _lifetime;

    private void Start()
    {
        StartCoroutine(RemoveAfterTime());
    }

    private IEnumerator RemoveAfterTime()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<Entity>().TakeDamage(damage);
        Destroy(gameObject);
    }
}