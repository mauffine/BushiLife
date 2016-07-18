using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SocketContainer : MonoBehaviour
{
    public List<Transform> sockets;

	// Use this for initialization
	void Start()
    {
		this.Validate();

    }
	
	// Update is called once per frame           oo
	void Update()
    {
       
	}

    public Vector3 RandomSpawnPoint()
    {
        
        var triangle = this.RandomSocketTriangle();

        var lerp1 = Random.value;
        var lerp2 = Random.value;

        var first = Vector3.Lerp(triangle[0].position, triangle[1].position, lerp1);
        var second = Vector3.Lerp(first, triangle[2].position, lerp2);

        return second;
    }

    public void Validate()
    {
        sockets = this.Sockets();
    }

    public List<Transform> Sockets()
    {
        var result = new List<Transform>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            result.Add(this.transform.GetChild(i));
        }

        return result;
    }

    public static int Closer(Transform t1, Transform t2, Transform self)
    {
        return Vector3.Distance(t1.position, 
                                self.position)
            .CompareTo(
                Vector3.Distance(
                    t2.position,
                    self.position));
    } 

    public List<Transform> SocketTriangle(Transform rootSocket)
    {
        var closestSockets = new List<Transform>(sockets);
        closestSockets.Sort((t1, t2) => Closer(t1, t2, rootSocket));

        return new List<Transform> { rootSocket, closestSockets[1], closestSockets[2] };
    }

    public List<Transform> RandomSocketTriangle()
    {
        return this.SocketTriangle(this.RandomSocket());
    }

    public Transform RandomSocket()
    {
        if (this.sockets.Count == 0)
        {

            if (this.sockets.Count == 0)
                return null;
        }
        var rootSocketIndex = Random.Range(0, this.sockets.Count - 1);
        var result = this.sockets[rootSocketIndex];

        return result;
    }
}
