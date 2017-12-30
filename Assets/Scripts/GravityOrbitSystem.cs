using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOrbitSystem : MonoBehaviour {
    
    private GameObject[] bodies;
    private float orbitRadius;

    public Vector3 velocityDirection;
    public Vector3 gravityDirection;
    public float gravity_coefficient;

    // Use this for initialization
    void Start () {
        //gravity_coefficient = 1;
        bodies = GameObject.FindGameObjectsWithTag("SpaceObjects");
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            for (int j = 0; j < bodies.Length; j++)
            {
                if (i != j)
                {
                    Vector3 headingTo = (bodies[i].transform.position - bodies[j].transform.position);
                    orbitRadius = (Mathf.Round(headingTo.magnitude / .01f) * .01f);
                    Vector3 headingToNorm = headingTo.normalized;
                    SetOrbit(bodies[i], bodies[j], headingToNorm);
                }
            }
        }
    }

    void SetOrbit(GameObject A, GameObject B, Vector3 direction)
    {
        this.gravityDirection = direction;

        Rigidbody rb_a = A.GetComponent<Rigidbody>();
        Rigidbody rb_b = B.GetComponent<Rigidbody>();

        CelestialProperties cp_a = A.GetComponent<CelestialProperties>();
        CelestialProperties cp_b = B.GetComponent<CelestialProperties>();

            //Set gravity force from A to B & B to A
        rb_a.AddForce((-direction * gravity_coefficient * (cp_a.GetMass() * cp_b.GetMass() / Mathf.Pow(direction.magnitude, 2))), ForceMode.Impulse);
        rb_b.AddForce((direction * gravity_coefficient * (cp_a.GetMass() * cp_b.GetMass() / Mathf.Pow(direction.magnitude, 2))), ForceMode.Impulse);

            //Set velocity of A
        Quaternion rotate = Quaternion.Euler(0f, 0f, 90f);
        Vector3 velocityDirection = rotate * direction;
        this.velocityDirection = velocityDirection;
        rb_a.velocity = cp_a.GetVelocity() * velocityDirection;
    }

    public float GetRadius()
    {
        return orbitRadius;
    }
}