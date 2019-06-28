using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCaBulletController : MonoBehaviour
{
    public float m_Speed = 2f;
    public float m_TimeStart = 3f;
    public float m_TimeAlive = 5f;
    private float m_Scale = 1.75f;
    private int m_direct = -1;
    private float m_Time = 0;
    
    public void SetDirect(int direct)
    {
        m_direct = direct;
    }

    public void SetScale(float scale)
    {
        m_Scale = scale;
    }
    public void SetSpeed(float speed)
    {
        m_Speed = speed;
    }
    void Start()
    {
        transform.localScale = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time >= m_TimeAlive)
            GameObject.Destroy(gameObject);

        if (m_Time >= m_TimeStart)
        {
            transform.localScale = new Vector3(m_Scale, m_Scale, 1);
            transform.position = new Vector3(transform.position.x + m_Speed * m_direct * Time.deltaTime,
                transform.position.y, transform.position.z);
        }
        else
            transform.localScale = new Vector3(m_Time *m_Scale/m_TimeStart, m_Time * m_Scale / m_TimeStart, 1);
        
    }
}
