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
enum UIType
{
    Bar,
    Text
}
public class StatUIScript : MonoBehaviour {
    [SerializeField]
    Character character;
    [SerializeField]
    statType Stat = statType.Health;
    UIType myType;
    float max = 100;
    float min = 0;
    float current = 100;

	// Use this for initialization
	void Start () {
        if (gameObject.GetComponent<Text>() != null)
            this.myType = UIType.Text;
        else
            this.myType = UIType.Bar;
	}
	// Update is called once per frame
	void Update () {
        if (this.myType == UIType.Text)
        {
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
        else
        {
            switch (this.Stat)
            {
                case (statType.Health):
                    {
                        this.current = this.character.stats.health.val;
                        GetComponent<Image>().fillAmount = current / max;
                        break;
                    }
                case (statType.Stamina):
                    {
                        this.current = this.character.stats.stamina.val;
                        GetComponent<Image>().fillAmount = current / max;
                        break;
                    }
                case (statType.Level):
                    {
                        this.current = this.character.stats.level.val;
                        GetComponent<Image>().fillAmount = current / max;
                        break;
                    }
                case (statType.Experience):
                    {
                        this.current = this.character.stats.experience.val;
                        GetComponent<Image>().fillAmount = current / max;
                        break;
                    }
                default:
                    break;
            }
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
