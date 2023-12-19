using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public static event EventHandler OnDamagedPlayer;
    public static event EventHandler OnHealed;

    public event EventHandler OnDamagedNPC;
    public event EventHandler OnDead;

    [SerializeField] private Unit unit;
    [SerializeField] private int maxHealth = 100;
    private int health;


    public void Awake()
    {
         health = maxHealth; 
    }

    private void Start()
    {
     
    }
    public void Damage(int damageAmount)
    { 
        health -= damageAmount;
        if (health < 0)
            health = 0;

        if (unit.IsEnemyUnit())
        {
            //Invoke Damaged event 
            OnDamagedNPC?.Invoke(this, EventArgs.Empty);
        }
        else
            OnDamagedPlayer?.Invoke(this, EventArgs.Empty);

        if (health == 0)
            Die();
    }


    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float) health / maxHealth;
    }

  


}

