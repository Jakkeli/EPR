using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public float speed;
    public ParticleSystem flame;
    public GameObject explosion;
    public bool fired;
    bool hasHit;
    public Vector3 direction;

    GameObject cam;
    public GameObject explosionPrefab;

    private void Awake() {
        cam = GameObject.Find("Main Camera");
        direction = cam.transform.forward;
    }

    public void Fire(Vector3 dir) {
        fired = true;
    }

	void Update () {
        if (fired) transform.position += direction * Time.deltaTime * speed;
    }

    void OnHit() {
        hasHit = true;
        Instantiate(explosionPrefab);
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 1);
        //print("rockethit a collider");
    }

    private void OnTriggerEnter(Collider c) {
        if (!hasHit) OnHit();

    }
}
