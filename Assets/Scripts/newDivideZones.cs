using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newDivideZones : MonoBehaviour {

    float x; float y; float z;

    public float testRadius;
    public float testScale;

    private GameObject testSubject;
    private GameObject dot;
    private Vector3 direction;
    private Quaternion lookRotation;
    private float delPhi = 0;
    private float delTheta = 0;

    private Vector3 mousePosition_cart;
    private Vector3 mousePosition_sphe;

    // Use this for initialization
    void Start ()
    {
        //for test drawing
        testSubject = GameObject.Find("testSphere");
        dot = GameObject.Find("dot");
        dot.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * testScale;

        DrawLatitudeLine(testRadius, 30 * Mathf.Deg2Rad, 2);
        DrawLongitudeLine(testRadius, 30 * Mathf.Deg2Rad, 2);

        Debug.Log("Draw Complete");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            testSubject.transform.Rotate(Vector3.up, -20.0f, Space.World);
            delPhi -= 20.0f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            testSubject.transform.Rotate(Vector3.up, +20.0f, Space.World);
            delPhi += 20.0f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            testSubject.transform.Rotate(Vector3.forward, 20.0f, Space.World);
            delTheta -= 20.0f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            testSubject.transform.Rotate(Vector3.forward, -20.0f, Space.World);
            delTheta += 20.0f;
        }
        if (Input.GetMouseButtonDown(0))
        {
            GetSpinnedSphericalMousePosition();
            GetSelectedZoneNumber(30 * Mathf.Deg2Rad, 30 * Mathf.Deg2Rad);
        }
    }

    //for test drawing
    void DrawLatitudeLine(float rad, float thetaInterval_rad, float interval)
    {
        for (int i = 0; i < 180 * Mathf.Deg2Rad / thetaInterval_rad; i++)
        {
            GameObject parentobj = new GameObject();
            parentobj.name = "Latitude" + (thetaInterval_rad * Mathf.Rad2Deg * i);

            for (int j = 0; j < 360 / interval; j++)
            {
                SphericalToCartesian(rad, thetaInterval_rad * i, j * interval * Mathf.Deg2Rad);
                direction = new Vector3(x, y, z);
                Vector3 dotPosition = testSubject.transform.position + direction;                           //dotPosition = parentposition + SphericalCoordinate
                GameObject linecomponent = Instantiate(dot, dotPosition, Quaternion.Euler(0, 0, 0));

                lookRotation = Quaternion.LookRotation(direction.normalized);                               //Rotate parallel to normal vector
                linecomponent.transform.rotation = lookRotation;

                linecomponent.transform.parent = parentobj.transform;
            }
            parentobj.transform.parent = testSubject.transform;
        }
    }

    //for test drawing
    void DrawLongitudeLine(float rad, float phiInterval_rad, float interval)
    {
        for (int i = 0; i < 360 * Mathf.Deg2Rad / phiInterval_rad; i++)
        {
            GameObject parentobj = new GameObject();
            parentobj.name = "Longitude" + (phiInterval_rad * Mathf.Rad2Deg * i);

            for (int j = 0; j < 180 / interval; j++)
            {
                SphericalToCartesian(rad, j * interval * Mathf.Deg2Rad, phiInterval_rad * i);
                direction = new Vector3(x, y, z);
                Vector3 dotPosition = testSubject.transform.position + new Vector3(x, y, z);                //dotPosition = parentposition + SphericalCoordinate
                GameObject linecomponent = Instantiate(dot, dotPosition, Quaternion.Euler(0, 0, 0));

                lookRotation = Quaternion.LookRotation(direction.normalized);                               //Rotate parallel to normal vector
                linecomponent.transform.rotation = lookRotation;

                linecomponent.transform.parent = parentobj.transform;
            }
            parentobj.transform.parent = testSubject.transform;
        }
    }

    void SphericalToCartesian(float rad, float theta, float phi)
    {
        x = rad * Mathf.Sin(theta) * Mathf.Cos(phi);
        y = rad * Mathf.Cos(theta);
        z = rad * Mathf.Sin(theta) * Mathf.Sin(phi);
    }

    void GetSpinnedSphericalMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition_cart = hit.point;
        }

        Debug.Log("Cartesian Mouse Position = " + mousePosition_cart);

        mousePosition_sphe = CartesianToSpherical(mousePosition_cart, testSubject.transform.position);

        mousePosition_sphe = mousePosition_sphe - new Vector3(0.0f, delTheta, delPhi); // correct spin
        mousePosition_sphe.y = CorrectTheta(mousePosition_sphe.y);  // correct corrected theta
        mousePosition_sphe.z = CorrectPhi(mousePosition_sphe.z);    // correct corrected phi

        Debug.Log("Corrected corrected Spherical Mouse Position = " + mousePosition_sphe);
    }

    Vector3 CartesianToSpherical(Vector3 sc, Vector3 origin)
    {
        //Cartesain To Spherical
        //x = x
        //y = z
        //z = y
        float x; float y; float z;
        float rad; float theta; float phi;

        x = sc.x - origin.x;
        y = sc.y - origin.y;
        z = sc.z - origin.z;

        rad = Mathf.Sqrt(x * x + y * y + z * z);
        theta = Mathf.Acos(y / rad) * Mathf.Rad2Deg;
        phi = Mathf.Atan(z / x) * Mathf.Rad2Deg + 90;

        return new Vector3(rad, theta, phi);
    }

    float CorrectTheta(float theta)
    {

        if (theta < 0)
        {
            theta = -theta;
        }

        while (theta > 180)
        {
            if ((System.Math.Truncate(theta / 180) % 2) == 1)
            {
                theta = 360 - theta;
            }
            if ((System.Math.Truncate(theta / 180) % 2) == 0 && (System.Math.Truncate(theta / 180) % 2) != 0)
            {
                theta -= 360;
            }
        }
        return theta;
    }

    float CorrectPhi(float phi)
    {
        while (phi > 360)
        {
            phi -= 360;
        }
        while (phi < 0)
        {
            phi += 360;
        }
        return phi;
    }

    void GetSelectedZoneNumber(float thetaInterval_rad, float phiInterval_rad)
    {
        int k = 0;
        for(int i=0; i < 180  * Mathf.Deg2Rad / thetaInterval_rad; i++)
        {
            for(int j=0; j < 360 * Mathf.Deg2Rad / phiInterval_rad; j++)
            {
                if(mousePosition_sphe.y > thetaInterval_rad * Mathf.Rad2Deg * i && mousePosition_sphe.y < thetaInterval_rad * Mathf.Rad2Deg * (i+1)             // 0 < mousePosition.theta < 30 if i=0
                    && mousePosition_sphe.z > phiInterval_rad * Mathf.Rad2Deg * j && mousePosition_sphe.z < phiInterval_rad * Mathf.Rad2Deg * (j + 1))          // 0 < mousePosition.phi > 30 if j=0
                {
                    Debug.Log("Selected Zone #(0~71): " + k);                                                                                                   // Zone# = 0 if i,j = 0
                }

                k++;
            }
        }
    }
}
