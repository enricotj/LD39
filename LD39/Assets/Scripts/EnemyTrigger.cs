using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    private GameObject enemyParent;

    public void SetEnemyParent(GameObject parent)
    {
        enemyParent = parent;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        bool isSpawn = enemyParent.GetComponent<EnemyController>().spawnTimer == 0;
        if (coll.name == "Player" && isSpawn)
        {
            PlayerController player = coll.gameObject.GetComponent<PlayerController>();
            int level = enemyParent.GetComponent<EnemyController>().level;
            if (player.state == State.Dash)
            {
                Debug.Log("Player's Kill");
                Game.Instance.Power += 8 * level;
            }
            else
            {
                Game.Instance.Power -= 2 * level;
            }
            Destroy(enemyParent);
            Destroy(this.gameObject);
        }
    }
}
