using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter : MonoBehaviour
{
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;
	[SerializeField] float m_JumpPower = 12f;
	[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
	[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
	[SerializeField] float m_MoveSpeedMultiplier = 1f;
	[SerializeField] float m_AnimSpeedMultiplier = 1f;
	[SerializeField] float m_GroundCheckDistance = 0.1f;
    [SerializeField] GameObject swordbox;
    [SerializeField] GameObject jumpAttackHB;
    [SerializeField] ParticleSystem blood;

	Rigidbody m_Rigidbody;
	Animator m_Animator;
	bool m_IsGrounded;
	float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_TurnAmount;
	float m_ForwardAmount;
	Vector3 m_GroundNormal;
	CapsuleCollider m_Capsule;

    float timer;
    void Start()
	{
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();

		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		m_OrigGroundCheckDistance = m_GroundCheckDistance;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Bleed();
        }
    }
    public void Move(Vector3 move, bool jump, bool lAttack = false, bool hAttack = false, bool dodge = false, 
        bool run = false)
	{
		// convert the world relative moveInput vector into a local-relative
		// turn amount and forward amount required to head in the desired
		// direction.
		if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
		CheckGroundStatus();
		move = Vector3.ProjectOnPlane(move, m_GroundNormal);
#if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.position + m_Rigidbody.velocity / 4, Color.white);
#endif
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;
        if (run)
            m_ForwardAmount *= 2;
            ApplyExtraTurnRotation();

		// control and velocity handling is different when grounded and airborne:
		if (m_IsGrounded)
		{
			HandleGroundedMovement(jump);
		}
		else
		{
			HandleAirborneMovement();
		}
            

		// send input and other state parameters to the animator
		UpdateAnimator(move, dodge, lAttack, hAttack);
	}
        
	void UpdateAnimator(Vector3 move, bool dodge, bool lAttack, bool hAttack)
	{
		// update the animator parameters
		m_Animator.SetFloat("Speed", m_ForwardAmount, 0.1f, Time.deltaTime);
		m_Animator.SetFloat("Rotation", m_TurnAmount, 0.1f, Time.deltaTime);
		m_Animator.SetBool("OnGround", m_IsGrounded);

		if (!m_IsGrounded)
		{
			m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
		}
        else
        {
            if (dodge)
                m_Animator.SetTrigger("Dodge");
            if (lAttack)
            {
                m_Animator.SetTrigger("Light Attack");
                int comboNum = m_Animator.GetInteger("Combo");
                if (comboNum < 1)
                    m_Animator.SetInteger("Combo", comboNum + 1);
            }
            else if (hAttack)
            {
                m_Animator.SetTrigger("Heavy Attack");
                int comboNum = m_Animator.GetInteger("Combo");
                if (comboNum < 1)
                    m_Animator.SetInteger("Combo", comboNum + 1);
            }
        }

		// calculate which leg is behind, so as to leave that leg trailing in the jump animation
		// (This code is reliant on the specific run cycle offset in our animations,
		// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
		float runCycle =
			Mathf.Repeat(
				m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);

		// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
		// which affects the movement speed because of the root motion.
		if (m_IsGrounded && move.magnitude > 0)
		{
			m_Animator.speed = m_AnimSpeedMultiplier;
		}
		else
		{
			// don't use that while airborne
			m_Animator.speed = 1;
		}
	}


	void HandleAirborneMovement()
	{
		// apply extra gravity from multiplier:
		Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
		m_Rigidbody.AddForce(extraGravityForce);

		m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
	}


	void HandleGroundedMovement(bool jump)
	{
		// check whether conditions are right to allow a jump:
		if (jump && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
		{
			// jump!
			m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
			m_IsGrounded = false;
			m_Animator.applyRootMotion = false;
			m_GroundCheckDistance = 0.1f;
		}
	}

	void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}


	public void OnAnimatorMove()
	{
		// we implement this function to override the default root motion.
		// this allows us to modify the positional speed before it's applied.
		if (m_IsGrounded && Time.deltaTime > 0)
		{
			Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

			// we preserve the existing y part of the current velocity.
			v.y = m_Rigidbody.velocity.y;
			m_Rigidbody.velocity = v;
		}
	}


	void CheckGroundStatus()
	{
        RaycastHit hitInfo;
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true;
			m_Animator.applyRootMotion = true;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
			m_Animator.applyRootMotion = false;
		}
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        if (hitInfo.point != Vector3.zero)
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance), Color.blue);
        else
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance), Color.red);
#endif
    }
    //bool CheckIFrames(Collider _col)
    //{
    //    if ()
    //    return true
    //}
    public void Bleed()
    {
        blood.Emit(10);
    }

    //Mecanim events
    public void ClearCombo()
    {
        m_Animator.SetInteger("Combo", 0);
    }
    public void TurnSwordOn()
    {
        swordbox.SetActive(true);
        m_Animator.SetInteger("Combo", m_Animator.GetInteger("Combo") - 1);
    }
    public void TurnSwordOff()
    {
        swordbox.SetActive(false);
    }

}

