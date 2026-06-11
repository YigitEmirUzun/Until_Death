using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTimeShow : MonoBehaviour
{
    public TextMeshProUGUI finishText;
    public AudioSource audio;
    private void Awake()
    {
        audio.time = 146.5f;
        int minutes = Mathf.FloorToInt(GameManager.counter / 60);
        int seconds = Mathf.FloorToInt(GameManager.counter % 60);

        finishText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void Start()
    {
        StartCoroutine(FinishRoutine());
    }

    IEnumerator FinishRoutine()
    {
        yield return new WaitForSeconds(11);
        SceneManager.LoadScene("MainMenu");
    }
}
