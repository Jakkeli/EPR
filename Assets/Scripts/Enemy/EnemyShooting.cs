using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

	public float rpm;
	public float reloadTime;
	public int maxAmmo;
	public float damage;

	public ParticleSystem smoke, flare;
	public AudioSource shotgunFireSound, shotgunReloadSound, shotgunPumpSound;

	public int currentAmmo { get; private set; }
	public bool reloading { get; private set; }

	float lastTimeShot;

	Transform player;
	PlayerManager playerManager;
	//EnemySenses senses;
	Animator anim;

    

	void Start () {
		player = FindObjectOfType<PlayerMovement>().transform;
		playerManager = player.GetComponent<PlayerManager>();
		//senses = GetComponent<EnemySenses>();
		anim = GetComponentInChildren<Animator>();

		currentAmmo = maxAmmo;
	}

    //public void ResetShooting() {
    //    StopAllCoroutines();
    //    reloading = false;
    //    currentAmmo = maxAmmo;
    //    lastTimeShot = 0;
    //}

	// DEBUG
	void Update () {
		//if (Input.GetKeyDown(KeyCode.O)) {
		//	Shoot();
		//}
	}

	public void Shoot () {
		if (lastTimeShot + 60f / rpm < Time.time) {
			smoke.Play();
			flare.Play();
            shotgunFireSound.Play();
			if (shotgunPumpSound != null) {
				shotgunPumpSound.PlayDelayed(60f / rpm / maxAmmo);
			}
			anim.SetTrigger("Fire");
			playerManager.PlayerTakesAHit(damage);

			currentAmmo--;
			lastTimeShot = Time.time;
		}
	}

	public void Reload () {
		if (!reloading) {
			StartCoroutine(ReloadCoroutine());
			StartCoroutine(ReloadSound());
		}
	}

	IEnumerator ReloadSound () {
		for (int i = 0; i < maxAmmo; i++) {
			yield return new WaitForSeconds(reloadTime / maxAmmo);
			shotgunReloadSound.Play();
		}
	}

	IEnumerator ReloadCoroutine () {
		reloading = true;
		yield return new WaitForSeconds(reloadTime);
		currentAmmo = maxAmmo;
		reloading = false;
	}
}
