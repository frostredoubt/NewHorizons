using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BenPrimeTest_SelectNothing : MonoBehaviour, IPointerClickHandler {
		
	public BenPrimeTest_GameDirector director;
	
	public void OnPointerClick (PointerEventData eventData) {
		director.setSelectedShip(null);
	}

}
