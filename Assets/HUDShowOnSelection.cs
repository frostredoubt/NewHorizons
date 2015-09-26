﻿using UnityEngine;
using System.Collections;

public class HUDShowOnSelection : MonoBehaviour {

	public BenPrimeTest_GameDirector director;
	
	private Canvas thisObject;

	// Use this for initialization
	void Start () {
		thisObject = GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (director) {
			if (director.SelectedShip) {
				thisObject.enabled = true;
			} else {
				thisObject.enabled = false;
			}
		}
	}

}