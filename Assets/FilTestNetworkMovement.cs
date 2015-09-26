using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FilTestNetworkMovement : NetworkBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (NetworkClient.active)
            UpdateClient();
	}

    void UpdateClient()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown("w"))
        {
            transform.Translate(Vector3.forward * 0.05f);
        }
    }
}
