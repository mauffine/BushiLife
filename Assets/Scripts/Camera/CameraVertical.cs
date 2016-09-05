using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CameraVertical : MonoBehaviour {
    string playerNumber;
	// Use this for initialization
	void Start () {
        playerNumber = GetComponentInParent<CameraController>().strPlayerNumber;
	}
	
	// Update is called once per frame
	void Update () {
        if (!CrossPlatformInputManager.GetButton(this.playerNumber + " Target"))
        {
            float v = Input.GetAxis(this.playerNumber + " Camera Vertical") * 50f * Time.deltaTime;
            this.transform.Rotate(new Vector3(v * 3f, 0, 0));
        }
    }
}
