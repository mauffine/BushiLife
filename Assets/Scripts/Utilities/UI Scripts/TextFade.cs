using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TextFade : MonoBehaviour {
    public float fadeRate = 1;
    Text myText;
	// Use this for initialization
	void Start () {
        myText = gameObject.GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        if (myText.color.a >= 0)
            myText.color = new Color(
                myText.color.r,
                myText.color.g,
                myText.color.b,
                myText.color.a - Time.deltaTime * fadeRate);
	}
    public void SetText(string _text)
    {
        myText.text = _text;
        myText.color = new Color(
            myText.color.r,
            myText.color.g,
            myText.color.b,
            1);
        return;
    }
}
