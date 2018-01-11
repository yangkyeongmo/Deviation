using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthReactions : MonoBehaviour {

    public GameObject followingShot;
    public float reduceHealth;
    public float velocityBoundary;

    private Vector3 radialSpawnPosition;
    private int firstValue;
    private int nextValue = 0;

    private List<GameObject> asteroidsNearby;
    private GameObject asteroid;

	// Use this for initialization
	void Start () {
        asteroidsNearby = new List<GameObject>();
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

        for(int i=0; i<asteroidsNearby.Count; i++)
        {
            if(asteroidsNearby[i] != asteroid)
            {
                asteroidsNearby.Add(asteroid);
            }
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

    void DestroyAsteroidAround()
    {
        //Get asteroid around earth
        //Get velocity of that asteroid
        Rigidbody rb_ast;
        rb_ast = asteroid.GetComponent<Rigidbody>();

        float velocity_ast = rb_ast.velocity.magnitude;

        //if asteroid velocity is below some value
        if (velocity_ast < velocityBoundary)
        {
            //reduce health of that asteroid per sec
            asteroid.transform.localScale -= new Vector3(0.01f * Time.deltaTime, 0.0f, 0.0f);
            if(asteroid.transform.localScale.x <= 0.05)
            {
                Destroy(asteroid);
            }
        }
    }

    public void GetAsteroidAround(GameObject asteroidAround)
    {
        asteroid = asteroidAround;
    }
}
