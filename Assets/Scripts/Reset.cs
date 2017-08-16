using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

    public void ResetGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
