using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOrbitSystem : MonoBehaviour {

    private GameObject orbit_body;
    private float origin_x;
    private float origin_y;
    private float orb_radius;
    private float orb_angularvelocity;
    private Rigidbody rb;

    public GameObject orbit_to;

    public static SimpleOrbitSystem instance;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        orbit_body = this.gameObject;
        instance = this;
        origin_x = orbit_to.transform.position.x;
        origin_y = orbit_to.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        SetOrbitPosition(orb_radius, orb_angularvelocity);
	}

    void SetOrbitPosition(float r, float av)
    {
        transform.position = new Vector3(origin_x + r * Mathf.Cos(av * Time.time), origin_y + r * Mathf.Sin(av * Time.time), 0);
    }

    public void SetOrbRadius(float r)
    {
        this.orb_radius = r;
    }

    public void SetAngularVelocity(float av)
    {
        this.orb_angularvelocity = av;
    }

    float GetOrbRadius()
    {
        return this.orb_radius;
    }

    float GetAngularVelocity()
    {
        return this.orb_angularvelocity;
    }
}
