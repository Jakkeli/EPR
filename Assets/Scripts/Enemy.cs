using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int hp = 1;
    Rigidbody rb;
    public float knockBackForce;
    Vector3 knockBackDir;
    Vector3 hitDir;
	
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	public void TakeAHit(Vector3 hitFrom) {
        hp--;
        hitDir = hitFrom;
        if (hp <= 0) Death();
    }

    void Death() {
        rb.freezeRotation = false;
        rb.useGravity = true;
        knockBackDir = (transform.position - hitDir).normalized * knockBackForce;
        rb.AddForce(knockBackDir, ForceMode.Impulse);
        //Destroy(gameObject, 5);
    }

	void Update () {
		
	}
}
