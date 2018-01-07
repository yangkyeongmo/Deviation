using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNormalShot : MonoBehaviour {
    
    public float shootspeed;

    private Vector3 shootDirection;

	// Use this for initialization
	void Start ()
    {
        shootDirection = transform.position.normalized;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += shootDirection * shootspeed;
    }
}
