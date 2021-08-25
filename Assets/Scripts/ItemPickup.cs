using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Shoot projectile;
    public Swing sword;
    public int value;
    public bool shoot, swing, key, slot1, health;
    public string printer = "nothing ";
    public AudioSource audiosource;
    private IItem whatType()
    {
        if (shoot)
        {
            return projectile;
        }
        else
        {
            return sword;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (key)
            {
                collision.gameObject.GetComponent<PlayerController>().getKey();
                audiosource.Play();
                StartCoroutine("WaitForAudio");
                GetComponent<SpriteRenderer>().enabled = false;
                collision.isTrigger = false;
                return;
            }
            if (health)
            {
                collision.gameObject.GetComponent<PlayerController>().ModifyHealth(value);
                audiosource.Play();
                StartCoroutine("WaitForAudio");
                GetComponent<SpriteRenderer>().enabled = false;
                collision.isTrigger = false;
                return;
            }
            gameObject.transform.parent = collision.gameObject.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            if (slot1)
            {
                collision.gameObject.GetComponent<PlayerController>().Equipped1 = whatType();
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().Equipped2 = whatType();
            }
            
            GetComponent<SpriteRenderer>().enabled = false;
            
        }
    }
    IEnumerator WaitForAudio()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
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
