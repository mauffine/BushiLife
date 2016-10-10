using UnityEngine;
using System.Collections;

public class DeleteEmitter : MonoBehaviour {
    float timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<ParticleSystem>().isStopped)
            Destroy(gameObject);
	}
}
