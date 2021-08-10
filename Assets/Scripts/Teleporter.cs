using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform to;
    private GameObject Player;
    private Animator FADEBLACK;

    //private CinemachineBrain brain;

    private void Start()
    {
        //brain = GameObject.FindObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
        Player = GameObject.FindGameObjectWithTag("Player");
        FADEBLACK = GameObject.FindGameObjectWithTag("Fader").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //brain.
            FADEBLACK.SetTrigger("Fade");
            StartCoroutine("TeleportPlayer");
        }
    }
    IEnumerator TeleportPlayer()
    {
        yield return  new WaitForSeconds(.25f);
        Player.transform.position = to.position;
    }
}
