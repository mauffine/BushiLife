using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent agent;
    AIController ai;
    float currentLeadDist;
    public float leadDist;
    public float passiveLeadDist;
    Timer wanderTimer = new Timer();
    float wanderTime = 1f;
    float minLeadDist = 0.3f;

    void Start()
    {
    }

    public void Init(AIController ai)
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.ai = ai;
        this.currentLeadDist = this.passiveLeadDist;

        this.ValidatePosition();
    }

    void Update()
    {
        this.UpdateDestination();

        this.ValidatePosition();
    }

    void ValidatePosition()
    {
        this.ai.target = this.transform.position;

        var displacement = this.transform.position - this.ai.transform.position;

        var currentDistance = displacement.magnitude;
        var direction = displacement.normalized;

        if (currentDistance > this.currentLeadDist || currentDistance < this.minLeadDist)
        {
            Teleport(this.ai.transform.position + direction * this.minLeadDist);
        }
    }

    void UpdateDestination()
    {
        var players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        if (players.Contains(this.ai.gameObject))
            players.Remove(this.ai.gameObject);

        if (players.Count > 0)
        {
            if (this.wanderTimer.running)
            {
                this.wanderTimer.Stop();

                this.agent.speed -= 2f;
                this.agent.angularSpeed -= 20f;
                this.currentLeadDist = this.leadDist;
            }
            this.ai.isPassive = false;

            players.Sort((t1, t2) => SocketContainer.Closer(t1.transform, t2.transform, this.transform));
            

            if (this.agent.enabled)
                if (this.agent.isOnNavMesh)
                    this.agent.SetDestination(players[0].transform.position);
        }
        else
        {
            this.ai.isPassive = true;
            if (!this.wanderTimer.running)
            {
                this.wanderTimer.Start();
                this.agent.speed += 2f;
                this.agent.angularSpeed += 20f;
                this.currentLeadDist = this.passiveLeadDist;
            }

            if (this.wanderTimer.ElapsedTime() > this.wanderTime)
            {
                this.wanderTimer.Start();
                if (this.agent.enabled)
                    if (this.agent.isOnNavMesh)
                        Wander();

            }
        }
    }

    void Teleport(Vector3 position)
    {
        this.agent.enabled = false;
        this.transform.position = position;
        this.agent.enabled = true;
    }

    public void Wander(float distance=10f)
    {
        var direction = Turn(Random.Range(-0.9f, 0.9f), Random.Range(0, 300) == 0);

        var newTarget = this.transform.position + direction * distance;
        
        this.agent.SetDestination(newTarget);
    }

    /// <summary>
    /// </summary>
    /// <param name="amount">negative for left, positive for right, -1 thru 1</param>
    /// <returns></returns>
    public Vector3 Turn(float amount, bool forwards = true)
    {
        Vector3 from = this.transform.forward;
        Vector3 to = this.transform.right;

        Vector3 newDirection;

        // if negative, turn left not right
        if (Mathf.Abs(amount) != amount)
        {
            to *= -1f;
        }

        if (!forwards)
            from *= -1f;

        newDirection = Vector3.Slerp(from, to, Mathf.Abs(amount));
        return newDirection;
    }
}
