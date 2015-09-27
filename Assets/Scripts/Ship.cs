﻿using UnityEngine;
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

    public ParticleSystem engine1;
    public ParticleSystem engine2;
    public ParticleSystem engine3;
    public ParticleSystem engine4;

    public bool Weapons_enabled = false;
    public float Scout_range = 1;
    public float Weapon_range = 1;

    public Type Ship_type;

	public float health = 100.0f;
	public string name;

    //Max values
    // (Speed, Pitch, Yaw)

    [SyncVar]
    public Vector3 Velocity_current = new Vector3(0, 0, 0);

    [SyncVar]
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
    private AudioSource shipWhoosh;

    // Use this for initialization
    void Start()
    {
        Momentum_ray = transform.FindChild("Momentum").gameObject;
        Vision_bubble = transform.FindChild("Vision").gameObject;
        Model = transform.FindChild("Model").gameObject;
        shipWhoosh = transform.FindChild("ShipWhoosh").GetComponent<AudioSource>();
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

    [ClientRpc]
    void RpcStartMoveFX()
    {
        if (!engine1.IsAlive())
        {
            engine1.Play();
        }
        if (!engine2.IsAlive())
        {
            engine2.Play();
        }
        if (!engine3.IsAlive())
        {
            engine3.Play();
        }
        if (!engine4.IsAlive())
        {
            engine4.Play();
        }

        shipWhoosh.Play();
    }

    [ClientRpc]
    void RpcStopMoveFX()
    {
        engine1.Stop();
        engine2.Stop();
        engine3.Stop();
        engine4.Stop();
    }

    [Server]
    public void Start_resolution(uint update_units)
    {

        RpcStartMoveFX();

        //Debug.Log("ship Start_resolution");
        Turn_update_units = update_units;

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
    }

    void Finish_resolution()
    {
        RpcStopMoveFX();

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
    }
}