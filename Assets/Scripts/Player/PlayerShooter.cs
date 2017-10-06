using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType { M3, RocketLauncher, Shotgun };
public enum GunState { Idle, Firing, Reloading, Changing };

public class PlayerShooter : MonoBehaviour {

    public GunType currentGunType;
    public GunState currentGunState;
    public Transform M3bulletStartPoint;
    public Transform shotgunPelletStartPoint;

    public float M3maxRange;
    public float M3accuracy;
    public float M3fireRate;
    public int M3damage = 1;
    public int shotgunDamage= 1;
    float tickTimeM3;
    public LayerMask hitByPlayer;
    public LayerMask wall;
    Ray ray;
    Ray ray2;
    Ray ray3;
    public PlayerMovement pm;
    public PlayerManager pManager;
    Vector3 shootDir;
    public GameObject c;
    public GameObject thingie;
    GameManager gm;
    public WeaponManager wm;

    public ParticleSystem M3flash;
    public ParticleSystem M3smoke;
    public Animator M3animator;
    public Animator shotgunAnimator;

    public AnimationClip shotgunStartReload;
    public AnimationClip shotgunShellIn;
    public AnimationClip shotgunEndReload;
    public AnimationClip shotgunFire;

    public ParticleSystem bzkFlash;
    public ParticleSystem bzkSmoke;

    public ParticleSystem shotgunFlash;
    public ParticleSystem shotgunSmoke;

    public float bazookaFireRate;
    public float bazookaMaxRange;
    //public GameObject M3;
    public GameObject bazooka;
    public ModelRecoil M3recoil;
    public ModelRecoil shotgunRecoil;
    float tickTimeBazooka;
    bool bazookaFired;
    public Transform bazookaStartPoint;
    public GameObject rocket;
    public MouseAim mouseAim;

    public AudioSource M3fireSound;
    public AudioSource reloadM3sound;

    public AudioSource bazookaFire;

    public AudioSource shotgunFireSound;
    public AudioSource reloadShotgun;
    public AudioSource pumpShotgun;
    public float pumpSoundDelay;

    public AudioSource gunEmptyClick;



    public WeaponSwayAds m3Sway;
    public WeaponSwayAds shotgunSway;
    public bool adsPressToToggle;
    public bool adsOn;

    public float M3maxReserveAmmunition;
    public float M3reserveAmmunition;
    public float M3clipAmmunition;
    public float M3reloadTime = 0.5f;
    public float M3clipSize = 30;

    public float RLmaxReserveAmmunition;
    public float RLreserveAmmuniton;
    public float RLclipAmmunition;
    public float RLreloadTime = 1.0f;
    public float RLclipSize = 1;

    public float ShotgunMaxReserveAmmunition;
    public float ShotgunReserveAmmunition;
    public float ShotgunClipAmmunition;
    public float ShotgunReloadTime = 0.5f;
    public float ShotgunClipSize = 5;

    //float reloadTime;
    float tickTimeReload;

    public float shotgunMaxRange = 25;

    float tickTimeShotgun;

    public float shotgunFireRate = 0.5f;

    bool recoilStopped;

    public ShellEjector M3ejectorHip;
    public ShellEjector M3ejectorADS;

    public ShellEjector shotgunEjectorHip;
    public ShellEjector shotgunEjectorADS;

    CrosshairFader chFader;

    bool emptyClickPlayed;

    PlayerWeaponAnimationController pwac;
    public BulletDecalManager bdm;

    public GameObject shotgunPelletDir1;
    public GameObject shotgunPelletDir2;
    public GameObject shotgunPelletDir3;

    public float maxShotgunSpread;

    bool canFire = true;

    public float shotgunShellEjectDelay = 0.2f;

    float shotgunClickFireCD;
    bool shotgunCanFire = true;
    bool shotgunFireStarted;

    public float rightTrigger;
    public bool padFireStarted;

    public float leftTrigger;
    public bool leftTriggerWasPressed;
    public bool leftTriggerWasReleased = true;

    public float padSensitivity = 1;

    public int shotgunPellets = 7;

    public GameObject[] shotgunRandomDirObjects;
    //public List<Vector3> shotgunRandomDirections; 

    void Start () {
        pwac = GetComponent<PlayerWeaponAnimationController>();
        chFader = GetComponent<CrosshairFader>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        UpdateAmmoCount();
        shotgunRandomDirObjects = GameObject.FindGameObjectsWithTag("srdo");
        //print(shotgunFire.length);
        //shotgunAnimator.SetBool("idle", true);        
    }

    public bool ToggleToADS() {
        adsPressToToggle = adsPressToToggle ? false : true;
        return adsPressToToggle;
    }

    public void UpdateAmmoCount() {
        if (currentGunType == GunType.M3) {
            gm.UpdateHud(M3clipAmmunition, M3reserveAmmunition);
        } else if (currentGunType == GunType.RocketLauncher) {
            gm.UpdateHud(RLclipAmmunition, RLreserveAmmuniton);
        } else if (currentGunType == GunType.Shotgun) {
            gm.UpdateHud(ShotgunClipAmmunition, ShotgunReserveAmmunition);
        }
    }

    public void ResetAmmosAfterDeath() {
        if (M3reserveAmmunition < 200) M3reserveAmmunition = 200;
        M3clipAmmunition = M3clipSize;
        if (ShotgunReserveAmmunition < 30) ShotgunReserveAmmunition = 30;
        ShotgunClipAmmunition = ShotgunClipSize;
        UpdateAmmoCount();
    }

    public void WeaponChange() {
        if (currentGunState == GunState.Reloading) {
            StopReloading();
            currentGunState = GunState.Idle;
        }
        tickTimeM3 = 0;
    }

    void FireM3() {
        if (currentGunType == GunType.M3) {
            if (M3clipAmmunition == 0) {
                pwac.StopWeaponAnimations(currentGunType);
                pwac.M3LastFired();
                if(!emptyClickPlayed) {
                    gunEmptyClick.Play();
                    emptyClickPlayed = true;
                }                
                //print("empty clip reload!!");
            } else {
                pwac.M3StartFiring();
                M3fireSound.Play();
                mouseAim.RecoilM3();
                M3flash.Play();
                M3smoke.Play();
                if (adsOn) {
                    M3ejectorADS.EjectShell();
                } else {
                    M3ejectorHip.EjectShell();
                }
                //print("fire M3");
                shootDir = c.transform.forward;
                RaycastHit hit;
                ray = new Ray(M3bulletStartPoint.position, shootDir);

                if (Physics.Raycast(ray, out hit, M3maxRange, hitByPlayer)) {
                    //print("hit something");
                    //Quaternion rot = hit.transform.rotation;
                    //Instantiate(thingie, hit.point, rot);

                    if (hit.transform.GetComponent<EnemyPart>() != null) {
                        hit.transform.GetComponent<EnemyPart>().Hit(M3damage);
                    } else {
                        bdm.PlaceBulletHole(hit.point, hit.normal);
                    }
				}
                //if (Physics.Raycast(ray, out hit, M3maxRange, wall)) {
                //    //print("hit a wall");
                //    Quaternion rot = hit.transform.rotation;
                //    bdm.PlaceBulletHole(hit.point, hit.normal);
                   
                //}
                M3clipAmmunition--;
                UpdateAmmoCount();
            }            
        }        
    }

    void Reload() {
        //print("reload plz?");
        if (currentGunState == GunState.Reloading) {
            return;
        } else {
            pm.EndSprint();
            if (currentGunType == GunType.M3) {
                if (M3reserveAmmunition == 0) {
                    //print("out of reserve ammo");
                    return;
                } else if (M3clipAmmunition == M3clipSize) {
                    //print("clip full");
                    return;
                } else {
                    StopFiring();
                    currentGunState = GunState.Reloading;
                    StartCoroutine(M3Reload());
                }
            } else if (currentGunType == GunType.RocketLauncher) {
                if (RLreserveAmmuniton == 0) {
                    //print("out of reserve ammo");
                    return;
                } else {
                    tickTimeReload = 0;
                    //reloadTime = RLreloadTime;
                    StopFiring();
                    currentGunState = GunState.Reloading;
                }                
            } else if (currentGunType == GunType.Shotgun) {
                if (ShotgunReserveAmmunition == 0) {
                    //print("out of reserve ammo");
                    return;
                } else if (ShotgunClipAmmunition == ShotgunClipSize) {
                    return;
                } else {
                    tickTimeReload = 0;
                    //reloadTime = ShotgunReloadTime;
                    StopFiring();
                    currentGunState = GunState.Reloading;
                    StartCoroutine(pwac.ShotgunStartReload());
                }
            }
        }        
    }

    void StopReloading() {
        if (reloadM3sound.isPlaying) reloadM3sound.Stop();
        currentGunState = GunState.Idle;
        tickTimeReload = 0;
        pwac.StopWeaponAnimations(currentGunType);
        //M3animator.SetBool("reload", false);
        //shotgunAnimator.SetBool("reload", false);
        //shotgunAnimator.SetBool("idle", true);
    }

    void FireBazoooka() {
        if (RLclipAmmunition == 0) {
            //print("reload!!!");
            gunEmptyClick.Play();
        } else {
            bazookaFired = true;
            bazookaFire.Play();
            bzkFlash.Play();
            bzkSmoke.Play();
            //print("Fire bazooka");
            shootDir = c.transform.forward;
            Instantiate(rocket, bazookaStartPoint.position, bazooka.transform.rotation);
            rocket.GetComponent<Rocket>().Fire(shootDir);
            RLclipAmmunition--;
            UpdateAmmoCount();
        }
        
    }

    void ADSon() {
        if (pm.jumped || pm.goingDown) return;
        adsOn = true;
        chFader.CrosshairFadeOut();
        if (currentGunType == GunType.M3) {
            m3Sway.ADSon();
            mouseAim.maxRecoil = 0.5f;
            M3recoil.ADSon();
        } else if (currentGunType == GunType.Shotgun) {
            shotgunSway.ADSon();
            mouseAim.maxRecoil = 0.5f;
            shotgunRecoil.ADSon();
        }     
        pm.ADS(true);
        mouseAim.ADS(true);
    }

    public void ADSoff() {
        adsOn = false;
        chFader.CrosshairFadeIn();
        if (currentGunType == GunType.M3) {
            m3Sway.ADSoff();
            mouseAim.maxRecoil = 1f;
            M3recoil.ADSoff();
        } else if (currentGunType == GunType.Shotgun) {
            shotgunSway.ADSoff();
            mouseAim.maxRecoil = 1f;
            shotgunRecoil.ADSoff();
        }
        pm.ADS(false);
        mouseAim.ADS(false);
    }

    void FireShotgun() {
        if (ShotgunClipAmmunition == 0) {
            gunEmptyClick.Play();
            //print("empty clip reload!!");
        } else {
            if (!shotgunFireStarted) {
                pwac.StopWeaponAnimations(currentGunType);
                StartCoroutine(pwac.ShotgunFire());
                shotgunFireStarted = true;
            }
            
            shotgunFireSound.Play();
            mouseAim.RecoilM3();
            shotgunFlash.Play();
            shotgunSmoke.Play();
            //print("fire shotgun");

            ShotgunRays3();

            StartCoroutine(ShotgunShellEject());

            ShotgunClipAmmunition--;
            UpdateAmmoCount();
            pumpShotgun.PlayDelayed(pumpSoundDelay);
            shotgunCanFire = false;
        }
    }

    void ShotgunRays3() {
        for (int i = 0; i < shotgunPellets; i++) {
            int randomIndex = Random.Range(0, shotgunRandomDirObjects.Length);
            shootDir = c.transform.forward + shotgunRandomDirObjects[randomIndex].transform.forward;
            ray = new Ray(shotgunPelletStartPoint.position, shootDir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, shotgunMaxRange, hitByPlayer)) {
                if (hit.transform.GetComponent<EnemyPart>() != null) {
                    hit.transform.GetComponent<EnemyPart>().Hit(shotgunDamage);
                    //print("hit enemypart");
                } else {
                    bdm.PlaceBulletHole(hit.point, hit.normal);
                    //print("hit wall");
                }
            }
        }
    }

    void ShotgunRays2() {
        for (int i = 0; i < shotgunPellets; i++) {
            shootDir = c.transform.forward;
            ray = new Ray(GetRandomizedPos(shotgunPelletStartPoint.position), shootDir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, shotgunMaxRange, hitByPlayer)) {
                if (hit.transform.GetComponent<EnemyPart>() != null) {
                    hit.transform.GetComponent<EnemyPart>().Hit(shotgunDamage);
                    //print("hit enemypart");
                } else {
                    bdm.PlaceBulletHole(hit.point, hit.normal);
                    print("hit wall");
                }
            }
        }
    }

    Vector3 GetRandomizedPos(Vector3 center) {
        Vector3 returnDir;
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(-1, 1);
        returnDir = center + new Vector3(randomX / 10, randomY / 10);
        return returnDir;
    }

    void ShotgunRays() {
        for (int i = 0; i < shotgunPellets; i++) {
            shootDir = c.transform.forward;
            Vector3 dir = shootDir + RandomDir();
            //print(dir);
            ray = new Ray(shotgunPelletStartPoint.position, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, shotgunMaxRange, hitByPlayer)) {
                if (hit.transform.GetComponent<EnemyPart>() != null) {
                    hit.transform.GetComponent<EnemyPart>().Hit(shotgunDamage);
                    print("hit enemypart");
                } else {
                    bdm.PlaceBulletHole(hit.point, hit.normal);
                }
            }            
        }
    }

    IEnumerator ShotgunShellEject() {
        yield return new WaitForSeconds(shotgunShellEjectDelay);
        if (adsOn) {
            shotgunEjectorADS.EjectShell();
        } else {
            shotgunEjectorHip.EjectShell();
        }
    }

    Vector3 RandomDir() {
        float x = Random.Range(-maxShotgunSpread, maxShotgunSpread);
        float y = Random.Range(-maxShotgunSpread, maxShotgunSpread);
        Vector3 dir = new Vector3(x * 1.5f, y, 0);
        return dir;
    }

    void StopReloadAnimation() {
        M3animator.SetBool("reload", false);
    }

    IEnumerator M3Reload() {
        currentGunState = GunState.Reloading;
        reloadM3sound.Play();
        pwac.StopWeaponAnimations(currentGunType);
        StartCoroutine(pwac.M3StartReload(M3reloadTime));
        yield return new WaitForSeconds(M3reloadTime);
        if ((M3clipSize - M3clipAmmunition) >= M3reserveAmmunition) {
            M3clipAmmunition += M3reserveAmmunition;
            M3reserveAmmunition = 0;
            UpdateAmmoCount();
            currentGunState = GunState.Idle;
        } else {
            M3reserveAmmunition -= M3clipSize - M3clipAmmunition;
            M3clipAmmunition = M3clipSize;
            UpdateAmmoCount();
            currentGunState = GunState.Idle;
        }
    }

    public void StartSprint() {
        if (currentGunType == GunType.M3) {
            m3Sway.StartSprint();
        } else if (currentGunType == GunType.Shotgun) {
            shotgunSway.StartSprint();
        }
    }

    public void EndSprint() {
        if (currentGunType == GunType.M3) {
            if (!adsOn) m3Sway.EndSprint();
        } else if (currentGunType == GunType.Shotgun) {
            if (!adsOn) shotgunSway.EndSprint();
        }
    }

    void StopFiring() {
        currentGunState = GunState.Idle;
        if (currentGunType == GunType.M3) {
            pwac.M3StopFiring();
        }
        tickTimeM3 = 0;
        tickTimeShotgun = 0;
        M3recoil.StopRecoiling();
        shotgunRecoil.StopRecoiling();
        recoilStopped = false;
        emptyClickPlayed = false;
    }

	void Update () {
        if (gm.currentState != GameState.Run || pManager.currentState != PlayerState.Alive) return;

        // reloading

        if (Input.GetButtonDown("Reload")) {
            canFire = false;
            Reload();
        }

        if (currentGunState == GunState.Reloading) {
            if (currentGunType == GunType.M3) {
                //tickTimeReload += Time.deltaTime;
                //if (tickTimeReload >= M3reloadTime) {                    
                //    if ((M3clipSize - M3clipAmmunition) >= M3reserveAmmunition) {
                //        M3clipAmmunition += M3reserveAmmunition;
                //        M3reserveAmmunition = 0;
                //        UpdateAmmoCount();
                //        currentGunState = GunState.Idle;
                //        Invoke("StopReloadAnimation", 0.7f);
                //    } else {
                //        M3reserveAmmunition -= M3clipSize - M3clipAmmunition;
                //        M3clipAmmunition = M3clipSize;
                //        UpdateAmmoCount();
                //        currentGunState = GunState.Idle;
                //        Invoke("StopReloadAnimation", 0.7f);
                //    }
                //}
            } else if (currentGunType == GunType.RocketLauncher) {
                //tickTimeReload += Time.deltaTime;
                //if (tickTimeReload >= RLreloadTime) {
                //    RLclipAmmunition++;
                //    RLreserveAmmuniton--;
                //    UpdateAmmoCount();
                //    currentGunState = GunState.Idle;
                //}
            } else if (currentGunType == GunType.Shotgun) {
                tickTimeReload += Time.deltaTime;
                
                if (tickTimeReload >= ShotgunReloadTime) {
                    if (ShotgunReserveAmmunition == 0) {
                        print("out of reserve ammo");
                        currentGunState = GunState.Idle;
                        pwac.StopWeaponAnimations(currentGunType);
                        StartCoroutine(pwac.ShotgunEndReload());
                        return;
                    } else if (ShotgunClipAmmunition < ShotgunClipSize) {
                        reloadShotgun.Play();
                        ShotgunClipAmmunition++;
                        ShotgunReserveAmmunition--;
                        UpdateAmmoCount();
                        tickTimeReload -= ShotgunReloadTime;
                    } else if (ShotgunClipAmmunition == ShotgunClipSize) {                        
                        currentGunState = GunState.Idle;
                        StartCoroutine(pwac.ShotgunEndReload());
                    }
                }
            }
        }

        // ads or "ironsights"

        leftTrigger = Input.GetAxisRaw("Fire2pad");

        if (currentGunType != GunType.RocketLauncher) {
            if (adsPressToToggle) {
                if (!leftTriggerWasPressed && leftTriggerWasReleased && leftTrigger > 0.1f) {
                    if (adsOn) {
                        ADSoff();
                    } else if (!adsOn) {
                        ADSon();
                        if (pm.currentState == PlayerMoveState.Sprinting) pm.EndSprint();
                    }
                    leftTriggerWasPressed = true;
                    leftTriggerWasReleased = false;
                } else if (leftTriggerWasPressed && !leftTriggerWasReleased && leftTrigger < 0.1f) {
                    leftTriggerWasReleased = true;
                    leftTriggerWasPressed = false;
                }
                if (Input.GetButtonDown("Fire2")) {                    
                    if (adsOn) {
                        ADSoff();
                    } else if (!adsOn) {
                        ADSon();
                        if (pm.currentState == PlayerMoveState.Sprinting) pm.EndSprint();
                    }

                }
            } else if (!adsPressToToggle) {
                if (Input.GetButton("Fire2") || leftTrigger > 0.1f) {                    
                    if (!adsOn) {
                        ADSon();
                        if (pm.currentState == PlayerMoveState.Sprinting) pm.EndSprint();
                    }
                } else {
                    if (adsOn) {
                        ADSoff();
                    }
                }
            }

            // fire 

            if (Input.GetButtonDown("Fire1")) {
                canFire = true;
                pm.EndSprint();
                if (currentGunState == GunState.Reloading) StopReloading();
                if (currentGunState == GunState.Idle) {
                    currentGunState = GunState.Firing;
                    if (currentGunType == GunType.M3) {
                        if (M3clipAmmunition > 0) {
                            M3recoil.StartRecoiling();
                        } else {
                            M3recoil.StopRecoiling();
                        }
                    } else if (currentGunType == GunType.Shotgun) {
                        
                        if (ShotgunClipAmmunition > 0) {
                            shotgunRecoil.StartRecoiling();
                        } else {
                            shotgunRecoil.StopRecoiling();
                        }
                    }
                }
                if (currentGunType == GunType.M3) FireM3();
                if (currentGunType == GunType.Shotgun && shotgunCanFire) FireShotgun();
            }

            if (!shotgunCanFire) {
                shotgunClickFireCD += Time.deltaTime;
                if (shotgunClickFireCD >= shotgunFireRate) {
                    shotgunClickFireCD = 0;
                    shotgunCanFire = true;
                }
            }
            rightTrigger = Input.GetAxisRaw("Fire1pad");
            if (rightTrigger < -0.1f && !padFireStarted) {
                padFireStarted = true;
                canFire = true;
            }

            if ((Input.GetButton("Fire1") || padFireStarted) && canFire) {
                if (currentGunState == GunState.Reloading) StopReloading();
                if (currentGunState == GunState.Changing) return;
                if (currentGunType == GunType.M3) {
                    if (currentGunState == GunState.Idle) {
                        currentGunState = GunState.Firing;
                    }
                    tickTimeM3 += Time.deltaTime;
                    if (tickTimeM3 >= M3fireRate) {
                        FireM3();
                        tickTimeM3 -= M3fireRate;
                    }
                    if (M3clipAmmunition == 0 && !recoilStopped) {
                        M3recoil.StopRecoiling();
                        recoilStopped = true;
                    }
                } else if (currentGunType == GunType.Shotgun) {
                    if (currentGunState == GunState.Idle) {
                        currentGunState = GunState.Firing;
                    }
                    tickTimeShotgun += Time.deltaTime;
                    if (tickTimeShotgun >=shotgunFireRate) {
                        FireShotgun();
                        tickTimeShotgun -= shotgunFireRate;
                    }
                    if (ShotgunClipAmmunition == 0 && !recoilStopped) {
                        shotgunRecoil.StopRecoiling();
                        recoilStopped = true;
                    }
                }
            }



            if ((Input.GetButtonUp("Fire1") || (padFireStarted && rightTrigger > -0.1f)) && currentGunState == GunState.Firing) {
                padFireStarted = false;
                currentGunState = GunState.Idle;
                if (currentGunType == GunType.M3) {
                    pwac.M3StopFiring();
                }
                tickTimeM3 = 0;
                tickTimeShotgun = 0;
                M3recoil.StopRecoiling();
                shotgunRecoil.StopRecoiling();
                recoilStopped = false;
                emptyClickPlayed = false;
                shotgunFireStarted = false;
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
