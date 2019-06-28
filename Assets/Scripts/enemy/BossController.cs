using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject m_Gate;
    public GameObject m_healItem;
    public HealManager m_HealManager;
    public float m_TimeDead = 3f;
    public float m_TimeHurt = 1f;



    private float m_Time = 0;
    private bool m_IsDead = false;
    [HideInInspector]
    public bool m_IsHurt = false;

    void Start()
    {
        m_Gate.gameObject.SetActive(false);
    }
    public void Dead()
    {
        m_IsDead = true;
        m_Time = 0;
    }

    void DeadEffect()
    {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        render.color = new Color(1, 1, 1, 1 - m_Time / m_TimeDead);

        if(m_Time>=m_TimeDead)
        {
            if (m_healItem)
            {
                GameObject item = Instantiate(m_healItem, transform.position, Quaternion.identity);
                item.name = "maxHeal";
            }
            m_Gate.gameObject.SetActive(true);
            m_HealManager.transform.gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }
    public void Hurt()
    {
        if (m_IsHurt)
            return;
        m_IsHurt = true;
        m_Time = 0;
    }

    void HurtEffect()
    {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        float a = render.color.a;
        a -= Time.deltaTime;
        if (a <= 0.25f)
            a = 1;
        render.color = new Color(1, 1, 1, a);
        if (m_Time >= m_TimeHurt)
        {
            render.color = new Color(1, 1, 1, 1 );
            m_IsHurt = false;
        }
    }
    void Update()
    {
        m_Time += Time.deltaTime;
        if (m_IsDead)
            DeadEffect();
        else if (m_IsHurt)
            HurtEffect();
    }
}
