using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType { M3, RocketLauncher };
public enum GunState { Idle, Firing, Reloading };

public class PlayerShooter : MonoBehaviour {

    public GunType currentGunType;
    public GunState currentGunState;
    public Transform startPoint;

    public float M3maxRange;
    public float M3accuracy;
    public float M3fireRate;
    float tickTimeM3;
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

    public ParticleSystem bzkFlash;
    public ParticleSystem bzkSmoke;

    public float bazookaFireRate;
    public float bazookaMaxRange;
    public GameObject M3;
    public WeaponM3 weaponM3;
    public GameObject bazooka;
    float tickTimeBazooka;
    bool bazookaFired;
    public Transform bazookaStartPoint;
    public GameObject rocket;
    public MouseAim mouseAim;

    public AudioSource M3fireSound;
    public AudioSource bazookaFire;

    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //tickTime = fireRate;
    }

    void ChangeWeapon() {
        if (currentGunType == GunType.M3) {
            currentGunType = GunType.RocketLauncher;
            M3.SetActive(false);
            bazooka.SetActive(true);
        } else {
            currentGunType = GunType.M3;
            bazooka.SetActive(false);
            M3.SetActive(true);
        }
        tickTimeM3 = 0;
    }
	
    void FireM3() {
        if (currentGunType == GunType.M3) {
            M3fireSound.Play();
            mouseAim.RecoilM3();
            M3flash.Play();
            M3smoke.Play();
            print("fire M3");
            shootDir = c.transform.forward;
            RaycastHit hit;
            ray = new Ray(startPoint.position, shootDir);

            if (Physics.Raycast(ray, out hit, M3maxRange, hitByPlayer)) {
                print("hit something");
                Quaternion rot = hit.transform.rotation;
                Instantiate(thingie, hit.point, rot);
                if (hit.transform.GetComponent<Enemy>() != null) {
                    hit.transform.GetComponent<Enemy>().TakeAHit(transform.position);
                }
            }
        }        
    }

    void FireBazoooka() {
        bazookaFired = true;
        bazookaFire.Play();
        bzkFlash.Play();
        bzkSmoke.Play();
        print("Fire bazooka");
        shootDir = c.transform.forward;
        Instantiate(rocket, bazookaStartPoint.position, bazooka.transform.rotation);
        rocket.GetComponent<Rocket>().Fire(shootDir);
    }

	void Update () {
        if (gm.currentState != GameState.Run) return;

        //print(Input.GetAxis("Mouse ScrollWheel"));

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        if (mouseWheel != 0) {
            ChangeWeapon();
        }

        if (currentGunType == GunType.M3) {
            if (Input.GetButtonDown("Fire1")) {
                if (currentGunState == GunState.Idle) {
                    currentGunState = GunState.Firing;
                    M3animator.SetBool("FireM3", true);
                    weaponM3.StartRecoiling();
                }
                FireM3();
            }

            if (Input.GetButton("Fire1")) {
                if (currentGunState == GunState.Idle) {
                    currentGunState = GunState.Firing;
                    M3animator.SetBool("FireM3", true);
                }
                tickTimeM3 += Time.deltaTime;
                if (tickTimeM3 >= M3fireRate) {
                    FireM3();
                    tickTimeM3 -= M3fireRate;
                }

            }
            if (Input.GetButtonUp("Fire1") && currentGunState == GunState.Firing) {
                currentGunState = GunState.Idle;
                M3animator.SetBool("FireM3", false);
                tickTimeM3 = 0;
                weaponM3.StopRecoiling();
            }
        } else if (currentGunType == GunType.RocketLauncher) {
            if (Input.GetButtonDown("Fire1")) {
                if (!bazookaFired) {
                    FireBazoooka();
                }
            }
            
        }
        if (bazookaFired) {
            tickTimeBazooka += Time.deltaTime;
            if (tickTimeBazooka >= bazookaFireRate) {
                bazookaFired = false;
                tickTimeBazooka -= bazookaFireRate;
            }
        }

    }
}
