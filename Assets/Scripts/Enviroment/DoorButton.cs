using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour {

    public GameObject button;

    bool playerInRange;

    bool buttonMoving;

    ButtonChanger bc;

    public AudioSource buttonPressNormal;
    public AudioSource buttonPressError;

    public GameObject myDoor;
    DoorLift dl;
    //bool doorOpen;

    void Start () {
        bc = GetComponentInChildren<ButtonChanger>();
        dl = myDoor.GetComponent<DoorLift>();
        //if (dl.currentState == LiftDoorState.Closed) {
        //    doorOpen = false;
        //} else {
        //    doorOpen = true;
        //}
	}

    public void ButtonColor(bool open) {
        bc.ChangeMaterial(open);
    }

    void ButtonPushed() {
        dl.ButtonPushed();
    }
	
	void Update () {
        if (playerInRange && Input.GetButtonDown("Use")) ButtonPushed();
	}

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player") playerInRange = true;
    }

    void OnTriggerExit(Collider c) {
        if (c.tag == "Player") playerInRange = false;
    }
}
