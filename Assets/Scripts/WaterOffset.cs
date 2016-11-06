using UnityEngine;
using System.Collections;

public class WaterOffset : MonoBehaviour {

	public float speedX = 0.1f;
	public float speedY = 0.1f;
	private float curX;
	private float curY;


	void Awake () 
	{
		//curX = GetComponent<Renderer>().material.mainTextureOffset.x;
		curX = GetComponent<Renderer>().material.mainTextureOffset.y;
	}		
	
	
	void Update () 
	{
		curX += Time.deltaTime * speedX;
		curY += Time.deltaTime * speedY;
		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", new Vector2(curX, curY));
	}
}
