using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateController : MonoBehaviour
{
    public int m_nextLevel = 0;

    public AudioSource m_audio;

    void Update()
    {
        m_audio.enabled = ThemeController.m_isOpenSound;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponent<SpriteRenderer>())
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            {
                m_audio.Play();
                GameController.m_isDead = false;
                GameController.m_MaxHealPlayer = GameController.m_currentMaxHeal;
                if (m_nextLevel > 0)
                    GameController.m_Level = m_nextLevel;
                other.gameObject.SetActive(false);
                Animator anim = GetComponent<Animator>();
                anim.ResetTrigger("open");
                anim.SetTrigger("close");
                Invoke("LoadScence", 0.5f);
            }
        }
    }

    void LoadScence()
    {
        if (GameController.m_Level <= GameController.m_maxLevel)
            SceneManager.LoadScene(GameController.m_Level);
        else
        {
            GameController.m_Level = 1;
            SceneManager.LoadScene(0);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
