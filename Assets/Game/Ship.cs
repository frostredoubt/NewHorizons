using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public enum Type
    {
        SCOUT,
        CRUISER,
        KING
    };

    public Vector3 momentum = new Vector3(0, 0, 0);
    public bool Weapons_enabled = false;
    public float Scout_range = 1;
    public float Weapon_range = 1;
    public Vector3 Direction_input;
    public Type Ship_type;

    public GameObject Momentum_ray;
    public GameObject Vision_bubble;
    float Resolve_time;
    Vector3 Update_step;

    // Use this for initialization
    void Start()
    {
        Momentum_ray = transform.FindChild("Momentum").gameObject;
        Vision_bubble = transform.FindChild("Vision").gameObject;
        //Vision_bubble.SetActive(false);
    }

    void Start_resolution( uint update_units )
    {
        Debug.Log("ship resolve " + update_units );
        Update_step = momentum / update_units;
        Resolve_time = update_units;
    }

    void Selected()
    {
        Vision_bubble.SetActive(true);
    }

    void Deselect()
    {
        Vision_bubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("[1]"))
            momentum += (Input.GetKey("left shift")) ? Vector3.right : Vector3.left;
        else if (Input.GetKeyDown("[2]"))
            momentum += (Input.GetKey("left shift")) ? Vector3.up : Vector3.down;
        else if (Input.GetKeyDown("[3]"))
            momentum += (Input.GetKey("left shift")) ? Vector3.forward : Vector3.back;

        LineRenderer lr = Momentum_ray.GetComponent<LineRenderer>();
        lr.SetPosition(1, momentum);
    }

    void FixedUpdate()
    {
        if (Resolve_time > 0)
        {
            gameObject.transform.Translate(Update_step);
            --Resolve_time;

            //Check for new vision
            Vision_bubble.GetComponent<SphereCollider>();

            //Check for firing opportunity
        }
        else
        {
            Resolve_time = 0;
        }
    }
}