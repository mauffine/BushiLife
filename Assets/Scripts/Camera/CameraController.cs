using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    public string strPlayerNumber;
    public ThirdPersonUserControl controller;
    public Vector3 look;


    Camera camera;

    // Use this for initialization
    void Start()
    {
    }

    public void Init(Transform target, int playerNumber)
    {
        this.camera = GetComponentInChildren<Camera>();
        this.strPlayerNumber = "P" + playerNumber.ToString();
        this.target = target;
        this.controller = target.GetComponent<ThirdPersonUserControl>();
    }

    void FindRect(int numPlayers)
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (!this.controller.IsButtonDown("Target"))
        //{
        //}

        this.transform.position = this.target.transform.position;
        float h = Input.GetAxis(this.strPlayerNumber + " Camera Horizontal");


        var vertical = this.controller.Axis("Camera Vertical");
        var horizontal = this.controller.Axis("Camera Horizontal");
        this.look = vertical * Vector3.back + horizontal * Vector3.left;


        if (this.look.magnitude < 0.01f)
            return;

        var angles = this.transform.rotation.eulerAngles;

        this.controller.offset = Quaternion.LookRotation(this.look, Vector3.up).eulerAngles.y + this.target.rotation.eulerAngles.y;
        this.transform.rotation = Quaternion.Euler(angles.x, this.controller.offset, angles.z);
    }

    public void SetRect(Rect rectangle)
    {
        this.camera.rect = rectangle;
    }
}
