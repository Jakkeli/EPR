using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    Enemy e;

	void Start () {
        e = GetComponent<Enemy>();
	}
	
    public void Resurrect() {
        e.Resurrect();
    }

    public void MoveToPool() {
        print("No function -MoveToPool-");
    }

	void Update () {
		
	}
}
