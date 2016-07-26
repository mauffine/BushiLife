using UnityEngine;
using System.Collections;

public class ehTempMoveScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Time.deltaTime * 2.0f;
        transform.Rotate(Vector3.up, Time.deltaTime * 20);
	}
}
