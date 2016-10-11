using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    public string playerNumber;
    public Camera myCamera;

    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    public Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private bool blocking;
    private bool dodge;
    private bool lAttack;
    private bool hAttack;
    private bool run;
    private bool strafing;
    private bool ghost = false;
    public float offset;
    public Targeter targeter;

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
        this.targeter = GetComponentInChildren<Targeter>();
    }


    private void Update()
    {
        CheckActions();
        if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " Jump");
            }
        this.strafing = Input.GetButton(playerNumber + " Target");
        
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
            this.m_CamForward = Vector3.Scale(this.m_Cam.forward, new Vector3(1, 0, 1)).normalized;
         //   var quat = Quaternion.LookRotation(m_CamForward);
       //     this.m_CamForward = (quat * Quaternion.Euler(0, -this.offset, 0)) * Vector3.forward;

            var rot = v*m_CamForward + h * m_Cam.right;

            //if (this.targeter.target != null)
            //{
            //    //var look = this.targeter.target.position - this.transform.position;
            //    //print(this.targeter.target);
            //    //look = look.normalized;

            //    //m_Move = v * look + h * Vector3.Cross(look, Vector3.up);


            //    m_Move = rot.magnitude * Vector3.forward;

            //    this.transform.parent.rotation = Quaternion.LookRotation(rot);
            //}
            //else
            //{
                m_Move = rot;
            //}
        }
       
     //   m_Move = v * Vector3.forward + h * Vector3.right;

            // calculate player relative direction to move:
       //     m_CamForward = Vector3.Scale(this.transform.forward, new Vector3(1, 0, 1)).normalized;
       //     m_Move = v * m_CamForward + h * Vector3.right;
        

        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_Jump, this.lAttack, this.hAttack, this.strafing, this.blocking, this.dodge, this.run);

        if (this.m_Move == Vector3.zero)
            this.run = false;
        m_Jump      = false;
        lAttack     = false;
        hAttack     = false;
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
        blocking = CrossPlatformInputManager.GetButton(this.playerNumber + " Block");
        if (!run)
            run = CrossPlatformInputManager.GetAxis(this.playerNumber + " Run") > .1f;

    }

    public void SetPlayerNumber(int _playernum)
    {
        this.playerNumber = "P" + _playernum.ToString();
    }

    public string Fullname(string input)
    {
        return this.playerNumber + " " + input;
    }

    public bool IsButtonDown(string name)
    {
        var result = CrossPlatformInputManager.GetButton(Fullname(name));
        return result;
    }

    public bool IsButtonReleased(string name)
    {
        if (CrossPlatformInputManager.GetButtonUp(Fullname(name)))
        {
            print(name + " button RELEASED!!!");
            return true;
        }

        return false;
    }

    public float Axis(string name)
    {
        return CrossPlatformInputManager.GetAxis(Fullname(name));
    }

    public void SetCamera(Camera _camera)
    {
        this.myCamera = _camera;
        this.m_Cam = this.myCamera.transform;
    }
}
