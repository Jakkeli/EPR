using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothV;
    public float mouseSensitivity = 5;
    public float padSensitivity = 1.5f;
    public float smoothing = 2;
    public GameObject player;

    public float vertical;
    public float horizontal;

    public float padVertical;
    public float padHorizontal;

    public bool invertedY;
    public bool invertedX;

    float invertYModifier = 1;
    float invertXModifier = 1;

    GameManager gm;
    bool recoiling;
    public float maxRecoil = 1;

    public Vector3 crouchOffSet;

    public PlayerManager pManager;

    Quaternion zeroRotation;

    public float startRotationOffset;
    public float padADSmodifier = 0.7f;
    float adsModifier = 1;

    private void Awake() {
        zeroRotation = transform.localRotation;
    }

    private void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();        
    }

    public bool InvertY() {
        invertedY = invertedY ? false : true;
        invertYModifier = invertedY ? -1 : 1;
        return invertedY;
    }

    //void OnGUI() { GUI.DrawTexture(Rect(Input.mousePosition.x - cursorSizeX / 2, (Screen.height - Input.mou.y) - cursorSizeY / 2, cursorSizeX, cursorSizeY), myCursor); }

    public void RecoilM3() {
        recoiling = true;
    }

    public void ResetRotation() {
        horizontal = 0;
        vertical = 0;
        mouseLook = Vector2.zero;
        transform.localRotation = zeroRotation;
    }

    public void ADS(bool adsOn) {
        if (adsOn) {
            adsModifier = padADSmodifier;
        } else {
            adsModifier = 1;
        }
    }

    void Update () {
        if (gm.currentState != GameState.Run || pManager.currentState != PlayerState.Alive) return;
        transform.localRotation = transform.localRotation;
        horizontal = Input.GetAxisRaw("Mouse X");
        vertical = Input.GetAxisRaw("Mouse Y");

        padHorizontal = Input.GetAxisRaw("PadAim X");
        padVertical = Input.GetAxisRaw("PadAim Y");

        horizontal += padHorizontal * padSensitivity / 10 * adsModifier;
        vertical += padVertical * padSensitivity / 10 * adsModifier;

        var md = new Vector2(horizontal, vertical);
        md = Vector2.Scale(md, new Vector2(mouseSensitivity * smoothing, mouseSensitivity * smoothing));
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

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y * invertYModifier, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis((mouseLook.x * invertXModifier) + startRotationOffset, player.transform.up);
    }
}
