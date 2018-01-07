using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFollowingShot : MonoBehaviour {

    public float shotSpeed;

    private GameObject playerObject;
    private Vector3 direction;

	// Use this for initialization
	void Start () {
        playerObject = GameObject.Find("Satellite0");
	}
	
	// Update is called once per frame
	void Update () {
        direction = (playerObject.transform.position - transform.position).normalized;
        transform.position += direction * shotSpeed * Time.deltaTime;
        transform.up = direction;
	}
}
