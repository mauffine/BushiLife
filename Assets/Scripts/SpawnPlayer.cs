using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPlayer : MonoBehaviour {
    public GameObject[] respawns;
    public GameObject cameraGenerator;
    public GameObject[] playerPrefab;
    public List<GameObject> players;
	// Use this for initialization
	void Start () {
	    if (respawns == null)
        {
            this.respawns = GameObject.FindGameObjectsWithTag("Respawn");
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Return) && this.players.Count < respawns.Length)
        {
            players.Add((GameObject)Instantiate(playerPrefab[this.players.Count], this.respawns[this.players.Count].transform.position, Quaternion.identity));//Random.Range(0, respawns.Length)].transform.position, Quaternion.identity));
            players[players.Count - 1].GetComponent<ThirdPersonUserControl>().SetPlayerNumber(this.players.Count);
            cameraGenerator.GetComponent<CameraCollection>().Add(players[players.Count - 1], this.players.Count);
        }
	}
}
