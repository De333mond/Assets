using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
	[System.Serializable]
	public class BoolEvent : UnityEvent<bool>
	{
		
	}
	
	public class CharacterController2D : CharacterBase
	{
		[Header("CharacterController2D")] [Space]

		[SerializeField] private float m_JumpForce = 10f; // Amount of force added when the player jumps.
		[SerializeField] private float _gravityForce = -9.8f;
		[SerializeField] [Range(0, 5f)] private float _gravityScale = 1f;
		[Space]
		

		[SerializeField] [Range(0, .3f)]
		private float m_MovementSmoothing = .05f; // How much to smooth out the movement

		[SerializeField] private bool m_AirControl = false; // Whether or not a player can steer while jumping;
		[SerializeField] private LayerMask m_WhatIsGround; // A mask determining what is ground to the character

		[SerializeField]
		private Transform m_GroundCheck; // A position marking where to check if the player is grounded.

		[SerializeField] private Transform m_WallCheck; //Posicion que controla si el personaje toca una pared

		const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
		private bool m_Grounded; // Whether or not the player is grounded.
		private Rigidbody2D m_Rigidbody2D;
		private bool m_FacingRight = true; // For determining which way the player is currently facing.
		private Vector3 velocity = Vector3.zero;
		private float limitFallSpeed = 25f; // Limit fall speed

		public bool canDoubleJump = true; //If player can double jump
		[SerializeField] private float m_DashForce = 25f;
		private bool canDash = true;
		private bool isDashing = false; //If player is dashing
		private bool m_IsWall = false; //If there is a wall in front of the player
		private bool isWallSliding = false; //If player is sliding in a wall
		private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
		private float prevVelocityX = 0f;
		private bool canCheck = false; //For check if player is wallsliding

		// public float life = 10f; //Life of the player
		// public bool invincible = false; //If player can die
		private bool canMove = true; //If player can move

		[SerializeField] protected Animator animator;

		public ParticleSystem particleJumpUp; //Trail particles
		public ParticleSystem particleJumpDown; //Explosion particles

		private float jumpWallStartX = 0;
		private float jumpWallDistX = 0; //Distance between player and wall
		private bool limitVelOnWallJump = false; //For limit wall jump distance with low fps
		const float doubleJumpScaleModifier = .8f; //For reduce jump force doing second jump

		protected UnityEvent OnFallEvent;
		protected UnityEvent OnLandEvent;

		private Player _player;
		
		protected override void OnAwake()
		{
			base.OnAwake();
			
			if (gameObject.layer == m_WhatIsGround)
				Debug.Log("Attention!: player layer == ground layer");
			
			if (!m_Rigidbody2D)
				m_Rigidbody2D = GetComponent<Rigidbody2D>();
			
			if (!animator)
				animator = GetComponent<Animator>();

			if (OnFallEvent == null)
				OnFallEvent = new UnityEvent();

			if (OnLandEvent == null)
				OnLandEvent = new UnityEvent();

			_player = GetComponent<Player>();
		}

		
		protected override void OnFixedUpdate()
		{
			GroundCheck();
			WallsAndFallCheck();
			LimitVelOnWallJump();
		}
		
		private void GroundCheck()
		{
			bool groundContact = Physics2D.CircleCast(m_GroundCheck.position, k_GroundedRadius, 
				Vector2.zero,0f, m_WhatIsGround);
			
			if(groundContact && !m_Grounded){
				OnLandEvent.Invoke();
				if (!m_IsWall && !isDashing)
					particleJumpDown.Play();
				canDoubleJump = true;
				if (m_Rigidbody2D.velocity.y < 0f)
					limitVelOnWallJump = false;
			}
			
			m_Grounded = groundContact;
		}
		
		private void WallsAndFallCheck()
		{
			m_IsWall = false;
			if (!m_Grounded)
			{
				OnFallEvent.Invoke();
				
				prevVelocityX = m_Rigidbody2D.velocity.x;

				m_Rigidbody2D.velocity += new Vector2(0, _gravityForce * _gravityScale * Time.fixedDeltaTime);
				
				bool wallContact = Physics2D.CircleCast(m_WallCheck.position, k_GroundedRadius, 
					Vector2.zero,0f, m_WhatIsGround);
				
				m_IsWall = wallContact;
				if (m_IsWall) isDashing = false;
			}
		}

		private void LimitVelOnWallJump()
		{
			if (limitVelOnWallJump)
			{
				if (m_Rigidbody2D.velocity.y < -0.5f)
					limitVelOnWallJump = false;
				jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
				if (jumpWallDistX < -0.5f && jumpWallDistX > -1f)
				{
					canMove = true;
				}
				else if (jumpWallDistX < -1f && jumpWallDistX >= -2f)
				{
					canMove = true;
					m_Rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, m_Rigidbody2D.velocity.y);
				}
				else if (jumpWallDistX < -2f || jumpWallDistX > 0)
				{
					limitVelOnWallJump = false;
					m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
				}
			}
		}

		protected void Move(float move, bool jump, bool dash)
		{
			if (canMove)
			{
				if (dash && canDash && !isWallSliding)
				{
					//m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
					StartCoroutine(DashCooldown());
				}

				// If crouching, check to see if the character can stand up
				if (isDashing)
				{
					m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
				}
				//only control the player if grounded or airControl is turned on
				else if (m_Grounded || m_AirControl)
				{
					if (m_Rigidbody2D.velocity.y < -limitFallSpeed)
						m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);
					// Move the character by finding the target velocity
					Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
					// And then smoothing it out and applying it to the character
					m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity,
						m_MovementSmoothing);

					// If the input is moving the player right and the player is facing left...
					if (move > 0 && !m_FacingRight && !isWallSliding)
					{
						// ... flip the player.
						Flip();
					}
					// Otherwise if the input is moving the player left and the player is facing right...
					else if (move < 0 && m_FacingRight && !isWallSliding)
					{
						// ... flip the player.
						Flip();
					}
				}

				// If the player should jump...
				if (m_Grounded && jump)
				{
					// Add a vertical force to the player.
					animator.SetBool("IsJumping", true);
					animator.SetBool("JumpUp", true);
					m_Grounded = false;
					// m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
					m_Rigidbody2D.velocity += new Vector2(0f, m_JumpForce);
					canDoubleJump = true;
					particleJumpDown.Play();
					particleJumpUp.Play();
				}
				else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
				{
					canDoubleJump = false;
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
					// m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
					m_Rigidbody2D.velocity += new Vector2(0f, m_JumpForce * doubleJumpScaleModifier);
					animator.SetBool("IsDoubleJumping", true);
				}

				else if (m_IsWall && !m_Grounded)
				{
					if (!oldWallSlidding && m_Rigidbody2D.velocity.y < 0 || isDashing)
					{
						isWallSliding = true;
						m_WallCheck.localPosition =
							new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
						Flip();
						StartCoroutine(WaitToCheck(0.1f));
						canDoubleJump = true;
						animator.SetBool("IsWallSliding", true);
					}

					isDashing = false;

					if (isWallSliding)
					{
						if (move * transform.localScale.x > 0.1f)
						{
							StartCoroutine(WaitToEndSliding());
						}
						else
						{
							oldWallSlidding = true;
							m_Rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, -5);
						}
					}

					if (jump && isWallSliding)
					{
						animator.SetBool("IsJumping", true);
						animator.SetBool("JumpUp", true);
						m_Rigidbody2D.velocity = new Vector2(0f, 0f);
						m_Rigidbody2D.velocity += new Vector2(transform.localScale.x * m_JumpForce * doubleJumpScaleModifier, m_JumpForce);
						jumpWallStartX = transform.position.x;
						limitVelOnWallJump = true;
						canDoubleJump = true;
						isWallSliding = false;
						animator.SetBool("IsWallSliding", false);
						oldWallSlidding = false;
						m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x),
							m_WallCheck.localPosition.y, 0);
						canMove = false;
					}
					else if (dash && canDash)
					{
						isWallSliding = false;
						animator.SetBool("IsWallSliding", false);
						oldWallSlidding = false;
						m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x),
							m_WallCheck.localPosition.y, 0);
						canDoubleJump = true;
						StartCoroutine(DashCooldown());
					}
				}
				else if (isWallSliding && !m_IsWall && canCheck)
				{
					isWallSliding = false;
					animator.SetBool("IsWallSliding", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x),
						m_WallCheck.localPosition.y, 0);
					canDoubleJump = true;
				}
			}
		}
		
		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			m_FacingRight = !m_FacingRight;

			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		protected void TakeDamage(Vector3 position)
		{
			animator.SetBool("Hit", true);
				
			Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.AddForce(damageDir * 10);
			
			StartCoroutine(Stun(0.25f));
			StartCoroutine(MakeInvincible(1f));
		}

		IEnumerator DashCooldown()
		{
			animator.SetBool("IsDashing", true);
			isDashing = true;
			canDash = false;
			yield return new WaitForSeconds(0.1f);
			isDashing = false;
			yield return new WaitForSeconds(0.5f);
			canDash = true;
		}

		IEnumerator Stun(float time)
		{
			canMove = false;
			yield return new WaitForSeconds(time);
			canMove = true;
		}

		IEnumerator MakeInvincible(float time)
		{
			Invincible = true;
			yield return new WaitForSeconds(time);
			Invincible = false;
		}

		IEnumerator WaitToMove(float time)
		{
			canMove = false;
			yield return new WaitForSeconds(time);
			canMove = true;
		}

		IEnumerator WaitToCheck(float time)
		{
			canCheck = false;
			yield return new WaitForSeconds(time);
			canCheck = true;
		}

		IEnumerator WaitToEndSliding()
		{
			yield return new WaitForSeconds(0.1f);
			canDoubleJump = true;
			isWallSliding = false;
			animator.SetBool("IsWallSliding", false);
			oldWallSlidding = false;
			m_WallCheck.localPosition =
				new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
		}

		protected IEnumerator WaitToDead()
		{
			animator.SetBool("IsDead", true);
			canMove = false;
			Invincible = true;
			_player.CanAttack = false;
			yield return new WaitForSeconds(0.4f);
			m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			yield return new WaitForSeconds(1.1f);
			Destroy(gameObject);
			// SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
		}
	}
}