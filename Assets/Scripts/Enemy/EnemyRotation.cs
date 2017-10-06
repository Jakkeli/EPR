using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour {

	public AnimationCurve rotationCurve;
	public float rotationSpeed;

	Coroutine currentLookAt;

	public void LookAt (Vector3 point) {
		if (currentLookAt == null && point != transform.position) {
			currentLookAt = StartCoroutine(LookAtCoroutine(point));
		}		
	}

	IEnumerator LookAtCoroutine (Vector3 point) {
		point.y = 0;
		var pos = new Vector3(transform.position.x, 0, transform.position.z);
		var dir = (point - pos).normalized;
		var rot = Quaternion.LookRotation(dir, Vector3.up);
		var startRot = transform.rotation;
		var angle = Quaternion.Angle(startRot, rot);
		float t = 0;
		while (t < 1 && angle > 0) {
			t += rotationSpeed * Time.deltaTime / angle;
			var c = rotationCurve.Evaluate(t);
			transform.rotation = Quaternion.Lerp(startRot, rot, c);
			yield return new WaitForEndOfFrame();
		}
		transform.rotation = rot;
		currentLookAt = null;
	}
}