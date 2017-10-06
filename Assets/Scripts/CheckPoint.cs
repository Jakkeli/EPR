using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	public int checkPointNumber;

    public bool firstCP;
    bool triggered;

    GameManager gm;

    public float rotation;

	void Start () {
        if (firstCP) triggered = true;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    void PlayerEnter() {
        triggered = true;
        gm.NewCheckPointReached(checkPointNumber, rotation);
    }
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player" && !triggered) PlayerEnter();
    }
}
