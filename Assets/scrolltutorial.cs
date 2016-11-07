using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class scrolltutorial : MonoBehaviour {
    public Image tutorial;
    public Sprite[] tutorialImages;
    public float swapTime;
    float timer;
    int tutorialIndex;
    // Use this for initialization
	void Start ()
    {
        timer = swapTime;

    }
	
	// Update is called once per frame
	void Update () {
        if (timer < 0)
        {
            timer = swapTime;
            if (tutorialIndex < tutorialImages.Length -1)
            {
                ++tutorialIndex;
                tutorial.sprite = tutorialImages[tutorialIndex];
            }
            else
            {
                tutorialIndex = 0;
                tutorial.sprite = tutorialImages[tutorialIndex];
            }
        }
        else
            timer -= Time.deltaTime;
	}
}
