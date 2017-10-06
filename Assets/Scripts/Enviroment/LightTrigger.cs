using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightTriggerType { SimpleOn, SimpleOff, WhileStayInTrigger, StaggeredOn };

public class LightTrigger : MonoBehaviour {

    bool triggered;

    public bool instantLightsOn;
    public LightTriggerType lightTriggerType;

    public Lamp[] lamps;

    bool startStagger;

    float tickTime;

    public float interval;

    int index;

	void Start () {
		
	}
	
	void Update () {
		if (startStagger) {
            tickTime += Time.deltaTime;
            if (tickTime >= interval) {
                if (index >= lamps.Length) {
                    startStagger = false;
                    return;
                }
                lamps[index].Lights(true);
                index++;
            }
        }
	}

    void OnTriggerEnter(Collider c) {
        if (!triggered && c.tag == "Player") {
            if (lightTriggerType == LightTriggerType.SimpleOn) {
                foreach (Lamp lamp in lamps) {
                    lamp.Lights(true);
                }                
            } else if (lightTriggerType == LightTriggerType.SimpleOff) {
                foreach (Lamp lamp in lamps) {
                    lamp.Lights(false);
                }
            } else if (lightTriggerType == LightTriggerType.StaggeredOn) {
                startStagger = true;
            }


            triggered = true;
        }
    }
}
