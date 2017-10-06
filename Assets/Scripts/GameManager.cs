using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { Run, Menu, Pause };
public enum GameLevel {  First, Second };

public class GameManager : MonoBehaviour {

    public GameState currentState;
    public GameLevel currentLevel;
    public bool startInMenu;
    public bool debug;

    public GameObject menuBG;
    public GameObject uiMenu;
    public GameObject uiSettings;
    public GameObject uiCredits;
    public Text playContinueText;
    public GameObject hud;

    bool firstTimeStart = true;

    public Text reserveCount;
    public Text clipCount;
    AudioSource audioSource;

    public GameObject[] checkPoints1;
    public GameObject currentCP;

    Fader fader;
    public TextFader deathText;
    public TextFader goalText;
    public TextFader missionText;
    PlayerManager pm;
    float cpRot;

    EnemyResetManager erm;
    DoorResetManager drm;
    ShellResetManager srm;
    DecalResetManager derm;

    public PlayerShooter ps;

    bool inMainMenu;

    public bool startWithEditorLightAndNoRoofs;
    public GameObject roofs;
    public GameObject editorLight;
    public SettingsChange sc;

	void Start () {
        Time.timeScale = 0;
        drm = GetComponent<DoorResetManager>();
        erm = GetComponent<EnemyResetManager>();
        srm = GetComponent<ShellResetManager>();
        derm = GetComponent<DecalResetManager>();
        currentCP = checkPoints1[0];
        pm = GameObject.Find("Player").GetComponent<PlayerManager>();
        fader = GetComponent<Fader>();
        audioSource = GetComponent<AudioSource>();
        if (startWithEditorLightAndNoRoofs) {
            roofs.SetActive(false);
            editorLight.SetActive(true);
        } else {
            roofs.SetActive(true);
            editorLight.SetActive(false);
        }
        if (startInMenu) {
            Cursor.lockState = CursorLockMode.None;
            hud.SetActive(false);
            menuBG.SetActive(true);
            uiMenu.SetActive(true);
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            menuBG.SetActive(false);
            uiMenu.SetActive(false);
            hud.SetActive(true);
        }
    }

    public void NewCheckPointReached(int number, float rot) {
        if (currentCP == checkPoints1[number]) {
            print("WTF");
        } else {
            currentCP = checkPoints1[number];
            cpRot = rot;
            erm.CheckPointChange();
            drm.CheckPointChange();
            srm.CheckPointChange();
            derm.CheckPointChange();
        }        
    }

    void DeathFadeText() {
        deathText.Fade(false, 1.5f);
    }

    void GoalFadeText() {
        goalText.Fade(false, 1.5f);
    }

    public IEnumerator Goal() {
        fader.Fade(false, 5);
        yield return new WaitForSeconds(4);
        GoalFadeText();
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public IEnumerator PlayerDieded() {
        fader.Fade(false, 3);
        yield return new WaitForSeconds(2);
        DeathFadeText();
        yield return new WaitForSeconds(1);
        pm.SetPosition(currentCP.transform.position, cpRot);
        erm.ResurrectEnemies();
        ps.ResetAmmosAfterDeath();
        srm.ResetShells();
        drm.ResetDoors();
        derm.ResetDecals();
        yield return new WaitForSeconds(2);
        deathText.Fade(true, 1);
        //fader.Fade(true, 3); // hieno reset pois, reloadScene tilalle
        yield return new WaitForSeconds(1.5f);
        //pm.Continue();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void UpdateHud(float clipAmmo, float reserveAmmo) {
        clipCount.text = "" + clipAmmo;
        reserveCount.text = "       " + reserveAmmo;
    }

    public void StartVideoMusic() {
        audioSource.Play();
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
            sc.PlayPauseMusic(false);
            Time.timeScale = 0;
        } else if (currentState == GameState.Pause) {
            currentState = GameState.Run;
            Time.timeScale = 1;
            sc.PlayPauseMusic(true);
        }
    }

    public void GoToMenu() {
        sc.PlayPauseMusic(false);
        pm.currentState = PlayerState.Menu; 
        Time.timeScale = 0;
        currentState = GameState.Menu;
        hud.SetActive(false);
        menuBG.SetActive(true);
        uiMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        inMainMenu = true;
    }

    public void PlayButton() {
        //print("pressed?");
        currentState = GameState.Run;
        StartCoroutine(FreePlayer());
        menuBG.SetActive(false);
        uiMenu.SetActive(false);
        Time.timeScale = 1;
        sc.PlayPauseMusic(true);
        hud.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        if (firstTimeStart) {
            StartCoroutine(ShowMissionText());
            playContinueText.text = "CONTINUE";
            firstTimeStart = false;
        }
    }

    IEnumerator ShowMissionText() {
        missionText.Fade(false, 0.5f);
        yield return new WaitForSeconds(5);
        missionText.Fade(true, 0.5f);
    }

    IEnumerator FreePlayer() {
        yield return new WaitForSeconds(0.2f);
        pm.currentState = PlayerState.Alive;
    }

    public void OutOfMainMenu() {
        inMainMenu = false;
    }

    void EscButton() {
        if (inMainMenu) {
            PlayButton();
        } else {
            print("tänne?");
            uiSettings.SetActive(false);
            uiCredits.SetActive(false);
            uiMenu.SetActive(true);
            inMainMenu = true;
        }
    }

	void Update () {

        if (Input.GetButtonDown("Cancel")) {
            if (currentState != GameState.Menu) {
                GoToMenu();
            } else if (currentState == GameState.Menu) {
                EscButton();
            }
        }

        if (currentState != GameState.Menu && Input.GetButtonDown("Pause")) Pause();

        if (!debug) return;
        if (Input.GetKeyDown(KeyCode.O)) {
            LockCursor();
        }
	}
}
