using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    [HideInInspector]
    public GameObject m_Player;
    void Start()
    {
        m_Player = null;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !m_Player)
        {
            m_Player = other.gameObject;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && !m_Player)
            m_Player = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            m_Player = null;
    }
}
