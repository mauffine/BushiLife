using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    public string strPlayerNumber;

	Camera camera;

    // Use this for initialization
    void Start ()
	{
    }

	public void Init(Transform target, int playerNumber)
	{
		camera = GetComponentInChildren<Camera>();
		this.strPlayerNumber = "P" + playerNumber.ToString();
		this.target = target;
	}

	void FindRect(int numPlayers)
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
        this.transform.position = this.target.transform.position;

		float h = 0f;// Input.GetAxis(this.strPlayerNumber + " Camera Horizontal") * 2f;
        this.transform.Rotate(Vector3.up * h);
    }

	public void SetRect(Rect rectangle)
	{
		camera.rect = rectangle;
	}
}
