using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour, IItem
{
    public GameObject swordPrefab;
    public static GameObject sword;

    public int Damage { get; set; }

    public void Action(Vector2 directionFacing)
    {
        if (sword != null) return;
        Transform swordTransform;
        sword = Instantiate(swordPrefab, transform.position + (Vector3)directionFacing, transform.rotation);
        swordTransform = sword.GetComponent<Transform>();
        if (directionFacing == Vector2.right)
        {
            swordTransform.localScale = new Vector3(swordTransform.localScale.x, swordTransform.localScale.y * - 1, swordTransform.localScale.z);
            swordTransform.position = new Vector3(swordTransform.position.x - .75f, swordTransform.position.y, swordTransform.position.z);
            sword.GetComponent<SwordRotate>().dir = SwordRotate.direction.right;
        }
        if (directionFacing == Vector2.up)
        {
            sword.GetComponent<SwordRotate>().dir = SwordRotate.direction.up;
            swordTransform.position = new Vector3(swordTransform.position.x, swordTransform.position.y-.75f, swordTransform.position.z);
        }
        if (directionFacing == Vector2.left)
        {
            sword.GetComponent<SwordRotate>().dir = SwordRotate.direction.left;
            swordTransform.position = new Vector3(swordTransform.position.x + .75f, swordTransform.position.y, swordTransform.position.z);
        }
        if (directionFacing == Vector2.down)
        {
            swordTransform.position = new Vector3(swordTransform.position.x, swordTransform.position.y + .7f, swordTransform.position.z);
            sword.GetComponent<SwordRotate>().dir = SwordRotate.direction.down;
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
