using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	public void Exit() {
		UnityEditor.EditorApplication.isPlaying = false; //makes the editor stop playing game too
		Application.Quit ();
	}
}
