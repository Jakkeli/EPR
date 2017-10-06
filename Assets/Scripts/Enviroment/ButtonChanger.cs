using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChanger : MonoBehaviour {

    public Material red;
    public Material green;

    Material m;
    Renderer r;

    public float speed = 1;

    void Start () {
        r = GetComponent<Renderer>();
	}

    public void ChangeMaterial(bool greenM) {
        m = greenM ? green : red;
        r.material = m;
    }
	
	void Update () {
		//if (r.material.color.)
	}
}
