using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullbulletController : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 m_Direct;
    public float m_Speed = 1f;

    public float m_TimeAlive = 3f;
    public float m_Time = 0;

    [HideInInspector] public bool m_AlllowFindPlayer = false;
    void Start()
    {
        
    }


    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player)
        {
            m_Direct.x = player.transform.position.x - transform.position.x;
            m_Direct.y = player.transform.position.y - transform.position.y;
            m_Direct.Normalize();
        }
    }
    // Update is called once per frame
    void Update()
    {
        m_Time += Time.deltaTime;

        if (m_Time >= m_TimeAlive)
            GameObject.Destroy(gameObject);
        if(m_Time>1f)
        {
            if(m_AlllowFindPlayer)
            {
                m_AlllowFindPlayer = false;
                FindPlayer();
            }
            transform.position = new Vector3(transform.position.x + m_Speed * m_Direct.x * Time.deltaTime,
                transform.position.y + m_Speed * m_Direct.y * Time.deltaTime, 0);
        }
    }

    
}
