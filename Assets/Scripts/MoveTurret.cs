using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTurret : MonoBehaviour {

    private GameObject testSubject;
    private GameObject gameController;
    private newDivideZones newDiv;
    private AddComponentsOnSphere addComp;

    private bool settled = false;
    private RaycastHit hit;
    private Ray ray;

    private int selectedZoneNumber;
    private Vector3 settlePosition;

    // Use this for initialization
    void Start () {
        testSubject = GameObject.Find("testSphere");
        gameController = GameObject.Find("GameController");
        newDiv = gameController.GetComponent<newDivideZones>();
        addComp = gameController.GetComponent<AddComponentsOnSphere>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (settled == false)
        {
            transform.position = new Vector3(0.0f, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, Camera.main.ScreenToWorldPoint(Input.mousePosition).z);

            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Player")
            {
                transform.position = hit.point;
                transform.up = hit.point - testSubject.transform.position;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Player")
            {
                selectedZoneNumber = newDiv.GetSelectedZoneNumber();
                Debug.Log("Zone #: " + selectedZoneNumber);
                settlePosition = GameObject.Find("MidPoint" + selectedZoneNumber).transform.position;
                Debug.Log(settlePosition);
                transform.position = settlePosition;
                transform.up = settlePosition - testSubject.transform.position;
                settled = true;
                addComp.EraseAllElementOnTurretsList();
                Debug.Log("Turret is Settled");
            }
        }
    }
}
