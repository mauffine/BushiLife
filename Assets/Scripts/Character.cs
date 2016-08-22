using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Stats))]
public class Character : MonoBehaviour
{
	public Stats stats;
    public Stat.Transact UseAttack;
    public Stat.Transact ReceiveDamage;

	// Use this for initialization
	void Start ()
	{
		stats = GetComponent<Stats>();
        //stats.attack.use = UseAttack;

        stats.health.recieve = TakeDamage;
    }
    void Update()
    {
        if (stats.health.val <= 0)
        {
            //GetComponent<ThirdPerson>().Die();
        }
    }
    void TakeDamage(Stat self, Stat other, Transform location)
    {
        self.val -= other.val;
    }
    void OnTriggerEnter(Collider col)
    {
        Character otherCharacter = col.GetComponentInParent<Character>();

        if (col.CompareTag("HurtBox")) //&& GetComponent<Third>().CheckIFrames(col))
        {
            stats.health.recieve(stats.health, otherCharacter.stats.attack, this.transform);  //col.GetComponent<Stats>().attack.val);
            GetComponentInChildren<HealthCylinder>().UpdateHPBar(stats.health.val);
            //GetComponent<ThirdPersonCharacter>().Bleed();
        }
    }

}
