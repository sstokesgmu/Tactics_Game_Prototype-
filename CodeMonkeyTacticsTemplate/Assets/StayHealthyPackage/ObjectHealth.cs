using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectHealth : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100;
    protected GameObject obj = null; 

    private float currentHealth = 0;

    //UI
    public GameObject healthbar = null;

    private void Start()
    {
        SetHealth();
    }

    public virtual void SetHealth()
    {
        obj = this.gameObject;
        currentHealth = maxHealth;
        UpdateHealthbar();
    }

    public virtual void UpdateHealthbar()
    {
        float healthPercentage = currentHealth / maxHealth;
        if(healthbar != null)
            healthbar.transform.localScale = new Vector3(healthPercentage,1,1);
    }

    public virtual void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthbar();
        if(currentHealth <= 0)
           Destroy(this.gameObject);
    }

    public virtual float GetHealth()
    {
        return currentHealth;
    }
    public void Die()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}

