using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public bool up, down, left, right;
    public AudioSource audiosource;
    public AudioClip clip;
    int face;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.dirFacing == face && player.takeKey())
            {
                audiosource.volume = 1;
                audiosource.PlayOneShot(clip);
                Destroy(this.gameObject);
            }
            else
            {
                return;
            }
        }
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