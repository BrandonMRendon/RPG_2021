using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Shoot projectile;
    public Swing sword;
    public bool shoot, swing, key, slot1;
    public string printer = "nothing ";
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
                Destroy(this.gameObject);
                return;
            }
            gameObject.transform.parent = collision.gameObject.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            print(whatType());
            if (slot1)
            {
                collision.gameObject.GetComponent<PlayerController>().Equipped1 = whatType();
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().Equipped2 = whatType();
            }
            
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(this);
        }
    }
    void Update()
    {
        if (GetComponent<Renderer>().isVisible)
        {
            print(printer+" Visible");
        }
        else
        {
            print(printer+" Not Visible");
        }
    }
    void Start()
    {
        
    }
}
