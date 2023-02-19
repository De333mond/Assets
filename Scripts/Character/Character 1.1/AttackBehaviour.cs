using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Character_1._0
{ 
	public class AttackBehaviour : MonoBehaviour
	{
		public float dmgValue = 4;
		public GameObject throwableObject;
		public Transform attackCheck;
		private Rigidbody2D m_Rigidbody2D;
		public Animator animator;
		public bool canAttack = true;
		public bool isTimeToCheck = false;

		public GameObject cam;
		private int _attackVariantCycle = 0;
		const int attacksCount = 4;

		private void Awake()
		{
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
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

		public void Attack()
		{
			if (!canAttack)
				return;
			
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

		public void DoDashDamage()
		{
			dmgValue = Mathf.Abs(dmgValue);
			Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
			for (int i = 0; i < collidersEnemies.Length; i++)
			{
				if (collidersEnemies[i].gameObject.tag == "Enemy")
				{
					if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
					{
						dmgValue = -dmgValue;
					}

					collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
					cam.GetComponent<CameraFollow>().ShakeCamera();
				}
			}
		}
	}
}