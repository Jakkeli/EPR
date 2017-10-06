using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float health;

	public bool dead { get; private set; }

	List<EnemyPart> parts;

    float originalHealth;

    //EnemyResetManager erm;

	void Start () {
		parts = new List<EnemyPart>(GetComponentsInChildren<EnemyPart>());
        originalHealth = health;
        //erm = GameObject.Find("GameManager").GetComponent<EnemyResetManager>();
	}

	public void TakeDamage (float damage) {
        if (dead) return;
		health -= damage;
		if (health <= 0) {            
			dead = true;
		}
	}

	public void SeparateAllParts () {
		StartCoroutine(SeparateAllPartsCoroutine());
	}

    public void ResetAllParts() {
        health = originalHealth;
        if (!dead) return;
        foreach (EnemyPart p in parts) {
            p.ResetPart();
        }
        dead = false;
    }

	IEnumerator SeparateAllPartsCoroutine () {
		foreach (EnemyPart p in parts) {
			yield return new WaitForEndOfFrame();
			p.Separate();
		}
	}
}
