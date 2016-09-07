using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CameraVertical : MonoBehaviour {
    string playerNumber;
    CameraController controller;
    float previousMagnitude = 0f;
    float offset= 0f;

	// Use this for initialization
	void Start () {
        this.controller = GetComponentInParent<CameraController>();
        this.playerNumber = this.controller.strPlayerNumber;
	}
	
	// Update is called once per frame
	void Update () {
        //if (!CrossPlatformInputManager.GetButton(this.playerNumber + " Target"))
        //{
        //}

        var currentMagnitude = this.controller.look.magnitude;

        if (currentMagnitude > this.previousMagnitude)
        {
            var angles = this.transform.rotation.eulerAngles;

            this.transform.rotation = Quaternion.Euler(Mathf.LerpUnclamped(90f, 25f, Mathf.Clamp(currentMagnitude + this.offset, 0f, 1f)), angles.y, angles.z);
        }
        else if (currentMagnitude - this.previousMagnitude < 0)
        {
            this.offset = this.previousMagnitude;
        }

        this.previousMagnitude = currentMagnitude;
    }
}
