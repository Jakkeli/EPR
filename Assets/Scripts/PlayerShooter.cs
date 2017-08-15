using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType { M3, RocketLauncher };
public enum GunState { Idle, Firing, Reloading };

public class PlayerShooter : MonoBehaviour {

    public GunType currentGunType;
    public GunState currentGunState;
    public Transform startPoint;
    public Transform theOtherPoint;

    public float maxRange;
    public float accuracy;
    public float fireRate;
    float tickTime;
    public LayerMask hitByPlayer;
    Ray ray;
    public PlayerMovement pm;
    Vector3 shootDir;
    public GameObject c;
    public GameObject thingie;
    GameManager gm;

    public ParticleSystem M3flash;
    public ParticleSystem M3smoke;
    public Animator M3animator;

    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        tickTime = fireRate;
    }
	
    void FireAGun() {
        if (currentGunType == GunType.M3) {
            M3flash.Play();
            M3smoke.Play();
            print("fire");
            shootDir = c.transform.forward;
            RaycastHit hit;
            ray = new Ray(startPoint.position, shootDir);
            if (Physics.Raycast(ray, out hit, maxRange, hitByPlayer)) {
                print("hit something");
                Quaternion rot = hit.transform.rotation;
                Instantiate(thingie, hit.point, rot);
            }
        }        
    }

	void Update () {
        if (gm.currentState != GameState.Run) return;
        if (Input.GetButton("Fire1")) {
            if (currentGunState == GunState.Idle) {
                currentGunState = GunState.Firing;
                M3animator.SetBool("FireM3", true);
            }
            tickTime += Time.deltaTime;
            if (tickTime >= fireRate) {
                FireAGun();
                tickTime -= fireRate;
            }
        }
        if (Input.GetButtonUp("Fire1") && currentGunState == GunState.Firing) {
            currentGunState = GunState.Idle;
            M3animator.SetBool("FireM3", false);
        }
	}
}
