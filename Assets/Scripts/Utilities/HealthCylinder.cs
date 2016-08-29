using UnityEngine;
using System.Collections;

public class HealthCylinder : MonoBehaviour {
    [SerializeField] float max;
    float current;
    float scaleOffset;
	// Use this for initialization 
	void Start () {
        current = max;
        scaleOffset = transform.localScale.y;

    }
	
	// Update is called once per frame
	void Update () {
        //UpdateHPBar(this.current - Time.deltaTime);
	}
    public void UpdateHPBar(float _newVal)
    {
        this.current = _newVal;
        transform.localScale = new Vector3(transform.localScale.x, (this.current / this.max) * this.scaleOffset , transform.localScale.z);
        if (transform.localScale.y < 0)
            transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
    }
}
