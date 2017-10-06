using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour {

    public int maxShotgunShells;
    public int maxM3Shells;

    public GameObject shotgunShell;
    public GameObject M3Shell;

    public List<GameObject> shotgunShells;
    public List<GameObject> M3Shells;

    public int M3index;
    int shotgunIndex;

    void Start () {
        CreateM3Shells();
        CreateShotgunShells();
	}

    public GameObject GetM3Shell() {
        GameObject shell;
        shell = M3Shells[M3index];
        M3index++;
        if (M3index >= M3Shells.Count) M3index = 0;
        return shell;
    }

    public GameObject GetShotgunShell() {
        GameObject shell;
        shell = shotgunShells[shotgunIndex];
        shotgunIndex++;
        if (shotgunIndex >= shotgunShells.Count) shotgunIndex = 0;
        return shell;
    }

    void CreateM3Shells() {
        for (int i = 0; i < maxM3Shells; i++) {
            GameObject M3shellClone = Instantiate(M3Shell, new Vector3(500 + i, -500, 500), Quaternion.identity);
            M3Shells.Add(M3shellClone);
        }
    }

    void CreateShotgunShells() {
        for (int i = 0; i < maxShotgunShells; i++) {
            GameObject shotgunShellClone = Instantiate(shotgunShell, new Vector3(505 + i, -505, 500), Quaternion.identity);
            shotgunShells.Add(shotgunShellClone);
        }
    }
}
