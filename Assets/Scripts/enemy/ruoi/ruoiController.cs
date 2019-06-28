using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ruoiController : MonoBehaviour
{
    public DameController m_Dame;
    public float m_speed;
    public FindPlayer m_Find;
    private float m_directY = 1;
    void Start()
    {
        
    }

    void Flip()
    {
        if(m_Find.m_Player)
        {
            if (m_Find.m_Player.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);

            if (m_Find.m_Player.transform.position.y > transform.position.y)
                m_directY = 1;
            else
                m_directY = -1;
        }
    }

    void Follow()
    {
        transform.position = new Vector3(transform.position.x + m_speed * transform.localScale.x * Time.deltaTime
                , transform.position.y + m_speed * Time.deltaTime * m_directY, 0);
    }
    // Update is called once per frame
    void Update()
    {
        Flip();
        if(m_Find.m_Player)
        {
            Follow();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            m_Dame.Dead();
    }
}
