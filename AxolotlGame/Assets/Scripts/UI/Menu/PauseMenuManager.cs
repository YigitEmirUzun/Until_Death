using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public bool isDead = false;
    
    public static bool paused = false;
    InputSystem_Actions action;
    public GameObject player;

    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject deadMenu;
    public GameObject originalCanvas;

    public GameObject pauseMenuFirstButton;
    public GameObject optionsMenuFirstButton;
    public GameObject deadMenuFirstButton;
    void Awake()
    {
        action = new InputSystem_Actions();
        player = GameObject.Find("Axolotl");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<AxolotlHealth>().currentHealth <= 0 && !isDead)
        {
            isDead = true;
            originalCanvas.SetActive(true);
            deadMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(deadMenuFirstButton);
            Time.timeScale = 0f;
            player.GetComponent<PlayerController>().enabled = false;
        }
    }

    private void OnEnable()
    {
        action.UI.Enable();

        action.UI.Cancel.performed += onPausePerformed;
    }

    private void OnDisable()
    {
        action.UI.Disable();

        action.UI.Cancel.performed -= onPausePerformed;
    }

    void onPausePerformed(InputAction.CallbackContext context)
    {
        if(!isDead)
        {
            if (!paused)
            {
                player.GetComponent<PlayerController>().enabled = false;
                originalCanvas.SetActive(true);
                pauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(pauseMenuFirstButton);
                Time.timeScale = 0f;
                paused = true;
            }
            else
            {
                player.GetComponent<PlayerController>().enabled = true;
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
                optionsMenu.SetActive(false);
                originalCanvas.SetActive(false);
                paused = false;
            }
        }
    }

    public void ContinueButton()
    {
        player.GetComponent<PlayerController>().enabled = true;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        originalCanvas.SetActive(false);
        paused = false;
        Time.timeScale = 1f;
    }

    public void OptionsMenuButton()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsMenuFirstButton);
    }

    public void BackButton()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(pauseMenuFirstButton);
        Time.timeScale = 0f;
        paused = true;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
