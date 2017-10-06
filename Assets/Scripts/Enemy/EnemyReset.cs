using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReset : MonoBehaviour {

    //Vector3 originalPosition;
    //Quaternion originalRotation;

    //EnemyShooting enemyShooting;
    //EnemyController enemyController;
    //EnemySenses enemySenses;
    //EnemyMovement enemyMovement;
    //EnemyPatrolling enemyPatrolling;
    //EnemyRotation enemyRotation;
    //EnemyHealth enemyHealth;
    //EnemyResetManager erm;

    //bool originalStillAlive;

	void Start () {
        //erm = GameObject.Find("GameManager").GetComponent<EnemyResetManager>();
        //if (!originalStillAlive) {
        //    CopyThySelf();
        //}
        
        //originalPosition = transform.localPosition;
        //originalRotation = transform.localRotation;
        //enemyShooting = GetComponent<EnemyShooting>();
        //enemyController = GetComponent<EnemyController>();
        //enemySenses = GetComponent<EnemySenses>();
        //enemyMovement = GetComponent<EnemyMovement>();
        //enemyPatrolling = GetComponent<EnemyPatrolling>();
        //enemyRotation = GetComponent<EnemyRotation>();
        //enemyHealth = GetComponent<EnemyHealth>();
	}

    //public void CopyThySelf() {
    //    GameObject copy = Instantiate(gameObject, transform.localPosition, Quaternion.identity, transform.parent);
    //    copy.GetComponent<EnemyReset>().Hibernate();
    //}

    //public void Hibernate() {
    //    originalStillAlive = true;
    //    erm.EnemyDied(gameObject);
    //    gameObject.SetActive(false);
    //}
	
	public void ResetEnemy() {
        //enemyHealth.ResetAllParts();
        //enemyMovement.StopAllCoroutines();
        //enemyMovement.Stop();
        //enemyShooting.ResetShooting();
        //enemyController.ResetBool();
        //enemySenses.ResetBool();
        //enemyPatrolling.Stop();
        //enemyRotation.StopAllCoroutines();

    }
}
