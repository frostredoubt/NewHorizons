using UnityEngine;
using System.Collections;

public class Vision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnTriggerStay( Collider other )
    {
        //Debug.Log("Collision");
    }

    void OnTriggerEnter( Collider other)
    {
        if( gameObject.name == "Model" && other.name == "Vision" )
            Debug.Log("Model detected collision");
        if (gameObject.name == "Vision" && other.name == "Model" )
            Debug.Log("Vision detected collision");
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("Collision");
    }
}
