using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JoinGameScript : MonoBehaviour {

	public Canvas joinCanvas;

	// Use this for initialization
	void Start () {
		joinCanvas.enabled = false;
	}

	public void joinGame() {
		joinCanvas.enabled = true;
	}

	public void cancelJoinGame() {
		joinCanvas.enabled = false;
	}
}
