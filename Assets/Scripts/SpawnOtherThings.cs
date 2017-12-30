using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOtherThings: MonoBehaviour {
    
    public GameObject asteroid;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(SpawnAsteroid());
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    IEnumerator SpawnAsteroid()
    {
        GameObject spawnedAsteroid = Instantiate(asteroid);
        Rigidbody rb = spawnedAsteroid.GetComponent<Rigidbody>();
        Transform tf = spawnedAsteroid.transform;

        List<Vector3> spawnPositions = new List<Vector3>();

        spawnPositions.Add(new Vector3(Random.Range(-13f, -12f), Random.Range(-7f, 7f)));
        spawnPositions.Add(new Vector3(Random.Range(-12.5f, 12.5f), Random.Range(6.5f, 7f)));
        spawnPositions.Add(new Vector3(Random.Range(12f, 13f), Random.Range(-7f, 7f)));
        spawnPositions.Add(new Vector3(Random.Range(-12.5f, 12.5f), Random.Range(-7f, -6.5f)));

        tf.position = spawnPositions[Random.Range(0, 3)];

        Vector3 direction = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;

        rb.velocity = direction * Random.Range(1, 3);

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(SpawnAsteroid());
    }
}
