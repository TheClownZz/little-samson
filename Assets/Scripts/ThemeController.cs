using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeController : MonoBehaviour
{
    public static bool m_isOpenTheme = true;
    public static bool m_isOpenSound = true;
    public AudioClip m_SenceTheme;
    public AudioClip m_BossTheme;
    public AudioClip m_WinTheme;
    public AudioClip m_GameOverTheme;
    private AudioSource m_audio;
    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        m_audio.clip = m_SenceTheme;
        m_audio.Play();
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            m_isOpenTheme = !m_isOpenTheme;
        }
        m_audio.enabled = m_isOpenTheme;
    }
    public void ChangeBossTheme()
    {
        m_audio.clip = m_BossTheme;
        m_audio.Play();
    }

    public void ChangeWinTheme()
    {
        m_audio.clip = m_WinTheme;
        m_audio.Play();
    }

    public void ChangeGameOverTheme()
    {
        m_audio.clip = m_GameOverTheme;
        m_audio.Play();
    }
}
