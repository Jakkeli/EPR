using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairFader : MonoBehaviour {

    public Image[] fadeImages;
    public float fadeTime;
    public bool inTransition;
    public bool isShowing;
    float fadeTarget;
    GameManager gm;

    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void CrosshairFadeOut() {
        Fade(true, 1);
    }

    public void CrosshairFadeIn() {
        Fade(false, 1);
    }

    void Fade(bool fadeIn, float duration) {
        inTransition = true;
        //fadeTime = duration;
        fadeTarget = fadeIn ? 0 : 1;
        isShowing = fadeIn;
    }
	
	void Update () {
        if (gm.debug) {
            if (Input.GetKeyDown(KeyCode.B)) {// to not showing
                Fade(true, 1);
            }
            if (Input.GetKeyDown(KeyCode.V)) {// to showing
                Fade(false, 1);
            }
        }


        if (!inTransition) return;

        fadeTarget += isShowing ? Time.deltaTime * 1 / fadeTime : -Time.deltaTime * 1 / fadeTime;
        foreach (Image i in fadeImages) {
            i.color = Color.Lerp(Color.green, new Color(0, 1, 0, 0), fadeTarget);
        }

        if (fadeTarget < 0 || fadeTarget > 1) inTransition = false;
    }
}
