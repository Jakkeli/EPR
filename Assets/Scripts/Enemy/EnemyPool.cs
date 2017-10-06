using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {

    List<Vector3> positions = new List<Vector3>() { };


    void Start () {
        CreatePositions();
	}

    void CreatePositions() {
        for (int i = 0; i < 20; i++) {
            for (int j = 0; j < 10; j++) {
                positions.Add(new Vector3(i * 2, 0, j * -2));
            }
        }
    }

    public void FillASlot() {

    }

    public Vector3 emptySlot() {
        return new Vector3(0, 0, 0);
    }
	
	void Update () {
		
	}
}
