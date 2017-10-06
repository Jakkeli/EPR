using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEjector : MonoBehaviour {

    public ShellManager sm;

    //public GameObject M3ejectPos;

    GameObject shell;

    Quaternion shellRot;

    public GameObject cam;

    public float ejectSpeed;

    public Vector3 position;

    public GameObject player;

    public bool isM3;

    public void EjectShell() {
        Vector3 dir = (transform.right + (transform.up / Random.Range(1.9f, 2.1f)) + (transform.forward / Random.Range(-11.0f, -9.0f)) * ejectSpeed * Random.Range(0.9f, 1.1f));
        //print("right: " + transform.right + (transform.up / Random.Range(1.9f, 2.1f) + "up: " + (transform.forward / Random.Range(-11.0f, -9.0f)) * ejectSpeed * Random.Range(0.9f, 1.1f) + "dir: " + dir));
        shellRot = cam.transform.rotation;
        shell = isM3 ? sm.GetM3Shell() : sm.GetShotgunShell();
        position = transform.position;
        shell.GetComponent<Shell>().Eject(dir, position, shellRot);
    }
}
