using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public bool m_Collision = false;

    private SoundController m_soundControl;
    void Start()
    {
        if (gameObject.name == "foot")
        {
            GameObject soundManager = GameObject.FindGameObjectWithTag("sound");
            if (soundManager)
            {
                m_soundControl = soundManager.GetComponent<SoundController>();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ground")
        {
            if (gameObject.name == "foot")
            {
                if (m_soundControl)
                    m_soundControl.PlayerClip(m_soundControl.m_PlayerFallClip);
            }
            m_Collision = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "ground")
            m_Collision = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "ground")
            m_Collision = false;
    }
}
