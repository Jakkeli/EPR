using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour {

    public Transform startPoint;
    public Transform theOtherPoint;

    public float maxRange;
    public float accuracy;
    public float fireRate;
    public LayerMask hitByPlayer;
    Ray ray;
    public PlayerMovement pm;
    Vector3 shootDir;
    public GameObject c;
    public GameObject thingie;

	void Start () {
		
	}
	
    void FireAGun() {
        print("fire");
        shootDir = c.transform.forward;
        RaycastHit hit;
        ray = new Ray(startPoint.position, shootDir);
        if (Physics.Raycast(ray, out hit, maxRange, hitByPlayer)) {
            print("hit something");
            Quaternion rot = hit.transform.rotation;
            Instantiate(thingie, hit.point, rot);
        }
    }

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
            FireAGun();
        }
	}
}
