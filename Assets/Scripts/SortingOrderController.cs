using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderController : LayerContainer
{
    public bool Level1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ColliderTilesLevelOneOnly.SetActive(Level1);
            ColliderTilesLevelTwoOnly.SetActive(!Level1);
            collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Level1 ? LAYER_One : LAYER_Two;


        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
