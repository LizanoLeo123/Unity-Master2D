using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 200f;                          // Amount of force added when the player jumps.
	//[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = true;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
																				//[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	public GameObject disk;

	//Wall jumping variables
	public float wallJumpTime = 0.2f;
	public float wallSlideSpeed = 0.4f;
	public float wallDistance = 0.51f;
	bool _isWallSliding = false;
	RaycastHit2D WallCheckHit;
	private float _jumpTime;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	private bool _diskInHand;

	[Header("Events")]
	[Space]

	public UnityEngine.Events.UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEngine.Events.UnityEvent<bool> { }

	//public BoolEvent OnCrouchEvent;
	//private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEngine.Events.UnityEvent();

		//if (OnCrouchEvent == null)
		//	OnCrouchEvent = new BoolEvent();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetAxis("Horizontal") != 0)
		{
			float direction = Input.GetAxis("Horizontal");
			Move(direction, false, false);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			float direction = Input.GetAxis("Horizontal");
			Move(direction, false, true);
		}

		if (Input.GetMouseButtonUp(0))
		{
			_diskInHand = false;
		}

		
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		//Wall jumping logic
		if (m_FacingRight)
		{
			WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, m_WhatIsGround);
		}
		else
		{
			WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), -wallDistance, m_WhatIsGround);
		}

		if (WallCheckHit && !m_Grounded && Input.GetAxis("Horizontal") != 0)
		{
			_isWallSliding = true;
			_jumpTime = Time.time + wallJumpTime;
		}
		else if(_jumpTime < Time.time)
		{
			_isWallSliding = false;
		}

		if (_isWallSliding)
		{
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, -wallSlideSpeed, float.MaxValue));
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		//if (!crouch)
		//{
		//	// If the character has a ceiling preventing them from standing up, keep them crouching
		//	if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
		//	{
		//		crouch = true;
		//	}
		//}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			//if (crouch)
			//{
			//	if (!m_wasCrouching)
			//	{
			//		m_wasCrouching = true;
			//		OnCrouchEvent.Invoke(true);
			//	}

			//	// Reduce the speed by the crouchSpeed multiplier
			//	move *= m_CrouchSpeed;

			//	// Disable one of the colliders when crouching
			//	if (m_CrouchDisableCollider != null)
			//		m_CrouchDisableCollider.enabled = false;
			//}
			//else
			//{
			//	// Enable the collider when not crouching
			//	if (m_CrouchDisableCollider != null)
			//		m_CrouchDisableCollider.enabled = true;

			//	if (m_wasCrouching)
			//	{
			//		m_wasCrouching = false;
			//		OnCrouchEvent.Invoke(false);
			//	}
			//}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump || _isWallSliding && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			if (_isWallSliding)
			{
				if(m_FacingRight)
					m_Rigidbody2D.AddForce(new Vector2(-500f, m_JumpForce));
				else
					m_Rigidbody2D.AddForce(new Vector2(500f, m_JumpForce)); //Desactivar airControl, esperar un decisegundo y reactivarlo
			}
			else
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Bullet")
		{
			_diskInHand = true;
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
}
