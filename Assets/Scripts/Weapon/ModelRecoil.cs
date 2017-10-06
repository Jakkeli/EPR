using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRecoil : MonoBehaviour {

    public float hipRecoilDistance = 0.1f;
    public float recoilSpeed = 1;
    float adsRecoilDistance = 0.05f;
    float recoilDistance;

    public int recoilTicks;
    public int maxRecoilTicks = 10;

    public bool recoiling;
    public bool returning;
    bool forward;
    Vector3 originalPos;

    private void Start() {
        recoilDistance = hipRecoilDistance;
    }

    public void StartRecoiling() {
        returning = false;        
        if (!recoiling) originalPos = transform.localPosition;
        recoiling = true;
    }

    public void StopRecoiling() {
        returning = true;
    }

    public void ADSon() {
        recoilDistance = adsRecoilDistance;
    }

    public void ADSoff() {
        recoilDistance = hipRecoilDistance;
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
