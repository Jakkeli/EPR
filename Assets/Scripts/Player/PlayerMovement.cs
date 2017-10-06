using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMoveState { Idle, Walking, Sprinting };

public class PlayerMovement : MonoBehaviour {

    public PlayerMoveState currentState;

    public bool toggleToCrouch;
    public bool toggleToSprint;
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
    public bool jumped;
    public bool goingDown;
    Rigidbody rb;

    public float jumpForce = 1;
    
    public bool crouching;
    public float crouchSpeed;
    public float adsSpeed;
    public float crouchAdsSpeed;

    public GameObject normalCapsule;
    public GameObject crouchCapsule;

    public MouseAim mouseAim;
    public PlayerManager pManager;
    public PlayerShooter ps;

    RaycastHit hit;

    public LayerMask groundOnly;

    public bool grounded;

    public float groundAngle;

    public float maxGroundAngle = 120;

    public bool debugLines;

    Vector3 forwardDir;
    Vector3 sideDir;

    public HeadBob headBob;

    public float crouchGroundCheckDist = 0.805f;
    public float groundCheckDist = 1.05f;

    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        speed = walkSpeed;
        crouchSpeed = walkSpeed / 2;
        adsSpeed = walkSpeed / 1.5f;
        crouchAdsSpeed = walkSpeed / 3;
    }

    public bool ToggleToCrouch() {
        print("done");
        toggleToCrouch = toggleToCrouch ? false : true;
        return toggleToCrouch;
    }

    public bool ToggleToSprint() {
        toggleToSprint = toggleToSprint ? false : true;
        return toggleToSprint;
    }
	
    void Jump() {
        if (ps.adsOn) ps.ADSoff();
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumped = true;
    }

    public void ADS(bool ads) {
        if (crouching) {
            speed = ads ? crouchAdsSpeed : crouchSpeed;
        } else {
            speed = ads ? adsSpeed : walkSpeed;
        }
    }

    void StartSprint() {
        if (ps.adsOn) return;
        if (crouching) EndCrouch();
        ps.StartSprint();
        tickTimeSprint = 0;
        sprinting = true;
        speed = sprintSpeed;
        currentState = PlayerMoveState.Sprinting;
        headBob.Sprint(true);
    }

    public void EndSprint() {
        if (!sprinting) return;
        ps.EndSprint();
        sprinting = false;
        tickTimeSprint = 0;
        //sprintCooldown = true;
        speed = walkSpeed;
        headBob.Sprint(false);
    }

    void StartCrouch() {
        crouching = true;
        speed = crouchSpeed;
        crouchCapsule.SetActive(true);
        normalCapsule.SetActive(false);
        //mouseAim.CameraCrouch(true);
        headBob.Crouch(crouching);
        
    }

    void EndCrouch() {
        crouching = false;
        speed = walkSpeed;
        normalCapsule.SetActive(true);
        crouchCapsule.SetActive(false);
        //mouseAim.CameraCrouch(false);
        headBob.Crouch(crouching);
    }

    void GetGroundAngle() {
        if (!grounded) return;
        
        groundAngle = Vector3.Angle(hit.normal, transform.forward);

        forwardDir = Vector3.Cross(hit.normal, -transform.right);
        sideDir = Vector3.Cross(hit.normal, transform.forward);
        //Debug.DrawRay(transform.position, forwardDir * 5, Color.blue);
        //Debug.DrawRay(transform.position, sideDir * 5, Color.green);
    }

    void CheckGround() {
        float dist;
        //dist = crouching ? crouchGroundCheckDist : groundCheckDist;
        dist = groundCheckDist;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, dist, groundOnly)) {
            grounded = true;
        } else {
            grounded = false;
        }
        //Debug.DrawRay(transform.position, Vector3.down, Color.blue, dist);
    }

    void Move() {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        moveX = moveX / strafeModifier;
        if (groundAngle > maxGroundAngle) return;
        moveDir = forwardDir * moveY + sideDir * moveX;
        transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);
    }

    void Move2() {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        moveX = moveX / strafeModifier;
        if (groundAngle > maxGroundAngle) return;
        moveDir = forwardDir * moveY + sideDir * moveX;
        transform.position += moveDir.normalized * speed * Time.deltaTime;
    }

	void Update () {
        if (gm.currentState != GameState.Run || pManager.currentState != PlayerState.Alive) return;

        // moving

        CheckGround();
        GetGroundAngle();

        //Move();
        Move2();

        //moveX = Input.GetAxis("Horizontal");
        //moveY = Input.GetAxis("Vertical");
        //moveX = moveX / strafeModifier;
        //if (groundAngle <= maxGroundAngle) {
        //    moveDir = forwardDir * moveY + sideDir * moveX;
        //    transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);
        //}
        

        // crouching

        if (toggleToCrouch) {
            if (Input.GetButtonDown("Crouch") && !crouching) {
                StartCrouch();
            } else if (Input.GetButtonDown("Crouch") && crouching) {
                EndCrouch();
            }
        } else if (!toggleToCrouch) {
            if (Input.GetButton("Crouch") && !crouching) StartCrouch();
            if (crouching && Input.GetButtonUp("Crouch")) EndCrouch();
        }

        // sprinting

        if (crouching) return;

        if (!toggleToSprint) {
            if (Input.GetButtonDown("Sprint")) {
                if (!sprinting && !sprintCooldown) StartSprint();
            }
            if (Input.GetButtonUp("Sprint")) {
                if (sprinting) {
                    EndSprint();
                }
            }
        } else {
            if (Input.GetButtonDown("Sprint")) {
                if (!sprinting && !sprintCooldown) {
                    StartSprint();
                } else if (sprinting) {
                    EndSprint();
                }
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

        // idle check
        if (currentState != PlayerMoveState.Idle) {
            if (rb.velocity.x == 0 && rb.velocity.z == 0) {
                currentState = PlayerMoveState.Idle;
            }
        }

        // walk check
        if (currentState != PlayerMoveState.Walking && !sprinting) {
            if (rb.velocity.x > 0 || rb.velocity.z > 0) currentState = PlayerMoveState.Walking;
        }               

        if (Input.GetButtonDown("Jump")) {
            if (!jumped && grounded) Jump();
        }

        if (jumped) {
            if (grounded) {
                jumped = false;
            }
        }
    }
}
