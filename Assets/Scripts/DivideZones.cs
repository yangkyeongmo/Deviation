using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideZones : MonoBehaviour
{

    float x; float y; float z;
    float rad; float theta; float phi;
    float minX, minY, minZ, maxX, maxY, maxZ;

    public float testRadius;
    public float testScale;

    private static float Rot2Deg = 117.59067678f;
    private static float Flt2Rot = 57.296f;

    private float rotPhi;
    public float rotTheta;

    private GameObject testSubject;
    private GameObject dot;
    private int p;
    private Vector3 currentMP;
    private Vector3 direction;
    private Quaternion lookRotation;
    private List<List<Vector3>> allVertexPositions;
    private List<Vector3> vertexPositions;

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

        allVertexPositions = new List<List<Vector3>>();

        Debug.Log("Draw Complete");
        
        //for getting zone number
        SetVertices(30 * Mathf.Deg2Rad, 30 * Mathf.Deg2Rad);
        Debug.Log("# of zones: " + GetVerticesNumber(30 * Mathf.Deg2Rad, 30 * Mathf.Deg2Rad));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetVertices(30 * Mathf.Deg2Rad, 30 * Mathf.Deg2Rad);
            CheckZoneNumber(30 * Mathf.Deg2Rad, 30 * Mathf.Deg2Rad);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            testSubject.transform.Rotate(Vector3.up, -20.0f , Space.World);
            rotPhi -= 20.0f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            testSubject.transform.Rotate(Vector3.up, +20.0f, Space.World);
            rotPhi += 20.0f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            testSubject.transform.Rotate(Vector3.forward, 20.0f, Space.World);
            rotTheta -= 20.0f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            testSubject.transform.Rotate(Vector3.forward, -20.0f, Space.World);
            rotTheta += 20.0f;
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

    void SetVertices(float thetaInterval_rad, float phiInterval_rad)
    {
        for (int i = 0; i <= 180 * Mathf.Deg2Rad / thetaInterval_rad; i++)
        {
            for (int j = 0; j <= 360 * Mathf.Deg2Rad / phiInterval_rad; j++)
            {
                vertexPositions = new List<Vector3>();

                vertexPositions.Add(SetVerticePosition(2, i * thetaInterval_rad * Mathf.Rad2Deg, j * phiInterval_rad * Mathf.Rad2Deg, testSubject));
                vertexPositions.Add(SetVerticePosition(2, (i + 1) * thetaInterval_rad * Mathf.Rad2Deg, j * phiInterval_rad * Mathf.Rad2Deg, testSubject));
                vertexPositions.Add(SetVerticePosition(2, (i + 1) * thetaInterval_rad * Mathf.Rad2Deg, (j + 1) * phiInterval_rad * Mathf.Rad2Deg, testSubject));

                allVertexPositions.Add(vertexPositions);
            }
        }
    }

    Vector3 SetVerticePosition(float rad, float theta, float phi, GameObject parentObj)
    {
        Vector2 position;
        position = new Vector2(theta + rotTheta, phi + rotPhi);

        position.x = CorrectTheta(position.x);
        position.y = CorrectPhi(position.y);

        return new Vector3(2, position.x, position.y);

    }

    int GetVerticesNumber(float thetaInterval_rad, float phiInterval_rad)
    {
        int num = 0;
        for (int i = 0; i < 180 * Mathf.Deg2Rad / thetaInterval_rad; i++)
        {
            for (int j = 0; j < 360 * Mathf.Deg2Rad / phiInterval_rad; j++)
            {
                num++;
            }
        }
        return num;
    }

    void CheckZoneNumber(float thetaInterval_rad, float phiInterval_rad)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            currentMP = hit.point;
        }

        Debug.Log("Current MousePosition(world) is: " + currentMP);

        currentMP = CartesianToSpherical(currentMP, testSubject.transform.position);         //currentMP = (rad, theta, phi)
        Vector2 currentMP2 = new Vector2(currentMP.y - rotTheta, currentMP.z - rotPhi);                          //currentMP2 = (theta, phi)

        currentMP2.y = CorrectPhi(currentMP2.y);
        currentMP2.x = CorrectTheta(currentMP2.x);

        currentMP = new Vector3(currentMP.x, currentMP2.x, currentMP2.y);

        Debug.Log("Current MousePosition(world, Spherical)) is: " + currentMP);

        for (int k = 0; k < allVertexPositions.Count; k++)
        {
            //Debug.Log(allvertexPositions[k][0].x + " " + allvertexPositions[k][0].y + " " + allvertexPositions[k][0].z);
            //Debug.Log(allVertexPositions[k][1].y + " " + allVertexPositions[k][0].y + " " + allVertexPositions[k][1].z + " " + allVertexPositions[k][2].z);
            if(currentMP.y > allVertexPositions[k][0].y && currentMP.y < allVertexPositions[k][1].y && currentMP.z > allVertexPositions[k][1].z && currentMP.z < allVertexPositions[k][2].z)
            {
                Debug.Log("Zone " + k + " is selected");

                selectedZoneVertexPositions = new float[4];
                selectedZoneVertexPositions[0] = allVertexPositions[k][0].y;
                selectedZoneVertexPositions[1] = allVertexPositions[k][1].y;
                selectedZoneVertexPositions[2] = allVertexPositions[k][1].z;
                selectedZoneVertexPositions[3] = allVertexPositions[k][2].z;

                selectedZoneNumber = k;

                break;
            }
        }
    }

    float CorrectTheta (float theta)
    {
        
        while (theta > 180)
        {
            if ((System.Math.Truncate(theta / 180) % 2) == 1 || (System.Math.Truncate(theta / 180) % 2) != 0)
            {
                theta = 360 - theta;
            }
            if ((System.Math.Truncate(theta / 180) % 2) == 0)
            {
                theta -= 360;
            }
        }

        while(theta < 0)
        {
            if ((System.Math.Truncate(theta / 180) % 2) == 1)
            {
                theta = -(360 - theta);
            }
            if ((System.Math.Truncate(theta / 180) % 2) == 0 && (System.Math.Truncate(theta / 180) % 2) != 0)
            {
                theta = -(theta - 360);
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

    public float[] GetSelectedZoneVertices()
    {
        return selectedZoneVertexPositions;
    }

    public int GetSelectedZoneNumber()
    {
        return selectedZoneNumber;
    }
}