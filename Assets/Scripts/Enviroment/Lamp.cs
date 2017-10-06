using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

    public bool onAtStart = true;

    Light[] lights;

    bool lightsOn;

	void Start () {
        lights = GetComponentsInChildren<Light>();
        if (onAtStart) {
            foreach (Light light in lights) {
                light.enabled = true;
            }
        } else {
            foreach (Light light in lights) {
                light.enabled = false;
            }
        }
	}

    public void Lights(bool on) {
        if (on) {
            foreach (Light light in lights) {
                light.enabled = true;
            }
        } else {
            foreach (Light light in lights) {
                light.enabled = false;
            }
        }
    }
	
	void Update () {
		//if (Input.GetKeyDown(KeyCode.L)) {
  //          Lights(lightsOn);
  //          lightsOn = lightsOn ? false : true;
  //      }
	}
}
