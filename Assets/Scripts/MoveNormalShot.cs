using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNormalShot : MonoBehaviour {
    
    public float shootspeed;

    private Vector3 shootDirection;

	// Use this for initialization
	void Start ()
    {
        shootDirection = (GameObject.Find(transform.parent.name + "/turret_shotPosition").transform.position - transform.parent.transform.position).normalized;
        Debug.Log("ShootDirection: " + shootDirection);
        transform.up = transform.parent.up;
        Debug.Log("Parent.up: " + transform.parent.up);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(shootDirection * shootspeed * Time.deltaTime);
    }
}
