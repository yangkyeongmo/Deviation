using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    private GameObject earth;
    private GameObject moon;
    private CelestialProperties cp_earth;
    private CelestialProperties cp_moon;
    private GravityOrbitSystem gos;

    public float satelliteVelocity;
    public float satelliteMass;
    public float planetVelocity;
    public float planetMass;
    public Text dataText;

	// Use this for initialization
	void Start ()
    {
        gos = GetComponent<GravityOrbitSystem>();
            //find objects by name
        earth = GameObject.Find("Planet0");
        moon = GameObject.Find("Satellite0");

        cp_earth = earth.GetComponent<CelestialProperties>();
        cp_moon = moon.GetComponent<CelestialProperties>();

            //set velocity&mass of objects
        //satelliteVelocity = 0.8f; satelliteMass = 2f;
        //planetVelocity = 0; planetMass = 10f;
        SetCelestialProperties(moon, satelliteVelocity, satelliteMass);
        SetCelestialProperties(earth, planetVelocity, planetMass);
    }
	
	// Update is called once per frame
	void Update ()
    {
        earth.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        dataText.text = "MOON \n Velocity: " + cp_moon.GetVelocity() + "\n Mass: " + cp_moon.GetMass() + 
            "\n\n EARTH \n Velocity: " + cp_earth.GetVelocity() + "\n Mass: " + cp_earth.GetMass() + 
            "\n\n Orbit RADIUS: " + gos.GetRadius();
    }

    void SetCelestialProperties(GameObject body, float v, float m)
    {
        CelestialProperties cp = body.GetComponent<CelestialProperties>();
        cp.SetVelocity(v);
        cp.SetMass(m);
    }

}
