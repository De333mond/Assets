using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{ 
	public class PlayerAttack : MonoBehaviour
	{
		// public float dmgValue = 4;
		[SerializeField] private float _attackRange = 1f;
		
		
		public GameObject throwableObject;
		public Transform attackCheck;
		private Rigidbody2D m_Rigidbody2D;
		public Animator animator;
		public bool canAttack = true;
		public bool isTimeToCheck = false;

		public GameObject cam;
		
		private int _attackVariantCycle = 0;
		const int attacksCount = 4;
		private Player _player;
		
		

		private void Awake()
		{
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			_player = GetComponent<Player>();
		}
		public void ThrowWeapon()
		{
			GameObject throwableWeapon = Instantiate(throwableObject,
				transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f),
				Quaternion.identity) as GameObject;
			Vector2 direction = new Vector2(transform.localScale.x, 0);
			throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
			throwableWeapon.name = "ThrowableWeapon";
		}

		
		//TODO: make it possible to damage enemies
		public void Attack()
		{
			if (!canAttack || _player.Inventory.ActiveWeapon is null)
				return;

			float damage = _player.GetDamage();

			Collider2D[] targets = Physics2D.OverlapCircleAll(attackCheck.position, _attackRange);
			foreach (var target in targets)
			{
				if (target.transform.CompareTag("Enemy"))
					target.GetComponent<Enemy>().TakeDamage(damage);
			}
			
			canAttack = false;
			animator.SetBool("IsAttacking", true);
			animator.SetInteger("numAttack", _attackVariantCycle++ % attacksCount);
			StartCoroutine(AttackCooldown());
		}

		IEnumerator AttackCooldown()
		{
			yield return new WaitForSeconds(0.4f);
			canAttack = true;
		}

		//TODO: make damaged dash later...
		
		// public void DoDashDamage()
		// {
		// 	dmgValue = Mathf.Abs(dmgValue);
		// 	Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		// 	for (int i = 0; i < collidersEnemies.Length; i++)
		// 	{
		// 		if (collidersEnemies[i].gameObject.tag == "Enemy")
		// 		{
		// 			if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
		// 			{
		// 				dmgValue = -dmgValue;
		// 			}
		//
		// 			collidersEnemies[i].gameObject.SendMessage("TakeDamage", dmgValue);
		// 			cam.GetComponent<CameraFollow>().ShakeCamera();
		// 		}
		// 	}
		// }
	}
}