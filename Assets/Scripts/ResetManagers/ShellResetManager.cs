using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellResetManager : MonoBehaviour {

    List<GameObject> shellsEjectedAfterLatestCP = new List<GameObject>() { };

    public void CheckPointChange() {
        shellsEjectedAfterLatestCP.Clear();
    }

    public void ShellEjected(GameObject shell) {
        shellsEjectedAfterLatestCP.Add(shell);
    }

    public void ResetShells() {
        foreach (GameObject shell in shellsEjectedAfterLatestCP) {
            if (shell.GetComponent<Shell>() != null) {
                shell.GetComponent<Shell>().ResetShell();
            } else if (shell.GetComponent<Shell>() == null) {

            }
        }
    }
}
