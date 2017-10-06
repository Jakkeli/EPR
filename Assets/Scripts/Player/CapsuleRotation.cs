using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleRotation : MonoBehaviour {

    public GameObject player;

	void Update () {
        transform.rotation = new Quaternion(0, player.transform.rotation.y, 0, transform.rotation.w);
    }
}
