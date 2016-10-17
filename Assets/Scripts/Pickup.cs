using UnityEngine;
using System.Collections;
public class Pickup : MonoBehaviour {
    [SerializeField]
    float rotationSpeed = 50;
    [SerializeField]
    float amplitude = 1;
    [SerializeField]
    float frequency = 1;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        this.transform.position += amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - Time.deltaTime))) * transform.up;
    }
    void OnTriggerEnter(Collider _col)
    {

        if (_col.CompareTag("Player"))
            Destroy(gameObject);
        else if (_col.CompareTag("Ghost"))
        {
            Animator[] tmp = _col.GetComponentsInChildren<Animator>();
            foreach(Animator i in tmp)
            {
                i.SetTrigger("Eat");
            }
            Destroy(gameObject);
        }
    }
}
