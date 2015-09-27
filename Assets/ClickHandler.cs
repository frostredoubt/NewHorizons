using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class ClickHandler : NetworkBehaviour, IPointerClickHandler {
	
	public BenPrimeTest_GameDirector director;

	private Ship ship;

	void Start () {
		ship = GetComponentInParent<Ship> ();
	}

    [ClientCallback]
	public void OnPointerClick (PointerEventData eventData) {
        /*if (!director)
            director = BenPrimeTest_GameDirector.singleton;

        if (ship.player == Game.singleton.local_player)
        {
            director.setSelectedShip(ship);
        }*/
	}
}
