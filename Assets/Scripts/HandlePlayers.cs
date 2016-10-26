using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public enum PlayState
{
    Start,
    Playing,
    End
}
public class HandlePlayers : MonoBehaviour {
    public GameObject[] respawns;
    public GameObject cameraGenerator;
    public GameObject[] playerPrefab;
    public List<GameObject> players;
    public PlayState currentState;
    public Text joinNotification;
    public Image winText;
    public Sprite[] victorySprites;

    public float endGameTimer = 5;

    private bool[] PlayerJoined = { false, false, false, false };

	// Use this for initialization
	void Start () {
	    if (respawns == null)
        {
            this.respawns = GameObject.FindGameObjectsWithTag("Respawn");
        }
        this.currentState = PlayState.Start;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.currentState == PlayState.Start)
        {
            //set players that will spawn when the game starts
            if (Input.GetButtonDown("P1 Join") && !PlayerJoined[0])
            {
                joinNotification.GetComponent<TextFade>().SetText("Player" + 1 + " has Joined");
                this.PlayerJoined[0] = true;
            }
            if (Input.GetButtonDown("P2 Join") && !PlayerJoined[1])
            {
                joinNotification.GetComponent<TextFade>().SetText("Player" + 2 + " has Joined");
                this.PlayerJoined[1] = true;
            }
            if (Input.GetButtonDown("P3 Join") && !PlayerJoined[2])
            {
                joinNotification.GetComponent<TextFade>().SetText("Player" + 3 + " has Joined");
                this.PlayerJoined[2] = true;
            }
            if (Input.GetButtonDown("P4 Join") && !PlayerJoined[3])
            {
                joinNotification.GetComponent<TextFade>().SetText("Player" + 4 + " has Joined");
                this.PlayerJoined[3] = true;
            }
            //spawn the players and start the game
            if (Input.GetButtonDown("P1 Jump") && PlayerJoined[0])
            {
                SpawnPlayers();
                this.currentState = PlayState.Playing;
            }
            if (Input.GetButtonDown("P2 Jump") && PlayerJoined[1])
            {
                SpawnPlayers();
                this.currentState = PlayState.Playing;
            }
            if (Input.GetButtonDown("P3 Jump") && PlayerJoined[2])
            {
                SpawnPlayers();
                this.currentState = PlayState.Playing;
            }
            if (Input.GetButtonDown("P4 Jump") && PlayerJoined[3])
            {
                SpawnPlayers();
                this.currentState = PlayState.Playing;
            }
        }
        if(this.currentState == PlayState.Playing)
        {
            if (!PlayersAlive())
            {
                currentState = PlayState.End;
            }
        }
        if(this.currentState == PlayState.End)
        {
            endGameTimer -= Time.deltaTime;
            if (endGameTimer < 0)
                gameObject.GetComponent<ReloadLevel>().ReloadScene();
        }
        
	}
    public void SpawnPlayers()
    {
        for(int i = 0; i < PlayerJoined.Length; ++i)
        {
            if (PlayerJoined[i] == true)
            {
                players.Add((GameObject)Instantiate(playerPrefab[i], this.respawns[i].transform.position, Quaternion.identity));
                var player = players[players.Count - 1].GetComponent<ThirdPersonUserControl>();
                if (player != null)
                    player.SetPlayerNumber(i+1);
                cameraGenerator.GetComponent<CameraCollection>().Add(players[players.Count - 1], i+1);
            }
        }
    }
    bool PlayersAlive()
    {
        List<GameObject> alivePlayers = new List<GameObject>();
        foreach(GameObject player in players)
        {
            if (player.CompareTag("Player"))
                alivePlayers.Add(player);
        }
        if (alivePlayers.Count > 1)
            return true;
        switch (alivePlayers[0].GetComponent<ThirdPersonUserControl>().playerNumber)
        {
            case ("P1"):
                {
                    winText.enabled = true;
                    winText.sprite = victorySprites[0];
                    break;
                }
            case ("P2"):
                {
                    winText.enabled = true;
                    winText.sprite = victorySprites[1];
                    break;
                }
            case ("P3"):
                {
                    winText.enabled = true;
                    winText.sprite = victorySprites[2];
                    break;
                }
            case ("P4"):
                {
                    winText.enabled = true;
                    winText.sprite = victorySprites[3];
                    break;
                }
            default:
                break;
        }
        return false;
    }
}
