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
        this.stats.stamina.recieve = this.TakeDamage;
        this.stats.health.onAboveMaximum = this.AboveMax;
    }
    void Update()
    {
    }
    void TakeDamage(Stat self, Stat other, Transform location)
    {
        self.val -= other.val;
    }
    void TakeCritDamage(Stat self, Stat other, Transform location)
    {
        self.val -= other.val * 3;
    }
    void AboveMax(Stat _parent)
    {
        _parent.val = _parent.range.y;
    }
    void OnTriggerEnter(Collider col)
    {

        Character otherCharacter = col.GetComponentInParent<Character>();
        ThirdPersonCharacter otherThirdPerson = col.GetComponentInParent<ThirdPersonCharacter>();

        if (col.CompareTag("HurtBox") && !GetComponent<ThirdPersonCharacter>().CheckIFrames(col))//&& col.gameObject.GetComponentInParent<Character>().tag != this.tag)
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
            HealthCylinder healthThing = GetComponentInChildren<HealthCylinder>();
            if (healthThing != null)
                healthThing.UpdateHPBar(this.stats.health.val);
            GetComponent<ThirdPersonCharacter>().Bleed();

            if (stats.health.val <= 0)
            {
                GetComponent<ThirdPersonCharacter>().Die();
            }
        }
        else if (col.CompareTag("Food") && this.CompareTag("Player"))
        {
            this.stats.health.Increase(10);
            this.stats.health.Validate();
        }
    }

}
