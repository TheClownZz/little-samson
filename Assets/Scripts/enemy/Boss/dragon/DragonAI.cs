using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAI : MonoBehaviour
{

    public FindPlayer m_Find;
    public GameObject m_bullet;
    public float m_TimeAttack = 2f;
    public Transform m_shooter;

    private DameController m_dameControl;
    private float m_Time = 0;
    private bool m_isPower = false;
    private bool m_IsAttack = false;
    private float m_TimePower = 0;
    private Animator m_Anim;

    void Start()
    {
        m_dameControl = GetComponent<DameController>();
        m_Anim = GetComponent<Animator>();
        m_Anim.SetTrigger("fly");
    }


    void fire(Vector2 direct, float rotation, float speed)
    {
        m_Time = 0;
        m_Anim.ResetTrigger("fly");
        m_Anim.ResetTrigger("power");
        m_Anim.SetTrigger("attack");
        m_IsAttack = true;

        GameObject bullet = Instantiate(m_bullet, m_shooter.position, Quaternion.identity);
        dragonBulletController bulletControl = bullet.GetComponent<dragonBulletController>();
        if (bulletControl)
        {
            bulletControl.SetRotation(rotation);
            bulletControl.m_speed = speed;
            if (m_isPower)
            {
                bullet.GetComponent<DameController>().m_Dame++;
                bulletControl.SetDirect(direct.x, direct.y, 1.5f);
            }
            else
                bulletControl.SetDirect(direct.x, direct.y, 1f);

        }
    }
    void Attack()
    {
        if (!m_isPower)
        {
            if (m_Find.m_Player)
            {
                Vector2 direct;
                direct.x = transform.position.x - m_Find.m_Player.transform.position.x;
                direct.y = transform.position.y - m_Find.m_Player.transform.position.y;
                direct.Normalize();
                direct.x *= -1;
                direct.y *= -1;

                float rotation = Mathf.Rad2Deg*Mathf.Atan2(direct.y, direct.x)+180;
                fire(direct, rotation, 1.5f);
            }
           
        }
        else
        {
            m_TimeAttack = 1.5f;
            m_Anim.ResetTrigger("fly");
            m_Anim.SetTrigger("power");
            m_TimePower += Time.deltaTime;
            if (m_TimePower >= 1.5f && m_Find.m_Player)
            {
                m_TimePower = 0;
                Vector2 direct;
                direct.x = transform.position.x - m_Find.m_Player.transform.position.x;
                direct.y = transform.position.y - m_Find.m_Player.transform.position.y;
                direct.Normalize();
                direct.x *= -1;
                direct.y *= -1;

                float rotation = Mathf.Rad2Deg * Mathf.Atan2(direct.y, direct.x) + 180;
                fire(direct, rotation, 3f);
            }
        }
    }


    void Update()
    {
        if (m_dameControl.m_Heal <= 0)
            return;
        if (m_dameControl.m_Heal <= 0.3f * m_dameControl.m_MaxHeal)
            m_isPower = true;

        m_Time += Time.deltaTime;

        if (m_Time >= 0.2f && m_IsAttack)
        {
            m_Anim.ResetTrigger("attack");
            m_Anim.ResetTrigger("power");
            m_Anim.SetTrigger("fly");
            m_IsAttack = false;
        }
        if (m_Time >= m_TimeAttack)
        {
            Attack();
        }

    }
}
