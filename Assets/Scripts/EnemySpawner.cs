using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	SocketContainer spawnpointGenerator;
	public bool spawnEnemy = false;
    Timer timer = new Timer();
    public float spawnDelay;


    public ObjectPool enemyPool;
    public int maxEnemies;



	// Use this for initialization
	void Start ()
	{
        this.enemyPool = GetComponent<ObjectPool>();
		this.spawnpointGenerator = this.transform.FindChild("Sockets").GetComponent<SocketContainer>();
        this.timer.Start();
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
        if (this.enemyPool.ActiveSize < this.maxEnemies)
        {
            var pos = this.spawnpointGenerator.RandomSpawnPoint();

            var enemy = this.enemyPool.Get();
            enemy.GetComponent<AIController>().Init(pos, this);
        }
    }
}
