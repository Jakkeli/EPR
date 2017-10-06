using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorResetManager : MonoBehaviour {

    List<GameObject> doorsInteracetedWithAfterLatestCP = new List<GameObject>() { };

    public void CheckPointChange() {
        doorsInteracetedWithAfterLatestCP.Clear();
    }

    public void DoorInteractedWith(GameObject door) {
        doorsInteracetedWithAfterLatestCP.Add(door);
    } 

    public void ResetDoors() {
        foreach (GameObject door in doorsInteracetedWithAfterLatestCP) {
            door.GetComponent<DoorReset>().ResetDoor();
        }
    }
}
