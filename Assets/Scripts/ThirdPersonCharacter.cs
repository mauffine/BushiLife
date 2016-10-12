using UnityEngine;
using Xft;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter : MonoBehaviour
{
    [SerializeField] public float m_MovingTurnSpeed = 360;
    [SerializeField] public float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_JumpPower = 12f;
    [Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;
    [SerializeField] float airControl = 1000;
    [SerializeField] float maxArialSpeed = 10;

    [SerializeField] GameObject swordbox;
    [SerializeField] GameObject jumpAttackHB;
    [SerializeField] GameObject ghostPref;
    [SerializeField] GameObject deathParticles;

    [SerializeField] ParticleSystem blood;
    [SerializeField] ParticleSystem groundSmash;
    [SerializeField] ParticleSystem swordClash;

    [SerializeField] int blockAngle = 180;

    [SerializeField] float lightAtackStamDrain = 5;
    [SerializeField] float heavyAttackStamDrain = 25;
    [SerializeField] float rollStamDrain = 10;
    [SerializeField] float stamRecharge = 2;
    [SerializeField] float rechargeRate = 15;
    [SerializeField] float runStamDrain = 15;
    [SerializeField] float jumpAttackStamDrain = 40;
    [SerializeField] float ghostSpawnTime = 3;

    [SerializeField] XWeaponTrail trail;

    public bool heavyAttack;

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    Vector3 strafeMove;
    CapsuleCollider m_Capsule;
    
    bool invincible = false;
    bool blocking = false;
    bool canRoll = true;
    bool rechargingStam = true;
    bool strafing = false;
    bool running = false;

    private bool ghost = false;
    Stat stamina;
    float ghostTimer = 3;
    void Start()
    {
        this.m_Animator = GetComponent<Animator>();
        this.m_Rigidbody = GetComponent<Rigidbody>();
        this.m_Capsule = GetComponent<CapsuleCollider>();
        this.stamina = GetComponent<Character>().stats.stamina;
        this.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        this.m_OrigGroundCheckDistance = this.m_GroundCheckDistance;
        this.trail.Deactivate();
    }

    void Update()
    {
        //this.rechargingStam = !blocking;
        if (this.rechargingStam && this.stamina.val <= 100 && !ghost)
        {
            this.stamina.Increase(Time.deltaTime * rechargeRate);
        }
        this.stamina.Validate();
        if (Input.GetKeyDown(KeyCode.Space))
            this.swordClash.Play();
        
        if (ghostTimer > 0 )
        {
            if(gameObject.layer == 10)
                ghostTimer -= Time.deltaTime;
        }
        else if (this.GetComponent<ThirdPersonUserControl>()!=null)
        {
            if (!ghost && gameObject.CompareTag("Dead"))
            {
                Renderer[] tmp = this.GetComponentsInChildren<Renderer>();
                foreach (Renderer i in tmp)
                {
                    i.enabled = false;
                }
                ghost = true;
                Instantiate(ghostPref, transform.position + new Vector3(0, 0.5f), transform.rotation, transform);
                this.m_Animator.SetTrigger("Ghost");
                gameObject.GetComponent<ThirdPersonUserControl>().enabled = true;
                gameObject.tag = "Ghost";
                gameObject.layer = 19;
                this.trail.Deactivate();
                Instantiate(deathParticles, gameObject.transform.position, deathParticles.transform.rotation);
            }
        }
    }
    public void Move(Vector3 move, bool jump, bool lAttack = false, bool hAttack = false, bool _strafing = false, bool block = false,
        bool dodge = false, bool run = false)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        CheckGroundStatus();
        strafing = _strafing;

        if (move.magnitude > 1f) move.Normalize();
        move = this.transform.InverseTransformDirection(move);

        if (this.m_IsGrounded)
            move = Vector3.ProjectOnPlane(move, this.m_GroundNormal);
        else
            move = Vector3.ProjectOnPlane(move, Vector3.up);
        if (!strafing)
        {
            

#if UNITY_EDITOR
            Debug.DrawLine(this.transform.position, this.transform.position + this.m_Rigidbody.velocity / 4, Color.white);
#endif
            this.m_TurnAmount = Mathf.Atan2(move.x, move.z);
            this.m_ForwardAmount = move.z;
            if (run && this.stamina.val > 0 && m_IsGrounded && !ghost)
            {
                if (this.rechargingStam)
                    this.rechargingStam = false;
                this.m_ForwardAmount *= 2;
                this.stamina.Decrease(runStamDrain * Time.deltaTime);
            }
            else if (!this.rechargingStam && m_IsGrounded && this.running && !run)
                rechargingStam = true;
            running = run;
            ApplyExtraTurnRotation();
        }
        else
        {
            this.strafeMove = move;
        }

        // control and velocity handling is different when grounded and airborne:
        if (this.m_IsGrounded)
        {
            HandleGroundedMovement(jump);
        }
        else
        {
            HandleAirborneMovement();
        }
        if (gameObject.CompareTag("Player"))
            m_Animator.SetLayerWeight(1, m_ForwardAmount);

        // send input and other state parameters to the animator
        UpdateAnimator(move, dodge, lAttack, hAttack, block, _strafing);
    }
    void UpdateAnimator(Vector3 move, bool dodge, bool lAttack, bool hAttack, bool block, bool _strafing)
    {
        // update the animator parameters
        if (!_strafing)
        {
            this.m_Animator.SetFloat("Speed", this.m_ForwardAmount, 0.1f, Time.deltaTime);
            this.m_Animator.SetFloat("Rotation", this.m_TurnAmount, 0.1f, Time.deltaTime);
        }
        else
        {
            this.m_Animator.SetFloat("StrafeXVel", this.strafeMove.x, 0.1f, Time.deltaTime);
            this.m_Animator.SetFloat("StrafeZVel", this.strafeMove.z, 0.1f, Time.deltaTime);
        }
        this.m_Animator.SetBool("OnGround", this.m_IsGrounded);
        this.m_Animator.SetBool("Strafing", this.strafing);
        if (stamina.val <= 0 && gameObject.CompareTag("Player"))
            this.m_Animator.SetBool("Tired", true);
        else if (stamina.val > 0 && gameObject.CompareTag("Player"))
            this.m_Animator.SetBool("Tired", false);
        if (!this.m_IsGrounded)
        {
            this.m_Animator.SetFloat("Jump", this.m_Rigidbody.velocity.y);

            if (lAttack && stamina.val > lightAtackStamDrain && !ghost)
            {
                this.m_Animator.SetTrigger("Light Attack");
                int comboNum = this.m_Animator.GetInteger("Combo");
                if (comboNum < 1)
                    this.m_Animator.SetInteger("Combo", comboNum + 1);
            }
            else if (hAttack && stamina.val > jumpAttackStamDrain && !ghost)
            {
                this.m_Animator.SetTrigger("Heavy Attack");
                int comboNum = this.m_Animator.GetInteger("Combo");
                if (comboNum < 1)
                    this.m_Animator.SetInteger("Combo", comboNum + 1);
            }
        }
        else
        {
            if (dodge && this.canRoll && this.stamina.val > this.rollStamDrain && !ghost)
                this.m_Animator.SetTrigger("Dodge");
            if (block && !ghost)
            {
                this.m_Animator.SetBool("Block", true);
            }
            else
            {
                this.m_Animator.SetBool("Block", false);
            }
            if (lAttack && stamina.val > lightAtackStamDrain && !ghost)
            {
                this.m_Animator.SetTrigger("Light Attack");
                int comboNum = this.m_Animator.GetInteger("Combo");
                if (comboNum < 1)
                    this.m_Animator.SetInteger("Combo", comboNum + 1);
            }
            else if (hAttack && this.stamina.val > heavyAttackStamDrain && !ghost)
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
        if (this.m_Rigidbody.velocity.magnitude < this.maxArialSpeed)
            this.m_Rigidbody.AddForce(this.transform.forward * this.m_ForwardAmount * this.airControl * Time.deltaTime, ForceMode.Acceleration);
        this.m_GroundCheckDistance = this.m_Rigidbody.velocity.y < 0 ? this.m_OrigGroundCheckDistance : 0.01f;
    }
    void HandleGroundedMovement(bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && this.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") && !ghost)
        {
            // jump!
            this.m_Rigidbody.velocity = new Vector3(this.m_Rigidbody.velocity.x, this.m_JumpPower, this.m_Rigidbody.velocity.z);
            this.m_IsGrounded = false;
            this.m_Animator.applyRootMotion = true;
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
        if (Physics.Raycast(this.transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, this.m_GroundCheckDistance) && !hitInfo.collider.isTrigger)
        {
            this.m_GroundNormal = hitInfo.normal;
            this.m_IsGrounded = true;
            this.m_Animator.applyRootMotion = true;
        }
        else
        {
            this.m_IsGrounded = false;
            this.m_GroundNormal = Vector3.up;
            this.m_Animator.applyRootMotion = true;
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
        //check if the character should take damage from an attack 
        //(Technically checks if the character shouldn't take damage tho)
        Vector3 colDir = _col.transform.position - transform.position;
        Debug.DrawLine(transform.position, _col.transform.position);
        float angle = Vector3.Angle(colDir, transform.forward);
        if (this.blocking && angle < blockAngle / 2)
        {
            if (_col.GetComponentInParent<ThirdPersonCharacter>().heavyAttack)
                stamina.Decrease(_col.GetComponentInParent<Character>().stats.attack.val * 3);
            else
                stamina.Decrease(_col.GetComponentInParent<Character>().stats.attack.val);
            this.swordClash.Play();
            if (stamina.val <= 0)
            {
                this.m_Animator.SetTrigger("Block Break");
                return false;
            }
            else
                return true;
        }
        return this.invincible;
    }
    public void Bleed()
    {
        this.blood.Emit(20);
    }
    public void Die()
    {
        m_Animator.SetBool("Dead", true);
        this.swordbox.SetActive(false);
        this.jumpAttackHB.SetActive(false);
        this.invincible = true;
        this.tag = "Dead";
        var script = gameObject.GetComponent<ThirdPersonUserControl>();
        if (script != null)
        {
            gameObject.GetComponent<ThirdPersonUserControl>().enabled = false;
            ghostTimer = ghostSpawnTime;
        }
        
    }
    //Mecanim events
    public void ClearCombo()
    {
        this.m_Animator.SetInteger("Combo", 0);
        this.m_Animator.ResetTrigger("Light Attack");
        this.m_Animator.ResetTrigger("HeavyAttack");
        canRoll = true;

    }
    public void TurnSwordOn()
    {
        this.swordbox.SetActive(true);
        this.trail.Activate();
        this.m_Animator.SetInteger("Combo", this.m_Animator.GetInteger("Combo") - 1);
        this.stamina.Decrease(this.lightAtackStamDrain);
        this.rechargingStam = false;
        this.canRoll = false;
    }
    public void HeavySword()
    {
        this.swordbox.SetActive(true);
        this.trail.Activate();
        //set this so the enemy takes 3x damage
        this.heavyAttack = true;
        this.m_Animator.SetInteger("Combo", this.m_Animator.GetInteger("Combo") - 1);
        this.stamina.Decrease(this.heavyAttackStamDrain);
        this.rechargingStam = false;
        this.canRoll = false;
    }
    public void TurnSwordOff()
    {
        this.swordbox.SetActive(false);
        this.heavyAttack = false;
        this.jumpAttackHB.SetActive(false);
        this.rechargingStam = true;
        this.trail.Deactivate();
    }
    public void StartIFrames()
    {
        this.invincible = true;
        this.stamina.Decrease(rollStamDrain);
        this.rechargingStam = false;
        this.canRoll = false;
    }
    public void EndIFrames()
    {
        this.invincible = false;
        this.rechargingStam = true;
        this.canRoll = true;
        if (gameObject.CompareTag("Player"))
            this.m_Animator.ResetTrigger("Dodge");
    }
    public void JumpAttackOn()
    {
        this.stamina.Decrease(jumpAttackStamDrain);
        this.rechargingStam = false;
        this.jumpAttackHB.SetActive(true);
    }
    public void JumpAttackOff()
    {
        this.jumpAttackHB.SetActive(false);
        this.rechargingStam = true; 
    }
    public void BlockOn()
    {
        blocking = true;
    }
    public void BlockOff()
    {
        blocking = false;
    }
    public void SmashGround()
    {
        this.groundSmash.Play();
    }
}

