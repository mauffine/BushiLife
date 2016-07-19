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
    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("HurtBox"))
            stats.health.Decrease(10);//col.GetComponent<Stats>().attack.val);
    }

}
