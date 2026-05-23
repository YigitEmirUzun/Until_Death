using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider musicSlider;
    public Slider sfxSlider;
    public TextMeshProUGUI counterText;
    public GameObject counterUI;
    void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume",Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume",Mathf.Log10(value) * 20);
    }

    public void Counter()
    {
        GameManager.isCounterOn = !GameManager.isCounterOn;
        if (GameManager.isCounterOn)
        {
            counterText.text = "(ON)";
            counterUI.SetActive(true);
        }
        else
        {
            counterText.text = "(OFF)";
            counterUI.SetActive(false);
        }
    }

    public void Windowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }
}
