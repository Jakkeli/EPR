using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySenses : MonoBehaviour {

	public Transform eyes;
	public float fov;
	public float alertRadius;
	public LayerMask mask;

	public bool playerSpotted { get; private set; }
	//public Vector3 lastKnownPlayerLocation { get; private set; }

	Transform player;

	void Start () {
		player = FindObjectOfType<PlayerMovement>().transform;
	}

    public void ResetBool() {
        playerSpotted = false;
    }

	public bool IsPlayerInSight () {
		var dirToPlayer = (player.position - eyes.position).normalized;
		var angle = Vector3.Angle(transform.forward, dirToPlayer);
		if (angle < fov / 2) {
			if (CastToPlayer(eyes.position)) {
				SpottedPlayer();
				return true;
			}
		}
		return false;
	}

	public bool CastToPlayer (Vector3 from) {
		var dirToPlayer = (player.position - from).normalized;
		Ray ray = new Ray(from, dirToPlayer);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask) && hit.collider.transform.parent == player) {
			return true;
		}
		return false;
	}

	public void SpottedPlayer () {
		playerSpotted = true;
		//lastKnownPlayerLocation = player.position;
	}

	public void AlertNearbyEnemies () {
		var allOtherEnemies = AIData.allEnemies;
		allOtherEnemies.Remove(GetComponent<EnemyController>()); // Removes self
		foreach (EnemyController e in allOtherEnemies) {
			var dist = Vector3.Distance(transform.position, e.transform.position);
			if (dist < alertRadius) {
				e.GetComponent<EnemySenses>().SpottedPlayer();
			}
		}
	}
}
