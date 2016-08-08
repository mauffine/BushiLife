using UnityEngine;
using System.Collections;

public class test_openo : MonoBehaviour {
	openo o;
	// Use this for initialization
	void Start () {
		o = new openo(this);
		var name = o.o("Test");
		var realName = name.Get("name");
		Debug.Log(realName.GetType());
		Debug.Log(realName.value);
		Debug.Log("test");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool Test(string name)
	{
		throw new System.Exception();
		Debug.Log("name");
		return true;
	}
}
