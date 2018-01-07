using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddComponentsOnSphere : MonoBehaviour {

    public Button selectButton;
    public GameObject turret;
    
    private bool turretSettled = true;
    private GameObject instTurret;
    private int turretNum = 0;

	// Use this for initialization
	void Start () {
        selectButton.onClick.AddListener(SetComponentOnPointer);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetComponentOnPointer()
    {
        //Add Turret
        if(turretSettled)
        {
            instTurret = Instantiate(turret, Input.mousePosition, Quaternion.identity);
            instTurret.name = instTurret.name + turretNum;
            turretNum++;
        }
    }

    public void ConfirmTurretSettled()
    {
        turretSettled = true;
    }
}
