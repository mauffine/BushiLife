using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	SocketContainer spawnpointGenerator;
	public bool spawnEnemy = false;

	public GameObject enemyPrefab;

	// Use this for initialization
	void Start ()
	{
		this.spawnpointGenerator = this.transform.FindChild("Sockets").GetComponent<SocketContainer>();
	}
	
	// Update is called once per frame
	void Update ()
	{	
		if (this.spawnEnemy)
		{
			this.SpawnEnemy();
			this.spawnEnemy = false;
		}
	}

	void SpawnEnemy()
	{
		var pos = this.spawnpointGenerator.RandomSpawnPoint();

		GameObject.Instantiate(this.enemyPrefab, pos, this.enemyPrefab.transform.rotation);
	}
}
