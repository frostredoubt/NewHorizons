using UnityEngine;
using System.Collections;

public class BenPrimeTest_GameDirector : MonoBehaviour {

	private BenPrimeTest_PropertiesStruct selectedShip;
	public BenPrimeTest_PropertiesStruct SelectedShip { get { return selectedShip; } }

	void Update() {
		if (Input.GetKeyUp (KeyCode.Space)) {
			selectedShip = null;
   			BroadcastMessage("newTurn");
		}
	}

	public void setSelectedShip(BenPrimeTest_PropertiesStruct ship) {
		selectedShip = ship;
		if (ship != null) {
			BroadcastMessage("shipSelected", ship);
		}
	}

}
