using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EnemyPartEditor : EditorWindow {

	Transform parent;

	[MenuItem("Window/Enemy Part Editor")]
	static void Init () {
		EnemyPartEditor window = (EnemyPartEditor)EditorWindow.GetWindow(typeof(EnemyPartEditor));
		window.Show();
	}

	void OnGUI () {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Parent Object");
		parent = (Transform)EditorGUILayout.ObjectField(parent, typeof(Transform), true);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Set Components")) {
			var colliders = parent.GetComponentsInChildren<MeshCollider>();
			foreach (MeshCollider c in colliders) {
				Rigidbody rb;
				EnemyPart ep;
				if (c.GetComponent<Rigidbody>() == null) {
					rb = c.gameObject.AddComponent<Rigidbody>();
					rb.isKinematic = true;
					rb.useGravity = false;
				}
				if (c.GetComponent<EnemyPart>() == null) {
					ep = c.gameObject.AddComponent<EnemyPart>();
				}
				c.convex = true;
			}
		}
	}
}
