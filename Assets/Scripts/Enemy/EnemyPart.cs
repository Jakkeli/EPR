using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour {

	public float multiplier;
	public float force = 100f;

	Rigidbody rb;
	Transform player;
	EnemyHealth health;

    Vector3 originalPosition;
    Quaternion originalRotation;
    Transform originalParent;

	void Start () {
        rb = GetComponent<Rigidbody>();
		player = FindObjectOfType<PlayerMovement>().transform;
		health = GetEnemyHealthFromParents();
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        originalParent = transform.parent;
	}

	public void Hit (int damage) {
		health.TakeDamage(damage * multiplier);
	}

    public void ResetPart() {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        transform.parent = originalParent;
    }

	public void Separate () {
        transform.parent = transform.root;
        rb.isKinematic = false;
        rb.useGravity = true;
        var dir = (transform.position - player.position).normalized;
        rb.AddForce(dir * force, ForceMode.Impulse);
    }

	EnemyHealth GetEnemyHealthFromParents () {
		Transform t = transform;
		EnemyHealth enemyHealth = null;
		while (enemyHealth == null && t != transform.root) {
			t = t.parent;
			enemyHealth = t.GetComponent<EnemyHealth>();
		}
		return enemyHealth;
	}
}
