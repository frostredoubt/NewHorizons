using UnityEngine;
using System.Collections;

public class BenPrimeTest_GameDirector : MonoBehaviour {

	private Ship selectedShip;
	public Ship SelectedShip { get { return selectedShip; } }

	public void nextTurn() {
		selectedShip = null;
		BroadcastMessage("Start_resolution", 10U);
	}
	
	public void setSelectedShip(Ship ship) {
		selectedShip = ship;
		if (ship != null) {
			BroadcastMessage("shipSelected", ship);
		}
	}

}
