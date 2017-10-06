using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIData : MonoBehaviour {

	public static List<Vector3> navMeshVerts { get; private set; }
	public static List<EnemyController> allEnemies { get; private set; }
	public bool visualizeVerts; // DEBUG

	void Awake () {
		// Makes a list of NavMesh vertices without overlapping vectors
		NavMeshTriangulation navMeshTriangles = NavMesh.CalculateTriangulation();
		var verts = navMeshTriangles.vertices;
		navMeshVerts = new List<Vector3>();
		foreach (Vector3 v in verts) {
			if (!navMeshVerts.Contains(v)) {
				navMeshVerts.Add(v);
			}
		}

		allEnemies = new List<EnemyController>(FindObjectsOfType<EnemyController>());
	}

	// DEBUG
	void Update () {
		if (visualizeVerts) {
			foreach (Vector3 v in navMeshVerts) {
				Debug.DrawRay(v, Vector3.up * 0.5f, Color.red);
			}
		}
	}
}
