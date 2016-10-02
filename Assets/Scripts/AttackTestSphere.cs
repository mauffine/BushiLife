using UnityEngine;
using System.Collections;

public class AttackTestSphere : MonoBehaviour {

    MeshRenderer myRenderer;
    // Use this for initialization
    void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.material.color = Color.blue;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider _col)
    {
        if (myRenderer.material.color == Color.blue)
            myRenderer.material.color = Color.red;
        else
            myRenderer.material.color = Color.blue;
    }
}
