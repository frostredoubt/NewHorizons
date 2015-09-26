using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FilTestNetworkMovement : NetworkBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (NetworkClient.active)
        {
            if (!isLocalPlayer)
                return;

            if (GameManagerScript.singleton.GameStarted())
            {
                UpdateClientGame();
            }
            else
            {
                UpdateClientLobby();
            }
        }
	}

    void UpdateClientGame()
    {
        if (Input.GetKeyDown("w"))
            transform.Translate(Vector3.forward * 0.05f);
    }

    void UpdateClientLobby()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            CmdStartGame();
    }

    [Command]
    void CmdStartGame()
    {
        GameManagerScript.singleton.StartGame();
    }
}
