using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocController : MonoBehaviour
{
    public Vector2 m_direct;
    public Transform m_posStart;
    public float m_Speed = 1f;
    public float m_TimeAlive = 3f;
    [HideInInspector]
    public bool m_isKill = false;
    float  m_Time = 0;
    void Start()
    {
        float scale = m_direct.x;
        if (scale == 0)
            scale = 1;
        transform.localScale = new Vector3(scale, 1, 1);
            
    }

    // Update is called once per frame
    void Update()
    {
        m_Time += Time.deltaTime;
        if(m_Time>=m_TimeAlive)
        {
            if(m_isKill)
            {
                m_isKill = false;
                GameObject.Destroy(gameObject);
                return;
            }
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CapsuleCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            m_Time -= m_TimeAlive;
            transform.position = m_posStart.position;
        }
        else if (m_Time >= 0.9f * m_TimeAlive)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        else if( m_Time>=0.5f * m_TimeAlive)
        {
            float a = GetComponent<SpriteRenderer>().color.a;
            a -= Time.deltaTime * 3;
            if (a <= 0f)
                a = 1f;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
        }
        transform.position = new Vector3(transform.position.x + m_Speed * m_direct.x*Time.deltaTime, 
            transform.position.y + m_Speed * m_direct.y*Time.deltaTime, 0);
    }
}
