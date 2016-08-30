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

    public bool heavyAttack;

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
    bool invincible;
    bool blocking = false;

    void Start()
    {
        this.m_Animator = GetComponent<Animator>();
        this.m_Rigidbody = GetComponent<Rigidbody>();
        this.m_Capsule = GetComponent<CapsuleCollider>();

        this.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        this.m_OrigGroundCheckDistance = this.m_GroundCheckDistance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Bleed();
        }
    }
    public void Move(Vector3 move, bool jump, bool lAttack = false, bool hAttack = false, bool block = false,
        bool dodge = false, bool run = false)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = this.transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, this.m_GroundNormal);
#if UNITY_EDITOR
        Debug.DrawLine(this.transform.position, this.transform.position + this.m_Rigidbody.velocity / 4, Color.white);
#endif
        this.m_TurnAmount = Mathf.Atan2(move.x, move.z);
        this.m_ForwardAmount = move.z;
        if (run)
            this.m_ForwardAmount *= 2;
        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (this.m_IsGrounded)
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
        this.m_Animator.SetFloat("Speed", this.m_ForwardAmount, 0.1f, Time.deltaTime);
        this.m_Animator.SetFloat("Rotation", this.m_TurnAmount, 0.1f, Time.deltaTime);
        this.m_Animator.SetBool("OnGround", this.m_IsGrounded);

        if (!this.m_IsGrounded)
        {
            this.m_Animator.SetFloat("Jump", this.m_Rigidbody.velocity.y);
            if (hAttack)
            {
                this.m_Animator.SetTrigger("Heavy Attack");
                int comboNum = this.m_Animator.GetInteger("Combo");
                if (comboNum < 1)
                    this.m_Animator.SetInteger("Combo", comboNum + 1);
            }
        }
        else
        {
            if (dodge)
                this.m_Animator.SetTrigger("Dodge");
            if (lAttack)
            {
                this.m_Animator.SetTrigger("Light Attack");
                int comboNum = this.m_Animator.GetInteger("Combo");
                if (comboNum < 1)
                    this.m_Animator.SetInteger("Combo", comboNum + 1);
            }
            else if (hAttack)
            {
                this.m_Animator.SetTrigger("Heavy Attack");
                int comboNum = this.m_Animator.GetInteger("Combo");
                if (comboNum < 1)
                    this.m_Animator.SetInteger("Combo", comboNum + 1);
            }
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                this.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + this.m_RunCycleLegOffset, 1);

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (this.m_IsGrounded && move.magnitude > 0)
        {
            this.m_Animator.speed = this.m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            this.m_Animator.speed = 1;
        }
    }


    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * this.m_GravityMultiplier) - Physics.gravity;
        this.m_Rigidbody.AddForce(extraGravityForce);

        this.m_GroundCheckDistance = this.m_Rigidbody.velocity.y < 0 ? this.m_OrigGroundCheckDistance : 0.01f;
    }


    void HandleGroundedMovement(bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && this.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump!
            this.m_Rigidbody.velocity = new Vector3(this.m_Rigidbody.velocity.x, this.m_JumpPower, this.m_Rigidbody.velocity.z);
            this.m_IsGrounded = false;
            this.m_Animator.applyRootMotion = false;
            this.m_GroundCheckDistance = 0.1f;
        }
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(this.m_StationaryTurnSpeed, this.m_MovingTurnSpeed, this.m_ForwardAmount);
        this.transform.Rotate(0, this.m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }


    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (this.m_IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (this.m_Animator.deltaPosition * this.m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = this.m_Rigidbody.velocity.y;
            this.m_Rigidbody.velocity = v;
        }
    }


    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, this.m_GroundCheckDistance))
        {
            this.m_GroundNormal = hitInfo.normal;
            this.m_IsGrounded = true;
            this.m_Animator.applyRootMotion = true;
        }
        else
        {
            this.m_IsGrounded = false;
            this.m_GroundNormal = Vector3.up;
            this.m_Animator.applyRootMotion = false;
        }
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        if (hitInfo.point != Vector3.zero)
            Debug.DrawLine(this.transform.position + (Vector3.up * 0.1f), this.transform.position + (Vector3.up * 0.1f) + (Vector3.down * this.m_GroundCheckDistance), Color.blue);
        else
            Debug.DrawLine(this.transform.position + (Vector3.up * 0.1f), this.transform.position + (Vector3.up * 0.1f) + (Vector3.down * this.m_GroundCheckDistance), Color.red);
#endif
    }
    public bool CheckIFrames(Collider _col)
    {
        Vector3 colDir = _col.transform.position - transform.position;
        Debug.DrawLine(transform.position, _col.transform.position);
        float angle = Vector3.Angle(colDir, transform.forward);
        if (this.blocking && angle < 45)
            return true;
        return this.invincible;
    }
    public void Bleed()
    {
        this.blood.Emit(20);
    }
    public void Die()
    {
        m_Animator.SetBool("Dead", true);
        this.invincible = true;
    }
    //Mecanim events
    public void ClearCombo()
    {
        this.m_Animator.SetInteger("Combo", 0);
    }
    public void TurnSwordOn()
    {
        this.swordbox.SetActive(true);
        this.m_Animator.SetInteger("Combo", this.m_Animator.GetInteger("Combo") - 1);
    }
    public void HeavySword()
    {
        this.swordbox.SetActive(true);
        this.heavyAttack = true;
        this.m_Animator.SetInteger("Combo", this.m_Animator.GetInteger("Combo") - 1);
    }
    public void TurnSwordOff()
    {
        this.swordbox.SetActive(false);
        this.heavyAttack = false;
    }
    public void StartIFrames()
    {
        this.invincible = true;
    }
    public void EndIFrames()
    {
        this.invincible = false;
    }
    public void JumpAttackOn()
    {
        this.jumpAttackHB.SetActive(true);
    }
    public void JumpAttackOff()
    {
        this.jumpAttackHB.SetActive(false);
    }
    public void BlockOn()
    {
        blocking = true;
    }
    public void BlockOff()
    {
        blocking = false;
    }

}

