using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public float speed;
    public ParticleSystem flame;
    public GameObject explosion;
    public bool fired;
    public Vector3 direction;

    GameObject cam;

    private void Awake() {
        cam = GameObject.Find("Main Camera");
        direction = cam.transform.forward;
    }

    public void Fire(Vector3 dir) {
        //direction = dir.normalized;
        //direction = cam.transform.forward;
        //fired = true;
        //print(direction);
    }

	void Update () {
        if (fired) transform.position += direction * Time.deltaTime * speed;
        //if (fired) transform.Translate(transform.forward * speed * Time.deltaTime);

    }
}
