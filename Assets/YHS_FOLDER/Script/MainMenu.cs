using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    SoundManager soundManager;

    public string levelToLoad;
    public SceneFader scenefader;
    public GameObject settingPanel;
    public GameObject creditPanel;

    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }
    public void Play()
    {
        soundManager.PlaySFX(soundManager.click);
        scenefader.FadeTo(levelToLoad);
    }

    public void Setting()
    {
        soundManager.PlaySFX(soundManager.click);
        settingPanel.SetActive(true);
    }

    public void Credit()
    {
        soundManager.PlaySFX(soundManager.click);
        creditPanel.SetActive(true);
    }

    public void Close()
    {
        soundManager.PlaySFX(soundManager.click);
        settingPanel.SetActive(false);
        creditPanel.SetActive(false);
    }
}
