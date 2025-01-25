using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn()
    {
        float time = 1f;

        while(time>0f)
        {
            time -= Time.deltaTime;
            img.color = new Color(0f, 0f, 0f, time);

            yield return 0;
        }

        
    }

    IEnumerator FadeOut(string scene)
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime;
            img.color = new Color(0f, 0f, 0f, time);

            yield return 0;
        }
        SceneManager.LoadScene(scene);

    }
}
