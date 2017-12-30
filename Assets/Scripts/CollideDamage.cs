using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name != "asteroid(clone)" || collision.gameObject.name != "shot(clone)")
        {
            CelestialProperties cp_collision = collision.gameObject.GetComponent<CelestialProperties>();
            //Decrease mass on collision
            cp_collision.SetMass(cp_collision.GetMass() - 0.01f);
            Destroy(this.gameObject);
        }
    }
}
