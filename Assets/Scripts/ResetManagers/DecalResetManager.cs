using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalResetManager : MonoBehaviour {

    List<GameObject> decalPlacedAfterLatestCP = new List<GameObject>() { };

    public void CheckPointChange() {
        decalPlacedAfterLatestCP.Clear();
    }

    public void DecalPlaced(GameObject decal) {
        decalPlacedAfterLatestCP.Add(decal);
    }

    public void ResetDecals() {
        foreach (GameObject decal in decalPlacedAfterLatestCP) {
            if (decal.GetComponent<BulletDecal>() != null) {
                decal.GetComponent<BulletDecal>().ResetDecal();
            } else if (decal.GetComponent<BulletDecal>() == null) {

            }
        }
    }
}
