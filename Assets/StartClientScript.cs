using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class StartClientScript : MonoBehaviour {

	public Text text;
	public UnityEngine.Networking.NetworkManagerHUD networkManagerHUD;
	
	void startTheClient() {
		networkManagerHUD.JoinGame (text.text);
	}
}
