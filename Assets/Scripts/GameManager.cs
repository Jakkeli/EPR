using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { Run, Menu, Pause };

public class GameManager : MonoBehaviour {

    public GameState currentState;
    public bool startInMenu;
    public bool debug;

    public GameObject menuBG;
    public GameObject uiMenu;
    public Text playContinueText;


    bool firstTimeStart = true;

	void Start () {
        if (startInMenu) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void LockCursor() {
        if (Cursor.lockState == CursorLockMode.None) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }
	
    public void Pause() {
        if (currentState == GameState.Run) {
            currentState = GameState.Pause;
            Time.timeScale = 0;
        } else if (currentState == GameState.Pause) {
            currentState = GameState.Run;
            Time.timeScale = 1;
        }
    }

    public void GoToMenu() {
        Time.timeScale = 0;
        currentState = GameState.Menu;
        menuBG.SetActive(true);
        uiMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayButton() {
        Cursor.lockState = CursorLockMode.Locked;
        currentState = GameState.Run;
        menuBG.SetActive(false);
        uiMenu.SetActive(false);
        Time.timeScale = 1;
        if (firstTimeStart) {
            playContinueText.text = "CONTINUE";
            firstTimeStart = false;
        }
    }

	void Update () {
		if (currentState != GameState.Menu && Input.GetButtonDown("Cancel")) GoToMenu();

        if (currentState != GameState.Menu && Input.GetButtonDown("Pause")) Pause();

        if (!debug) return;
        if (Input.GetKeyDown(KeyCode.O)) {
            LockCursor();
        }
	}
}
