using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_DivideZones : MonoBehaviour
{

    float x; float y; float z;
    float rad; float theta; float phi;
    float minX, minY, minZ, maxX, maxY, maxZ;

    public float testRadius;
    public float testScale;

    private static float Rot2Deg = 117.59067678f;
    private static float Flt2Rot = 57.296f;

    private float delPhi = 0;
    public float delTheta = 0;

    private GameObject testSubject;
    private GameObject dot;
    private int p;
    private Vector3 currentMousePosition;
    private Vector3 direction;
    private Quaternion lookRotation;
    private List<List<Vector2>> allVertexPositions;
    private List<Vector2> vertexPositions;

    private float totalZoneNumber =0;
    private float[] selectedZoneVertexPositions;
    private int selectedZoneNumber;

    // Use this for initialization
    void Start()
    {
        //for test drawing
        testSubject = GameObject.Find("testSphere");
        dot = GameObject.Find("dot");
        dot.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * testScale;

        DrawLatitudeLine(testRadius, 30 * Mathf.Deg2Rad, 2);
        DrawLongitudeLine(testRadius, 30 * Mathf.Deg2Rad, 2);

        allVertexPositions = new List<List<Vector2>>();

        Debug.Log("Draw Complete");

        SetVertices(30 * Mathf.Deg2Rad, 30 * Mathf.Deg2Rad, delTheta, delPhi);
    }

    // Update is called once per frame
    void Update()
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
            SetVertices(30, 30, delTheta, delPhi);
            GetZoneNumber(SetSphericalMousePosition(delTheta, delPhi));
        }
    }

    void SphericalToCartesian(float rad, float phi, float theta)
    {
        x = rad * Mathf.Sin(theta) * Mathf.Cos(phi);
        y = rad * Mathf.Cos(theta);
        z = rad * Mathf.Sin(theta) * Mathf.Sin(phi);
    }

    Vector3 CartesianToSpherical(Vector3 sc, Vector3 origin)
    {
        //Cartesain To Spherical
        //x = x
        //y = z
        //z = y
        x = sc.x - origin.x;
        y = sc.y - origin.y;
        z = sc.z - origin.z;
        rad = Mathf.Sqrt(x * x + y * y + z * z);
        theta = Mathf.Acos(y / rad) * Mathf.Rad2Deg;
        phi = Mathf.Atan(z / x) * Mathf.Rad2Deg + 90;

        return new Vector3(rad, theta, phi);
    }

    Vector3 GetSphericalToCartesian(float rad, float phi, float theta)
    {
        x = rad * Mathf.Sin(theta) * Mathf.Cos(phi);
        y = rad * Mathf.Cos(theta);
        z = rad * Mathf.Sin(theta) * Mathf.Sin(phi);

        return new Vector3(x, y, z);
    }

    //for test drawing
    void MakeDot(float rad, float phi, float theta)
    {
        SphericalToCartesian(rad, phi, theta);
        Vector3 dotPosition = testSubject.transform.position + new Vector3(x, y, z);
        Instantiate(dot, dotPosition, Quaternion.Euler(0, 0, 0));
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
                SphericalToCartesian(rad, j * interval * Mathf.Deg2Rad, thetaInterval_rad * i);
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
                SphericalToCartesian(rad, phiInterval_rad * i, j * interval * Mathf.Deg2Rad);
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

    void SetVertices(float thetaInterval, float phiInterval, float delTheta, float delPhi)
    {
        totalZoneNumber = 0;
        for (int i = 0; i <= 180 / thetaInterval; i++)
        {
            for (int j = 0; j <= 360 / phiInterval; j++)
            {
                vertexPositions = new List<Vector2>();

                vertexPositions.Add(SetVerticePosition(i * thetaInterval, j * phiInterval, delTheta, delPhi));
                vertexPositions.Add(SetVerticePosition((i + 1) * thetaInterval, j * phiInterval, delTheta, delPhi));
                vertexPositions.Add(SetVerticePosition((i + 1) * thetaInterval, (j + 1) * phiInterval, delTheta, delPhi));

                allVertexPositions.Add(vertexPositions);

                totalZoneNumber++;
            }
        }
    }

    Vector2 SetVerticePosition(float theta, float phi, float delTheta, float delPhi)
    {
        Vector2 position = new Vector2(theta + delTheta, phi + delPhi);
        return position;
    }

    Vector2 SetSphericalMousePosition(float delTheta, float delPhi)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            currentMousePosition = hit.point;
        }

        Debug.Log("Current MousePosition(world) is: " + currentMousePosition);

        currentMousePosition = CartesianToSpherical(currentMousePosition, testSubject.transform.position);                             //currentMP = (rad, theta, phi)
        Vector2 currentMousePosition2 = new Vector2(currentMousePosition.y - delTheta, currentMousePosition.z - delPhi);                          //currentMP2 = (theta, phi)

        currentMousePosition2.y = CorrectPhi(currentMousePosition2.y);
        currentMousePosition2.x = CorrectTheta(currentMousePosition2.x);

        Debug.Log("Current MousePosition(world, Spherical)) is: " + currentMousePosition2);

        return currentMousePosition2;
    }

    void GetZoneNumber(Vector2 mousePosition_spherical)
    {
        for(int i = 0; i < totalZoneNumber; i++)
        {
            if (mousePosition_spherical.x > allVertexPositions[i][0].x && mousePosition_spherical.x < allVertexPositions[i][1].x
                && mousePosition_spherical.y > allVertexPositions[i][1].y && mousePosition_spherical.y < allVertexPositions[i][2].y)
            {
                Debug.Log(allVertexPositions[i][0] + " " + allVertexPositions[i][1] + " " + allVertexPositions[i][2]);
                Debug.Log("Selected Zone #: " + i);
            }
        }
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
}
