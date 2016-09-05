using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    public string strPlayerNumber;
    public ThirdPersonUserControl controller;

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
        if (!this.controller.IsButtonDown("Target"))
        {
            this.transform.position = this.target.transform.position;
            float h = Input.GetAxis(this.strPlayerNumber + " Camera Horizontal") * 300f * Time.deltaTime;
            this.transform.Rotate(Vector3.up * h);
        }
    }

    public void SetRect(Rect rectangle)
    {
        this.camera.rect = rectangle;
    }
}
