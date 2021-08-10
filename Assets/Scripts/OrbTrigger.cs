using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbTrigger : MonoBehaviour
{
    bool isOn;
    public GameObject activeWhenOn, activeWhenOff;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Sword"))
        {
            if (isOn)
            {
                activeWhenOn.SetActive(false);
                activeWhenOff.SetActive(true);
                isOn = false;
            }
            else
            {
                activeWhenOn.SetActive(true);
                activeWhenOff.SetActive(false);
                isOn = true;
            }
        }
    }
    void Start()
    {
        isOn = false;
    }

}
