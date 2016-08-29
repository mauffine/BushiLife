using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    public string playerNumber;
    public Camera myCamera;

    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private bool blocking;
    private bool dodge;
    private bool lAttack;
    private bool hAttack;
    private bool run;

    private void Start()
    {
        // get the transform of the main camera
        //if (Camera.main != null)
        //{
        //    m_Cam = Camera.main.transform;
        //}
        //else
        //{
        //    Debug.LogWarning(
        //        "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        //    // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        //}

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
    }


    private void Update()
    {
        CheckActions();
        if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " Jump");
            }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis(this.playerNumber + " Horizontal");
        float v = -Input.GetAxis(this.playerNumber + " Vertical");

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v*m_CamForward + h*m_Cam.right;
        }
        else
        {
            //  we use world-relative directions in the case of no main camera
            m_Move = v*Vector3.forward + h*Vector3.right;
        }
        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_Jump, this.lAttack, this.hAttack, this.blocking, this.dodge, this.run);

        if (this.m_Move == Vector3.zero)
            this.run = false;
        m_Jump      = false;
        lAttack     = false;
        hAttack     = false;
        blocking    = false;
        dodge       = false;
    }
    private void CheckActions()
    {
        if (!lAttack)
            lAttack = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " LAttack");
        if (!hAttack)
            hAttack = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " HAttack");
        if (!dodge)
            dodge = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " Dodge");
        if (!blocking)
            blocking = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " Block");
        if (!run)
            run = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " Run");

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
