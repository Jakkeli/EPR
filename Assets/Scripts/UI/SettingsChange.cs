using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsChange : MonoBehaviour {

    public MouseAim mouseAim;
    public PlayerMovement pm;
    public PlayerShooter ps;

    public Slider mouseSensitivitySlider;
    public Text mouseSensText;
    float mouseSensitivityRounded;

    public Text invertYtext;
    string invertYyesNo;

    public Text toggleToCrouchText;
    string toggleToCrouchyesNo;

    public Text toggleToSprintText;
    string toggleToSprintYyesNo;

    public Text toggleToADSText;
    string toggleToADSYesNo;

    public Text musicOnOffText;
    string musicOnOff;

    public AudioSource music;
    public bool musicOn = false;

    void Start () {
        invertYtext.text = "No";
        toggleToCrouchText.text = "No";
        toggleToSprintText.text = "No";
        toggleToADSText.text = "No";
        musicOnOffText.text = "Off";
        music.Play();
        music.Pause();
    }
	
	public void MouseSensitivityChange() {
        mouseAim.mouseSensitivity = mouseSensitivitySlider.value;
        mouseSensitivityRounded = mouseSensitivitySlider.value;
        mouseSensText.text = "Mouse Sensitivity " + System.Math.Round(mouseSensitivityRounded, 2);
    }

    public void PlayPauseMusic(bool play) {
        if (musicOn) {
            if (play) {
                music.UnPause();
            } else {
                music.Pause();
            }
        } 
    }

    public void MusicOnOff() {
        musicOn = musicOn ? false : true;
        musicOnOffText.text = musicOn ? "On" : "Off";        
    }

    public void InvertY() {
        invertYyesNo = mouseAim.InvertY() ? "Yes" : "No";
        invertYtext.text = "" + invertYyesNo;
    }

    public void ToggleToCrouch() {
        toggleToCrouchyesNo = pm.ToggleToCrouch() ? "Yes" : "No";
        toggleToCrouchText.text = "" + toggleToCrouchyesNo;
    }

    public void ToggleToSprint() {
        toggleToSprintYyesNo = pm.ToggleToSprint() ? "Yes" : "No";
        toggleToSprintText.text = "" + toggleToSprintYyesNo;
    }

    public void ToggleToADS() {
        toggleToADSYesNo = ps.ToggleToADS() ? "Yes" : "No";
        toggleToADSText.text = "" + toggleToADSYesNo;
    }
}
