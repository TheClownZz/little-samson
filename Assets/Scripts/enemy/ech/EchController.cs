using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchController : MonoBehaviour
{
    public FindPlayer m_Find;
    public float m_TimeAttack = 3f;
    public GameObject m_BulletObject;

    public Transform m_Shooter;

    private bool m_isAttack = false;
    private float m_Time = 0;
    private Animator m_Anim;
    void Start()
    {
        m_Anim = GetComponent<Animator>();
    }

    void Flip()
    {
        if (m_Find.m_Player)
        {
            if (m_Find.m_Player.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Attack()
    {
        GameObject bullet = Instantiate(m_BulletObject, m_Shooter.position, Quaternion.identity);
        bullet.transform.parent = transform.parent;
        NormanBulletController control = bullet.GetComponent<NormanBulletController>();
        if(control)
        {
            control.SetDirect(transform.localScale.x, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_Time += Time.deltaTime;
        Flip();
        if (m_Time >= 1f && m_isAttack)
        {
            m_isAttack = false;
            Attack();
            m_Anim.ResetTrigger("attack");
            m_Anim.SetTrigger("stand");

        }
        if (m_Time >= m_TimeAttack)
        {
            m_Time -= m_TimeAttack;
            if (m_Find.m_Player)
            {
                m_isAttack = true;
                m_Anim.ResetTrigger("stand");
                m_Anim.SetTrigger("attack");
            }
        }

    }
}
