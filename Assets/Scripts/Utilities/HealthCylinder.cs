using UnityEngine;
using System.Collections;

public class HealthCylinder : MonoBehaviour {
    float max, current;

	// Use this for initialization 
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void UpdateHPBar(float _newVal)
    {
        this.current = _newVal;
        transform.localScale = new Vector3(transform.localScale.x, this.current / this.max, transform.localScale.z);
    }
}
