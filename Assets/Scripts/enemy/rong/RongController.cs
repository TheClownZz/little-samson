using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RongController : MonoBehaviour
{
    
    [HideInInspector]
    public enum dragonColor
    {
        green, red, purple
    }
    public FindPlayer m_Find;
    public GameObject m_bulletObject;
    public Transform m_Shooter;
    public float m_TimeAttack = 2f;
    public dragonColor m_Color;
    public float m_Speed = 1f;

    private Animator m_Anim;
    private float m_time = 0f;

    private float top, bot;
    public int m_direct = 1;
    private bool m_isChange = true;
    private bool m_isKill = false;
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        if(m_Color!=dragonColor.purple)
            m_Anim.SetTrigger("fly");
        top = transform.position.y + 0.25f;
        bot = transform.position.y - 0.25f;
    }

    void Flip()
    {
        if (m_Find.m_Player && m_Color != dragonColor.purple)
        {
            if (m_Find.m_Player.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Fire()
    {
        Vector2 direct;
        direct.x = m_Find.m_Player.transform.position.x - transform.position.x;
        direct.y = m_Find.m_Player.transform.position.y - transform.position.y;

        direct.Normalize();

        GameObject bullet = Instantiate(m_bulletObject, m_Shooter.position, Quaternion.identity);
        bullet.transform.parent = transform.parent;
        NormanBulletController control = bullet.GetComponent<NormanBulletController>();
        control.SetDirect(direct.x, direct.y);
    }

    void Fly()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + m_Speed * m_direct * Time.deltaTime, 0);
        if (transform.position.y > top && m_isChange)
        {
            m_isChange = false;
            m_direct = 0;
            Invoke("ChangeTop", 2f);

        }
        else if (transform.position.y < bot && m_isChange)
        {
            m_isChange = false;
            m_direct = 0;
            Invoke("Changebot", 2f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Flip();
        m_time += Time.deltaTime;
        if (m_time <= 0.3f && m_Find.m_Player)
        {
            if (m_Color != dragonColor.purple)
            {
                m_Anim.ResetTrigger("fly");
                m_Anim.SetTrigger("attack");
            }
        }
        else
        {
            if (m_Color != dragonColor.purple)
            {
                m_Anim.ResetTrigger("attack");
                m_Anim.SetTrigger("fly");
            }
        }


        if (m_time >= m_TimeAttack && m_Color!=dragonColor.purple)
        {
            m_time -= m_TimeAttack;
            if (m_Find.m_Player)
            {
                Fire();
            }
            else
            {
                m_Anim.ResetTrigger("attack");
                m_Anim.SetTrigger("fly");
            }
        }

        if (m_Color == dragonColor.red)
        {
            Fly();
        }

        if(m_Color==dragonColor.purple && m_Find.m_Player)
        {
            if(!m_isKill)
            {
                m_isKill = true;
                GameObject.Destroy(gameObject.transform.parent.gameObject, 3f);
            }
        }

        if(m_Color==dragonColor.purple && m_isKill)
            transform.position = new Vector3(transform.position.x - 1.5f * m_Speed * Time.deltaTime, transform.position.y, 0);

    }

    void ChangeTop()
    {
        m_direct = -1;
        m_isChange = true;
    }

    void Changebot()
    {
        m_direct = 1;
        m_isChange = true;
    }
}
