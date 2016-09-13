using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float verticalOffset;
    public string strPlayerNumber;

	Camera camera;
    Vector3 totalOffset;
    // Use this for initialization
    void Start ()
	{
        this.totalOffset = new Vector3(0, this.verticalOffset, 0);
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
        this.totalOffset = new Vector3(0, this.verticalOffset * 10, 0);
        this.transform.position = this.target.transform.position + this.totalOffset;
        float h = Input.GetAxis(this.strPlayerNumber + " Camera Horizontal") * 300f * Time.deltaTime;
        this.transform.Rotate(Vector3.up * h);
    }

	public void SetRect(Rect rectangle)
	{
		camera.rect = rectangle;
	}
}
