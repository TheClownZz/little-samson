using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafController : MonoBehaviour
{
    public float m_TimeHide = 3f;
    private BoxCollider2D m_box;
    private bool m_isHide = false;
    private float m_Time = 0;
    void Start()
    {
        m_box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isHide)
        {
            m_Time += Time.deltaTime;
            SpriteRenderer render = GetComponent<SpriteRenderer>();
            if (m_Time <= m_TimeHide / 2)
            {
                float a = render.color.a;
                a -= 2 * m_Time / m_TimeHide;
                if (a <= 0)
                {
                    m_box.enabled = false;
                    a = 0;
                }
                render.color = new Color(1, 1, 1, a);
            }
            else
            {
                float a = render.color.a;
                a += 2 * m_Time / m_TimeHide;
                if (a >= 1)
                {
                    m_box.enabled = true;
                    a = 1;
                }
                render.color = new Color(1, 1, 1, a);

                if (m_Time >= m_TimeHide)
                {
                    m_Time = 0;
                    m_isHide = false;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_isHide = true;
            m_Time = 0;
        }
    }
}
