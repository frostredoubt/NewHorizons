using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopulateShipHealth : MonoBehaviour {

	Slider slider;

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
	}
	
	void shipSelected (Ship ship) {
		slider.value = ship.health;
	}
}
