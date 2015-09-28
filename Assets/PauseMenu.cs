using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	public Canvas pauseCanvas;
	public Canvas HUDCanvas;
	public Canvas SlidersCanvas; 

	// Use this for initialization
	void Start () {
		pauseCanvas.enabled = false;
	}
	
	public void pauseGame() {
		pauseCanvas.enabled = true;
		HUDCanvas.enabled = SlidersCanvas.enabled = false;
	}
	
	public void resumeGame() {
		pauseCanvas.enabled = false;
		HUDCanvas.enabled = true;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (pauseCanvas.enabled)
				resumeGame();
			else
				pauseGame();
		}
	}
}
