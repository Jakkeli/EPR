using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponM3 : MonoBehaviour {

    public float recoilDistance = 0.1f;
    public float recoilSpeed = 1;

    int recoilTicks;
    public int maxRecoilTicks = 10;

    bool recoiling;
    bool returning;
    bool forward;
    Vector3 originalPos;

	void Start () {
		
	}
	
	public void StartRecoiling() {
        returning = false;
        originalPos = transform.localPosition;
        recoilTicks = 0;
        recoiling = true;
        forward = false;
    }

    public void StopRecoiling() {
        returning = true;
    }

	void Update () {
		if (recoiling) {
            if (!forward) {
                transform.localPosition -= new Vector3(0, 0, recoilDistance * Time.deltaTime * recoilSpeed);
                recoilTicks++;
                if (recoilTicks >= maxRecoilTicks) {
                    recoilTicks = 0;
                    forward = true;
                }
            } else if (forward) {
                transform.localPosition += new Vector3(0, 0, recoilDistance * Time.deltaTime * recoilSpeed);
                recoilTicks++;
                if (recoilTicks >= maxRecoilTicks) {
                    if (returning) {
                        transform.localPosition = originalPos;
                        recoiling = false;
                    }
                    recoilTicks = 0;
                    forward = false;
                }
            }
        }
	}
}
