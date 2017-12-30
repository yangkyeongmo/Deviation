using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialProperties : MonoBehaviour {
    
    private float c_velocity;
    private float c_mass;
    private Rigidbody rb;

    public static CelestialProperties Instance;

	// Use this for initialization
	void Start () {
        Instance = this;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void SetVelocity(float v)
    {
        c_velocity = v;
    }

    public void SetMass(float m)
    {
        c_mass = m;
    }

    public float GetVelocity()
    {
        return c_velocity;
    }

    public float GetMass()
    {
        return c_mass;
    }
}