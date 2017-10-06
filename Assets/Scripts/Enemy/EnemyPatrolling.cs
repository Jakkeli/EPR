using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolling : MonoBehaviour {

	public Transform patrolPoints;
	public float distTolerance;
	public float waitTime;
	public float patrollingMovementSpeed;

	float distToNextPoint;
	List<Vector3> points = new List<Vector3>();
	int patrolIndex;
	bool patrolling;

	EnemyMovement movement;

	void Start () {
		movement = GetComponent<EnemyMovement>();
		InitPatrolPoints();
	}

	void Update () {
		if (!patrolling) return;

		distToNextPoint = Vector3.Distance(transform.position, points[patrolIndex]);
		if (distToNextPoint < distTolerance) {
			movement.StopForTime(waitTime);
			MoveToNextPoint();
		}
	}

	void MoveToNextPoint () {
		patrolIndex = (int)Mathf.Repeat(patrolIndex + 1, points.Count);
		movement.MoveToTarget(points[patrolIndex], patrollingMovementSpeed);
	}

	public void Continue () {
		patrolling = true;
	}
	public void Stop () {
		patrolling = false;
	}

	void InitPatrolPoints () {
		var patrolChildren = patrolPoints.GetComponentsInChildren<Transform>();
		foreach (Transform t in patrolChildren) {
			points.Add(t.position);
		}
		points.Remove(patrolPoints.transform.position);

		patrolPoints.name += "(" + transform.name + ")";
		patrolPoints.transform.parent = transform.root;
	}
}
