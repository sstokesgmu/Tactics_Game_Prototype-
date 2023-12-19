using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : ObjectHealth
{
    [SerializeField] private AudioSource audioSource;

    private static float staticHealth = 100f;
    
    public override void SetHealth()
    {
        base.obj = this.gameObject;
        UpdateHealthbar();
    }

    //Override the UpdateHealth Method to use satica currentHealth
    public override void UpdateHealthbar()
    {
        float healthPercentage = staticHealth / base.maxHealth;
        if (healthbar != null)
            healthbar.transform.localScale = new Vector3(healthPercentage, 1, 1);
    }

    // Override the TakeDamage method to use the static currentHealth
    public override void TakeDamage(float damageAmount)
    {
        staticHealth -= damageAmount;
        UpdateHealthbar();
        if (staticHealth <= 0 && this.gameObject.tag == "Player")
            base.Die();
        audioSource.Play();
    }

    // Override the GetHealth method to return the static currentHealth
    public override float GetHealth()
    {
        return staticHealth;
    }
}
