using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float timeToAttack;
	public float attackMovementSpeed;

	bool spottedLastFrame;
	bool diedLastFrame;

	Transform player;
	EnemyPatrolling patrolling;
	EnemyMovement movement;
	EnemyRotation rotation;
	EnemySenses senses;
	EnemyShooting shooting;
	EnemyHealth health;
	Animator anim;
    GameManager gm;	

	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		player = FindObjectOfType<PlayerMovement>().transform;
		patrolling = GetComponent<EnemyPatrolling>();
		movement = GetComponent<EnemyMovement>();
		rotation = GetComponent<EnemyRotation>();
		senses = GetComponent<EnemySenses>();
		shooting = GetComponent<EnemyShooting>();
		health = GetComponent<EnemyHealth>();
		anim = GetComponentInChildren<Animator>();
	}

    public void ResetBool() {
        spottedLastFrame = false;
    }

	void Update () {
		// DEBUG
		//if (Input.GetKey(KeyCode.H)) {
		//	Debug.DrawRay(ClosestPointWithCover(), Vector3.up, Color.cyan);
		//}

		if (diedLastFrame || gm.currentState != GameState.Run) {
			return;
		}

		if (health.dead && !diedLastFrame) {
			patrolling.Stop();
			movement.Stop();
			anim.enabled = false;
			health.SeparateAllParts();
		}
		diedLastFrame = health.dead;

		var playerInSight = senses.IsPlayerInSight();

		if (senses.playerSpotted && !spottedLastFrame) {
			patrolling.Stop();
			movement.StopForTime(timeToAttack);
			senses.AlertNearbyEnemies();
			rotation.LookAt(player.position);
		} else if (senses.playerSpotted && spottedLastFrame) {
			if (playerInSight && shooting.currentAmmo > 0) {
				rotation.LookAt(player.position);
				shooting.Shoot();
				movement.Stop();
			} else if (shooting.currentAmmo < 1) {
				var coverPoint = ClosestPointWithCover();
				rotation.LookAt(coverPoint);
				shooting.Reload();
				movement.MoveToTarget(coverPoint, attackMovementSpeed);
				movement.Continue();
			} else if (!playerInSight && !shooting.reloading) {
				rotation.LookAt(player.position);
				movement.MoveToTarget(player.position, attackMovementSpeed);
				movement.Continue();
			}
		} else {
			rotation.LookAt(movement.GetNextPathPoint());
			patrolling.Continue();
		}

		spottedLastFrame = senses.playerSpotted;
	}

	Vector3 ClosestPointWithCover () {
		var coverPoints = new List<Vector3>();
		foreach (Vector3 v in AIData.navMeshVerts) {
			if (!senses.CastToPlayer(v + Vector3.up * senses.eyes.position.y) && movement.HasPathToPoint(v)) {
				coverPoints.Add(v);
			}
		}

		var closestDist = Mathf.Infinity;
		var closestPoint = Vector3.down;
		foreach (Vector3 v in coverPoints) {
			var dist = Vector3.Distance(v, transform.position);
			if (dist < closestDist) {
				closestDist = dist;
				closestPoint = v;
			}
		}

		return closestPoint;
	}
}
