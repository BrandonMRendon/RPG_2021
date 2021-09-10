using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour, IItem
{
    public int Damage { get; set; }
    public Sprite sprite { get; set; }
    public int usesMana { get; set; }
    public string itemName { get; set; }
    public int stackHolding { get; set; }
    public int stack { get; set; }
    public bool isActivelyUsed { get; set; }
    public GameObject lanternPrefab;
    public Sprite setImage;
    public string setItemName;
    public bool setActiveUse;
    GameObject player, lantern;
    PlayerController playerController;
    public int setManaCost, setlightProtection;
    bool lanternOn;
    public void Action(Vector2 directionFacing)
    {
        if (!lanternOn)
        {
            
            lantern = Instantiate(lanternPrefab, transform.position + new Vector3(.0f,-.5f,0) , transform.rotation, transform);
            lantern.GetComponent<SpriteRenderer>().sortingLayerName = player.GetComponent<SpriteRenderer>().sortingLayerName;
            lantern.GetComponent<SpriteRenderer>().sortingOrder = -1;
            lanternOn = true;
            playerController.lightProtection = setlightProtection;
        }
        else
        {
            playerController.lightProtection = 0;
            Destroy(lantern);
            lanternOn = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        Damage = 0;
        stack = 1;
        stackHolding = 1;
        sprite = setImage;
        usesMana = setManaCost;
        itemName = setItemName;
        lanternOn = false;
        isActivelyUsed = setActiveUse;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
