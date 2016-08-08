using UnityEngine;
using System.Collections;

public class test_openo : MonoBehaviour {
	bool isTrue = false;
	// Use this for initialization
	void Start () {
		isTrue = this.o("Test").o("test", 50).As<bool>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool Test(string name, int size)
	{
		Debug.Log(size);
		return true;
	}
}
