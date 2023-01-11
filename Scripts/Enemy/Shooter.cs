using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Arrow arrow;
    
    public void Shoot(Vector3 position, Vector3 direction, float damage)
    {
        Debug.Log("shoot");
        var instance = Instantiate(arrow.projectilePrefab, position, Quaternion.identity);
        if (direction.x < 0)
            instance.transform.localScale = new Vector3(-1, 1, 1);

        instance.GetComponent<Projectile>().damage = damage;
        var rb = instance.GetComponent<Rigidbody2D>();
        rb.velocity = direction * arrow.speed;
    }
}

[Serializable]
public struct Arrow
{
    public GameObject projectilePrefab;
    public float speed;
}


