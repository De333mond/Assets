using UnityEngine;

namespace Character
{

	public class PlayerMovement : CharacterController2D
	{
		[Header("PlayerMovement")] [Space]
		
		public float runSpeed = 40f;

		private float horizontalMove = 0f;
		private bool jump = false;
		private bool dash = false;

		//bool dashAxis = false;

		protected override void OnAwake()
		{
			base.OnAwake();
			
			OnFallEvent.AddListener(OnFall);
			OnLandEvent.AddListener(OnLanding);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			
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

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();
			
			// Move our character
			Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
			jump = false;
			dash = false;
		}
	}
}