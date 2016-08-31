//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//[ RequireComponent(typeof(LineRenderer))]
//public class Targeter : MonoBehaviour {
//    LineRenderer line = null;
//    ThirdPersonUserControl controller;
//    Collider collider;
//    Vector3 aim;
    
//    private List<Transform> targets = new List<Transform>();

//    public Transform target;


//    void Start()
//    {
//        this.line = GetComponent<LineRenderer>();
//        this.collider = GetComponent<Collider>();
//    }

//    public void Init(ThirdPersonUserControl controller)
//    {
//        this.controller = controller;
//    }
	
//	// Update is called once per frame
//	void Update ()
//    {
//	    if (this.controller.IsButtonDown("Target"))
//        {
//            UpdateAim();

//            this.line.enabled = true;
//        }

//        if (this.targets.Count > 0)
//        {
//            if (this.target != null)
//            {
//                this.line.SetPositions(new Vector3[] { this.transform.position, this.targets[0].position });
//            }
//        }


//	}

//    void UpdateAim()
//    {
//        this.target = null;

//        var vertical = this.controller.Axis("Camera Vertical");
//        var horizontal = this.controller.Axis("Camera Horizontal");
//        this.aim = vertical * Vector3.forward + horizontal * Vector3.right;

//        var distance = this.aim.magnitude;

//        this.aim.Normalize();



//        this.collider.enabled = true;
//        (this.collider as SphereCollider).radius = distance * 20f;
//    }

//    // If a new enemy enters the trigger, add it to the list of targets
//    void OnTriggerEnter(Collider other)
//    {
//        this.targets.Add(other.transform);
//        this.targets.Sort((t1, t2) => Score(t1).CompareTo(Score(t2)) * -1);
//    }

//    private float Score(Transform target)
//    {
//        var offset = target.position - this.transform.position;
//        var distance = offset.magnitude;
//        var angle = Quaternion.FromToRotation(this.aim, offset).eulerAngles.y;

//        return distance / angle;
//    }

//    // When an enemy exits the trigger, remove it from the list
//    void OnTriggerExit(Collider other)
//    {
//        this.targets.Remove(other.transform);
//    }
//}


//}
