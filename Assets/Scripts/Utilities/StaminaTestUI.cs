using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class StaminaTestUI : MonoBehaviour {
    public Character character;

    float min, max;
	// Use this for initialization
	void Start () {
        min = 0;
        max = character.stats.stamina.val;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
