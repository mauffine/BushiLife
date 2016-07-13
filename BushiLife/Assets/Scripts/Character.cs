using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Stats))]
public class Character : MonoBehaviour
{
	public Stats stats;
	

	// Use this for initialization
	void Start ()
	{
		stats = GetComponent<Stats>();

		// eg. stats.attack.use = UseAttack;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
