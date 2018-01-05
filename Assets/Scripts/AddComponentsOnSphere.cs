using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddComponentsOnSphere : MonoBehaviour {

    public Button selectButton;
    public GameObject turret;

    private List<GameObject> turretsOnPointer;
    private List<GameObject> turretsSettled;

	// Use this for initialization
	void Start () {
        turretsOnPointer = new List<GameObject>();
        turretsSettled = new List<GameObject>();
        selectButton.onClick.AddListener(SetComponentOnPointer);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetComponentOnPointer()
    {
        if(turretsOnPointer.Count == 0)
        {
            GameObject instTurret = Instantiate(turret, Input.mousePosition, Quaternion.identity);
            turretsOnPointer.Add(instTurret);
            Debug.Log(turretsOnPointer.Count);
        }
    }

    public void EraseAllElementOnTurretsList()
    {
        turretsOnPointer.Clear();
    }
}
