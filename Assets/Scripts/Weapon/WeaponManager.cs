using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    public List<GunType> weapons = new List<GunType> { };

    int weaponIndex;

    GameManager gm;
    public PlayerShooter ps;
    public PlayerManager pm;

    public GameObject M3;
    public GameObject bazooka;
    public GameObject shotgun;

    public WeaponSwayAds M3Wsa;
    public WeaponSwayAds shotgunWsa;

    GunType previousGunType;

    public float weaponChangeCooldown = 0.2f;
    float tickTime;

    bool cd;

    public float weaponSwapTime;

    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        weapons.Add(GunType.M3);
        weapons.Add(GunType.Shotgun);
        weaponIndex = 0;
        //print(weapons.Count);
        if (weapons.Count > 0) {
            M3.SetActive(false);
            bazooka.SetActive(false);
            shotgun.SetActive(false);
            ps.currentGunType = weapons[weaponIndex];
            //SetWeapon();
            StartWithWeapon(weaponIndex);
        }
	}

    void StartWithWeapon(int i) {
        ps.currentGunType = weapons[i];
        if (ps.currentGunType == GunType.M3) {
            M3.SetActive(true);
        } else if (ps.currentGunType == GunType.RocketLauncher) {
            bazooka.SetActive(true);
        } else if (ps.currentGunType == GunType.Shotgun) {
            shotgun.SetActive(true);
        }
    }

        public void AddWeapon(GunType gunType) {

    }

    void NextWeapon() {
        if (weapons.Count < 2) return;
        previousGunType = ps.currentGunType;
        weaponIndex++;
        if (weaponIndex > weapons.Count - 1) weaponIndex = 0;
        ps.currentGunType = weapons[weaponIndex];
        SetWeapon();
    }    

    void PreviousWeapon() {
        if (weapons.Count < 2) return;
        previousGunType = ps.currentGunType;
        weaponIndex--;
        if (weaponIndex < 0) weaponIndex = weapons.Count - 1;
        ps.currentGunType = weapons[weaponIndex];
        SetWeapon();
    }

    void SetWeapon() {
        // old stuff
        //if (previousGunType == GunType.M3) {
        //    M3.SetActive(false);
        //} else if (previousGunType == GunType.RocketLauncher) {
        //    bazooka.SetActive(false);
        //} else if (previousGunType == GunType.Shotgun) {
        //    shotgun.SetActive(false);
        //}

        //if (ps.currentGunType == GunType.M3) {
        //    M3.SetActive(true);
        //} else if (ps.currentGunType == GunType.RocketLauncher) {
        //    bazooka.SetActive(true);
        //} else if (ps.currentGunType == GunType.Shotgun) {
        //    shotgun.SetActive(true);
        //}

        if (previousGunType == GunType.M3) {
            StartCoroutine(SwitchFromM3ToShotgun());
        } else if (previousGunType == GunType.Shotgun) {
            StartCoroutine(SwitchFromShotgunToM3());
        }
        ps.UpdateAmmoCount();
    }

    IEnumerator SwitchFromM3ToShotgun() {
        M3Wsa.SwapWeaponOut(true);
        yield return new WaitForSeconds(weaponSwapTime);
        shotgun.SetActive(true);
        shotgunWsa.SwapWeaponOut(false);
        M3.SetActive(false);
    }

    IEnumerator SwitchFromShotgunToM3() {
        shotgunWsa.SwapWeaponOut(true);
        yield return new WaitForSeconds(weaponSwapTime);
        M3.SetActive(true);
        M3Wsa.SwapWeaponOut(false);
        shotgun.SetActive(false);
    }

    void ChangeToSpecificWeapon(GunType gunType) {
        previousGunType = ps.currentGunType;
        ps.currentGunType = gunType;
        SetWeapon();
        weaponIndex = weapons.IndexOf(gunType);
    }

    void Update () {

        if (gm.currentState != GameState.Run || pm.currentState != PlayerState.Alive) return;

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        if (cd) {
            tickTime += Time.deltaTime;
            if (tickTime > weaponChangeCooldown) {
                tickTime = 0;
                cd = false;
                ps.currentGunState = GunState.Idle;
            }
            return;
        }

        if (mouseWheel > 0) {
            ps.WeaponChange();
            NextWeapon();
            //Invoke("NextWeapon", 0.5f);
            cd = true;
            ps.currentGunState = GunState.Changing;
        }
        if (mouseWheel < 0) {
            ps.WeaponChange();
            PreviousWeapon();
            //Invoke("PreviousWeapon", 0.5f);
            cd = true;
            ps.currentGunState = GunState.Changing;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9) && weapons.Contains(GunType.RocketLauncher) && ps.currentGunType != GunType.RocketLauncher) {
            ps.WeaponChange();
            ChangeToSpecificWeapon(GunType.RocketLauncher);
            //Invoke("ChangeToSpecificWeapon(GunType.RocketLauncher)", 0.5f);
            cd = true;
            ps.currentGunState = GunState.Changing;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Contains(GunType.M3) && ps.currentGunType != GunType.M3) {
            ps.WeaponChange();
            ChangeToSpecificWeapon(GunType.M3);
            cd = true;
            ps.currentGunState = GunState.Changing;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Contains(GunType.Shotgun) && ps.currentGunType != GunType.Shotgun) {
            ps.WeaponChange();
            ChangeToSpecificWeapon(GunType.Shotgun);
            cd = true;
            ps.currentGunState = GunState.Changing;
        }
    }
}
