using UnityEngine;
using System.Collections;

public class HealthCylinder : MonoBehaviour {
    [SerializeField] float max;
    float current;
	// Use this for initialization 
	void Start () {
        current = max;
	}
	
	// Update is called once per frame
	void Update () {
        //UpdateHPBar(this.current - Time.deltaTime);
	}
    public void UpdateHPBar(float _newVal)
    {
        this.current = _newVal;
        transform.localScale = new Vector3(transform.localScale.x, this.current / this.max, transform.localScale.z);
    }
}
