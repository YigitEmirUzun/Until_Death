using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image HealthBar;
    public Image StaminaBar;

    public float currentHealth;
    public float currentStamina;
    public float maxHealth;
    public float maxStamina;

    public TextMeshProUGUI counterUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        HealthBar.fillAmount = Mathf.MoveTowards(HealthBar.fillAmount, currentHealth / maxHealth, 2f * Time.deltaTime);
        StaminaBar.fillAmount = Mathf.MoveTowards(StaminaBar.fillAmount, currentStamina / maxStamina, 2f * Time.deltaTime);

        int minutes = Mathf.FloorToInt(GameManager.counter / 60);
        int seconds = Mathf.FloorToInt(GameManager.counter % 60);

        counterUI.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }


}
