using UnityEngine;
using System.Collections.Generic;

public delegate void AnimResponse();

public class ThirdPerson : MonoBehaviour
{
    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_JumpPower = 12f;
    [Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;
    [SerializeField] float speed = 3f;
    [SerializeField] float lightAttackMoveSpeed = 5f;
    [SerializeField] float heavyAttackMoveSpeed = 5f;
    [SerializeField] float dodgeSpeed = 5f;
    [SerializeField] float runningModifier = 2f;
    [SerializeField] GameObject LA1Hurtbox;
    [SerializeField] GameObject LA2Hurtbox;
    [SerializeField] GameObject HAHurtbox;
    [SerializeField] GameObject JumpAttaclHB;
    [SerializeField] GameObject Swordbox;
    [SerializeField] GameObject blood;

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    CapsuleCollider m_Capsule;
    bool m_Crouching;
    bool lAttack;
    bool hAttack;
    bool block;
    bool doubleJump;
    bool attacking;
    bool rolling;
    bool dead;

    AnimationStateMachine animStateMach;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
        doubleJump = true;
        animStateMach = GetComponent<AnimationStateMachine>();
        attacking = false;
        rolling = false;
        dead = false;
    }
    void Update()
    {
        if (animStateMach.currentAnimation == CustomAnimationState.Jump && m_IsGrounded)
        {
            animStateMach.SetAnimation(CustomAnimationState.Idle);
        }
    }
    public void Move(Vector3 move, bool _jump)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (!dead)
        {
            if (move.magnitude > 1f) move.Normalize();
            move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_TurnAmount = Mathf.Atan2(move.x, move.z);
            m_ForwardAmount = move.z;
            ApplyExtraTurnRotation();
            // control and velocity handling is different when grounded and airborne:
            if (m_IsGrounded)
            {
                HandleGroundedMovement(_jump);
            }
            else
            {
                HandleAirborneMovement(_jump);
            }

            if (rolling)
            {
                m_Rigidbody.velocity = m_Rigidbody.velocity = new Vector3(transform.forward.x * Time.deltaTime * this.dodgeSpeed, m_Rigidbody.velocity.y, transform.forward.z * Time.deltaTime * this.dodgeSpeed);
            }
            else if (attacking)
            {

            }
            if (animStateMach.currentAnimation == CustomAnimationState.Walking)
            {
                m_Rigidbody.velocity = new Vector3(transform.forward.x * Time.deltaTime * this.speed, m_Rigidbody.velocity.y, transform.forward.z * Time.deltaTime * this.speed);
            }
            else if (animStateMach.currentAnimation == CustomAnimationState.Running)
            {
                m_Rigidbody.velocity = new Vector3(transform.forward.x * Time.deltaTime * this.speed * this.runningModifier, m_Rigidbody.velocity.y, transform.forward.z * Time.deltaTime * this.speed * this.runningModifier);
            }
            else if (animStateMach.currentAnimation == CustomAnimationState.Idle)
            {
                m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);

            }
        }
    }

    void HandleAirborneMovement(bool jump)
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);
        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : .1f;
    }

    void HandleGroundedMovement(bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && m_IsGrounded)
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            m_IsGrounded = false;
            m_GroundCheckDistance = .1f;
            this.animStateMach.SetAnimation(CustomAnimationState.Jump);
        }
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            this.doubleJump = true;
            m_Animator.SetBool("Jump", false);
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
        }
    }
    public bool CheckIFrames(Collider _col)
    {
        Vector3 colDir = _col.transform.position - transform.position;
        float angle = Vector3.Angle(colDir, transform.forward);
        if (this.rolling || (this.block && angle < 45) || _col.GetComponentInParent<Character>() == null || dead)
            return false;
        //TODO:fix dis shit
        //Instantiate(blood, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), blood.transform.rotation);//AngleAxis(-90, Vector3.right));
        return true;
    }

    void StartLAttack1()
    {

        m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
        LA1Hurtbox.SetActive(true);
        this.attacking = true;
    }
    void EndLAttack1()
    {
        LA1Hurtbox.SetActive(false);
        Swordbox.SetActive(false);
        this.attacking = false;
    }
    void StartLAttack2()
    {
        m_Rigidbody.velocity = m_Rigidbody.velocity = new Vector3(transform.forward.x * Time.deltaTime * this.lightAttackMoveSpeed, m_Rigidbody.velocity.y, transform.forward.z * Time.deltaTime * this.lightAttackMoveSpeed);
        Swordbox.SetActive(true);
        this.attacking = true;
        //m_Rigidbody.velocity = m_Rigidbody.velocity = new Vector3(transform.forward.x * this.lightAttackMoveSpeed, m_Rigidbody.velocity.y, transform.forward.z * this.lightAttackMoveSpeed);
    }
    void EndLAttack2()
    {
        Swordbox.SetActive(false);
        this.attacking = false;
        m_Rigidbody.velocity = m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
    }
    void StartHeavyAttack()
    {
        Swordbox.SetActive(true);
    }
    void EndHeavyAttack()
    {
        HAHurtbox.SetActive(false);
    }
    void BeginRoll()
    {
        rolling = true;
    }
    void EndRoll()
    {
        rolling = false;
    }
    void StartBlock()
    {
        m_Rigidbody.velocity = m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
        block = true;
    }
    void EndBlock()
    {
        block = false;

    }
    void Walk()
    {
        m_Rigidbody.velocity = new Vector3(transform.forward.x * this.speed, m_Rigidbody.velocity.y, transform.forward.z * this.speed);
    }
    void Run()
    {

    }
    void Idle()
    {
        m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
    }
    void JumpAttackStart()
    {
        JumpAttaclHB.SetActive(true);
    }
    void JumpAttackEnd()
    {
        JumpAttaclHB.SetActive(false);
    }
    void Jump()
    {

    }
    void RunningSlashStart()
    {
        Swordbox.SetActive(true);
    }
    void RunningSlashEnd()
    {
        Swordbox.SetActive(false);
    }
    public void Die()
    {
        if (!dead)
        {
            dead = true;
            this.animStateMach.SetAnimation(CustomAnimationState.Dead);
        }
    }
}

