using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAnimationController : MonoBehaviour {

    public Animator M3Animator;
    public Animator shotgunAnimator;

    public AnimationClip M3Reload;
    public AnimationClip M3Idle;
    public AnimationClip M3Fire;
    bool M3FireIsPlaying;

    public AnimationClip shotgunFire;
    public AnimationClip shotgunStartReload;
    public AnimationClip shotgunShellIn;
    public AnimationClip shotgunEndReload;
    public AnimationClip shotgunIdle;

    PlayerShooter ps;

    public float M3reloadAnimationPadding = 0.05f;

    void Start () {
        ps = GetComponent<PlayerShooter>();
	}
	
	public void StopWeaponAnimations(GunType currentGun) {
        StopAllCoroutines();
        if (currentGun == GunType.M3) {
            M3Animator.SetBool("fire", false);
            M3Animator.SetBool("lastFired", false);
            M3Animator.SetBool("reload", false);
        } else if (currentGun == GunType.Shotgun) {
            shotgunAnimator.SetBool("startReload", false);
            shotgunAnimator.SetBool("shellIn", false);
            shotgunAnimator.SetBool("endReload", false);
            shotgunAnimator.SetBool("idle", true);
        }
    }

    public void M3StartFiring() {
        if (!M3FireIsPlaying) {
            StopWeaponAnimations(ps.currentGunType);
            M3Animator.SetBool("fire", true);
            M3FireIsPlaying = true;
        }
    }

    public void M3StopFiring() {
        M3Animator.SetBool("fire", false);
        M3FireIsPlaying = false;
    }

    public void M3LastFired() {
        M3Animator.SetBool("lastFired", true);
    }

    public IEnumerator M3StartReload(float M3reloadTime) {
        M3Animator.SetBool("fire", false);
        M3Animator.SetBool("lastFired", false);
        M3Animator.SetBool("reload", true);
        yield return new WaitForSeconds(M3reloadTime + M3reloadAnimationPadding);
        M3Animator.SetBool("reload", false);
    }

    public IEnumerator ShotgunStartReload() {
        shotgunAnimator.SetBool("idle", false);
        shotgunAnimator.SetBool("startReload", true);
        yield return new WaitForSeconds(shotgunStartReload.length);
        shotgunAnimator.SetBool("startReload", false);
        shotgunAnimator.SetBool("shellIn", true);
    }

    public IEnumerator ShotgunEndReload() {
        shotgunAnimator.SetBool("shellIn", false);
        shotgunAnimator.SetBool("endReload", true);
        yield return new WaitForSeconds(shotgunEndReload.length);
        shotgunAnimator.SetBool("endReload", false);
        shotgunAnimator.SetBool("idle", true);
    }

    public IEnumerator ShotgunFire() {
        //print("shotgun fire");
        shotgunAnimator.SetBool("idle", false);
        shotgunAnimator.SetBool("fire", true);
        yield return new WaitForSeconds(shotgunFire.length);
        if (ps.currentGunState == GunState.Firing) {
            StopCoroutine(ShotgunFire());
            StartCoroutine(ShotgunFire());
        } else {
            shotgunAnimator.SetBool("fire", false);
            shotgunAnimator.SetBool("idle", true);
        }
        
    }
}
