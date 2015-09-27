﻿﻿using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public enum Type
    {
        SCOUT,
        CRUISER,
        KING
    };


    public bool Weapons_enabled = false;
    public float Scout_range = 1;
    public float Weapon_range = 1;

    public Type Ship_type;

    //Max values
    // (Speed, Pitch, Yaw)
    public Vector3 Velocity_current = new Vector3(0, 0, 0);
    public Vector3 Velocity_delta;
    public Vector3 Max_velocity;
    public Vector3 Min_velocity;
    public Vector3 Max_velocity_delta;
    public Vector3 Min_velocity_delta;

    public uint Player_id;
    public GameObject Momentum_ray;
    public GameObject Vision_bubble;
    public GameObject Model;
    float Resolve_time;
    Vector3 Update_step;

    Quaternion start_rotation;
    Quaternion end_rotation;

    private bool shouldReset = false;
    private bool do_resolve = false;

    // Use this for initialization
    void Start()
    {
        Momentum_ray = transform.FindChild("Momentum").gameObject;
        Vision_bubble = transform.FindChild("Vision").gameObject;
        Model = transform.FindChild("Model").gameObject;
    }

    public void Set_model_visible( bool vis )
    {
        Model.GetComponent<MeshRenderer>().enabled = vis;
    }

    public void Set_all_visible( bool visibility )
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
                r.enabled = visibility;
    }

    void Start_resolution(uint update_units)
    {
        start_rotation = gameObject.transform.rotation;

        Update_step = start_rotation * Velocity_current / update_units;

        Vector3 originalori = new Vector3(0, 1, 0);

        Vector3 currentvecori = start_rotation * originalori;
        Vector3 targetvecori = start_rotation * Velocity_current.normalized;

        Quaternion delta_rotation = Quaternion.FromToRotation(currentvecori, targetvecori);

        end_rotation = start_rotation * delta_rotation;

        //Turn on colliders
        Vision_bubble.GetComponent<SphereCollider>().enabled = true;

        Resolve_time = update_units;
        do_resolve = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("[1]"))
            Velocity_current += (Input.GetKey("left shift")) ? Vector3.right : Vector3.left;
        else if (Input.GetKeyDown("[2]"))
            Velocity_current += (Input.GetKey("left shift")) ? Vector3.up : Vector3.down;
        else if (Input.GetKeyDown("[3]"))
            Velocity_current += (Input.GetKey("left shift")) ? Vector3.forward : Vector3.back;

        LineRenderer lr = Momentum_ray.GetComponent<LineRenderer>();
        lr.SetPosition(1, Velocity_current);
    }

    void FixedUpdate()
    {
        float elapsed_time_fraction = (20 - Resolve_time) / 20;
        if (do_resolve)
        {
            gameObject.transform.Translate(Update_step,Space.World);
            if (--Resolve_time < 0)
                do_resolve = false;

            //Quaternion step = Quaternion.Lerp(start_rotation, end_rotation, elapsed_time_fraction);
            //gameObject.transform.rotation = step;

            //Check for new vision
            Vision_bubble.GetComponent<SphereCollider>();

            //Check for firing opportunity

            shouldReset = true;
        }
        else
        {
            //add the delta velocity and reset the delta for the next turn
            if (shouldReset)
            {
                Vision_bubble.GetComponent<SphereCollider>().enabled = false;
                Velocity_current += Velocity_delta;
                Velocity_delta = new Vector3(0, 0, 0);
                shouldReset = false;
            }
        }
    }
}