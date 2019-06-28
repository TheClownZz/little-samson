using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource m_PlayerAudio;
    public AudioSource m_EnemyAudio;
    public AudioSource m_ItemAudio;
    public AudioClip m_PlayerFallClip, m_PlayerShootClip, m_PlayerHurtClip, m_playDeadClip;
    public AudioClip m_EnemyHurtClip, m_explosionClip;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            ThemeController.m_isOpenSound = !ThemeController.m_isOpenSound;
        }
        m_PlayerAudio.enabled = m_EnemyAudio.enabled = m_ItemAudio.enabled = ThemeController.m_isOpenSound;
    }

    public void PlayerClip(AudioClip clip)
    {
        if (m_PlayerAudio.isPlaying)
            m_PlayerAudio.Stop();
        m_PlayerAudio.clip = clip;
        m_PlayerAudio.Play();
    }

    public void EnemyClip(AudioClip clip)
    {
        if (m_EnemyAudio.isPlaying)
            m_EnemyAudio.Stop();
        m_EnemyAudio.clip = clip;
        m_EnemyAudio.Play();
    }

    public void ItemClip()
    {
        m_ItemAudio.Play();
    }

}
