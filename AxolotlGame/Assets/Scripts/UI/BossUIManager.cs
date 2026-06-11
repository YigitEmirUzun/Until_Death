using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    public Image BossHealthImage;
    public float BossHealth = 1000f;
    public float currentHealth;
    public GameObject boss;
    BossHealth healthSc;

    private void Awake()
    {
        
    }

    private void Start()
    {
        healthSc = boss.GetComponent<BossHealth>();
        BossHealth = healthSc.Health;
    }

    private void Update()
    {
        currentHealth = healthSc.Health;

        BossHealthImage.fillAmount = Mathf.MoveTowards(BossHealthImage.fillAmount, currentHealth / BossHealth, 2f * Time.deltaTime);
    }
}
