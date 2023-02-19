using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Character_1._0
{

	public class PlayerMovement : MonoBehaviour
	{

		public CharacterController2D controller;
		public Animator animator;

		public float runSpeed = 40f;

		private float horizontalMove = 0f;
		private bool jump = false;
		private bool dash = false;

		//bool dashAxis = false;

		// Update is called once per frame
		void Update()
		{

			// horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

			// if (Input.GetKeyDown(KeyCode.Z))
			// {
			// 	jump = true;
			// }
			//
			// if (Input.GetKeyDown(KeyCode.C))
			// {
			// 	dash = true;
			// }

			/*if (Input.GetAxisRaw("Dash") == 1 || Input.GetAxisRaw("Dash") == -1) //RT in Unity 2017 = -1, RT in Unity 2019 = 1
			{
				if (dashAxis == false)
				{
					dashAxis = true;
					dash = true;
				}
			}
			else
			{
				dashAxis = false;
			}
			*/

		}

		public void Jump()
		{
			jump = true;
		}

		public void Dash()
		{
			dash = true;
		}

		public void Move(float direction)
		{
			horizontalMove = direction * runSpeed;
		}
		
		public void OnFall()
		{
			animator.SetBool("IsJumping", true);
		}

		public void OnLanding()
		{
			animator.SetBool("IsJumping", false);
		}

		void FixedUpdate()
		{
			// Move our character
			controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
			jump = false;
			dash = false;
		}
	}
}