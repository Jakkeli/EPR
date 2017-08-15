using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Run, Menu, Pause };

public class GameManager : MonoBehaviour {

    public GameState currentState;
    public bool startInMenu;
    public bool debug;

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

	void Update () {
		if (currentState != GameState.Menu && Input.GetButtonDown("Cancel")) {
            Pause();
        }

        if (!debug) return;
        if (Input.GetKeyDown(KeyCode.O)) {
            LockCursor();
        }
	}
}
