using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    static public CustomNetworkManager s_Singleton;

    void Start()
    {
        s_Singleton = this;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Player " + conn.connectionId + " connected.");

        var player = (GameObject)GameObject.Instantiate(playerPrefab, startPositions[0].position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

}
