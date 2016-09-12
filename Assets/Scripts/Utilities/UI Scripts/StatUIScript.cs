using UnityEngine;
using System.Collections;
using UnityEngine.UI;
enum statType
{
    Health,
    Stamina,
    Experience,
    Level
}
public class StatUIScript : MonoBehaviour {
    [SerializeField]
    Character character;
    [SerializeField]
    statType Stat = statType.Health;
    float max = 100;
    float min = 0;
    float current = 100;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        switch (this.Stat)
        {
            case (statType.Health):
                {
                    this.current = this.character.stats.health.val;
                    gameObject.GetComponent<Text>().text = "health:" + this.max.ToString() + "/" + this.current.ToString();
                    break;
                }
            case (statType.Stamina):
                {
                    this.current = this.character.stats.stamina.val;
                    gameObject.GetComponent<Text>().text = "Stamina:" + this.max.ToString() + "/" + this.current.ToString();
                    break;
                }
            case (statType.Level):
                {
                    this.current = this.character.stats.level.val;
                    gameObject.GetComponent<Text>().text = "Level:" + this.current.ToString();
                    break;
                }
            case (statType.Experience):
                {
                    this.current = this.character.stats.experience.val;
                    gameObject.GetComponent<Text>().text = "Exp:" + this.max.ToString() + "/" + this.current.ToString();
                    break;
                }
            default:
                break;
        }
       
	}

    public void SetCharacter(GameObject _player)
    {
        this.character = _player.GetComponent<Character>();
        switch (this.Stat)
        {
            case (statType.Health):
                {
                    this.current = this.character.stats.health.val;
                    break;
                }
            case (statType.Stamina):
                {
                    this.current = this.character.stats.stamina.val;
                    break;
                }
            default:
                break;
        }
        this.current = this.character.stats.stamina.val;
    }
}
