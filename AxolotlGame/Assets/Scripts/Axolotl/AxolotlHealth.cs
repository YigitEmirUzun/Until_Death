using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AxolotlHealth : MonoBehaviour ,IDamageable
{  
    PlayerController playerController;
    public SkeletonAnimation anim;
    
    public bool canRegenStamina = true;
    public bool invincible = false;
    public float staminaRegenTimer = 0;

    public float maxHealth;
    public float currentHealth;
    float lastHealt;

    public float maxStamina;
    public float currentStamina;
    float lastStamina;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        
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
            currentStamina += 40 * Time.deltaTime;
        }

        if (!canRegenStamina)
        {
            staminaRegenTimer += Time.deltaTime;
        }

        if(staminaRegenTimer >= 1f)
        {
            canRegenStamina = true;
            staminaRegenTimer = 0;
        }

    }

    public void TakeDamage(int damage)
    {
        if (!playerController.isDashing && !invincible)
        {
            currentHealth -= damage;
            invincible = true;
            StartCoroutine(HurtRoutine());
        }

        
    }

    IEnumerator HurtRoutine()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.03f);
        Time.timeScale = 1;
        anim.skeleton.SetColor(Color.gray);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.white);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.gray);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.white);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.gray);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.white);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.gray);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.white);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.gray);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.white);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.gray);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.white);
        yield return new WaitForSeconds(0.1f);
        invincible = false;
    }

}
