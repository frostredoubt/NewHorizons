using UnityEngine;
using System.Collections;

public class Vision : MonoBehaviour {

    GameObject Vision_object;
    GameObject Model_object;

	// Use this for initialization
	void Start () {
        Vision_object = gameObject.transform.parent.FindChild("Vision").gameObject;
        Model_object = gameObject.transform.parent.FindChild("Model").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnTriggerStay( Collider other )
    {
        
    }

    void OnTriggerEnter( Collider other)
    {
        if (gameObject.name == "Vision" && other.name == "Model")
        {
            GameObject my_parent = gameObject.transform.parent.gameObject;
            GameObject their_parent = other.transform.parent.gameObject;
            if (my_parent != their_parent)
            {
                Ship a = my_parent.GetComponent("Ship") as Ship;
                Ship b = their_parent.GetComponent("Ship") as Ship;

                if (a.Player_id == 1 && b.Player_id == 2)
                {
                    b.Set_model_visible(true);
                    Debug.Log("Seeing enemy ship");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (gameObject.name == "Vision" && other.name == "Model")
        {
            GameObject my_parent = gameObject.transform.parent.gameObject;
            GameObject their_parent = other.transform.parent.gameObject;
            if (my_parent != their_parent)
            {
                Ship a = my_parent.GetComponent("Ship") as Ship;
                Ship b = their_parent.GetComponent("Ship") as Ship;

                if (a.Player_id == 1 && b.Player_id == 2)
                {
                    b.Set_model_visible(false);
                    Debug.Log("Not seeing enemy ship");
                }
            }
        }
    }

    void OnMouseEnter()
    {
        Ship s = gameObject.transform.parent.GetComponent("Ship") as Ship;
        if (gameObject.name == "Model" && s.Player_id == 1 )
        {
            Vision_object.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void OnMouseExit()
    {
        Ship s = gameObject.transform.parent.GetComponent("Ship") as Ship;
        if ( gameObject.name == "Model" && s.Player_id == 1 )
        {
            Vision_object.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
