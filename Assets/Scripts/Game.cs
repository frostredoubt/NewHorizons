using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Game : NetworkBehaviour
{
    [System.NonSerialized]
    static public Game singleton;

    [System.NonSerialized]
    public GameObject local_player;

    [SyncVar]
    bool game_started;

    uint Turn_count;
    public GameObject ScoutShips;
    public GameObject KingShips;
    public GameObject CruiserShips;

    List<GameObject> ships = new List<GameObject>();

    public Vector3 My_start = new Vector3(0, 0, 0);
    public Vector3 Enemy_start = new Vector3(10, 10, 10);

    public GameObject UX;

    public bool GameStarted()
    {
        return game_started;
    }

    public void StartGame()
    {
        if (!game_started)
        {
            Debug.Log("Game started");

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log(players.Length);

            int count = 0;
            foreach ( GameObject player in players )
                SpawnShip(player, new Vector3(-11, 10+(count+=10), 11), Ship.Type.SCOUT);

            game_started = true;
        }
        else
        {
            Debug.Log("Game already started");
        }
    }

    [Server]
    void SpawnShip( GameObject player, Vector3 position, Ship.Type type )
    {
        GameObject ship;
        GameObject prefab;

        if (type == Ship.Type.CRUISER)
            prefab = CruiserShips;
        else if (type == Ship.Type.KING)
            prefab = KingShips;
        else
            prefab = ScoutShips;

        // generate a location around the daddy ship
        ship = (GameObject)Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        ship.transform.position = position;

        Ship shipcomp = ((Ship)ship.GetComponent("Ship"));
        shipcomp.Ship_type = type;
        shipcomp.player = player;

        // -- set up director for clickhandlers
        ClickHandler[] clickhandlers = ship.GetComponentsInChildren<ClickHandler>();
        foreach (ClickHandler ch in clickhandlers)
        {
            ch.director = BenPrimeTest_GameDirector.singleton;
        }

        NetworkServer.Spawn(ship);

        ships.Add(ship);

        /*if (friendly)
        {
            ((Ship)ship.GetComponent("Ship")).Player_id = 1;
            My_ships.Add(ship);
        }
        else
        {
            ((Ship)ship.GetComponent("Ship")).Player_id = 2;
            ((Ship)ship.GetComponent("Ship")).Set_all_visible(false);
            Enemy_shipsp.Add(ship);
            Debug.Log("Spawn enemy");
        }*/
    }

    // Use this for initialization
    void Start () {
        singleton = this;
    }

    [Server]
    public void StartResolution()
    {
        foreach(GameObject ship in ships) {
            Ship shipcomp = ((Ship)ship.GetComponent("Ship"));
            shipcomp.Start_resolution(30U);
        }
    }

    /*[Server]
    void ResolveTurn()
    {
        Debug.Log("Resolve started");
        foreach( GameObject obj in My_ships )
            obj.GetComponent("Ship").SendMessage("Start_resolution", 20U );
        //foreach (GameObject obj in Enemy_shipsp )
         //   obj.GetComponent("Ship").SendMessage("Start_resolution", 20U );
    }
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {
        if (Input.GetKeyDown("a"))
            ResolveTurn();
	}*/
}
