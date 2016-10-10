using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject template;
    public int size = 20;
    public bool willGrow = true;

    List<GameObject> objects = null;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        this.objects = new List<GameObject>();
        for (int i = 0; i < this.size; i++)
        {
            var item = Instantiate(this.template) as GameObject;

            item.SetActive(false);
            this.objects.Add(item);
        }
    }

    public GameObject Get()
    {
        for (int i = 0; i < this.objects.Count; i++)
        {
            if (!this.objects[i].activeInHierarchy)
            {
                UnityEditor.PrefabUtility.ResetToPrefabState(this.objects[i]);
                this.objects[i].SetActive(true);
                return this.objects[i];
            }
        }

        if (this.willGrow)
        {
            var item = Instantiate(this.template) as GameObject;
            this.objects.Add(item);
            return item;
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
