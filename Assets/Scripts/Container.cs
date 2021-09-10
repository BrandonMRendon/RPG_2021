using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    public GameObject[] inventoryArray = new GameObject[18]; //Player's inventory
    public Sprite[] inventorySprites = new Sprite[18]; //the sprites of their inventorty
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().OpenChest(this);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CloseChest();
        }
            
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
