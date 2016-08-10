using UnityEngine;
using System.Collections;

public class test_openo : MonoBehaviour
{
	bool isTrue = true;
	bool isFalse = false;


	void Start ()
	{
		isTrue = this.os("isTsdfsdfrue", "Test").o("test", 50).As<bool>();
		Debug.Log(this.os("isFalse", "isTrue").As<bool>());
	}
	

	void Update ()
	{
		
	}

	bool Test(string name, int size)
	{
		Debug.Log(isTrue);
		Debug.Log(size);
		return true;
	}

	
}
