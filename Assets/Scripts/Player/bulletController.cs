using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{

    public CircleCollider2D m_Collider;
    public float m_SpeedX = 2.5f;
    public float m_SpeedY = 2.5f;
    public float m_TimeFly = 0.5f;
    public bool m_IsReady = true;
    public float m_MaxTimeFire = 3;

    private float m_Time = 0;
    private float m_DirectX = 1;

    public int m_Type = 0;
    private SpriteRenderer m_Render;
    private Animator m_Anim;

    // Update is called once per frame

    void Start()
    {
        m_Render = GetComponent<SpriteRenderer>();
        m_Anim = GetComponent<Animator>();
    }
    public void StartFire(Vector3 pos)
    {
        transform.position = pos;
    }
    public void FindDirect(int direct)
    {
        m_DirectX = direct;
    }
    public void SetType(int type)
    {
        m_Type = type;

        if(m_Type==0)
        {
            m_Anim.ResetTrigger("fire");
            m_Anim.SetTrigger("bullet");
        }
        else
        {
            m_Anim.ResetTrigger("bullet");
            m_Anim.SetTrigger("fire");
        }
    }
    void SetEnable(bool f)
    {
        if (m_Render.enabled != f)
            m_Render.enabled = f;
        if (m_Collider.enabled != f)
            m_Collider.enabled = f;
    }
    void Update()
    {

        if (!m_IsReady)
        {
            SetEnable(true);
            m_Time += Time.deltaTime;
            float x = transform.position.x;
            float y = transform.position.y;
            x += m_DirectX * m_SpeedX * Time.deltaTime;
            if (m_Type == 1)
            {
                if (m_Time >= m_TimeFly)
                    y += m_SpeedY * Time.deltaTime;
                else
                    y -= m_SpeedY * Time.deltaTime / 10;
            }
            transform.position = new Vector3(x, y, transform.position.z);
        }
        else
        {
            SetEnable(false);
        }

        if (m_Time > m_MaxTimeFire)
        {
            m_IsReady = true;
            m_Time = 0;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="enemy" || other.tag=="enemyArmor")
        {
            m_IsReady = true;
            m_Time = 0;
            SetEnable(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "MainCamera")
        {
            m_IsReady = true;
            m_Time = 0;
            SetEnable(false);
        }
    }
}
