using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.Universal;

public class DunegonCrawler : MonoBehaviour
{
    public GameObject monsterField;
    public AudioSource audioS;
    public AudioClip screech, hit;
    public Animator anim;
    public Transform target, playerTransform;
    public PlayerController player;
    public NavMeshAgent agent;
    public float multiplier, range;
    public bool isPlayertarget;
    GameObject previousAttack;
    Collider2D collider2;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        isPlayertarget = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target = playerTransform;
        audioS = GetComponent<AudioSource>();
        collider2 = GetComponent<Collider2D>();
        Collider2D col2 = GameObject.FindGameObjectWithTag("ColliderProjectile1").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider2, col2);
        col2 = GameObject.FindGameObjectWithTag("ColliderProjectile").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider2, col2);
        col2 = GameObject.FindGameObjectWithTag("LayerFloorOneOnly").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider2, col2);
        col2 = GameObject.FindGameObjectWithTag("MainCollider").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider2, col2);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Light2D>() != null)
        {
            GameObject col = collision.gameObject;
            if (Vector3.Distance(transform.position, target.position) > Vector3.Distance(transform.position, col.GetComponent<Transform>().position))
            {
                target = col.GetComponent<Transform>();
                range = collision.gameObject.GetComponent<Light2D>().pointLightOuterRadius;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        //agent.SetDestination(target.position);
        if (target == null) target = playerTransform;
        Vector3 runTo = transform.position + ((transform.position - target.position) * multiplier);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < range)
        {
            agent.SetDestination(runTo);
            agent.stoppingDistance = 0;
            agent.speed = 6;
        }
        else
        {
            agent.SetDestination(playerTransform.position);
            agent.stoppingDistance = player.lightProtection;
            agent.speed = 3.5f;
        }
        /*
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
        }*/
    }
}
