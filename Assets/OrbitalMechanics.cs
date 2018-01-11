using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMechanics : MonoBehaviour {

    public GameObject marker;
    public Vector3 first_velocity;

    private bool initialUpdate = true;
    private Vector3 i1, i2, i3;
    private Vector3 o1, o2;
    public Vector3 init_position, init_velocity;
    public float p, e, i, w, v;
    public Vector3 h, vector_e;
    public float G, mu;
    public float next_v;
    public float E, M;
    public float k, a;
    public float next_M, next_E;
    public Vector3 next_position, next_velocity;
    private GameObject largerMass;
    private CelestialProperties lm_cp;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        i1 = new Vector3(1, 0, 0);
        i2 = new Vector3(0, 1, 0);
        i3 = new Vector3(0, 0, 1);
        o1 = new Vector3(1, 0, 0);
        o2 = new Vector3(0, 1, 0);

        largerMass = GameObject.Find("Planet0");
        lm_cp = largerMass.GetComponent<CelestialProperties>();

        rb = GetComponent<Rigidbody>();
        rb.velocity = first_velocity;

        G = Mathf.Pow(10,10);
        StartCoroutine(fdCoroutine());
    }
	
	// Update is called once per frame
	void Update () {
        //PositionUpdateAlgorithm();
	}

    void PositionUpdateAlgorithm()
    {
        if (initialUpdate)
        {
            ConvertToOrbitalElements();
        }

        E = Mathf.Atan(Mathf.Sqrt((1 - e) / (1 + e)) * Mathf.Tan(v / 2)) * 2;
        M = E - e * Mathf.Sin(E);
        next_M = M + Mathf.Sqrt(mu / Mathf.Pow(a, 3)) * Time.deltaTime;// - 2 * Mathf.PI * k;
        for(int i=0; i<25; i++)
        {
            next_E = next_M + e * Mathf.Sin(E);
            E = next_E;
        }
        next_v = Mathf.Atan(Mathf.Sqrt((1 - e) / (1 + e)) * Mathf.Tan(next_E / 2)) * 2;
        v = next_v;

        ConvertFromOrbitalElements();

        transform.position = next_position; init_position = next_position;
        rb.velocity = next_velocity; init_velocity = next_velocity;
        Instantiate(marker, transform.position, Quaternion.identity);
        initialUpdate = false;
    }

    void ConvertToOrbitalElements()
    {
        Debug.Log("Convert to OE Started");
        init_position = transform.position - largerMass.transform.position;
        init_velocity = rb.velocity;
        h = Vector3.Cross(init_position, init_velocity);
        mu = G * lm_cp.GetMass();
        p = h.sqrMagnitude / mu;
        vector_e = (init_position * (init_velocity.sqrMagnitude - mu / init_position.magnitude) - Vector3.Dot(init_position, init_velocity) * init_velocity) / mu;
        e = vector_e.magnitude;
        a = p / (1 - Mathf.Pow(e, 2));
        i = 0;
        w = Mathf.Acos(Vector3.Dot(i1, vector_e) / e);
        if(Vector3.Dot(i2, vector_e) < 0)
        {
            w = -w;
        }
        if (e != 0)
        {
            v = Mathf.Acos(Vector3.Dot(vector_e, init_position) / (e * init_position.magnitude));
            if (Vector3.Dot(init_position, init_velocity) < 0)
            {
                v = -v;
            }
        }
        else if (e == 0)
        {
            v = Mathf.Acos(Vector3.Dot(i1, init_position) / init_position.magnitude);
            if (Vector3.Dot(i1, init_position) < 0)
            {
                v = -v;
            }
        }
    }

    void ConvertFromOrbitalElements()
    {
        Debug.Log("Convert From OE Started");
        next_position = (p/(1+e* Mathf.Cos(v))) * (Mathf.Cos(v) * o1 + Mathf.Sin(v) * o2);
        next_velocity = Mathf.Sqrt(p / mu) * (-Mathf.Sin(v) * o1 + (e + Mathf.Cos(v)) * o2);
    }

    IEnumerator fdCoroutine()
    {
        PositionUpdateAlgorithm();
        yield return new WaitForSeconds(2.0f);
    }
}
