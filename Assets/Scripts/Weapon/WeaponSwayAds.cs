using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwayAds : MonoBehaviour {    

    PlayerManager pm;
    GameManager gm;

    public Transform hipTarget;
    public Vector3 hipPos;
    Quaternion hipRotation;

    public Transform adsTarget;
    public Vector3 adsPos;
    Quaternion adsRotation;

    public Transform sprintTarget;
    public Vector3 sprintPos;    
    Quaternion sprintRotation;

    public Transform swapTarget;
    public Vector3 swapPos;
    Quaternion swapRotation;

    public Vector3 currentPos;
    public Vector3 targetPos;
    public Vector3 newOldPos;

    Quaternion currentRotation;
    Quaternion targetRotation;
    Quaternion newOldRotation;

    public float sprintSwayAmountX;
    public float sprintSwayAmountY;
    public float sprintSwaySpeed;
    public Vector2 sprintSwayAmount;

    public float swaySpeed;

    public float swayX;
    public float swayY;

    float adsSwaySpeed;
    public Vector2 adsSwayAmount;

    public float hipSwaySpeed = 1;
    public Vector2 hipSwayAmount;

    public float crouchSwayAmountX;
    public float crouchSwayAmountY;
    public float crouchSwaySpeed;

    public float crouchAdsSwayAmountX;
    public float crouchAdsSwayAmountY;
    public float crouchAdsSwaySpeed;

    public Vector2 swapSwayAmount;
    public float swapSwaySpeed;

    public float adsModifierX;
    public float adsModifierY;
    public float adsModifierSpeed;

    public float adsYSwayOffset = 0.01f;

    public bool ads;

    public bool sprinting;

    public bool crouch;

    float swayTime;

    float locRotChange;  // 0 = walk, 1 = sprint

    public float magicNumber;

    public float locRotChangeSpeed = 1;

    Vector2 currentSway;
    Vector2 targetSway;

    public float swayChangeSpeed = 1;

    float swayCycle; // 0 .... 1

    void Start () {
        hipPos = hipTarget.localPosition;
        hipRotation = hipTarget.localRotation;

        sprintPos = sprintTarget.localPosition;
        sprintRotation = sprintTarget.localRotation;

        adsPos = adsTarget.localPosition;
        adsRotation = adsTarget.localRotation;

        swapPos = swapTarget.localPosition;
        swapRotation = swapTarget.localRotation;

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        pm = GetComponentInParent<PlayerManager>();
            
        targetPos = hipPos;
        currentPos = targetPos;
        targetRotation = hipRotation;
        currentRotation = targetRotation;
        transform.localPosition = targetPos;
        transform.localRotation = targetRotation;

        adsSwayAmount.x = hipSwayAmount.x / adsModifierX;
        adsSwayAmount.y = hipSwayAmount.y / adsModifierY;
        adsSwaySpeed = hipSwaySpeed / adsModifierSpeed;

        swaySpeed = hipSwaySpeed;
        currentSway = hipSwayAmount;
        targetSway = hipSwayAmount;
        newOldPos = hipPos;
        newOldRotation = hipRotation;
    }

    public void StartSprint() {
        //print("start sprint");
        locRotChange = 0;
        sprinting = true;

        currentPos = newOldPos;
        targetPos = sprintPos;
        newOldPos = sprintPos;

        currentRotation = newOldRotation;
        targetRotation = sprintRotation;
        newOldRotation = sprintRotation;

        currentSway = targetSway;
        targetSway = sprintSwayAmount;

        swaySpeed = sprintSwaySpeed;
    }

    public void EndSprint() {
        //print("end sprint");
        locRotChange = 0;
        sprinting = false;

        currentPos = newOldPos;
        targetPos = hipPos;
        newOldPos = hipPos;

        currentRotation = newOldRotation;
        targetRotation = hipRotation;
        newOldRotation = hipRotation;

        currentSway = targetSway;
        targetSway = hipSwayAmount;

        swaySpeed = hipSwaySpeed;
    }

    public void ADSon() {
        //print("ads on");
        locRotChange = 0;
        sprinting = false;
        currentPos = newOldPos;
        targetPos = adsPos;
        newOldPos = adsPos;

        currentRotation = newOldRotation;
        targetRotation = adsRotation;
        newOldRotation = adsRotation;

        currentSway = targetSway;
        targetSway = adsSwayAmount;

        swaySpeed = adsSwaySpeed;
        ads = true;
    }

    public void ADSoff() {
        locRotChange = 0;
        currentPos = adsPos;
        targetPos = hipPos;
        newOldPos = hipPos;

        currentRotation = newOldRotation;
        targetRotation = hipRotation;
        newOldRotation = hipRotation;

        currentSway = targetSway;
        targetSway = hipSwayAmount;

        swaySpeed = hipSwaySpeed;
        ads = false;
    }

    public void SwapWeaponOut(bool swapOut) {
        if (swapOut) {
            locRotChange = 0;

            currentPos = newOldPos;
            targetPos = swapPos;
            newOldPos = swapPos;

            currentRotation = newOldRotation;
            targetRotation = swapRotation;
            newOldRotation = swapRotation;

            currentSway = targetSway;
            targetSway = swapSwayAmount;
            swaySpeed = swapSwaySpeed;
        } else if (!swapOut) {
            if (ads) {
                sprinting = false;
                ADSon();
            } else if (sprinting) {
                ads = false;
                StartSprint();
            } else {
                ads = false;
                EndSprint();
            }
        }
    } 
	
	void Update () {
        if (gm.currentState != GameState.Run || pm.currentState != PlayerState.Alive) return;

        if (Input.GetKeyDown(KeyCode.T)) StartSprint();
        if (Input.GetKeyDown(KeyCode.Y)) EndSprint();

        if (Input.GetKeyDown(KeyCode.KeypadMultiply)) magicNumber = 1;
        if (Input.GetKeyDown(KeyCode.KeypadDivide)) magicNumber = -1;


        swayCycle += swaySpeed * Time.deltaTime;
        swayCycle -= (int)swayCycle;

        locRotChange += locRotChangeSpeed * Time.deltaTime * 1; // magicNumber
        locRotChange = Mathf.Clamp01(locRotChange);

        transform.localPosition = Vector3.Lerp(currentPos, targetPos, locRotChange);

        transform.localRotation = Quaternion.Lerp(currentRotation, targetRotation, locRotChange);

        currentSway = Vector2.MoveTowards(currentSway, targetSway, swayChangeSpeed * Time.deltaTime);

        swayX = Mathf.Sin(swayCycle * 2 * Mathf.PI) * currentSway.x;
        swayY = Mathf.Sin(swayCycle * 2 * 2 * Mathf.PI) * currentSway.y;
        transform.localPosition = transform.localPosition + new Vector3(swayX, swayY, 0);
    }
}
