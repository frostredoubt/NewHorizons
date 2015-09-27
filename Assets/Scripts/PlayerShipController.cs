using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// -- This class was added so client can have network authority over ship /commands/
// -- but not ship movement

public class PlayerShipController : NetworkBehaviour {

    public Vector3 last_pitch_yaw_speed = new Vector3(0, 0, 10.0f);
    public Vector3 pitch_yaw_speed;
    public Vector3 max_abs_delta_pitch_yaw_speed = new Vector3(45.0f, 45.0f, 40.0f);
    public Vector3 max_pitch_yaw_speed = new Vector3(90.0f, 90.0f, 100.0f);
    public Vector3 min_pitch_yaw_speed = new Vector3(-90.0f, -90.0f, 10.0f);

    [SyncVar]
    public bool finishedenteringmoves;

    // Use this for initialization
    [ClientCallback]
	void Start () {
	
	}

    // Update is called once per frame
    [ClientCallback]
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (!BenPrimeTest_GameDirector.singleton)
        {
            return;
        }

        Ship ship = BenPrimeTest_GameDirector.singleton.SelectedShip;

        if (!ship)
            return;

        {
            Quaternion pitchchange = Quaternion.AngleAxis(pitch_yaw_speed.x, new Vector3(1, 0, 0));
            Quaternion yawchange = Quaternion.AngleAxis(pitch_yaw_speed.y, new Vector3(0, 0, 1));

            Vector3 velocity = pitch_yaw_speed.z * (pitchchange * yawchange * (new Vector3(0, 1, 0)));

            CmdSetVelocity(ship.gameObject, velocity);
        }

        if (ship.player == Game.singleton.local_player)
        {
            LineRenderer lr = ship.Momentum_ray.GetComponent<LineRenderer>();
            //Debug.Log(Velocity_current);
            lr.SetPosition(1, ship.Velocity_current);
        }
    }

    [Command]
    void CmdSetVelocity(GameObject shipgo, Vector3 v)
    {
        shipgo.GetComponentInChildren<Ship>().Velocity_current = v;
    }

    [Client]
    public void Player_start_resolution()
    {
        if (isLocalPlayer)
        {
            last_pitch_yaw_speed = pitch_yaw_speed;
            CmdStartResolution();
        }
    }

    [Command]
    private void CmdStartResolution()
    {
        finishedenteringmoves = true;
        Game.singleton.StartResolution(gameObject);
    }
}
