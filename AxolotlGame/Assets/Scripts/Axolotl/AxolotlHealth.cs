using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AxolotlHealth : MonoBehaviour
{  // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image HealthBar;
    public Image StaminaBar;
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

    // Update is called once per frame
    void Update()
    {
       if((currentStamina < maxStamina) && canRegenStamina)
        {
            currentStamina += 25 * Time.deltaTime;
        }
        
        HealthBar.fillAmount = Mathf.MoveTowards(HealthBar.fillAmount, currentHealth / maxHealth, 2f * Time.deltaTime);      
        StaminaBar.fillAmount = Mathf.MoveTowards(StaminaBar.fillAmount, currentStamina / maxStamina, 2f * Time.deltaTime);

        if (!canRegenStamina)
        {
            staminaRegenTimer += Time.deltaTime;
        }

        if(staminaRegenTimer >= 1.2f)
        {
            canRegenStamina = true;
            staminaRegenTimer = 0;
        }

    }

}
