using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    GameManager gm;

	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            print("GOAL!1!");
            StartCoroutine(gm.Goal());
        }
    }
}
