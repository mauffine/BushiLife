using UnityEngine;
using System.Collections;

public class CameraHorizontal : MonoBehaviour {
    [SerializeField] GameObject player;
    public string playerNumber;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = this.player.transform.position;
        float h = Input.GetAxis(this.playerNumber + " Camera Horizontal");
        this.transform.Rotate(new Vector3(0, h*2, 0));

    }
}
