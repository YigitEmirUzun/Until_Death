using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public static bool paused = false;
    InputSystem_Actions action;
    public GameObject player;

    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject originalCanvas;

    public GameObject pauseMenuFirstButton;
    public GameObject optionsMenuFirstButton;
    void Awake()
    {
        action = new InputSystem_Actions();
        player = GameObject.Find("Axolotl");
    }

    // Update is called once per frame
    void Update()
    {
        if(paused)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            player.GetComponent<PlayerController>().enabled = true;
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

    public void ContinueButton()
    {
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
