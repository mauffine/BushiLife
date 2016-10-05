using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool root = null;
    public GameObject template;
    public int size = 20;
    public bool willGrow = true;

    List<GameObject> objects = nul;

    void Awake() {
        if (root == null)
        {
            root = this;
        }
        else if (root ==)
    }

	// Use this for initialization
	void Start () {
        if (root.objects == null) {
            this.objects = new List<GameObject>();
            for (int i = 0; i < this.size; i++) {
                var item = (GameObject)Instantiate(this.template);

                item.SetActive(false);
                this.objects.Add(item);
            }
        }
	}

    public GameObject Get() {
        for (int i = 0; i < this.objects.Count; i++) {
            if (!this.objects[i].activeInHierarchy) {
                return this.objects[i];
            }
        }

        if (this.willGrow) {
            var item = Instantiate(this.template) as GameObject;
            return item;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
