using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

    uint Turn_count;
    public GameObject ScoutShips;
    public GameObject KingShips;
    public GameObject CruiserShips;

    List<GameObject> My_ships = new List<GameObject>();
    List<GameObject> Enemy_shipsp = new List<GameObject>();

    public Vector3 My_start = new Vector3(0, 0, 0);
    public Vector3 Enemy_start = new Vector3(10, 10, 10);

    public GameObject UX;

    void SpawnShip( bool friendly, Vector3 position, Ship.Type type )
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
        ship = (GameObject)Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
        ship.transform.position = position;
        ((Ship)ship.GetComponent("Ship")).Ship_type = type;

        if (friendly)
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
        }
    }

    // Use this for initialization
    void Start () {
        SpawnShip(true, new Vector3(0, 0, 100), Ship.Type.SCOUT);
        SpawnShip(false, new Vector3(0, 100, 100), Ship.Type.SCOUT);
        SpawnShip(false, new Vector3(0, 200, 100), Ship.Type.SCOUT);
    }

    void ResolveTurn()
    {
        Debug.Log("Resolve started");
        foreach( GameObject obj in My_ships )
            obj.GetComponent("Ship").SendMessage("Start_resolution", 20U );
        //foreach (GameObject obj in Enemy_shipsp )
         //   obj.GetComponent("Ship").SendMessage("Start_resolution", 20U );
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("a"))
            ResolveTurn();
	}
}
