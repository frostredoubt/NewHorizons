﻿﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Ship : NetworkBehaviour
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
    public Vector3 last_pitch_yaw_speed = new Vector3(0, 0, 10.0f);
    public Vector3 Velocity_current = new Vector3(0, 0, 0);
    public Vector3 pitch_yaw_speed;
    public Vector3 max_abs_delta_pitch_yaw_speed = new Vector3(45.0f, 45.0f, 40.0f);
    public Vector3 max_pitch_yaw_speed = new Vector3(90.0f, 90.0f, 100.0f);
    public Vector3 min_pitch_yaw_speed = new Vector3(-90.0f, -90.0f, 10.0f);

    public GameObject player;
    public GameObject Momentum_ray;
    public GameObject Vision_bubble;
    public GameObject Model;
    float Resolve_time;
    Vector3 Update_step;

    Quaternion start_rotation;
    Quaternion end_rotation;

    private bool do_resolve = false;
    private uint Turn_update_units;

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

    [Server]
    public void Start_resolution(uint update_units)
    {
        //Debug.Log("ship Start_resolution");
        Turn_update_units = update_units;

        last_pitch_yaw_speed = pitch_yaw_speed;

        start_rotation = gameObject.transform.rotation;

        Update_step = start_rotation * Velocity_current / update_units;

        Vector3 originalori = new Vector3(0, 1, 0);

        /* Please don't delete my scratch work comments in case this it wrong.
         * 
         * Love,
         * Fil
         */

        //Vector3 currentvecori = start_rotation * originalori;
        Quaternion fromoriginaltotarget = Quaternion.FromToRotation(originalori, Velocity_current.normalized);
        //Vector3 targetvecori = fromoriginaltotarget * start_rotation * originalori;

        //Quaternion delta_rotation = Quaternion.FromToRotation(currentvecori, targetvecori);

        //end_rotation = start_rotation * delta_rotation;

        end_rotation = start_rotation * fromoriginaltotarget;

        //Turn on colliders
        Vision_bubble.GetComponent<SphereCollider>().enabled = true;

        Resolve_time = update_units;
        do_resolve = true;
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lr = Momentum_ray.GetComponent<LineRenderer>();
        //Debug.Log(Velocity_current);
        lr.SetPosition(1, Velocity_current);
    }

    void Finish_resolution()
    {
        do_resolve = false;
        Vision_bubble.GetComponent<SphereCollider>().enabled = false;
    }

    [ServerCallback]
    void FixedUpdate()
    {
        float elapsed_time_fraction = (Turn_update_units - Resolve_time) / Turn_update_units;
        if (do_resolve)
        {
            gameObject.transform.Translate(Update_step,Space.World);
            if (--Resolve_time < 0)
                Finish_resolution();

            Quaternion step = Quaternion.Lerp(start_rotation, end_rotation, elapsed_time_fraction);
            gameObject.transform.rotation = step;
        }

        {
            Quaternion pitchchange = Quaternion.AngleAxis(pitch_yaw_speed.x, new Vector3(1, 0, 0));
            Quaternion yawchange = Quaternion.AngleAxis(pitch_yaw_speed.y, new Vector3(0, 0, 1));

            Vector3 velocity = pitch_yaw_speed.z * (pitchchange * yawchange * (new Vector3(0, 1, 0)));

            Velocity_current = velocity;
        }
    }
}