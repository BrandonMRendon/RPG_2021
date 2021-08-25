using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public bool up, down, left, right;
    
    int face;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.dirFacing == face && player.takeKey())
            {
                GetComponent<AudioSource>().Play();
                StartCoroutine("WaitForAudio");
            }
            else
            {
                return;
            }
        }
    }
    IEnumerator WaitForAudio()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
    void Start()
    {
        if (up)
        {
            face = 1;
        }
        if (down)
        {
            face = 3;
        }
        if (right)
        {
            face = 0;
        }
        if (left)
        {
            face = 2;
        }
    }
}