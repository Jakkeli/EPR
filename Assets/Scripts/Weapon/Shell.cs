using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

    Rigidbody rb;

    public float rotSpeed = 1;

    Vector3 originalPosition;
    Quaternion originalRotation;
    ShellResetManager srm;

    public float rotate = 90;

    public float addMoarForce;

    void Start() {
        srm = GameObject.Find("GameManager").GetComponent<ShellResetManager>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void ResetShell() {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    public void Eject(Vector3 dir, Vector3 pos, Quaternion rot) {
        srm.ShellEjected(gameObject);
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.rotation = rot;
        transform.Rotate(transform.up, rotate);
        transform.position = pos;
        rb.useGravity = true;
        rb.AddForce(dir * addMoarForce, ForceMode.Impulse);
    }
}
