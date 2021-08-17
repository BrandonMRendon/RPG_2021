using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMan : MonoBehaviour
{
    public AudioSource boomSound;
    public void Boom()
    {
        boomSound.Play();
    }
}
