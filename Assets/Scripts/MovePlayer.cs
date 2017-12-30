using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed);
            Debug.Log("Moving Left");
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * speed);
            Debug.Log("Moving Down");
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed);
            Debug.Log("Moving Right");
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * speed);
            Debug.Log("Moving Up");
        }
    }
}
