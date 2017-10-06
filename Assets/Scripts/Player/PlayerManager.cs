using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { Menu, Dead, Cutscene, Alive };

public class PlayerManager : MonoBehaviour {

    public PlayerState currentState;
    public Slider playerHealthSlider;
    public Image fill;
    public Image bg;

    public float actualHP;
    public float hudHP;
    float giveHUDHP;
    public float starthp = 100;
    public float maxhp = 100;

    public Color fillRed;
    public Color fillWhite;
    public Color flashColor;

    GameManager gm;
    public MouseAim ma;

    bool under26;

    public float regenCD = 1;
    float regenCDtickTime;
    public float regenRate = 1;
    public float regenAcc;

    public bool regeneratingHealth;

    public float hideHealthBarTime;
    public float hideHealthBarSpeed;
    float hideHealthBarTickTime;
    bool healthBarHidden;
    bool bringBackHealthBar;

    public Color hideColor;

    bool flashing;

    public float flashSpeed = 1;
    float flashAlpha = 1;
    float dir = 1;

    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentState = gm.startInMenu ? PlayerState.Menu : PlayerState.Alive;
        actualHP = starthp;
        UpdateHealthBar();
	}

    float GetHudHP() {
        if (actualHP == 100) {
            giveHUDHP = actualHP;
        } else if (actualHP == 0) {
            giveHUDHP = actualHP;
        } else {
            giveHUDHP = actualHP * (actualHP / 100);
        }
        return giveHUDHP;
    }
	
    void UpdateHealthBar() {
        if (currentState == PlayerState.Dead) return;
        if (actualHP <= 0) Die();
        hudHP = GetHudHP();
        if (hudHP <= 25 && !under26) {
            fill.color = flashColor;

            flashing = true;

            under26 = true;
        } else if (hudHP > 25 && under26) {
            flashing = false;
            fill.color = fillWhite;
            under26 = false;
        }
        playerHealthSlider.value = hudHP;
    }



    void Die() {
        if (currentState == PlayerState.Dead) return;
        print("u dieded");
        currentState = PlayerState.Dead;
        StartCoroutine(gm.PlayerDieded());
    }

    public void SetPosition(Vector3 pos, float rot) {
        transform.position = pos;
        transform.rotation = new Quaternion(0, rot, 0, 0);
        ma.ResetRotation();
    }

    public void Continue() {
        actualHP = 100;
        UpdateHealthBar();
        currentState = PlayerState.Alive;
    }

    public void SetAmmoAfterDeath() {
        // set ammos
    }

    public void PlayerTakesAHit(float damage) {
        //print("player takes a hit");
        regenCDtickTime = 0;
        hideHealthBarTickTime = 0;
        regenAcc = 0;
        actualHP -= damage;
        healthBarHidden = false;
        bringBackHealthBar = true;
        //hideColor.a = 1;
        //fill.color = hideColor;
        //bg.color = hideColor;
        UpdateHealthBar();
    }

	void Update () {

        if (gm.currentState != GameState.Run || currentState != PlayerState.Alive) return;

        if (actualHP < 100 && !regeneratingHealth) regeneratingHealth = true;

        if (regeneratingHealth) {
            if (actualHP >= 100) {
                actualHP = 100;
                regeneratingHealth = false;
                UpdateHealthBar();
            } else {
                regenCDtickTime += Time.deltaTime;
                if (regenCDtickTime >= regenCD) {
                    regenAcc += Time.deltaTime * 1.4f;
                    actualHP += (regenRate + regenAcc) * Time.deltaTime;
                    UpdateHealthBar();
                }
            }
        }

        if (flashing) {
            if (flashAlpha >= 0.99f) dir = -1;
            if (flashAlpha <= 0.6f) dir = 1;

            flashAlpha += flashSpeed * Time.deltaTime * dir;
            flashColor.a = flashAlpha;
            fill.color = flashColor;
        }

        if (actualHP == 100 && !healthBarHidden) {
            hideHealthBarTickTime += Time.deltaTime;
            if (hideHealthBarTickTime >= hideHealthBarTime) {
                hideColor.a -= hideHealthBarSpeed * Time.deltaTime;
                fill.color = hideColor;
                bg.color = hideColor;
                if (hideColor.a <= 0) {
                    healthBarHidden = true;
                }
            }
        }

        if (actualHP < 100 && bringBackHealthBar) {
            hideColor.a += hideHealthBarSpeed * Time.deltaTime * 10;
            fill.color = hideColor;
            bg.color = hideColor;
            if (hideColor.a >= 1) {
                bringBackHealthBar = false;
            }
        }

        if (!gm.debug) return;

		if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            PlayerTakesAHit(1);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            if (actualHP < maxhp) {
                actualHP++;
                UpdateHealthBar();
            }            
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            PlayerTakesAHit(25);
        }
	}
}
