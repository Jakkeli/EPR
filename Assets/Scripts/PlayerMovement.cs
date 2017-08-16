using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMoveState { Idle, Walking, Sprinting };

public class PlayerMovement : MonoBehaviour {

    public PlayerMoveState currentState;
    public float walkSpeed;
    public float sprintSpeed;
    float speed;
    bool sprinting;
    bool sprintCooldown;
    public float maxSprintTime = 5;
    public float sprintCooldownTime = 2;
    float tickTimeSprint;

    public float strafeModifier;

    float moveX;
    float moveY;
    Vector3 forward;
    public Vector3 moveDir;
    GameManager gm;
    bool jumped;
    bool goingDown;
    Rigidbody rb;


    public float jumpForce = 1;

    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        speed = walkSpeed;
    }
	
    void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumped = true;
    }

    void StartSprint() {
        tickTimeSprint = 0;
        sprinting = true;
        speed = sprintSpeed;
    }

    void EndSprint() {
        sprinting = false;
        tickTimeSprint = 0;
        sprintCooldown = true;
        speed = walkSpeed;
    }

	void Update () {
        if (gm.currentState != GameState.Run) return;

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        moveDir = new Vector3(moveX / strafeModifier, 0, moveY);

        transform.Translate(moveDir.normalized * speed * Time.deltaTime);

        if (Input.GetButtonDown("Sprint")) {
            if (!sprinting && !sprintCooldown) StartSprint();
        }

        if (Input.GetButtonUp("Sprint")) {
            if (sprinting) {
                sprinting = false;
                tickTimeSprint = 0;
                speed = walkSpeed;
            }

        }

        if (sprinting) {
            tickTimeSprint += Time.deltaTime;
            if (tickTimeSprint >= maxSprintTime) {
                EndSprint();
            }
        }

        if (sprintCooldown) {
            tickTimeSprint += Time.deltaTime;
            if (tickTimeSprint >= sprintCooldownTime) {
                sprintCooldown = false;
            }
        }



        if (Input.GetButtonDown("Jump")) {
            if (!jumped) Jump();
        }

        if (jumped) {

            if (rb.velocity.y == 0) {
                jumped = false;
            }
        }
    }
}
