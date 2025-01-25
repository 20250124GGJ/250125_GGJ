using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public SceneFader scenefader;

    public void Play()
    {
        scenefader.FadeTo(levelToLoad);
    }
}
