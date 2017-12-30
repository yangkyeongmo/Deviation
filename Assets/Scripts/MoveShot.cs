using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShot : MonoBehaviour {

    public float shootspeed;

    private GameObject moon;
    private Vector3 shootdirection;
    private Vector3 rotateddirection;
    private Vector3 rotationangle;
    private Quaternion shotrotation;
    private Vector3 worldmouseposition;

	// Use this for initialization
	void Start ()
    {
        moon = GameObject.Find("Satellite0");

        worldmouseposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)); worldmouseposition.z = 0;
        shootdirection = (worldmouseposition - moon.transform.position).normalized; shootdirection.z = 0;

        rotationangle = new Vector3(0f,0f,Mathf.Atan2(shootdirection.y, shootdirection.x) * Mathf.Rad2Deg);
        shotrotation = Quaternion.Euler(rotationangle);
        transform.Rotate(rotationangle);

        rotateddirection = Quaternion.Euler(new Vector3(0f, 0f, rotationangle.z * -1f -90f)) * shootdirection;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(rotateddirection * shootspeed * Time.deltaTime);
    }
}
