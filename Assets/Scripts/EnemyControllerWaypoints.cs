using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerWaypoints : Entity
{
    public GameObject  master, drop;
    public AudioSource audioS;
    public AudioClip screech,hit;
    public Animator anim;
    public Transform target, A, B, C, D;
    Vector3 hitTarget;
    public NavMeshAgent agent;
    public Transform[] targets;
    public HealthBar healthBar;
    public int damage, multiplier, range;
    public bool isPlayertarget, isDying, inPlayer, enemyHit;
    GameObject previousAttack;




    // Start is called before the first frame update
    void Start()
    {
        SetHealth(startingHealth);
        healthBar.UpdateHealth(getHealth(), maxHealth);
        hitTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        enemyHit = true;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        inPlayer = false;
        isPlayertarget = false;
        isDying = false;
        range = 5;
        multiplier = 1;
        master = transform.parent.gameObject;
        if(master.transform.Find("Drop") != null) drop = master.transform.Find("Drop").gameObject;
        audioS = GetComponent<AudioSource>();
        target = master.transform.Find("Waypoint_A");
        A = master.transform.Find("Waypoint_A");
        B = master.transform.Find("Waypoint_B");
        C = master.transform.Find("Waypoint_C");
        D = master.transform.Find("Waypoint_D");
        targets = new Transform[] { A, B, C, D };
        //healthBarLength = Screen.width / 6;
    }
    public void ChangeTarget(Transform tar)
    {
        target = tar;
        //audioS.PlayOneShot(screech);
        agent.speed = 4;

    }
    public void TakeDamage(int damage)
    {
        audioS.PlayOneShot(hit);
        ModifyHealth(-1 * damage);
        healthBar.UpdateHealth(getHealth(), maxHealth);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword"  && previousAttack== null)
        {
            enemyHit = true;
            agent.acceleration = 100;
            hitTarget = collision.gameObject.GetComponent<Transform>().position;
            previousAttack = collision.gameObject;
            audioS.PlayOneShot(hit);
            ModifyHealth(-1 * collision.gameObject.GetComponent<DamageModifier>().damage);
            healthBar.UpdateHealth(getHealth(), maxHealth);
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
        {
            return;
        }
        if(collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.ModifyHealth((-1)*damage);
            player.StartCoroutine("PlayerInvincible");
            player.Knockback(transform);
            inPlayer = true;
            agent.isStopped = true;
            agent.speed = 0;
            //Vector3 moveDirection = collision.gameObject.GetComponent<Transform>().position - transform.parent.position;
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(moveDirection * 500f);
            StartCoroutine("PlayerInvinc");
        }
        if (collision.gameObject.tag == "Projectile")
        {
            audioS.PlayOneShot(hit);
            enemyHit = true;
            agent.acceleration = 100;
            hitTarget = collision.gameObject.GetComponent<Transform>().position;
            ModifyHealth(-1*collision.gameObject.GetComponent<DamageModifier>().damage);
            healthBar.UpdateHealth(getHealth(), maxHealth);
        }
    }
    IEnumerator PlayerInvinc()
    {
        yield return new WaitForSeconds(2);
        inPlayer = false;
        agent.isStopped = false;
        agent.speed = 3.5f;

    }
    IEnumerator Death()
    {
        anim.Play("Die");
        if (drop != null)
        {
            Instantiate(drop,transform.position, Quaternion.identity);
        }
        
        yield return new WaitForSeconds(1);
        Destroy(master);
    }


    // Update is called once per frame
    void Update()
    {
        //print(GetComponent<Collider2D>().isTrigger);
        if (isDead)
        {
            if (isDying)
            {
                return;
            }
            agent.isStopped = true;
            audioS.PlayOneShot(screech);
            StartCoroutine("Death");
            isDying = true;

        }
        if (enemyHit)
        {
            
            Vector3 runTo = transform.position + ((transform.position - hitTarget) * multiplier);
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < range)
            {
                agent.SetDestination(runTo);
                agent.stoppingDistance = 0;
                agent.speed = 6;
            }
            else
            {
                enemyHit = false;
                agent.acceleration = 10;
            }
        }
        else if (!inPlayer)
        {
            //healthBar.value = getHealth();
            if(target == null)
            {
                target = A;
            }
            agent.SetDestination(target.position);
            if (isPlayertarget)
            {
                return;
            }
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
}
