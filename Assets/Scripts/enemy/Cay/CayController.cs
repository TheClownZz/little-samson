using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CayController : MonoBehaviour
{

    public FindPlayer m_Find;
    public GameObject m_BulletObject;
    public Transform m_Shooter;
    public float m_distaceBullet = 0.2f;

    public float m_TimeAttack = 3f;

    private float m_Time = 0;
    private float m_dx, m_dy;

    private bool m_isAttack = false;

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

    void Fire(Vector2 direct)
    {
        GameObject bullet = Instantiate(m_BulletObject, m_Shooter.position, Quaternion.identity);
        bullet.transform.parent = transform.parent;
        NormanBulletController control = bullet.GetComponent<NormanBulletController>();
        if (control)
        {
            control.SetDirect(direct.x, direct.y);
        }
    }

    void Attack()
    {
        Vector2 direct = new Vector2(m_dx, m_dy);
        direct.Normalize();
        Fire(direct);
        //

        Vector2 top = direct;
        top.y += m_distaceBullet;
        Fire(top);

        Vector2 bot = direct;
        bot.y -= m_distaceBullet;
        Fire(bot);

    }
    // Update is called once per frame
    void Update()
    {
        Flip();
        m_Time += Time.deltaTime;

        if (m_Time >= 0.75f && m_isAttack)
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
                m_dx = m_Find.m_Player.transform.position.x - transform.position.x;
                m_dy = m_Find.m_Player.transform.position.y - transform.position.y;
            }
        }
    }
}
