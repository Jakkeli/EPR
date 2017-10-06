using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorReset : MonoBehaviour {

	public void ResetDoor() {
        if (GetComponent<DoorLift>() != null) {
            GetComponent<DoorLift>().ResetDoor();
        }
    }
}
