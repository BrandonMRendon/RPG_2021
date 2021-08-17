using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour, IItem
{
    public GameObject swordPrefab;
    public static GameObject sword;
    Transform swordTransform;
    GameObject player;
    AudioSource swish;
    public int setDamage,setSpeed;
    public int Damage { get; set; }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Damage = setDamage;
        swish = GetComponent<AudioSource>();
    }
    public void Action(Vector2 directionFacing)
    {
        if (sword != null) return;
        
        sword = Instantiate(swordPrefab, transform.position + (Vector3)directionFacing, transform.rotation);
        swordTransform = sword.GetComponent<Transform>();
        sword.GetComponent<DamageModifier>().damage = Damage;
        sword.GetComponent<SwordRotate>().speed = setSpeed;
        swish.Play();
        if (directionFacing == Vector2.right)
        {
            swordTransform.localScale = new Vector3(swordTransform.localScale.x, swordTransform.localScale.y * - 1, swordTransform.localScale.z);
            swordTransform.position = new Vector3(swordTransform.position.x - .75f, swordTransform.position.y, swordTransform.position.z);
            sword.GetComponent<SwordRotate>().dir = SwordRotate.direction.right;
            sword.GetComponent<SpriteRenderer>().sortingLayerName = player.GetComponent<SpriteRenderer>().sortingLayerName;
            sword.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        if (directionFacing == Vector2.up)
        {
            sword.GetComponent<SwordRotate>().dir = SwordRotate.direction.up;
            swordTransform.position = new Vector3(swordTransform.position.x, swordTransform.position.y-.75f, swordTransform.position.z);
            sword.GetComponent<SpriteRenderer>().sortingLayerName = player.GetComponent<SpriteRenderer>().sortingLayerName;
            sword.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        if (directionFacing == Vector2.left)
        {
            sword.GetComponent<SwordRotate>().dir = SwordRotate.direction.left;
            swordTransform.position = new Vector3(swordTransform.position.x + .75f, swordTransform.position.y, swordTransform.position.z);
            sword.GetComponent<SpriteRenderer>().sortingLayerName = player.GetComponent<SpriteRenderer>().sortingLayerName;
            sword.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        if (directionFacing == Vector2.down)
        {
            swordTransform.position = new Vector3(swordTransform.position.x, swordTransform.position.y + .7f, swordTransform.position.z);
            sword.GetComponent<SwordRotate>().dir = SwordRotate.direction.down;
            sword.GetComponent<SpriteRenderer>().sortingLayerName = player.GetComponent<SpriteRenderer>().sortingLayerName;
            sword.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        /*
        Rigidbody2D rigid = sword.GetComponent<Rigidbody2D>();
        rigid.angularVelocity = swingSpeed;
        rigid.AddTorque(swingSpeed*5);
        rigid.angularDrag = 0;
        sword.GetComponent<Transform>().parent = GameObject.FindGameObjectWithTag("Player").transform;
        //sword.GetComponent<Rigidbody2D>().AddTorque(swingSpeed);*/
    }

}
