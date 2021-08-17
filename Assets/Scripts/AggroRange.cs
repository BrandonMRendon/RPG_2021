using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour
{
    public EnemyControllerWaypoints enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.ChangeTarget(collision.gameObject.transform);
            enemy.isPlayertarget = true;
            enemy.agent.stoppingDistance = 1;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.ChangeTarget(enemy.targets[0]);
            enemy.agent.stoppingDistance = 2;
            enemy.isPlayertarget = false;
        }

    }
}
