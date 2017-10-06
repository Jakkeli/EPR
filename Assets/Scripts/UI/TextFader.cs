using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour {

    public Text theText;
    public float fadeTime;
    public bool inTransition;
    public bool isShowing;
    float fadeTarget;
    Color startColor;
    Color targetColor;

    public void Fade(bool fadeIn, float duration) { // fadeIn true = from black, false = to black
        //startColor = theText.color;
        //targetColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        inTransition = true;
        fadeTime = duration;
        fadeTarget = fadeIn ? 0 : 1;
        isShowing = fadeIn;
    }

    void Update() {
        if (!inTransition) return;

        fadeTarget += isShowing ? Time.deltaTime * 1 / fadeTime : -Time.deltaTime * 1 / fadeTime;
        //theText.color = Color.Lerp(startColor, targetColor, fadeTarget);
        theText.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), fadeTarget);

        if (fadeTarget < 0 || fadeTarget > 1) inTransition = false;
    }
}
