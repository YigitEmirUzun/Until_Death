using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenuManager : MonoBehaviour
{
    public GameObject deadMenuCanvas;
    public GameObject canvasOrigin;
    public GameObject player;
    public PauseMenuManager pauseMenuManager;

    private void Awake()
    {
        player = GameObject.Find("Axolotl");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartButton()
    {
        pauseMenuManager.isDead = false;
        player.GetComponent<AxolotlHealth>().currentHealth = player.GetComponent<AxolotlHealth>().maxHealth;
        canvasOrigin.SetActive(false);
        deadMenuCanvas.SetActive(false);
        player.GetComponent<PlayerController>().enabled = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void MainMenuButton()
    {
        Destroy(player);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
