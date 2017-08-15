using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour {

    bool recoiling;
    bool up;
    float tickTime;
    public float recoilTime = 0.2f;
    public float straightRecoil = 0.1f;
    int upTick;
    int downTick;

	void Start () {
		
	}

    public void Fire() {
        if (recoiling) return;
        up = true;
        recoiling = true;
        //transform.Rotate(Vector3.right, -15);
        //transform.Translate(Vector3.back * straightRecoil);
    }
	
	// Update is called once per frame
	void Update () {
		if (recoiling) {
            //tickTime += Time.deltaTime;
            //if (tickTime >= recoilTime) {
            //    recoiling = false;
            //    transform.Rotate(Vector3.right, 15);
            //    transform.Translate(Vector3.forward * straightRecoil);
            //}
            if (up) {
                transform.Rotate(Vector3.right, -3);
                transform.Translate(0, 0, -0.01f);
                upTick++;
                if (upTick >= 15) {
                    up = false;
                }
            } else {
                transform.Rotate(Vector3.right, 3);
                transform.Translate(0, 0, 0.01f);
                downTick++;
                if (downTick >= 15) {
                    upTick = 0;
                    downTick = 0;
                    recoiling = false;
                }
            }
            
        }
	}
}
