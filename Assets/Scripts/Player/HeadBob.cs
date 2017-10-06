using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour {

    public float walkBobSpeed;
    float bobSpeed;
    float sprintBobSpeed;
    float adsBobSpeed;
    float crouchBobSpeed;
    float crouchAdsBobSpeed;
    public float bobAmount;
    float midPoint;
    float timer;
    Vector3 localPos;
    float translateChange;

    float crouchYoffset = 0.4f;
    float crouchY;

    bool adsOn;

    void Start () {
        midPoint = transform.localPosition.y;
        bobSpeed = walkBobSpeed;
        sprintBobSpeed = walkBobSpeed * 2;
        adsBobSpeed = walkBobSpeed / 1.5f;
        crouchBobSpeed = walkBobSpeed / 2;
        crouchAdsBobSpeed = walkBobSpeed / 3;
    }

    public void Sprint(bool sprint) {
        bobSpeed = sprint ? sprintBobSpeed : walkBobSpeed;
    }
	
    public void ADS(bool ads) {
        bobSpeed = ads ? adsBobSpeed : walkBobSpeed;
        adsOn = ads ? true : false;
    }

    public void Crouch(bool crouch) {
        crouchY = crouch ? crouchYoffset : 0;
        if (crouch && adsOn) {
            bobSpeed = crouchAdsBobSpeed;
        } else if (crouch && !adsOn) {
            bobSpeed = crouchBobSpeed;
        } else if (!crouch && !adsOn) {
            bobSpeed = walkBobSpeed;
        } else if (!crouch && adsOn) {
            bobSpeed = adsBobSpeed;
        }
    }

	void Update () {
        float waveslice = 0;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
            timer = 0.0f;
        } else {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobSpeed;
            if (timer > Mathf.PI * 2) {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0) {
            translateChange = waveslice * bobAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            localPos = transform.localPosition;
            localPos.y = midPoint + translateChange - crouchY;
            transform.localPosition = localPos;
        } else {
            localPos = transform.localPosition;
            localPos.y = midPoint + translateChange - crouchY;
            transform.localPosition = localPos;
        }
    }
}
