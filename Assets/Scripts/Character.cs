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
    void TakeCritDamage(Stat self, Stat other, Transform location)
    {
        self.val -= other.val * 3;
    }
    void OnTriggerEnter(Collider col)
    {
        Character otherCharacter = col.GetComponentInParent<Character>();
        ThirdPersonCharacter otherThirdPerson = col.GetComponentInParent<ThirdPersonCharacter>();

        if (col.CompareTag("HurtBox") && !GetComponent<ThirdPersonCharacter>().CheckIFrames(col))
        {
            if (otherThirdPerson.heavyAttack)
            {
                stats.health.recieve = TakeCritDamage;
                stats.health.recieve(stats.health, otherCharacter.stats.attack, this.transform);
            }
            else
            {
                stats.health.recieve = TakeDamage;
                stats.health.recieve(stats.health, otherCharacter.stats.attack, this.transform);
            }
            GetComponentInChildren<HealthCylinder>().UpdateHPBar(stats.health.val);
            GetComponent<ThirdPersonCharacter>().Bleed();
        }
    }

}
