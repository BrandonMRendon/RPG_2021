using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int value;
    public bool key, health, destroyAnimator, destroyAudio, destroyLight;
    public string printer = "nothing ";
    public AudioSource audiosource;
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (key)
            {
                collision.gameObject.GetComponent<PlayerController>().getKey();
                audiosource.Play();
                GetComponent<SpriteRenderer>().enabled = false;
                collision.isTrigger = false;
                return;
            }
            if (health)
            {
                collision.gameObject.GetComponent<PlayerController>().ModifyHealth(value);
                audiosource.Play();
                GetComponent<SpriteRenderer>().enabled = false;
                collision.isTrigger = false;
                return;
            }
            
            gameObject.transform.parent = collision.gameObject.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            if (collision.gameObject.GetComponent<PlayerController>().AddItemToInv(this.gameObject))
            {
                Destroy(this.gameObject);
            }
            
            GetComponent<SpriteRenderer>().enabled = false;
            if (destroyAnimator)
            {
                Destroy(GetComponent<Animator>());
            }
            if (destroyAudio)
            {
                Destroy(GetComponent<AudioSource>());
            }
            if (destroyLight)
            {
                Destroy(GetComponent<Light2D>());
            }
            Destroy(GetComponent<SpriteRenderer>());
            
            Destroy(GetComponent<Collider2D>());

            Destroy(this);
        }
    }
    void Update()
    {
        if (GetComponent<Renderer>().isVisible)
        {
            //print(printer+" Visible");
        }
        else
        {

        }
    }
    void Start()
    {
        
    }
}
