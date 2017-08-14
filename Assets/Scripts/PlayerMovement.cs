using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float walkSpeed;
    public float sprintSpeed;
    public float strafeModifier;

    float moveX;
    float moveY;
    Vector3 forward;
    public Vector3 moveDir;

    void Start () {
		
	}
	
	void Update () {

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        //forward = new Vector3(Vector3.forward.x, 0, Vector3.forward.z);

        //transform.Translate(forward * moveY * walkSpeed * Time.deltaTime);

        moveDir = new Vector3(moveX / strafeModifier, 0, moveY);

        transform.Translate(moveDir.normalized * walkSpeed * Time.deltaTime);
    }
}
