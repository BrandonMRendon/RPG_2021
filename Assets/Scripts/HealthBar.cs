using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform healthBar;
    
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        float healthPercent = (float)(currentHealth) / (float)(maxHealth);
        healthBar.localScale = new Vector3(healthPercent, 1, 1);
        healthBar.localPosition = new Vector3(((1-healthPercent) / -2), 0, 0);
    }

}
