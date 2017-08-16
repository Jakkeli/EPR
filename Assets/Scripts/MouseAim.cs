using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5;
    public float smoothing = 2;
    public GameObject player;

    float vertical;
    float horizontal;

    public bool invertedY;
    public bool invertedX;

    GameManager gm;
    bool recoiling;
    public float maxRecoil = 1;

    private void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void RecoilM3() {
        print("called");
        recoiling = true;
    }

    void Update () {
        if (gm.currentState != GameState.Run) return;
        transform.localRotation = transform.localRotation;
        horizontal = Input.GetAxisRaw("Mouse X");
        vertical = Input.GetAxisRaw("Mouse Y");
        var md = new Vector2(horizontal, vertical);
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1 / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1 / smoothing);
        mouseLook += smoothV;
        if (recoiling) {
            float recoilX = Random.Range(-(maxRecoil / 2), maxRecoil);
            float recoilY = Random.Range(-(maxRecoil / 2), maxRecoil);
            mouseLook.y += recoilY;
            mouseLook.x += recoilX;
            recoiling = false;
        }
        mouseLook.y = Mathf.Clamp(mouseLook.y, -85, 85);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
    }
}
