using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour {

    bool playerInRange;

    GameManager gm;

    bool actionDone;

	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    void RadioButton() {
        if (!actionDone) {
            gm.StartVideoMusic();
            actionDone = true;
        }        
    }
	
	void Update () {
        if (playerInRange && Input.GetButtonDown("Use")) RadioButton();
	}

    void OnTriggerEnter(Collider c) {
        playerInRange = true;
    }

    void OnTriggerExit(Collider c) {
        playerInRange = false;
    }
}
