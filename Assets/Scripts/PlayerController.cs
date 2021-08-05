using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 5;
    [Header("Set Dynamically")]
    public int dirHeld = -1; // Direction of the held movement key
    private Rigidbody2D rigid;
    private Animator anim;

    private Vector3[] directions = new Vector3[] {Vector3.right, Vector3.up, Vector3.left, Vector3.down };

    private KeyCode[] keys = new KeyCode[] { KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S };


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dirHeld = -1;
        // Delete the four "if ( Input.GetKey..." lines that were here
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKey(keys[i])) dirHeld = i; 
        }
        Vector3 vel = Vector3.zero;
        if (dirHeld > -1) vel = directions[dirHeld];
        rigid.velocity = vel * speed;

        // Animation
        if (dirHeld == -1)
        { 
            anim.speed = 0;
        }
        else
        {
            anim.CrossFade("Player_Walk_" + dirHeld, 0); 
            anim.speed = 1;
        }
    }

    //Collision tells us what trigger the player is colliding with
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            //picked up test rupee
            print("Rupee collected!!!");
            //add a "money" increment
            Destroy(collision.gameObject);
        }



    }

}
