using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip click;
    public AudioClip shakeSound;
    public AudioClip fire;

    private static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // ���� �ٲ� �� ��ü�� �����˴ϴ�.
        }
        else
        {
            Destroy(gameObject);  // �̹� �����ϴ� �ν��Ͻ��� ������ �� ��ü�� �����մϴ�.
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
