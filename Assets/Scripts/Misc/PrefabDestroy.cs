using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDestroy : MonoBehaviour {

    public float lifeTimer = 5;

    private void Start() {
        Destroy(gameObject, lifeTimer);
    }
}
