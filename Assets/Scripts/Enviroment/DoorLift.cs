using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LiftDoorState { Open, Closed, MovingDown, MovingUp };

public class DoorLift : MonoBehaviour {

    public LiftDoorState currentState;
    LiftDoorState originalState;

    public float closedY;
    public float openY;

    public AudioSource move;
    public AudioSource bang;

    public float speed = 1;

    public DoorButton db;

    DoorResetManager drm;

    bool hasBeenInteractedWith;

	void Start () {
        drm = GameObject.Find("GameManager").GetComponent<DoorResetManager>();
        if (transform.localPosition.y < 0.75f) {
            currentState = LiftDoorState.Closed;            
        } else if (transform.localPosition.y > 0.75f) {
            currentState = LiftDoorState.Open;
        }
        originalState = currentState;
    }

    public void ResetDoor() {
        if (originalState == LiftDoorState.Closed) {
            Vector3 pos = transform.localPosition;
            pos.y = closedY;
            transform.localPosition = pos;
            db.ButtonColor(false);
        } else if (originalState == LiftDoorState.Open) {
            Vector3 pos = transform.localPosition;
            pos.y = openY;
            db.ButtonColor(true);
            transform.localPosition = pos;
        }
    }

    void OpenDoor() {
        if (currentState == LiftDoorState.Open || currentState == LiftDoorState.MovingUp) return;
        currentState = LiftDoorState.MovingUp;
        move.Play();
    }

    void CloseDoor() {
        if (currentState == LiftDoorState.Closed || currentState == LiftDoorState.MovingDown) return;
        currentState = LiftDoorState.MovingDown;
        move.Play();
    }

    public void ButtonPushed() {
        if (currentState == LiftDoorState.Closed) OpenDoor();
        if (currentState == LiftDoorState.Open) CloseDoor();
        if (!hasBeenInteractedWith) {
            drm.DoorInteractedWith(gameObject);
            hasBeenInteractedWith = true;
        }
    }
	
	void Update () {
		
        if (currentState == LiftDoorState.MovingDown) {
            if (transform.localPosition.y <= closedY) {
                currentState = LiftDoorState.Closed;
                db.ButtonColor(false);
                Vector3 pos = transform.localPosition;
                pos.y = closedY;
                transform.localPosition = pos;
                move.Stop();
                bang.Play();
            } else {
                transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
            }
        }

        if (currentState == LiftDoorState.MovingUp) {
            if (transform.localPosition.y >= openY) {
                currentState = LiftDoorState.Open;
                db.ButtonColor(true);
                Vector3 pos = transform.localPosition;
                pos.y = openY;
                transform.localPosition = pos;
                move.Stop();
                bang.Play();
            } else {
                transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
            }
        }

        //if (Input.GetKeyDown(KeyCode.K)) OpenDoor();
        //if (Input.GetKeyDown(KeyCode.L)) CloseDoor();
    }
}
