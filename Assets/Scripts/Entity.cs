using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;
    public int maxHealth;
    protected bool isDead;
    public int startingHealth;
    public int getHealth()
    {
        return health;
    }
    public void ModifyHealth(int modifier)
    {
        health += modifier;
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }
    protected void SetHealth(int hp)
    {
        health = hp;
    }
    public bool IsDead()
    {
        return isDead;
    }

   
}
