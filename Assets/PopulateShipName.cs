using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopulateShipName : MonoBehaviour {

	private Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void shipSelected (Ship ship) {
		text.text = ship.name;
	}
}
