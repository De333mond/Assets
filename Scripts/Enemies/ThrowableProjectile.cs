﻿using Character;
using UnityEngine;
using UniversalStatsSystem;

namespace Enemies
{
	public class ThrowableProjectile : MonoBehaviour
	{
		public Vector2 direction;
		public bool hasHit = false;
		public float speed = 15f;
		public GameObject owner;

		// Update is called once per frame
		void FixedUpdate()
		{
			if (!hasHit)
				GetComponent<Rigidbody2D>().velocity = direction * speed;
		}

		void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.tag == "Player")
			{
				collision.gameObject.GetComponent<Player>().TakeDamage(new AttackStats(2), transform.position);
				Destroy(gameObject);
			}
			else if (owner != null && collision.gameObject != owner && collision.gameObject.tag == "Enemy")
			{
				collision.gameObject.SendMessage("TakeDamage", Mathf.Sign(direction.x) * 2f);
				Destroy(gameObject);
			}
			else if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Player")
			{
				Destroy(gameObject);
			}
		}
	}
}