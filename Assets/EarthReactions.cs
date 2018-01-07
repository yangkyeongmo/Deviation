using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EarthReactions : MonoBehaviour {

    public GameObject followingShot;

    private Vector3 radialSpawnPosition;
    private int firstValue;
    private int nextValue = 0;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        firstValue = (int)System.Math.Truncate(Time.time / 1);

        if (firstValue == nextValue)
        {
            radialSpawnPosition = CircularToCartesianCoordinate(transform.localScale.x + 2.0f, Random.Range(0.0f, 360.0f * Mathf.Deg2Rad));
            Instantiate(followingShot, radialSpawnPosition, Quaternion.identity);
            nextValue = firstValue + 3;
        }
	}
    
    Vector3 CircularToCartesianCoordinate(float rad, float angle)
    {
        Vector3 circToCart;
        circToCart.x = rad * Mathf.Cos(angle);
        circToCart.y = rad * Mathf.Sin(angle);
        circToCart.z = 0;
        return circToCart;
    }
}
