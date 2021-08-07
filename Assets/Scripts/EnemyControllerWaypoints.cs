using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerWaypoints : MonoBehaviour
{
    public GameObject monsterField;
    //public AudioSource audioS;
    //public AudioClip screech;
    //public Animator anim;
    public Transform target, A, B, C, D;
    public NavMeshAgent agent;
    public Transform[] targets;
    


    // Start is called before the first frame update
    void Start()
    {
        targets = new Transform[] { A, B, C, D };
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    public void ChangeTarget(Transform tar)
    {
        target = tar;
        //audioS.PlayOneShot(screech);
        agent.speed = 4;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ChangeTarget(collision.gameObject.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ChangeTarget(targets[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    target = targets[Random.Range(0, 4)];
                    agent.speed = 1;
                    //PlayerController.Instance.ResetTracker();
                }
            }
        }
    }
}
