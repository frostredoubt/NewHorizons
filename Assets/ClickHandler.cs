using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler {
	
	public BenPrimeTest_GameDirector director;

	private Ship ship;

	void Start () {
		ship = GetComponentInParent<Ship> ();
	}

	public void OnPointerClick (PointerEventData eventData) {
		director.setSelectedShip(ship);
	}
}
