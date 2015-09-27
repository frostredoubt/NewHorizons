using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

    public ParticleSystem ps;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) {
            if (!ps.IsAlive())
            {
                ps.Play();
            }
        }
        if (Input.GetKeyUp("space"))
        {
            ps.Stop();
        }
    }
}
