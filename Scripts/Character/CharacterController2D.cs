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

		[SerializeField] private bool canDoubleJump = true; //If player can double jump
		[SerializeField] private float jumpForce = 10f; // Amount of force added when the player jumps.
		[SerializeField] private float gravityForce = -9.8f;
		[SerializeField] [Range(0, 5f)] private float gravityScale = 1f;

		[Space]
		[SerializeField] [Range(0, .3f)] private float movementSmoothing = .05f; // How much to smooth out the movement
		[SerializeField] private float dashForce = 25f;

		[SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
		[SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character

		[SerializeField] private Transform groundCheck; // A position marking where to check if the player is grounded.
		[SerializeField] private Transform wallCheck; //Posicion que controla si el personaje toca una pared

		[Space]
		[SerializeField] protected Animator animator;
		
		[Space]
		public ParticleSystem particleJumpUp; //Trail particles
		public ParticleSystem particleJumpDown; //Explosion particles
		
		const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
		private bool _grounded; // Whether or not the player is grounded.
		private Rigidbody2D _rigidbody2D;
		private bool _facingRight = true; // For determining which way the player is currently facing.
		private Vector3 _velocity = Vector3.zero;
		private float _limitFallSpeed = 25f; // Limit fall speed

		private bool _canDash = true;
		private bool _isDashing = false; //If player is dashing
		private bool _isWall = false; //If there is a wall in front of the player
		private bool _isWallSliding = false; //If player is sliding in a wall
		private bool _oldWallSliding = false; //If player is sliding in a wall in the previous frame
		private float _prevVelocityX = 0f;
		private bool _canCheck = false; //For check if player is wallsliding

		private bool _canMove = true; //If player can move
		
		private float _jumpWallStartX = 0;
		private float _jumpWallDistX = 0; //Distance between player and wall
		private bool _limitVelOnWallJump = false; //For limit wall jump distance with low fps
		const float DoubleJumpScaleModifier = .8f; //For reduce jump force doing second jump

		protected UnityEvent OnFallEvent;
		protected UnityEvent OnLandEvent;

		private Player _player;
		
		protected override void OnAwake()
		{
			base.OnAwake();
			
			if (gameObject.layer == whatIsGround)
				Debug.Log("Attention!: player layer == ground layer");
			
			if (!_rigidbody2D)
				_rigidbody2D = GetComponent<Rigidbody2D>();
			
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
			bool groundContact = Physics2D.CircleCast(groundCheck.position, GroundedRadius, 
				Vector2.zero,0f, whatIsGround);
			
			if(groundContact && !_grounded){
				OnLandEvent.Invoke();
				if (!_isWall && !_isDashing)
					particleJumpDown.Play();
				canDoubleJump = true;
				if (_rigidbody2D.velocity.y < 0f)
					_limitVelOnWallJump = false;
			}
			
			_grounded = groundContact;
		}
		
		private void WallsAndFallCheck()
		{
			_isWall = false;
			if (!_grounded)
			{
				OnFallEvent.Invoke();
				
				_prevVelocityX = _rigidbody2D.velocity.x;

				_rigidbody2D.velocity += new Vector2(0, gravityForce * gravityScale * Time.fixedDeltaTime);
				
				bool wallContact = Physics2D.CircleCast(wallCheck.position, GroundedRadius, 
					Vector2.zero,0f, whatIsGround);
				
				_isWall = wallContact;
				if (_isWall) _isDashing = false;
			}
		}

		private void LimitVelOnWallJump()
		{
			if (_limitVelOnWallJump)
			{
				if (_rigidbody2D.velocity.y < -0.5f)
					_limitVelOnWallJump = false;
				_jumpWallDistX = (_jumpWallStartX - transform.position.x) * transform.localScale.x;
				if (_jumpWallDistX < -0.5f && _jumpWallDistX > -1f)
				{
					_canMove = true;
				}
				else if (_jumpWallDistX < -1f && _jumpWallDistX >= -2f)
				{
					_canMove = true;
					_rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, _rigidbody2D.velocity.y);
				}
				else if (_jumpWallDistX < -2f || _jumpWallDistX > 0)
				{
					_limitVelOnWallJump = false;
					_rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
				}
			}
		}

		protected void Move(float move, bool jump, bool dash)
		{
			if (_canMove)
			{
				if (dash && _canDash && !_isWallSliding)
				{
					//m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
					StartCoroutine(DashCooldown());
				}

				// If crouching, check to see if the character can stand up
				if (_isDashing)
				{
					_rigidbody2D.velocity = new Vector2(transform.localScale.x * dashForce, 0);
				}
				//only control the player if grounded or airControl is turned on
				else if (_grounded || airControl)
				{
					if (_rigidbody2D.velocity.y < -_limitFallSpeed)
						_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_limitFallSpeed);
					// Move the character by finding the target velocity
					Vector3 targetVelocity = new Vector2(move * 10f, _rigidbody2D.velocity.y);
					// And then smoothing it out and applying it to the character
					_rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity,
						movementSmoothing);

					// If the input is moving the player right and the player is facing left...
					if (move > 0 && !_facingRight && !_isWallSliding)
					{
						// ... flip the player.
						Flip();
					}
					// Otherwise if the input is moving the player left and the player is facing right...
					else if (move < 0 && _facingRight && !_isWallSliding)
					{
						// ... flip the player.
						Flip();
					}
				}

				// If the player should jump...
				if (_grounded && jump)
				{
					// Add a vertical force to the player.
					animator.SetBool("IsJumping", true);
					animator.SetBool("JumpUp", true);
					_grounded = false;
					// m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
					_rigidbody2D.velocity += new Vector2(0f, jumpForce);
					canDoubleJump = true;
					particleJumpDown.Play();
					particleJumpUp.Play();
				}
				else if (!_grounded && jump && canDoubleJump && !_isWallSliding)
				{
					canDoubleJump = false;
					_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
					// m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
					_rigidbody2D.velocity += new Vector2(0f, jumpForce * DoubleJumpScaleModifier);
					animator.SetBool("IsDoubleJumping", true);
				}

				else if (_isWall && !_grounded)
				{
					if (!_oldWallSliding && _rigidbody2D.velocity.y < 0 || _isDashing)
					{
						_isWallSliding = true;
						wallCheck.localPosition =
							new Vector3(-wallCheck.localPosition.x, wallCheck.localPosition.y, 0);
						Flip();
						StartCoroutine(WaitToCheck(0.1f));
						canDoubleJump = true;
						animator.SetBool("IsWallSliding", true);
					}

					_isDashing = false;

					if (_isWallSliding)
					{
						if (move * transform.localScale.x > 0.1f)
						{
							StartCoroutine(WaitToEndSliding());
						}
						else
						{
							_oldWallSliding = true;
							_rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, -5);
						}
					}

					if (jump && _isWallSliding)
					{
						animator.SetBool("IsJumping", true);
						animator.SetBool("JumpUp", true);
						_rigidbody2D.velocity = new Vector2(0f, 0f);
						_rigidbody2D.velocity += new Vector2(transform.localScale.x * jumpForce * DoubleJumpScaleModifier, jumpForce);
						_jumpWallStartX = transform.position.x;
						_limitVelOnWallJump = true;
						canDoubleJump = true;
						_isWallSliding = false;
						animator.SetBool("IsWallSliding", false);
						_oldWallSliding = false;
						wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x),
							wallCheck.localPosition.y, 0);
						_canMove = false;
					}
					else if (dash && _canDash)
					{
						_isWallSliding = false;
						animator.SetBool("IsWallSliding", false);
						_oldWallSliding = false;
						wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x),
							wallCheck.localPosition.y, 0);
						canDoubleJump = true;
						StartCoroutine(DashCooldown());
					}
				}
				else if (_isWallSliding && !_isWall && _canCheck)
				{
					_isWallSliding = false;
					animator.SetBool("IsWallSliding", false);
					_oldWallSliding = false;
					wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x),
						wallCheck.localPosition.y, 0);
					canDoubleJump = true;
				}
			}
		}
		
		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			_facingRight = !_facingRight;

			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		protected void TakeDamage(Vector3 position)
		{
			animator.SetBool("Hit", true);
				
			Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;
			_rigidbody2D.velocity = Vector2.zero;
			_rigidbody2D.AddForce(damageDir * 10);
			
			StartCoroutine(Stun(0.25f));
			StartCoroutine(MakeInvincible(1f));
		}

		IEnumerator DashCooldown()
		{
			animator.SetBool("IsDashing", true);
			_isDashing = true;
			_canDash = false;
			yield return new WaitForSeconds(0.1f);
			_isDashing = false;
			yield return new WaitForSeconds(0.5f);
			_canDash = true;
		}

		IEnumerator Stun(float time)
		{
			_canMove = false;
			yield return new WaitForSeconds(time);
			_canMove = true;
		}

		IEnumerator MakeInvincible(float time)
		{
			Invincible = true;
			yield return new WaitForSeconds(time);
			Invincible = false;
		}

		IEnumerator WaitToMove(float time)
		{
			_canMove = false;
			yield return new WaitForSeconds(time);
			_canMove = true;
		}

		IEnumerator WaitToCheck(float time)
		{
			_canCheck = false;
			yield return new WaitForSeconds(time);
			_canCheck = true;
		}

		IEnumerator WaitToEndSliding()
		{
			yield return new WaitForSeconds(0.1f);
			canDoubleJump = true;
			_isWallSliding = false;
			animator.SetBool("IsWallSliding", false);
			_oldWallSliding = false;
			wallCheck.localPosition =
				new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
		}

		protected IEnumerator WaitToDead()
		{
			animator.SetBool("IsDead", true);
			_canMove = false;
			Invincible = true;
			_player.CanAttack = false;
			yield return new WaitForSeconds(0.4f);
			_rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
			yield return new WaitForSeconds(1.1f);
			Destroy(gameObject);
			// SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(groundCheck.position, GroundedRadius);
		}
	}
}