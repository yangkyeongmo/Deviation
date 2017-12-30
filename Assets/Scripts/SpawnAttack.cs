using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAttack : MonoBehaviour {

    public GameObject shot;

    private Vector3 shootdirection;
    private Quaternion shootrotation;
    private GameObject moon;
    private Vector3 worldmouseposition;


	// Use this for initialization
	void Start () {
        moon = GameObject.Find("Satellite0");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        worldmouseposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); worldmouseposition.z = 0;
        shootdirection = (worldmouseposition - moon.transform.position).normalized; shootdirection.z = 0;
        Instantiate(shot, moon.transform.position + shootdirection * 3f, Quaternion.Euler(0, 0, 90f));
    }
}