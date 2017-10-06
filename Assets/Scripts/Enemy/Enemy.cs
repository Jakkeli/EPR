using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Idle, Patrol, Fight, Dead, InPool };

public class Enemy : MonoBehaviour {

    public EnemyState currentState;

    public int hp = 1;
    Rigidbody rb;
    public float knockBackForce;
    public float videoKnockbackForce;
    Vector3 knockBackDir;
    Vector3 hitDir;
    //EnemyManager em;
    EnemyResetManager erm;

    public GameObject markerPrefab;

    public bool alphaVideoKnockback;

    Vector3 originalPos;
    Quaternion originalRot;
    EnemyState originalState;
    int originalHP;

    void Start () {
        SetOriginalState();
        erm = GameObject.Find("GameManager").GetComponent<EnemyResetManager>();
        rb = GetComponent<Rigidbody>();
        //em = GetComponent<EnemyManager>();
        //GetMarker();
        if (alphaVideoKnockback) knockBackForce = videoKnockbackForce;
	}

    void SetOriginalState() {
        originalPos = transform.position;
        originalRot = transform.rotation;
        originalState = currentState;
        originalHP = hp;
    }

    public void Resurrect() {
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
        rb.useGravity = false;
        transform.position = originalPos;
        transform.rotation = originalRot;
        hp = originalHP;
        currentState = originalState;
    }
	
	public void TakeAHit(Vector3 hitFrom) {
        hp--;
        hitDir = hitFrom;
        if (hp <= 0) Death();
    }

    void GetMarker() {
        Instantiate(markerPrefab);
        //markerPrefab.GetComponent<EnemyMarker>().GetEnemy(gameObject);
    }

    void Death() {
        currentState = EnemyState.Dead;
        erm.EnemyDied(gameObject);
        rb.freezeRotation = false;
        rb.useGravity = true;
        knockBackDir = (transform.position - hitDir).normalized * knockBackForce;
        rb.AddForce(knockBackDir, ForceMode.Impulse);
    }

	void Update () {
		
	}
}
