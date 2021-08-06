using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform to;
    private GameObject Player;
    private Animator FADEBLACK;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        FADEBLACK = GameObject.FindGameObjectWithTag("Fader").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FADEBLACK.SetTrigger("Fade");
            StartCoroutine("TeleportPlayer");
        }
    }
    IEnumerator TeleportPlayer()
    {
        yield return  new WaitForSeconds(.5f);
        print("Yeet");
        Player.transform.position = to.position;
    }
}
