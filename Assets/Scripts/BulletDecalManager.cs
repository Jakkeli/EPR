using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDecalManager : MonoBehaviour {

    public DecalResetManager drm;

    public GameObject arrow;
    public GameObject bulletDecalPrefab;

    //GameObject decal;

    public float offset = 0.01f;

    public List<GameObject> decals;

    public float maxDecals = 50;
    public int decalIndex;

	void Start () {
        decalIndex = 0;
        CreateDecals();
	}
	
	public void PlaceBulletHole(Vector3 position, Vector3 normal) {
        GameObject decal = GetDecal();
        decal.transform.position = position;
        decal.transform.forward = normal;
        //decal.transform.Translate(Vector3.forward * offset);
        decal.transform.position += new Vector3(0, 0, offset);
        drm.DecalPlaced(decal);
    }

    GameObject GetDecal() {
        GameObject decal;
        decal = decals[decalIndex];
        decalIndex++;
        if (decalIndex >= decals.Count) decalIndex = 0;
        return decal;
    }

    void CreateDecals() {
        for (int i = 0; i < maxDecals; i++) {
            GameObject DecalClone = Instantiate(bulletDecalPrefab, new Vector3(510 + i, -510, 500), Quaternion.identity);
            decals.Add(DecalClone);
        }
    }
}
