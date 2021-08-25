using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerWaypoints : Entity
{
    public GameObject monsterField, master, drop;
    public AudioSource audioS;
    public AudioClip screech,hit;
    public Animator anim;
    public Transform target, A, B, C, D;
    public NavMeshAgent agent;
    public Transform[] targets;
    public HealthBar healthBar;
    public int damage;
    public bool isPlayertarget, isDying;
    GameObject previousAttack;




    // Start is called before the first frame update
    void Start()
    {
        SetHealth(startingHealth);
        healthBar.UpdateHealth(getHealth(), maxHealth);
        targets = new Transform[] { A, B, C, D };
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        isPlayertarget = false;
        isDying = false;
        audioS = GetComponent<AudioSource>();
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
            collision.gameObject.GetComponent<PlayerController>().ModifyHealth((-1)*damage);
            GetComponent<Collider2D>().isTrigger = true;
            //Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            Vector3 moveDirection = collision.gameObject.GetComponent<Transform>().position - transform.parent.position;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(moveDirection * 500f);
            StartCoroutine("PlayerInvinc", collision);
        }
        if (collision.gameObject.tag == "Projectile")
        {
            audioS.PlayOneShot(hit);
            ModifyHealth(-1*collision.gameObject.GetComponent<DamageModifier>().damage);
            healthBar.UpdateHealth(getHealth(), maxHealth);
        }
    }
    IEnumerator PlayerInvinc(Collision2D collision)
    {
        yield return new WaitForSeconds(2);
        //Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), false);
        GetComponent<Collider2D>().isTrigger = false;
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
        //healthBar.value = getHealth();
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
