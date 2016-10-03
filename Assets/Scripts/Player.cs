using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPerson))]
public class Player : MonoBehaviour
{
    public string playerNumber;
    public Camera myCamera;
    private ThirdPerson m_Character; // A reference to the ThirdPersonCharacter on the object
    private AnimationStateMachine animStateMach;
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private bool blocking;
    private bool dodge;
    private bool lAttack;
    private bool HAttack;
    private bool run;

    private void Awake()
    {
        // get the transform of the main camera
        //this.m_Cam = this.myCamera.transform;
        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPerson>();
        animStateMach = GetComponent<AnimationStateMachine>();
    }


    private void Update()
    {
        if (!m_Jump && !dodge)
        {
            m_Jump = Input.GetButton(this.playerNumber + " Jump");
        }
        lAttack = Input.GetButtonDown(this.playerNumber + " LAttack");
        HAttack = Input.GetButtonDown(this.playerNumber + " HAttack");
        blocking = Input.GetButtonDown(this.playerNumber + " Block");
        dodge = Input.GetButtonDown(this.playerNumber + " Dodge");
        
        UpdateAnimator();
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis(this.playerNumber + " Horizontal");
        float v = Input.GetAxis(this.playerNumber + " Vertical");
        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(-m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
        if (Input.GetButton(this.playerNumber + " Run") && this.animStateMach.currentAnimation == CustomAnimationState.Walking)
        {
            this.animStateMach.SetAnimation(CustomAnimationState.Running);
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_Jump);
        m_Jump      = false;
    }
    void UpdateAnimator()
    {
        if (dodge)
        {
            this.animStateMach.SetAnimation(CustomAnimationState.Dodge);
        }
        else if (lAttack)
        {
            if (this.animStateMach.currentAnimation != CustomAnimationState.LightAttack)
                this.animStateMach.SetAnimation(CustomAnimationState.LightAttack);
            else
                this.animStateMach.SetAnimation(CustomAnimationState.LightAttack2);
        }
        else if (HAttack)
        {
            this.animStateMach.SetAnimation(CustomAnimationState.HeavyAttack);
        }
        else if (blocking)
        {
            this.animStateMach.SetAnimation(CustomAnimationState.Block);
        }
        else if (m_Jump)
        {
            this.animStateMach.SetAnimation(CustomAnimationState.Jump);
        }
        else if (m_Move != Vector3.zero)
        {
            this.animStateMach.SetAnimation(CustomAnimationState.Walking);
        }
        else
        {
            this.animStateMach.SetAnimation(CustomAnimationState.Idle);
        }
    }
    public void SetPlayerNumber(int _playernum)
    {
        this.playerNumber = "P" + _playernum.ToString();
    }
    public void SetCamera(Camera _camera)
    {
        this.myCamera = _camera;
        this.m_Cam = this.myCamera.transform;
    }
}