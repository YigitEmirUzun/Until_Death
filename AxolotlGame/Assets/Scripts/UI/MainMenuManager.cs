using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public GameObject mainMenuCanvas;
    public GameObject creditsCanvas;
    public GameObject optionsCanvas;

    public GameObject mainMenuFirstButton;
    public GameObject creditsFirstButton;
    public GameObject optionsFirstButton;
    public void StartTutorial()
    {
        SceneManager.LoadScene("StartCutscene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        mainMenuCanvas.SetActive(false);
        creditsCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
    }

    public void ShowMainMenu()
    {
        creditsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
    }

    public void ShowOptions()
    {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void OrcaBoss()
    {
        SceneManager.LoadScene("OrcaBoss");
    }
}
