using UnityEngine;
using System.Reflection;
using System.Collections;

public class Triggerer : MonoBehaviour {
    private Character character;
    int numCollidingWith = 0;
    public Vector3 groundNormal;

    public bool IsColliding
    {
        get
        {
            return numCollidingWith > 0;
        }
    }
  

	// Use this for initialization
	void Start () {
        character = GetComponentInParent<Character>();

    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider col)
    {
        numCollidingWith++;
        MethodInfo dynMethod = this.GetType().GetMethod("OnTriggerEnter",
       BindingFlags.NonPublic | BindingFlags.Instance);
        dynMethod.Invoke(character, new object[] { col, gameObject });
    }

    void OnCollisionStay(Collision col)
    {
        //groundNormal = col.frictionForceSum
    }

    void OnTriggerExit(Collider col)
    {
        numCollidingWith--;
    }
}
