using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FilTestNetworkShipMovement : NetworkBehaviour
{

    public Vector3 velocity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isServer)
        {
            transform.Translate(velocity);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        velocity = Vector3.zero;
    }
}
