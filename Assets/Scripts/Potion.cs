using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, IItem
{
    public int Damage { get; set; }
    public Sprite sprite { get; set; }
    public int usesMana { get; set; }
    public string itemName { get; set; }
    public int stack { get; set; }
    public int stackHolding { get; set; }
    public bool isActivelyUsed { get; set; }

    public Sprite setImage;
    public string setItemName;
    public int health, mana;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        Damage = -health;
        usesMana = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sprite = setImage;
        itemName = setItemName;
        isActivelyUsed = false;
        stackHolding = 1;
        stack = 64;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Action(Vector2 directionFacing)
    {
        if(health > 0)
        {
            player.ModifyHealth(health);
        }
        else
        {
            player.ManaBar.value += mana;
            if (player.ManaBar.value > player.ManaBar.maxValue)
            {
                player.ManaBar.value = player.ManaBar.maxValue;
            }
        }
    }
}
