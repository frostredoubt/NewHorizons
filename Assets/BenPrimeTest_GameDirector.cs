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
		setSelectedShip (null);

        Game.singleton.local_player.BroadcastMessage("Player_start_resolution");
	}
	
	public void setSelectedShip(Ship ship) {
		if (selectedShip)
			selectedShip.drawArrow(false);
		selectedShip = ship;
		if (selectedShip) {
			selectedShip.drawArrow(true);
			BroadcastMessage("shipSelected", ship);
		}
	}

}
