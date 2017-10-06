using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResetManager : MonoBehaviour {

    public List<GameObject> enemiesDeadAfterLatestCP;
	
	public void CheckPointChange() {
        enemiesDeadAfterLatestCP.Clear();
    }

    public void EnemyDied(GameObject enemyGO) {
        enemiesDeadAfterLatestCP.Add(enemyGO);
    }

    public void ResurrectEnemies() {
        //foreach (GameObject enemy in enemiesDeadAfterLatestCP) {
        //    enemy.GetComponent<EnemyReset>().ResetEnemy();
        //}
    }
}
