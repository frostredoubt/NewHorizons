using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class GameManagerScript : NetworkBehaviour {

    static public GameManagerScript singleton;

    [SyncVar]
    bool game_started;

	// Use this for initialization
	void Start () {
        singleton = this;
        if(isServer)
            game_started = false;
	}

    public bool GameStarted()
    {
        return game_started;
    }

    [Server]
    public void StartGame()
    {
        if (!game_started)
        {
            Debug.Log("Game started");
            game_started = true;
        }
        else
        {
            Debug.Log("Game already started");
        }
    }
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {
	}
}
