using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	NavMeshAgent agent;
	Animator anim;
    //GameManager gm;
    public bool shouldBeWalking;

	void Start () {
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator>();
        //gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    public void MoveToTarget (Vector3 target, float speed) {
		agent.speed = speed;
		agent.SetDestination(target);
	}

	public void Stop () {
		agent.isStopped = true;
		anim.SetTrigger("Idle");
        shouldBeWalking = false;
        //print("stop called");
	}
	public void Continue () {
		agent.isStopped = false;
		anim.SetTrigger("Walk");
        shouldBeWalking = true;
	}

	public void StopForTime (float time) {
		if (!agent.isStopped) {
			StartCoroutine(StopForTimeCoroutine(time));
		}
	}

	IEnumerator StopForTimeCoroutine (float time) {
        //print("coroutine called");
		agent.isStopped = true;
		anim.SetTrigger("Idle");
        shouldBeWalking = false;
        yield return new WaitForSeconds(time);
		agent.isStopped = false;
		anim.SetTrigger("Walk");
        shouldBeWalking = true;
    }
	
	public Vector3 GetNextPathPoint () {
		if (agent.path.corners.Length > 1) {
			return agent.path.corners[1];
		} else {
			return transform.position;
		}
	}

	public bool HasPathToPoint (Vector3 point) {
		return agent.CalculatePath(point, new NavMeshPath());
	}
}
