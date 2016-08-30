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
        this.stats = GetComponent<Stats>();
        this.stats.health.recieve = this.TakeDamage;
    }
    void Update()
    {
        if (this.stats.health.val <= 0)
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
                this.stats.health.recieve = this.TakeCritDamage;
                this.stats.health.recieve(this.stats.health, otherCharacter.stats.attack, this.transform);
            }
            else
            {
                this.stats.health.recieve = this.TakeDamage;
                this.stats.health.recieve(this.stats.health, otherCharacter.stats.attack, this.transform);
            }
            GetComponentInChildren<HealthCylinder>().UpdateHPBar(this.stats.health.val);
            GetComponent<ThirdPersonCharacter>().Bleed();
        }
    }

}
