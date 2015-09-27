using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BenPrimeTest_GameDirector : MonoBehaviour {

    public static BenPrimeTest_GameDirector singleton;

	private Ship selectedShip;
	public Ship SelectedShip { get { return selectedShip; } }

    public void Start() {
        singleton = this;
    }

	public void nextTurn() {
		selectedShip = null;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.BroadcastMessage("Player_start_resolution");
        }
	}
	
	public void setSelectedShip(Ship ship) {
		selectedShip = ship;
		if (ship != null) {
			BroadcastMessage("shipSelected", ship);
		}
	}

}
