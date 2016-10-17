using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    public GameObject template;
    public int size = 20;
    public bool willGrow = true;

    List<GameObject> objects = null;

    public int ActiveSize
    {
        get
        {
            List<GameObject> ais = new List<GameObject>(GameObject.FindGameObjectsWithTag("AI"));
            return ais.Where(ai => ai.activeInHierarchy).ToList().Count;
        }
    }

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
