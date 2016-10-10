//using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class AIController : MonoBehaviour
{
    public string playerNumber;
    public Camera myCamera;
    public GameObject[] foodDrops;
    public GameObject deathParticles;

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
    public bool isPassive;
    private bool hasDied = false;
    private bool IsNearPlayer
    {
        get
        {
            return this.nearbyPlayers > 0;
        }
    }
    private int nearbyPlayers;

    public GameObject navNodeTemplate;
    GameObject navNode;

    public Vector3 target;

    private void Start()
    {
        this.isPassive = true;
        // get the transform of the main camera
        //if (Camera.main != null)
        //{
        //    m_Cam = Camera.main.transform;
        //}
        //
        //{
        //    Debug.LogWarning(
        //        "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        //    // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        //}
        this.navNode = GameObject.Instantiate(this.navNodeTemplate, this.transform.position, this.navNodeTemplate.transform.rotation) as GameObject;

        this.navNode.GetComponent<EnemyMovement>().Init(this);

        // get the third person character ( this should never be null due to require component )
        this.m_Character = GetComponent<ThirdPersonCharacter>();
    }


    private void Update()
    {
        if (this.tag == "Dead")
        {
            if (!this.hasDied)
                OnDeath();
            return;
        }
        CheckActions();
        if (!this.m_Jump)
        {
            //this.m_Jump = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " Jump");
        }

        if (this.isPassive)
        {
            this.m_Character.m_MovingTurnSpeed = 831.6f;
            this.m_Character.m_StationaryTurnSpeed = 415.8f;
        }
        else
        {
            this.m_Character.m_MovingTurnSpeed = 576f;
            this.m_Character.m_StationaryTurnSpeed = 288f;
        }
    }


    // Fixed update is called in sync with physics'
    private void FixedUpdate()
    {
        if (this.tag == "Dead")
            return;
        // read inputs
        float h = Input.GetAxis(this.playerNumber + " Horizontal");
        float v = -Input.GetAxis(this.playerNumber + " Vertical");

        // calculate move direction to pass to character
        //if (this.m_Cam != null)
        //{
        //    // calculate camera relative direction to move:
        //    this.m_CamForward = Vector3.Scale(this.m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        //    this.m_Move = v * this.m_CamForward + h * this.m_Cam.right;
        //}
        //else
        //{
        //    //  we use world-relative directions in the case of no main camera
        //    this.m_Move = v * Vector3.forward + h * Vector3.right;
        //}
        this.m_Move = (this.target - this.transform.position).normalized;
        this.m_Move.y = 0f;

        if ((this.target - this.transform.position).magnitude < 0.7f)
            this.m_Move = Vector3.zero;
        // pass all parameters to the character control script
        this.m_Character.Move(this.m_Move, this.m_Jump, this.lAttack, this.hAttack, this.dodge, this.run);
        if (this.isPassive)
            this.run = false;
        if (this.m_Move == Vector3.zero)
            this.run = false;
        this.m_Jump = false;
        this.lAttack = false;
        this.hAttack = false;
        this.blocking = false;
        this.dodge = false;

    }
    private void CheckActions()
    {
        if (!this.lAttack)
        {

            this.lAttack = !this.isPassive && this.IsNearPlayer;
        }
        //if (!hAttack)
        //    hAttack = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " HAttack");
        //if (!dodge)
        //    dodge = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " Dodge");
        //if (!blocking)
        //    blocking = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " Block");
        //if (!run)
        //    run = CrossPlatformInputManager.GetButtonDown(this.playerNumber + " Run");

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
            this.nearbyPlayers++;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Player")
            this.nearbyPlayers--;
    }

    public void Init(Vector3 pos)
    {
        this.transform.position = pos;
    }

    void OnDeath()
    {
        this.hasDied = true;
        StartCoroutine(DeleteSoon());
    }

    void OnDelete()
    {
        Instantiate(this.foodDrops[Random.Range(0, 2)], this.transform.position + new Vector3(0, 0.5f), transform.rotation);
        Instantiate(this.deathParticles, this.transform.position + new Vector3(0, 0.5f), deathParticles.transform.rotation);
        return;
    }

    IEnumerator DeleteSoon()
    {
        yield return new WaitForSeconds(5);
        this.OnDelete();
        this.gameObject.SetActive(false);
    }
}
