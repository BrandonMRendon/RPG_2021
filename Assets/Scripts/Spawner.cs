using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obj;
    public GameObject clone;
    public GameObject parent;
    public GameObject toDestroy;
    public bool notDestroyable;

    private void Awake()
    {
        Destroy(toDestroy);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Spawn")
        {
            clone = Instantiate(obj, transform.position, transform.rotation);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Spawn" && !notDestroyable)
        {
            Destroy(clone);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Spawn")
        {
            clone = Instantiate(obj, transform.position, transform.rotation, parent.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spawn" && !notDestroyable)
        {
            Destroy(clone);
        }
    }
    private void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
    }
}
