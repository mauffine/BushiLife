using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeter : MonoBehaviour
{
    LineRenderer line = null;
    public ThirdPersonUserControl controller;
    Collider collider;
    Vector3 aim;
    Transform look;
    public Transform test;

    private List<Transform> targets = new List<Transform>();

    public Transform target;


    void Start()
    {
        this.line = GetComponentInChildren<LineRenderer>();
        this.collider = GetComponent<Collider>();
        this.look = this.transform.parent;
    }

    public void Init(ThirdPersonUserControl controller)
    {
        this.controller = controller;
    }

    // Update is called once per frame
    void Update()
    {
        var test = this.controller.IsButtonDown("Target");

        if (this.controller.IsButtonDown("Target"))
        {
            UpdateAim();
            UpdateLine();
        }

        print(Score(this.target, true));
    }

    void UpdateLine()
    {
        if (this.targets.Count > 0)
        {
            if (this.target != null)
            {
                this.line.enabled = true;
                this.line.SetPositions(new Vector3[] { this.transform.position, this.targets[0].position });
            }
         //   print(Score(this.targets[0]));
        }
        else
        {
          //  print(Score(this.target));
            //  this.line.enabled = true;
            //  this.line.SetPositions(new Vector3[] { this.transform.position, this.target.position });
        }

    }

    void UpdateAim()
    {
        //   this.target = null;

        var vertical = this.controller.Axis("Camera Vertical");
        var horizontal = this.controller.Axis("Camera Horizontal");
        this.aim = vertical * Vector3.back + horizontal * Vector3.left;

        var distance = this.aim.magnitude;

        this.collider.enabled = true;
        this.transform.localScale = Vector3.one * distance * 40f;

        this.targets.Sort((t1, t2) => Score(t1).CompareTo(Score(t2)));
    }

    // If a new enemy enters the trigger, add it to the list of targets
    void OnTriggerEnter(Collider other)
    {
        this.targets.Add(other.transform);
        this.targets.Sort((t1, t2) => Score(t2).CompareTo(Score(t1)));
    }

    private float Score(Transform target, bool isOther = false)
    {
        var offset = target.position - this.look.position;
        var distance = offset.magnitude;

        var dirA = this.aim.normalized;
        dirA.y = 0;
        var dirB = offset.normalized;
        dirB.y = 0;


        var angle = Quaternion.Angle(Quaternion.LookRotation(this.look.forward, Vector3.up), Quaternion.FromToRotation(dirA, dirB));

        
        //   Debug.DrawRay(this.look.position, offset, Color.red);
        return angle;//distance / Mathf.Pow(angle, 1.0f + this.aim.magnitude * 3f);


    }

    // When an enemy exits the trigger, remove it from the list
    void OnTriggerExit(Collider other)
    {
        this.targets.Remove(other.transform);
    }
}