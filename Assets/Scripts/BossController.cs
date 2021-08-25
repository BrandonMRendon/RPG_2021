using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Entity
{
    public GameObject bulletPrefab;
    public Transform parent, tail, rightGun, leftGun, tailLaser, playerTran;
    public Animator animator, fader;
    public float setSpeed;
    GameObject player;
    public AudioSource audios, audios2, audiosource1, audiosource2;
    public AudioClip pew, hit, roar;
    public int setDamage;
    public bool ready, isIdle, isLeft;
    public string actionChoice;
    public HealthBar healthBar;
    public bool isDying;
    GameObject previousAttack;
    //string[] actions = new string[] {"ArmShoot",  "Jump" };
    string[] actions = new string[] {"ScissorShoot", "ArmShoot", "TailLaser", "Jump", "Charge" };
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTran = player.GetComponent<Transform>();
        StartCoroutine("BossTransition");
        animator = GetComponent<Animator>();
        ready = false;
        isIdle = true;
        isLeft = true;
        SetHealth(startingHealth);
        healthBar.UpdateHealth(getHealth(), maxHealth);
    }
    IEnumerator Death()
    {
        animator.Play("Death");

        yield return new WaitForSeconds(7);
        fader.Play("FadeToBlack");
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
        {
            return;
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().ModifyHealth((-1) * setDamage);
            //GetComponent<Collider2D>().isTrigger = true;
            //Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            //Vector3 moveDirection = collision.gameObject.GetComponent<Transform>().position - transform.parent.position;
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(moveDirection * 500f);
            //StartCoroutine("PlayerInvinc", collision);
        }
        if (collision.gameObject.tag == "Projectile")
        {
            audios2.PlayOneShot(hit);
            ModifyHealth(-1 * collision.gameObject.GetComponent<DamageModifier>().damage);
            healthBar.UpdateHealth(getHealth(), maxHealth);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword" && previousAttack == null)
        {

            previousAttack = collision.gameObject;
            audios2.PlayOneShot(hit);
            ModifyHealth(-1 * collision.gameObject.GetComponent<DamageModifier>().damage);
            healthBar.UpdateHealth(getHealth(), maxHealth);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            if (isDying)
            {
                return;
            }
            audiosource1.Stop();
            audiosource2.Play();
            audios2.PlayOneShot(roar);
            StartCoroutine("Death");
            isDying = true;

        }
        if (ready)
        {
            if (isIdle)
            {
                actionChoice = actions[Random.Range(0, 5)];
                animator.SetTrigger(actionChoice);
                isIdle = false;
                isLeft = actionChoice == "Jump" ? !isLeft : isLeft;
            }
            else
            {
                animator.SetTrigger("Idle");
                isIdle = true;
            }
            ready = false;
        }
    }
    public void ShootRight()
    {
        audios.PlayOneShot(pew);
        Shoot(rightGun);
    }
    public void ShootLeft()
    {
        audios.PlayOneShot(pew);
        Shoot(leftGun);
    }
    IEnumerator BossTransition()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            ready = true;
        }
        
    }
    private void TailLaser()
    {
        audios.PlayOneShot(pew);
        GameObject projectile;
        projectile = Instantiate(bulletPrefab, tailLaser.position + Vector3.right, tailLaser.parent.rotation);
        projectile.GetComponent<SpriteRenderer>().sortingLayerName = player.GetComponent<SpriteRenderer>().sortingLayerName;
        projectile.GetComponent<DamageModifier>().damage = setDamage;
        Collider2D col1 = projectile.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(col1, GetComponent<Collider2D>());
        if (GameObject.FindGameObjectWithTag("ColliderProjectile") != null)
        {
            Collider2D col2 = GameObject.FindGameObjectWithTag("ColliderProjectile").GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(col1, col2);
        }
        if (GameObject.FindGameObjectWithTag("ColliderProjectile1") != null)
        {
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("ColliderProjectile1").GetComponent<Collider2D>());
        }
        Vector3 direction = tailLaser.position - playerTran.position;
        direction.Normalize();
        projectile.GetComponent<Rigidbody2D>().AddRelativeForce((direction) * setSpeed * 200);
        //projectile.GetComponent<Rigidbody2D>().AddTorque(50 * 5);
    }
    private void Shoot(Transform tran)
    {
        GameObject projectile;
        projectile = Instantiate(bulletPrefab, tran.position + Vector3.right, tran.parent.rotation);
        projectile.GetComponent<SpriteRenderer>().sortingLayerName = player.GetComponent<SpriteRenderer>().sortingLayerName;
        projectile.GetComponent<DamageModifier>().damage = setDamage;
        //audios.Play();
        Collider2D col1 = projectile.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(col1, GetComponent<Collider2D>());
        if (GameObject.FindGameObjectWithTag("ColliderProjectile") != null)
        {
            Collider2D col2 = GameObject.FindGameObjectWithTag("ColliderProjectile").GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(col1, col2);
        }
        if (GameObject.FindGameObjectWithTag("ColliderProjectile1") != null)
        {
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("ColliderProjectile1").GetComponent<Collider2D>());
        }
        projectile.GetComponent<Rigidbody2D>().AddRelativeForce((isLeft ? Vector2.right:Vector2.left) * setSpeed * 200);
        //projectile.GetComponent<Rigidbody2D>().AddTorque(50 * 5);
    }
}
