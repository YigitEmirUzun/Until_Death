using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AxolotlHealth : MonoBehaviour ,IDamageable
{  
    public bool canRegenStamina = true;
    public float staminaRegenTimer = 0;

    public float maxHealth;
    public float currentHealth;
    float lastHealt;

    public float maxStamina;
    public float currentStamina;
    float lastStamina;
    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        
        lastHealt = currentHealth;
        lastStamina = currentStamina;       
    }


    void Update()
    {
        UIManager.Instance.currentHealth = currentHealth;
        UIManager.Instance.maxHealth = maxHealth;
        UIManager.Instance.currentStamina = currentStamina;
        UIManager.Instance.maxStamina = maxStamina;

        if ((currentStamina < maxStamina) && canRegenStamina)
        {
            currentStamina += 25 * Time.deltaTime;
        }

        if (!canRegenStamina)
        {
            staminaRegenTimer += Time.deltaTime;
        }

        if(staminaRegenTimer >= 1.5f)
        {
            canRegenStamina = true;
            staminaRegenTimer = 0;
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

}
