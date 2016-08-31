using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	SocketContainer spawnpointGenerator;
	public bool spawnEnemy = false;

	public GameObject enemyPrefab;
    Timer timer = new Timer();
    public float spawnDelay;

    int numEnemies;
    public int maxEnemies;

	// Use this for initialization
	void Start ()
	{
		this.spawnpointGenerator = this.transform.FindChild("Sockets").GetComponent<SocketContainer>();
        timer.Start();
	}
	
	// Update is called once per frame
	void Update ()
	{	
		if (this.spawnEnemy)
		{
			this.SpawnEnemy();
			this.spawnEnemy = false;
		}

        if (this.timer.ElapsedTime() > this.spawnDelay)
        {
            this.SpawnEnemy();
            this.timer.Start();
        }

	}

	void SpawnEnemy()
	{
        if (this.numEnemies < this.maxEnemies)
        {
            this.numEnemies++;
            var pos = this.spawnpointGenerator.RandomSpawnPoint();

            GameObject.Instantiate(this.enemyPrefab, pos, this.enemyPrefab.transform.rotation);
        }
    }
}
